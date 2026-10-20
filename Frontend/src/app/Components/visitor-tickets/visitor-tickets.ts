import { Component, inject, OnInit, signal} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatCardModule } from "@angular/material/card";
import { VisitorService } from '../../Services/visitor.service';
import { AvailableTicketResponse } from '../../Models/availabletTicket.response';
import { MatListModule } from '@angular/material/list';
import { DatePipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-visitor-tickets',
  standalone: true,
  imports: [
    MatDialogModule,
    MatCardModule,
    MatListModule,
    DatePipe,
    MatButtonModule
],
  templateUrl: './visitor-tickets.html',
  styleUrl: './visitor-tickets.css'
})
export class VisitorTickets implements OnInit {
  private data = inject(MAT_DIALOG_DATA) as { visitorId: number };
  private visitorService = inject(VisitorService);

  loading = signal(false);
  error = signal<string | null>(null);
  tickets: AvailableTicketResponse[] = [];

  ngOnInit() {
    this.LoadData();
  }

  LoadData() {
    this.loading.set(true);
    this.error.set(null);
    this.visitorService.GetAvailableTickets(this.data.visitorId).subscribe({
      next: (res: AvailableTicketResponse[]) => {
        this.tickets = res;
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Error al cargar los tickets disponibles.');
        this.loading.set(false);
      }
    });
  }
}
