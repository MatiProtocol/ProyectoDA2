import { 
  Component,
  inject, 
  OnInit, 
  signal
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog } from '@angular/material/dialog';
import { RewardService } from '../../Services/reward.service';
import { RewardModel } from '../../Models/reward.model';
import { AuthService } from '../../Services/auth.service';
import { RewardEdit } from '../reward-edit/reward-edit';
import { RewardCreate } from '../reward-create/reward-create';
import { RewardRedeem } from '../reward-redeem/reward-redeem';
import { SideNavBar } from '../side-nav-bar/side-nav-bar';

@Component({
  selector: 'app-reward-list-menu',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    SideNavBar
  ],
  templateUrl: './reward-list-menu.html',
  styleUrl: './reward-list-menu.css'
})
export class RewardListMenu implements OnInit {
  private rewardService = inject(RewardService);
  private authService = inject(AuthService);
  private dialog = inject(MatDialog);

  isVisitor = this.authService.IsRole('Visitor');
  isParkAdmin = this.authService.IsRole('ParkAdmin');

  displayedColumns: string[] = [
    'name', 
    'description', 
    'pointsCost', 
    'availableQuantity',
    'requiredMembershipLevel',
    'active',
    'actions'
  ];
  
  dataSource = new MatTableDataSource<RewardModel>([]);
  loading = signal(true);
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.LoadData();
  }

  LoadData(): void {
    this.loading.set(true);
    this.error.set(null);

    this.rewardService.GetAll().subscribe({
      next: (items) => {
        this.dataSource.data = items;
      },
      error: (err) => {
        this.error.set(err?.message || 'No se pudieron cargar las recompensas');
        this.loading.set(false);
      },
      complete: () => {
        this.loading.set(false);
      }
    });
  }

  onCreateDialog(): void {
    this.dialog.open(RewardCreate, {
      width: '500px',
      disableClose: false
    }).afterClosed().subscribe(() => {
      this.LoadData();
    });
  }

  onEditDialog(id: number): void {
    const reward = this.dataSource.data.find(r => r.id === id);
    if (!reward) return;

    this.dialog.open(RewardEdit, {
      width: '500px',
      disableClose: false,
      data: { reward }
    }).afterClosed().subscribe(() => {
      this.LoadData();
    });
  }

  onRedeemDialog(rewardId: number): void {
    const reward = this.dataSource.data.find(r => r.id === rewardId);
    if (!reward) return;

    this.dialog.open(RewardRedeem, {
      width: '450px',
      disableClose: false,
      data: { reward }
    }).afterClosed().subscribe((success) => {
      if (success) {
        this.LoadData();
      }
    });
  }

  Delete(id: number): void {
    if (this.loading()) return;
    
    const confirmed = window.confirm('¿Seguro que deseas eliminar esta recompensa?');
    if (!confirmed) return;

    this.loading.set(true);
    this.rewardService.Delete(id).subscribe({
      next: () => {
        this.dataSource.data = this.dataSource.data.filter(r => r.id !== id);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set(err?.message || 'No se pudo eliminar la recompensa');
        this.loading.set(false);
      }
    });
  }
}