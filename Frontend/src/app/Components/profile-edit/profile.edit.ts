import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { VisitorService } from '../../Services/visitor.service';
import { WorkerService } from '../../Services/worker.service';
import { AuthService } from '../../Services/auth.service';
import { VisitorModel } from '../../Models/visitor.model';
import { WorkerModel } from '../../Models/worker.model';
import { WorkerRequest } from '../../Models/worker.request';


@Component({
  selector: 'app-profile-edit',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule
  ],
  templateUrl: './profile.edit.html',
  styleUrl: './profile.edit.css'
})
export class ProfileEdit {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<ProfileEdit>);
  public data = inject(MAT_DIALOG_DATA) as { profile?: VisitorModel | WorkerModel; userType?: string };
  private workerService = inject(WorkerService);
  private visitorService = inject(VisitorService);
  private as = inject(AuthService);
  public changedMail = false;
  public changedPassword = false;

  loading = false;
  error: string | null = null;
  success = false;

  userType = this.data?.userType ?? this.as.GetUserType();

  workerForm = this.fb.group({
    name: [this.data?.profile?.name ?? '', [Validators.required]],
    email: [this.data?.profile?.email ?? '', [Validators.required, Validators.email]],
    oldPassword: [ ''],
    newPassword: [ ''],
  });
  visitorForm = this.fb.group({
    name: [this.data?.profile?.name ?? '', [Validators.required]],
    email: [this.data?.profile?.email ?? '', [Validators.required, Validators.email]],
    birthDate: [(this.data?.profile as VisitorModel)?.birthDate ?? '', [Validators.required]],
    oldPassword: '',
    newPassword: '',
  });
  
  Submit() {
    if (this.userType === 'Worker') {
      this.WorkerEdit();
    } else {
      this.VisitorEdit();
    }
  }

  VisitorEdit() {
    if (this.visitorForm.invalid) {
      this.visitorForm.markAllAsTouched();
      return;
    }
    this.loading = true;

    const id = this.data?.profile?.id;
    if (!id) {
      this.loading = false;
      this.error = 'ID de visitante no disponible.';
      return;
    }

    const userEmail = this.as.GetEmail()!;
    this.visitorService.GetByEmail(userEmail).subscribe({
      next: (users) => {
        if (!users || users.length === 0) {
          this.loading = false;
          this.error = 'Usuario conectado no encontrado.';
          return;
        }

        this.changedMail = false;
        const connectedUser = users[0];
        
        const updateData: any = {
          name: this.visitorForm.value.name!,
          email: this.visitorForm.value.email!,
          birthDate: this.visitorForm.value.birthDate!
        };
        
        if (this.visitorForm.value.email !== connectedUser.email) this.changedMail = true;
        
        if (this.visitorForm.value.newPassword && this.visitorForm.value.oldPassword) {
          updateData.newPassword = this.visitorForm.value.newPassword;
          updateData.oldPassword = this.visitorForm.value.oldPassword;
          this.changedPassword = true;
        } else {
          this.changedPassword = false;
        }
        const edit$ = this.visitorService.Update(id, updateData);

        edit$.subscribe({
          next: (_) => {
            this.loading = false;
            this.success = true;
            if (this.changedMail || this.changedPassword) {
              setTimeout(() => {
                this.as.Logout();
                this.dialogRef.close(true);
              }, 1200);
            }
            setTimeout(() => {
              window.location.reload();
              this.dialogRef.close(false);
            }, 1200);
            this.error = null;
          },
          error: (err) => {
            this.loading = false;
            this.error = 'No fue posible actualizar el perfil';
          }
        });
      },
      error: (err) => {
        this.loading = false;
        this.error = 'Error al editar el perfil conectado';
      }
    });
  }

  WorkerEdit() {
    if (this.workerForm.invalid) {
      this.workerForm.markAllAsTouched();
      return;
    }
    this.loading = true;

    const id = this.data?.profile?.id;
    if (!id) {
      this.loading = false;
      this.error = 'ID de worker no disponible.';
      return;
    }
    const userEmail = this.as.GetEmail()!;
    this.workerService.GetByEmail(userEmail).subscribe({
      next: (users) => {
        if (!users || users.length === 0) {
          this.loading = false;
          this.error = 'Usuario conectado no encontrado.';
          return;
        }
        this.changedMail = false;
        const connectedUser = users[0];
        
        const updateData: WorkerRequest = {
          Name: this.workerForm.value.name!,
          Email: this.workerForm.value.email!,
          Roles: connectedUser.roles
        };
        
        if (this.workerForm.value.email !== connectedUser.email) this.changedMail = true;
        
        if (this.workerForm.value.newPassword && this.workerForm.value.oldPassword) {
          updateData.NewPassword = this.workerForm.value.newPassword;
          updateData.OldPassword = this.workerForm.value.oldPassword;
          this.changedPassword = true;
        } else {
          this.changedPassword = false;
        }
        
        const edit$ = this.workerService.Update(id, updateData);
        edit$.subscribe({
          next: (_) => {
          
            this.loading = false;
            this.success = true;
            if (this.changedMail || this.changedPassword) {
              setTimeout(() => {
                this.as.Logout();
                this.dialogRef.close(true);
              }, 1200);
            } else {
              setTimeout(() => {
                window.location.reload();
                this.dialogRef.close(false);
              }, 1200);
            }
            this.error = null;
          },
          error: (err) => {
            this.loading = false;
            this.error = 'No fue posible actualizar el perfil';
          }
        });
      },
      error: (err) => {
        this.loading = false;
        this.error = 'Error al obtener usuario conectado';
      }
    });
  }

  closeDialog() {
    this.dialogRef.close();
  }
}
