import { 
  Component,
  inject, 
  OnInit, 
  signal
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AttractionsService } from '../../Services/attractions.service';
import { MatDialog } from '@angular/material/dialog';
import { AttractionEdit } from '../attraction-edit/attraction.edit';
import { AttractionModel } from '../../Models/attraction.model'
import { AuthService } from '../../Services/auth.service';
import { AttractionAccess } from '../attraction-access/attraction.access';
import { IncidenceCreate } from '../incidence-create/incidence-create';
import { IncidenceList } from '../incidence-list/incidence-list';

@Component({
  selector: 'app-attraction-list-menu',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './attraction-list.menu.html',
  styleUrl: './attraction-list.menu.css'
})
export class AttractionListMenu implements OnInit {
  private AttractionService = inject(AttractionsService);
  private authService = inject(AuthService);

  isOperator = this.authService.IsRole('Operator');
  isParkAdmin = this.authService.IsRole('ParkAdmin');

  displayedColumns: string[] = [
    'name', 
    'type', 
    'minAge', 
    'maxCapacity', 
    'currentCapacity',
    'actions'
  ];
  dataSource = new MatTableDataSource<AttractionModel>([]);

  loading = signal(true);
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.LoadData();
  }

  dialog = inject(MatDialog);

  onEditDialog(id: number) {
    let attraction = this.dataSource.data.find(a => a.id === id);
    if(!attraction) return;
    this.dialog.open(AttractionEdit, {
      width: '400px',
      disableClose: false,
      data: {
        attraction: attraction
      }
    }).afterClosed().subscribe(() => {
      this.LoadData();
    });
  }

  onAccessDialog(int: number) {
    this.dialog.open(AttractionAccess, {
      width: '400px',
      disableClose: false,
      data: {
        id: int
      }
    }).afterClosed().subscribe(() => {
      this.LoadData();
    });
  }

  onIncidenceListDialog(id: number) {
    let attraction = this.dataSource.data.find(a => a.id === id);
    if(!attraction) return;
    this.dialog.open(IncidenceList, {
      width: '500px',
      disableClose: false,
      data: {
        attraction: attraction
      }
    }).afterClosed().subscribe(() => {
      this.LoadData();
    });
  }

  LoadData(): void {
    this.loading.set(true);
    this.error.set(null);

    this.AttractionService.GetAll().subscribe({
      next: (items) => {
        this.dataSource.data = this.mapAttractions(items);
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

  mapAttractions(items: any[]): AttractionModel[] {
    return items.map(item => this.mapAttraction(item));
  }
  
  mapAttraction(item: any): AttractionModel {
    return {
      id: item.id,
      name: item.name,
      description: item.description,
      type: item.type,
      minAge: item.minAge ?? 0,
      maxCapacity: item.maxCapacity ?? 0,
      currentCapacity: item.currentCapacity ?? 0
    };
  }

  Delete(id: number) {
    if (this.loading()) return;
    const confirmed = window.confirm('¿Seguro que deseas eliminar esta atracción?');
    if (!confirmed) return;

    this.AttractionService.Delete(id).subscribe({
      next: () => {
        this.dataSource.data = this.dataSource.data.filter(a => a.id !== id);
      },
      error: (err) => {
        this.error.set(err?.message || 'No se pudo eliminar la atracción');
      }
    });
  }

  onExitAttraction(id: number, currentCapacity: number) {
    for (let i = 0; i < currentCapacity; i++) {
      this.AttractionService.ExitAttraction(id).subscribe({
        next: () => {
        },
        error: (err) => {
          this.error.set(err?.message || 'No se pudo registrar la salida de la atracción');
        }
      }); 
    }
    this.LoadData();
  }
}
