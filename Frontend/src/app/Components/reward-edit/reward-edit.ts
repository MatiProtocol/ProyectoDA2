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
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { RewardService } from '../../Services/reward.service';
import { RewardModel } from '../../Models/reward.model';
import { RewardRequest } from '../../Models/reward.request';

@Component({
  selector: 'app-reward-edit',
  standalone: true,
  imports: [
    MatDialogTitle,
    MatDialogActions,
    MatDialogClose,
    ReactiveFormsModule,
    CommonModule,
    MatButtonModule,
    MatDialogContent,
    MatInputModule,
    MatProgressSpinner,
    MatSelectModule,
    MatCheckboxModule
  ],
  templateUrl: './reward-edit.html',
  styleUrl: './reward-edit.css'
})
export class RewardEdit implements OnInit {
  private rewardService = inject(RewardService);
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<RewardEdit>);
  data = inject<{ reward: RewardModel }>(MAT_DIALOG_DATA);

  loading = signal(false);
  error = signal<string | null>(null);
  success = signal<string | null>(null);

  form = this.fb.group({
    name: ['', [Validators.required]],
    description: ['', [Validators.required]],
    pointsCost: [0, [Validators.required, Validators.min(1)]],
    availableQuantity: [0, [Validators.required, Validators.min(0)]],
    requiredMembershipLevel: [null as string | null],
    active: [true]
  });

  ngOnInit(): void {
    if (this.data.reward) {
      this.form.patchValue({
        name: this.data.reward.name,
        description: this.data.reward.description,
        pointsCost: this.data.reward.pointsCost,
        availableQuantity: this.data.reward.availableQuantity,
        requiredMembershipLevel: this.data.reward.requiredMembershipLevel || null,
        active: this.data.reward.active
      });
    }
  }

  Submit(): void {
    if (this.form.invalid) return;

    this.loading.set(true);
    this.error.set(null);
    this.success.set(null);

    const request: RewardRequest = {
      name: this.form.value.name!,
      description: this.form.value.description!,
      pointsCost: this.form.value.pointsCost!,
      availableQuantity: this.form.value.availableQuantity!,
      requiredMembershipLevel: this.form.value.requiredMembershipLevel || undefined
    };

    this.rewardService.Update(this.data.reward.id, request).subscribe({
      next: () => {
        this.success.set('Recompensa actualizada con éxito');
        this.loading.set(false);
        setTimeout(() => {
          this.dialogRef.close(true);
        }, 1200);
      },
      error: (err) => {
        this.error.set(err?.error?.message || 'Error al actualizar la recompensa');
        this.loading.set(false);
      }
    });
  }
}