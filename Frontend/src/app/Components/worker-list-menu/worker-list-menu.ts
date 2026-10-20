import {
  Component,
  inject,
  OnInit,
  signal
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { WorkerService } from '../../Services/worker.service';
import { VisitorModel } from '../../Models/visitor.model';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialog } from '../confirmation-dialog/confirmation.dialog';
import { SideNavBar } from "../side-nav-bar/side-nav-bar";
import { WorkerModel } from '../../Models/worker.model';
import { ConfigWorker } from '../config-worker/config-worker';
import { AuthService } from '../../Services/auth.service';
import { RegisterWorker } from '../register-worker/register-worker';

@Component({
  selector: 'app-worker-list-menu',
  standalone: true,
  imports: [
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    MatProgressSpinner,
    CommonModule,
    SideNavBar,

  ],
  templateUrl: './worker-list-menu.html',
  styleUrl: './worker-list-menu.css'
})
export class WorkerListMenu {
  private WorkerService = inject(WorkerService);
  private dialog = inject(MatDialog);
  private AuthService = inject(AuthService);

  dataSource = new MatTableDataSource<WorkerModel>([]);
  loading = signal(false);
  error = signal<string | null>(null);
  errorMessage: string | null = null;

  displayedColumns: string[] = [
    'name',
    'email',
    'roles',
    'actions'
  ];

  ngOnInit() {
    this.LoadData();
  }

  LoadData() {
    this.loading.set(true);
    this.error.set(null);

    this.WorkerService.GetAll().subscribe({
      next: (items: WorkerModel[]) => {
        this.dataSource.data = items;
      },
      error: (err) => {
        this.error.set(err?.message || 'No se pudieron cargar las atracciones');
        this.loading.set(false);
      },
      complete: () => {
        this.loading.set(false);
      }
    });
  }
  isRole(role: string): boolean {
    return this.AuthService.IsRole(role);
  }

  isCurrentUser(email: string): boolean {
    return this.AuthService.GetEmail() === email ? true : false;      
  }
  openWorkerDialog() {
    this.dialog.open(RegisterWorker, {
      width: '800px',
      height: 'auto'
    
    }).afterClosed().subscribe(() => {
      this.LoadData();
    });
  }
  configWorkerDialog(id: number) {
    this.dialog.open(ConfigWorker, {
    width: '400px',
      height: '260px',
      data: {
        workerId: id
      }
    }).afterClosed().subscribe(() => {
      this.LoadData();
    });
  };

  deleteWorker(id: number) {
    const dialogRef = this.dialog.open(ConfirmationDialog, {
      width: '300px',
      data: {
        title: 'Confirmar eliminación',
        message: '¿Estás seguro de que deseas eliminar este trabajador?'
      }
    }).afterClosed().subscribe(result => {
      if (result) {
        this.WorkerService.Delete(id).subscribe({
          next: () => {
            this.LoadData();
          },
          error: (err) => {
            this.error.set(err?.message || 'No se pudo eliminar el trabajador');
          }
        });
      }
    });
  }
}
