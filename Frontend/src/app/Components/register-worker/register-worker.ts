import { Component, inject, signal} from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { VisitorService } from '../../Services/visitor.service'; 
import { MatIconModule } from '@angular/material/icon';
import { WorkerService } from '../../Services/worker.service';
import { WorkerRequest } from '../../Models/worker.request';
import { MatSelectModule } from '@angular/material/select';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelect } from '@angular/material/select';

@Component({
  selector: 'app-register-worker',
  standalone: true,
  imports: [
    MatCardModule,
    CommonModule,
    MatButtonModule,
    MatInputModule,
    MatDialogModule,
    ReactiveFormsModule,
    MatIconModule,
    MatSelect,
    MatFormFieldModule,
    MatSelectModule,
    FormsModule
  ],
  templateUrl: './register-worker.html',
  styleUrl: './register-worker.css'
})
export class RegisterWorker {
  private fb = inject(FormBuilder);
  private ws = inject(WorkerService);
  public dialogRef = inject(MatDialogRef<RegisterWorker>);

  loading = signal(false);
  error = signal<string | null>(null);
  success = signal(false);

  form = this.fb.group({
    name: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    roles: ['', [Validators.required]],
    newPassword: ['', [Validators.required]],
    passwordConfirmation: ['', [Validators.required]],
  });

  Submit() {
    if (this.form.invalid) return;
    this.loading.set(true);
    if(this.form.value.newPassword !== this.form.value.passwordConfirmation){
      this.error.set("Las contraseñas no coinciden");
      this.loading.set(false);
      return;
    }

    const { name, email, roles, newPassword } = this.form.value as {
      name: string;
      email: string;
      roles: string;
      newPassword: string;
    };
    const workerRequest: WorkerRequest = {
      Name: name,
      Email: email,
      Roles: roles,
      OldPassword: null,
      NewPassword: newPassword
    };
    console.log('Creating worker with payload:', workerRequest);
    this.ws.Create(workerRequest).subscribe({
      next: () => {
        this.success.set(true);
        this.loading.set(false);
        this.error.set(null);
      },
      error: (err) => {
        this.success.set(false);
        this.error.set("Error al registrar al trabajador");
        this.loading.set(false);
      }
    });
  }

  onCancel() {
    this.dialogRef.close();
  }
}
