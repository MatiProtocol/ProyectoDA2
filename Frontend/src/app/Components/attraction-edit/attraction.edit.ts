import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AttractionsService } from '../../Services/attractions.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AttractionModel } from '../../Models/attraction.model';
import { AttractionRequest } from '../../Models/attraction.request';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-attraction-edit',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatIconModule
  ],
  templateUrl: './attraction.edit.html',
  styleUrl: './attraction.edit.css'
})

export class AttractionEdit {
  private fb = inject(FormBuilder);
  private as = inject(AttractionsService);
  private dialogRef = inject(MatDialogRef<AttractionEdit>);
  private data = inject(MAT_DIALOG_DATA) as { attraction: AttractionModel };
  private currentCapacity = this.data.attraction.currentCapacity || 0;

  loading = signal(false);
  error = signal<string | null>(null);
  success = signal(false);

  form = this.fb.group({
    name: [this.data.attraction.name, [Validators.required]],
    description: [this.data.attraction.description, [Validators.required]],
    type: [this.data.attraction.type, [Validators.required]],
    minAge: [this.data.attraction.minAge, [Validators.required]],
    maxCapacity: [this.data.attraction.maxCapacity, [Validators.required]],
  });

  Submit() {
    if (this.form.invalid || this.loading()) return;
    this.loading.set(true);

    const { name, description, type, minAge, maxCapacity } = this.form.value as {
      name: string;
      description: string;
      type: string;
      minAge: number;
      maxCapacity: number;
    };

    const request: AttractionRequest = {
      name,
      description,
      type,
      minAge,
      maxCapacity,
      currentCapacity: this.currentCapacity
    };

    const edit$ = this.as.Update(this.data.attraction.id, request);

    edit$.subscribe({
      next: (_) => {
        this.loading.set(false);
        this.success.set(true);
        this.error.set(null);
      },
      error: (err) => {
        this.error.set(err.message || 'Error al crear la atracción');
        this.success.set(false);
        this.loading.set(false);
      }
    });
  }
  
  onCancel() {
    this.dialogRef.close();
  }
}
