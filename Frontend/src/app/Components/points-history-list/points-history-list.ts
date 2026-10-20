import { 
  Component,
  inject, 
  OnInit, 
  signal
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog } from '@angular/material/dialog';
import { PointsHistoryService } from '../../Services/points-history.service';
import { AuthService } from '../../Services/auth.service';
import { VisitorService } from '../../Services/visitor.service';
import { PointsHistoryModel } from '../../Models/points-history.model';
import { SideNavBar } from '../side-nav-bar/side-nav-bar';
import { PointsHistoryFilter } from '../points-history-filter/points-history-filter';
import { PointsSummaryResponse } from '../../Models/points-summary.response';

@Component({
  selector: 'app-points-history-list',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    SideNavBar
  ],
  templateUrl: './points-history-list.html',
  styleUrl: './points-history-list.css'
})
export class PointsHistoryList implements OnInit {
  private pointsHistoryService = inject(PointsHistoryService);
  private authService = inject(AuthService);
  private visitorService = inject(VisitorService);
  private dialog = inject(MatDialog);

  historyData = signal<PointsHistoryModel[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);
  visitorId = signal<number | null>(null);
  summary = signal<PointsSummaryResponse | null>(null);
  isParkAdmin = this.authService.IsRole('ParkAdmin');

  ngOnInit(): void {
    this.loadVisitorData();
  }

  private loadVisitorData(): void {
    const email = this.authService.GetEmail();
    if (!email) {
      this.error.set('No se pudo obtener la información del usuario');
      this.loading.set(false);
      return;
    }

    this.visitorService.GetByEmail(email).subscribe({
      next: (visitors) => {
        if (visitors && visitors.length > 0) {
          const visitor = visitors[0];
          this.visitorId.set(visitor.id);
          this.LoadData();
        } else {
          this.error.set('No se encontró información del visitante');
          this.loading.set(false);
        }
      },
      error: (err) => {
        console.error('Error loading visitor:', err);
        this.error.set('Error al cargar datos del visitante');
        this.loading.set(false);
      }
    });
  }

  LoadData(): void {
    if (!this.visitorId()) return;

    this.loading.set(true);
    this.error.set(null);

    this.pointsHistoryService.GetVisitorHistory(this.visitorId()!).subscribe({
      next: (response) => {
        console.log('History response:', response);
        
        if (response && response.history) {
          this.historyData.set(response.history);
          
          this.createSummaryFromResponse(response);
          
          if (this.isParkAdmin) {
            this.loadFullSummary(this.visitorId()!);
          }
        } else if (Array.isArray(response)) {
          this.historyData.set(response);
        } else {
          this.historyData.set([]);
        }
        
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error loading history:', err);
        this.error.set(err?.error?.message || err?.message || 'No se pudo cargar el historial');
        this.loading.set(false);
      }
    });
  }

  private createSummaryFromResponse(response: any): void {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    
    const todayTransactions = response.history.filter((h: PointsHistoryModel) => {
      const txDate = new Date(h.transactionDate);
      txDate.setHours(0, 0, 0, 0);
      return txDate.getTime() === today.getTime();
    });

    const todayPoints = todayTransactions
      .filter((h: PointsHistoryModel) => h.points > 0)
      .reduce((sum: number, h: PointsHistoryModel) => sum + h.points, 0);

    const redemptionsToday = todayTransactions
      .filter((h: PointsHistoryModel) => h.points < 0 && h.origin === 'Reward')
      .length;

    this.summary.set({
      visitorId: this.visitorId()!,
      totalPointsEarned: response.totalPointsEarned || 0,
      totalPointsSpent: response.totalPointsSpent || 0,
      currentBalance: response.currentBalance || 0,
      todayPoints: todayPoints,
      redemptionsToday: redemptionsToday
    });
  }

  private loadFullSummary(visitorId: number): void {
    this.pointsHistoryService.GetPointsSummary(visitorId).subscribe({
      next: (summaryData) => {
        console.log('Full summary response:', summaryData);
        this.summary.set(summaryData);
      },
      error: (err) => {
        console.warn('Could not load full summary (using calculated summary instead):', err);
      }
    });
  }

  onFilterDialog(): void {
    if (!this.visitorId()) return;

    this.dialog.open(PointsHistoryFilter, {
      width: '500px',
      disableClose: false,
      data: { visitorId: this.visitorId() }
    }).afterClosed().subscribe((result) => {
      if (result) {
        this.applyFilter(result.startDate, result.endDate);
      }
    });
  }

  private applyFilter(startDate?: Date, endDate?: Date): void {
    if (!this.visitorId()) return;

    this.loading.set(true);
    this.error.set(null);

    this.pointsHistoryService.GetHistoryByDateRange(
      this.visitorId()!,
      startDate,
      endDate
    ).subscribe({
      next: (response) => {
        console.log('Filtered history response:', response);
        
        if (response && response.history) {
          this.historyData.set(response.history);
          this.createSummaryFromResponse(response);
        } else if (Array.isArray(response)) {
          this.historyData.set(response);
        } else {
          this.historyData.set([]);
        }
        
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error loading filtered history:', err);
        this.error.set(err?.error?.message || err?.message || 'No se pudo cargar el historial filtrado');
        this.loading.set(false);
      }
    });
  }

  getOriginIcon(origin: string): string {
    const icons: { [key: string]: string } = {
      'Attraction': 'attractions',
      'Reward': 'card_giftcard',
      'Mission': 'flag',
      'SpecialEvent': 'celebration',
      'Manual': 'edit'
    };
    return icons[origin] || 'help';
  }

  getOriginLabel(origin: string): string {
    const labels: { [key: string]: string } = {
      'Attraction': 'Atracción',
      'Reward': 'Recompensa',
      'Mission': 'Misión',
      'SpecialEvent': 'Evento Especial',
      'Manual': 'Ajuste Manual'
    };
    return labels[origin] || origin;
  }
}