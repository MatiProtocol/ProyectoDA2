import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DateTimeService } from '../../Services/date-time.service';
import { DateTimeResponse } from '../../Models/date-time.response';
import { MatCardModule } from "@angular/material/card";


@Component({
  selector: 'app-datetime-editor',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    MatCardModule
],
  templateUrl: './datetime.editor.html',
  styleUrl: './datetime.editor.css'
})
export class DatetimeEditor implements OnInit {
  private dialogRef = inject(MatDialogRef<DatetimeEditor>);
  private dateTimeService = inject(DateTimeService);

  Date: Date = new Date();
  Time: string = '';
  loading = signal(false);
  error =  signal<string | null>(null);
  success = signal<string | null>(null);

  ngOnInit(): void {
    this.loadCurrentDateTime();
  }

  loadCurrentDateTime(): void {
    this.loading.set(true);
    this.error.set(null);

    this.dateTimeService.GetDateTime().subscribe({
      next: (response: DateTimeResponse | any) => {
        let dateTimeStr: string;
        
        if (typeof response === 'string') {
          dateTimeStr = response;
        } else if (response.DateTime) {
          dateTimeStr = response.DateTime;
        } else if (response.dateTime) {
          dateTimeStr = response.dateTime;
        } else {
          console.error('Invalid response format:', response);
          this.error.set('Respuesta inválida del servidor');
          this.loading.set(false);
          return;
        }
        
        const dt = new Date(dateTimeStr);
        this.Date = dt;
        this.Time = `${dt.getHours().toString().padStart(2, '0')}:${dt.getMinutes().toString().padStart(2, '0')}`;
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error loading datetime:', err);
        this.error.set('Error al cargar la fecha y hora actual');
        this.loading.set(false);
      }
    });
  }

  onConfirm(): void {
    const [hours, minutes] = this.Time.split(':').map(Number);
    const combinedDateTime = new Date(this.Date);
    combinedDateTime.setHours(hours, minutes, 0, 0);

    this.loading.set(true);
    this.error.set(null);

    this.dateTimeService.SetDateTime({ dateTime: combinedDateTime.toISOString() }).subscribe({
      next: (response) => {
        this.loading.set(false);
        this.success.set('Fecha y hora actualizadas correctamente');
        setTimeout(() => {
          this.dialogRef.close(response);
        }, 1200);
      },
      error: (err) => {
        console.error('Error setting datetime:', err);
        this.error.set('Error al actualizar la fecha y hora');
        this.loading.set(false);
      }
    });
  }
}
