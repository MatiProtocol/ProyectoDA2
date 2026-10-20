import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatDialogRef, MatDialogModule, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { IncidenceService } from '../../Services/incidence.service';
import { AttractionModel } from '../../Models/attraction.model';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { IncidenceRequest } from '../../Models/incidence.request';

@Component({
  selector: 'app-incidence-create',
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
  templateUrl: './incidence-create.html',
  styleUrl: './incidence-create.css'
})
export class IncidenceCreate {
  private fb = inject(FormBuilder);
  private is = inject(IncidenceService);
  public dialogRef = inject(MatDialogRef<IncidenceCreate>);
  public data = inject(MAT_DIALOG_DATA) as { attraction?: AttractionModel };
  
  loading = signal(false);
  error = signal<string | null>(null);
  success = signal(false);

  form = this.fb.group({
    description: ['', [Validators.required, Validators.maxLength(500)]],
    isMaintenance: [false],
    startDate: ['', { disabled: true }],
    startTime: ['', { disabled: true }],
    duration: ['', [Validators.min(1)]],
});

Submit() {
    if (this.form.invalid || this.loading()) return;
    this.loading.set(true);

    var { description, attractionId } = this.form.value as {
      description: string;
      attractionId: number;
    };
    if (!this.data.attraction) {
      this.loading.set(false);
      this.error.set('Atracción no disponible');
      return;
    }
    attractionId = this.data.attraction.id;
    description = this.form.value.description!;
    let startTime = this.form.value.startTime! + ':00';

    if(this.form.controls.isMaintenance.value){
      const newIncidence: IncidenceRequest = {
        attractionId,
        description,
        isMaintenance: true,
        startDate: new Date(this.form.value.startDate as string || ''),
        startTime,
        duration: Number(this.form.value.duration)
      };
      const create$ = this.is.Create(newIncidence);
      create$.subscribe({
      next: (_) => {
        this.loading.set(false);
        this.form.reset();
        this.success.set(true);
      },
      error: (err) => {
        this.error.set('Error al crear la incidencia, intente nuevamente');
        this.success.set(false);
        this.loading.set(false);
      }
    });
    } else {
      const newIncidence: IncidenceRequest = {
        attractionId,
        description,
        isMaintenance: false
      };
      const create$ = this.is.Create(newIncidence);
      create$.subscribe({
      next: (_) => {
        this.loading.set(false);
        this.form.reset();
        this.success.set(true);
      },
      error: (err) => {
        this.error.set(err.message || 'Error al crear la incidencia');
        this.success.set(false);
        this.loading.set(false);
      }
    });
    }
  }

  onCancel() {
    this.dialogRef.close();
  }

}
