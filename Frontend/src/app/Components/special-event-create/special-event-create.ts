import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatDialogRef, MatDialogModule, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { SpecialEventService } from '../../Services/special-event.service'; 
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { SpecialEventRequest } from '../../Models/special-event.request';

@Component({
  selector: 'app-special-event-create',
  standalone: true,
  imports: [
    CommonModule, 
    MatCardModule, 
    MatButtonModule, 
    MatInputModule, 
    MatDialogModule,
    ReactiveFormsModule, 
    MatIconModule, 
    MatCheckboxModule, 
    MatFormFieldModule],
  templateUrl: './special-event-create.html',
  styleUrl: './special-event-create.css'
})
export class SpecialEventCreate {
  private fb = inject(FormBuilder);
  private ss = inject(SpecialEventService);
  public dialogRef = inject(MatDialogRef<SpecialEventCreate>);
  
  loading = signal(false);
  error = signal<string | null>(null);
  success = signal(false);

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    date: ['', [Validators.required, Validators.maxLength(500)]],
    startTime: ['', [Validators.required]],
    endTime: ['', [Validators.required]],
    maxCapacity: ['', [Validators.required, Validators.min(1)]],
    additionalCost: ['', [Validators.required, Validators.min(0)]],
    idToAdd: ['', [Validators.pattern(/^\s*\d+(\s*,\s*\d+)*\s*$/)
]]
});

  Submit() {
    if (this.form.invalid || this.loading()) return;
    this.loading.set(true);

    let name = this.form.value.name!;
    let date = new Date(this.form.value.date!);
    let startTime = this.form.value.startTime!;
    let endTime = this.form.value.endTime!;
    let maxCapacity = this.form.value.maxCapacity!;
    let additionalCost = this.form.value.additionalCost!;
    let attractionsIds = this.IdsToList(this.form.value.idToAdd!);

    const newSpecialEvent: SpecialEventRequest = {
      name,
      date,
      startTime,
      endTime,
      maxCapacity: Number(maxCapacity),
      additionalCost: Number(additionalCost),
      attractionsIds
    };
    
    this.ss.Create(newSpecialEvent).subscribe({
      next: (_) => {
        this.loading.set(false);
        this.form.reset();
        this.success.set(true);
      },
      error: (error) => {
        this.error.set('Error al crear la incidencia, intente nuevamente' + error.message);
        this.success.set(false);
        this.loading.set(false);
      }
    });
    
  }

  private IdsToList(ids: string): number[] {
    if (!ids) return [];
    return ids.split(',').map(id => parseInt(id.trim(), 10));
  }

  onCancel() {
    this.dialogRef.close();
  }
}
