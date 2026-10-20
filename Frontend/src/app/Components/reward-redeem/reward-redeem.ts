import { 
  Component,
  inject,
  signal,
  OnInit
} from '@angular/core';
import {
  MatDialogTitle,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { RewardService } from '../../Services/reward.service';
import { VisitorService } from '../../Services/visitor.service';
import { AuthService } from '../../Services/auth.service';
import { RewardModel } from '../../Models/reward.model';

@Component({
  selector: 'app-reward-redeem',
  standalone: true,
  imports: [
    MatDialogTitle,
    MatDialogActions,
    MatDialogClose,
    CommonModule,
    MatButtonModule,
    MatDialogContent,
    MatProgressSpinner,
    MatIconModule
  ],
  templateUrl: './reward-redeem.html',
  styleUrl: './reward-redeem.css'
})
export class RewardRedeem implements OnInit {
  private rewardService = inject(RewardService);
  private visitorService = inject(VisitorService);
  private authService = inject(AuthService);
  private dialogRef = inject(MatDialogRef<RewardRedeem>);
  data = inject<{ reward: RewardModel }>(MAT_DIALOG_DATA);

  loading = signal(true);
  error = signal<string | null>(null);
  success = signal<string | null>(null);
  visitorPoints = signal<number | null>(null);
  visitorId = signal<number | null>(null);
  canRedeem = signal(false);
  validationMessage = signal<string>('');

  ngOnInit(): void {
    this.loadVisitorData();
  }

  loadVisitorData(): void {
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
          this.visitorPoints.set(visitor.points);
          this.validateRedemption(visitor.id);
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

  validateRedemption(visitorId: number): void {
    this.rewardService.ValidateRedemption(visitorId, this.data.reward.id).subscribe({
      next: (response: any) => {
        if (response.canRedeem) {
          this.canRedeem.set(true);
          this.validationMessage.set('');
        } else {
          this.canRedeem.set(false);
          this.validationMessage.set('No puedes canjear esta recompensa');
        }
        this.loading.set(false);
      },
      error: (err) => {
        this.canRedeem.set(false);
        this.validationMessage.set('Error al validar la recompensa');
        this.loading.set(false);
      }
    });
  }

  Redeem(): void {
    if (!this.canRedeem() || !this.visitorId()) return;

    this.loading.set(true);
    this.error.set(null);

    this.rewardService.RedeemReward(this.visitorId()!, this.data.reward.id).subscribe({
      next: (redemption) => {
        this.success.set('¡Recompensa canjeada con éxito!');
        this.loading.set(false);
        
        const newPoints = this.visitorPoints()! - this.data.reward.pointsCost;
        this.visitorPoints.set(newPoints);
        
        setTimeout(() => {
          this.dialogRef.close(true);
        }, 2000);
      },
      error: (err) => {
        this.error.set(err?.error?.message || 'Error al canjear la recompensa');
        this.loading.set(false);
      }
    });
  }
}