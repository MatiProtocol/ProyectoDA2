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
import { VisitorService } from '../../Services/visitor.service';
import { VisitorModel } from '../../Models/visitor.model';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from '../../Services/auth.service';
import { ConfirmationDialog } from '../confirmation-dialog/confirmation.dialog';
import { SideNavBar } from "../side-nav-bar/side-nav-bar";
import { VisitorTickets } from '../visitor-tickets/visitor-tickets';

@Component({
  selector: 'app-visitor-list-menu',
  standalone: true,
  imports: [
    MatTableModule,
    MatIconModule,
    MatProgressSpinner,
    MatButtonModule,
    CommonModule,
    SideNavBar
],
  templateUrl: './visitor-list.menu.html',
  styleUrl: './visitor-list.menu.css'
})
export class VisitorListMenu implements OnInit {
  private VisitorService = inject(VisitorService);
  private AuthService = inject(AuthService);
  private dialog = inject(MatDialog);

  dataSource = new MatTableDataSource<VisitorModel>([]);
  loading = signal(false);
  error = signal<string | null>(null);
  errorMessage: string | null = null;

  ngOnInit() {
    this.LoadData();
  }

  displayedColumns: string[] = [
    'name',
    'birthdate',
    'email',
    'membership',
    'points',
    'actions'
  ];

  LoadData() {
    this.loading.set(true);
    this.error.set(null);

    this.VisitorService.GetAll().subscribe({
      next: (items: VisitorModel[]) => {
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

  GetNfc(id: number) {
    this.VisitorService.GetNfc(id).subscribe({
      next: (nfcData) => {
        alert("Código NFC: " + nfcData);
      },
      error: (_) => {
        alert("Error al obtener el código NFC: " + 'Error desconocido');
      }
    });
  }

  isRole(role: string): boolean {
    return this.AuthService.IsRole(role);
  }

  RemoveUser(id: number) {
    this.VisitorService.Delete(id).subscribe({
      next: () => {
        alert("Usuario removido exitosamente.");
        this.LoadData();
      },
      error: (_) => {
        alert("Error al remover el usuario: " + 'Error desconocido');
      }
    });
  }

  onConfirmRemoveDialog(id : number) {
    this.dialog.open(ConfirmationDialog, {
      data: {
        title: 'Confirmar eliminación',
        message: '¿Estás seguro de que deseas eliminar este usuario? \n Esta acción no se puede deshacer.'
      }
    }).afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
        this.RemoveUser(id);
      }
    });
  }

  onTickets(data: number) {
    this.dialog.open(VisitorTickets, {
      width: '50%',
      height: '70%',
      data: {
        visitorId: data
      }
    });
  }

}
