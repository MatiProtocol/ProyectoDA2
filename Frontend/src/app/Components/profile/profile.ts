import { Component, inject, OnInit, Pipe } from '@angular/core';
import { SideNavBar } from '../side-nav-bar/side-nav-bar';
import { CommonModule } from '@angular/common';
import { VisitorService } from '../../Services/visitor.service';
import { WorkerService } from '../../Services/worker.service';
import { MatCardModule } from '@angular/material/card';
import { WorkerModel } from '../../Models/worker.model';
import { VisitorModel } from '../../Models/visitor.model';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { AuthService } from '../../Services/auth.service';
import { MatDivider } from '@angular/material/divider';
import { MatList, MatListItem } from '@angular/material/list';
import {MatButtonModule} from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { ProfileEdit } from '../profile-edit/profile.edit';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    SideNavBar, 
    CommonModule, 
    MatCardModule, 
    MatProgressSpinner, 
    MatDivider,
    MatList,
    MatListItem,
    MatButtonModule,
  ],

  templateUrl: './profile.html',
  styleUrls: ['./profile.css']
})

export class Profile implements OnInit {
  private visitorService = inject(VisitorService);
  private workerService = inject(WorkerService);
  private authService = inject(AuthService);
  dialog = inject(MatDialog);
  visitorProfile: VisitorModel | null = null;
  workerProfile: WorkerModel | null = null;
  route = inject(Router);

  loading = true;
  error: string | null = null;
 

  ngOnInit() {
    const userType = this.authService.GetUserType();
    const userMailStorage = this.authService.GetEmail();

    if (userType === 'Worker' ) {
      const user$ = this.workerService.GetByEmail(userMailStorage!);
      user$.subscribe({
        next: (data) => {
          this.workerProfile = data[0];
        },
        error: (err) => {
          this.error = 'Error al cargar el perfil';
        },
        complete: () => {
          this.loading = false;
        }
      });
    } else if (userType === 'Visitor' ) {      
      const user$ = this.visitorService.GetByEmail(userMailStorage!);
      user$.subscribe({
        next: (data) => {
          this.visitorProfile = data[0];
        },
        error: (err) => {
          this.error = 'Error al cargar el perfil';
        },
        complete: () => {
          this.loading = false;
        }
      });
    } else{
      this.error = 'Usuario no autenticado';
      this.loading = false;
    }
  }
  
  LoadData(): void {
    this.loading = true;
    this.error = null;
    this.ngOnInit();
  }

  openEditModal(userType: string, data: any) {
    if (data.birthDate) {
        const date = new Date(data.birthDate);
        data.birthDate = date.toISOString().split('T')[0];
    }
    this.dialog.open(ProfileEdit, {
      width: '400px',
      data: {
        profile: data,
        userType: userType
      }
    }).afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
        this.route.navigate(['/login']);
      }
    });
  }
}
