import { Component, inject, OnInit, signal } from '@angular/core';
import { SideNavBar } from "../side-nav-bar/side-nav-bar";
import { MatTableDataSource } from '@angular/material/table';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { SpecialEventService } from '../../Services/special-event.service';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { SpecialEventCreate } from '../special-event-create/special-event-create';
import { SpecialEvent } from '../../Models/special-event.model';

@Component({
  selector: 'app-special-events',
  standalone: true,
  imports: [
    SideNavBar,
    CommonModule,
    MatTableModule,
    MatProgressSpinner,
    MatIconModule,
    MatButtonModule,],
  templateUrl: './special-events.html',
  styleUrl: './special-events.css'
})
export class SpecialEvents implements OnInit {
  private dialog = inject(MatDialog);
  private service = inject(SpecialEventService);
  private route = inject(Router);
  public events: SpecialEvent[] = [];

  ngOnInit(): void {
    this.LoadData();
  }

  dataSource = new MatTableDataSource<SpecialEvent>([]);
  loading = signal(false);
  error = signal<string | null>(null);

  displayedColumns: string[] = [
    'name', 'date', 'startTime', 'endTime', 'maxCapacity', 'currentCapacity', 'additionalCost', 'includedAttractionIds', 'active'
  ];

  openSpecialEventDialog() {
     this.dialog.open(SpecialEventCreate, {
           width: '400px',
           disableClose: true,
         }).afterClosed().subscribe(() => {
           this.LoadData();
         });
  }

  LoadData() {
    this.loading.set(true);
    this.error.set(null);
    
    this.service.GetAll().subscribe({
      next: (res) => {
        this.dataSource.data = res;
        this.events = res;
        this.loading.set(false);
      },
      error: (_) => {
        this.loading.set(false);
        this.error.set('No fue posible obtener los eventos especiales');
      }
    });
  }
}
