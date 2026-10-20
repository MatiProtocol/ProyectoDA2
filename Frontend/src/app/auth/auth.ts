import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../Services/auth.service';
import { MatCardModule } from '@angular/material/card';
import { MatRadioModule } from '@angular/material/radio';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import { MatDivider } from "@angular/material/divider";
import { MatDialog } from '@angular/material/dialog';
import { Register } from '../Components/register/register';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatRadioModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDivider
],
  templateUrl: './auth.html',
  styleUrls: ['./auth.css']
})
export class Auth {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  dialog = inject(MatDialog);

  loading = signal(false);
  error = signal<string | null>(null);

  form = this.fb.group({
    role: ['visitor', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
  });

  navigateToRegister() {
    this.dialog.open(Register, {
      width: '800px',
      disableClose: true,
    });
  }

  Submit() {
    if (this.form.invalid || this.loading()) return;
    this.error.set(null);
    this.loading.set(true);

    const { role, email, password } = this.form.value as {
      role: 'visitor' | 'worker';
      email: string;
      password: string;
    };

    const login$ =
      role === 'worker'
        ? this.auth.WorkerLogin({ email, password })
        : this.auth.VisitorLogin({ email, password });

    this.loading.set(true);

    login$.subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/home']);
      },
      error: (_) => {
        this.error.set('Credenciales inválidas o error del servidor.');
        this.loading.set(false);
      }
    });
  }
}
