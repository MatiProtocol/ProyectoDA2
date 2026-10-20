import { Component, inject, signal} from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { VisitorService } from '../../Services/visitor.service'; 
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatInputModule, MatDialogModule, ReactiveFormsModule, MatIconModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  private fb = inject(FormBuilder);
  private as = inject(VisitorService);
  public dialogRef = inject(MatDialogRef<Register>);

  loading = signal(false);
  error = signal<string | null>(null);
  success = signal(false);

  form = this.fb.group({
    name: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    birthdate: ['', [Validators.required]],
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

    const { name, email, birthdate, newPassword } = this.form.value as {
      name: string;
      email: string;
      birthdate: string;
      newPassword: string;
    };
    this.as.Create({ name, email, birthdate: new Date(birthdate), oldPassword: undefined, newPassword, }).subscribe({
      next: () => {
        this.success.set(true);
        this.loading.set(false);
        this.error.set(null);
      },
      error: (err) => {
        this.success.set(false);
        this.error.set("Error al registrar el visitante");
        this.loading.set(false);
      }
    });
  }

  onCancel() {
    this.dialogRef.close();
  }
}