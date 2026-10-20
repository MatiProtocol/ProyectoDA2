import { Component, inject, signal} from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AttractionsService } from '../../Services/attractions.service';
import { MatIconModule } from '@angular/material/icon';
import { AttractionRequest } from '../../Models/attraction.request';


@Component({
  selector: 'app-attraction-create',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    MatDialogModule,
    MatIconModule,
    ReactiveFormsModule,
  ],
  templateUrl: './attraction.create.html',
  styleUrls: ['./attraction.create.css']
})
export class AttractionCreate {
  private fb = inject(FormBuilder);
  private as = inject(AttractionsService);
  public dialogRef = inject(MatDialogRef<AttractionCreate>);
  
  loading = signal(false);
  error = signal<string | null>(null);
  success = signal(false);

  form = this.fb.group({
    name: ['', [Validators.required]],
    description: ['', [Validators.required]],
    type: ['', [Validators.required]],
    minAge: [0, [Validators.required]],
    maxCapacity: [0, [Validators.required]],
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
      currentCapacity: 0
    };

    const create$ = this.as.Create(request);

    create$.subscribe({
      next: (_) => {
        this.loading.set(false);
        this.form.reset();
        this.success.set(true);
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
