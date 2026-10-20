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
import { RouterModule } from '@angular/router';
import { RewardService } from '../../Services/reward.service';
import { VisitorService } from '../../Services/visitor.service';
import { AuthService } from '../../Services/auth.service';
import { RewardRedemptionModel } from '../../Models/reward-redemption.model';
import { SideNavBar } from '../side-nav-bar/side-nav-bar';

@Component({
  selector: 'app-my-rewards',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    RouterModule,
    SideNavBar
  ],
  templateUrl: './my-rewards.html',
  styleUrl: './my-rewards.css'
})
export class MyRewards implements OnInit {
  private rewardService = inject(RewardService);
  private visitorService = inject(VisitorService);
  private authService = inject(AuthService);

  redemptions = signal<RewardRedemptionModel[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.LoadData();
  }

  LoadData(): void {
    this.loading.set(true);
    this.error.set(null);

    const email = this.authService.GetEmail();
    if (!email) {
      this.error.set('No se pudo obtener la información del usuario');
      this.loading.set(false);
      return;
    }

    this.visitorService.GetByEmail(email).subscribe({
      next: (visitors) => {
        if (visitors && visitors.length > 0) {
          const visitorId = visitors[0].id;
          this.loadRedemptions(visitorId);
        } else {
          this.error.set('No se encontró información del visitante');
          this.loading.set(false);
        }
      },
      error: (err) => {
        this.error.set('Error al cargar datos del visitante');
        this.loading.set(false);
      }
    });
  }

  private loadRedemptions(visitorId: number): void {
    this.rewardService.GetVisitorRedemptions(visitorId).subscribe({
      next: (response: any) => {
        this.redemptions.set(response.redemptions);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set(err?.message || 'No se pudieron cargar las recompensas canjeadas');
        this.loading.set(false);
      }
    });
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Completed':
        return 'status-completed';
      case 'Pending':
        return 'status-pending';
      case 'Cancelled':
        return 'status-cancelled';
      default:
        return '';
    }
  }

  getStatusIcon(status: string): string {
    switch (status) {
      case 'Completed':
        return 'check_circle';
      case 'Pending':
        return 'schedule';
      case 'Cancelled':
        return 'cancel';
      default:
        return 'help';
    }
  }

  getStatusText(status: string): string {
    switch (status) {
      case 'Completed':
        return 'Completado';
      case 'Pending':
        return 'Pendiente';
      case 'Cancelled':
        return 'Cancelado';
      default:
        return status;
    }
  }
}
