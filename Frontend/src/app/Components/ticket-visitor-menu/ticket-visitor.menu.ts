import { Component, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Observable, map, startWith } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { SpecialEvent } from '../../Models/special-event.model';
import { SpecialEventService } from '../../Services/special-event.service';
import { SideNavBar } from '../side-nav-bar/side-nav-bar';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { TicketService } from '../../Services/ticket.service';
import { MatButton } from '@angular/material/button';
import { VisitorService } from '../../Services/visitor.service';
import { VisitorModel } from '../../Models/visitor.model';
import { AuthService } from '../../Services/auth.service';
import { TicketBuyRequest } from '../../Models/ticket-buy.request';

@Component({
  selector: 'app-ticket-menu',
  standalone: true,
  templateUrl: './ticket-visitor.menu.html',
  styleUrl: './ticket-visitor.menu.css',
  imports: [
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    AsyncPipe,
    SideNavBar,
    MatCardModule,
    MatProgressSpinner,
    MatButton,
  ],
})
export class TicketVisitorMenu implements OnInit {
  private SpecialEventService = inject(SpecialEventService);
  private TicketService = inject(TicketService);
  private VisitorService = inject(VisitorService);
  private AuthService = inject(AuthService);

  control = new FormControl<SpecialEvent | null>(null);
  options: SpecialEvent[] = [];
  filteredOptions: Observable<SpecialEvent[]> | undefined;
  error = signal<string | null>(null);
  loading = signal(false);
  success = signal<string | null>(null);

  specialEvents: SpecialEvent[] = [];

  ngOnInit() {
    this.loading.set(true);
    this.LoadSpecialEvents();
  }

  private LoadSpecialEvents() {
    this.SpecialEventService.GetAll().subscribe({
      next: (res: SpecialEvent[]) => {
        this.specialEvents = res;
        this.options = res;
        this.FilterOptions();
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set(err.message || "Error al cargar los eventos");
        this.loading.set(false);
      }
    });
  }

  private FilterOptions() {
    this.filteredOptions = this.control.valueChanges.pipe(
      startWith(''),
      map(value => {
        const name = typeof value === 'string' ? value : value?.name;
        return name ? this.Filter(name as string) : this.options.slice();
      }),
    );
  }
  
  private Filter(name: string): SpecialEvent[] {
    const filterValue = name.toLowerCase();

    return this.options.filter(option => option.name.toLowerCase().includes(filterValue));
  }

  displayFn(event: SpecialEvent): string {
    return event && event.name ? event.name : '';
  }


  public BuyTicket() {
    const visitorEmail = this.AuthService.GetEmail();
    this.loading.set(true);
    this.error.set(null);
    this.success.set(null);

    if (this.control.value !== null) {
      this.VisitorService.GetByEmail(visitorEmail!).subscribe({
        next: (res: VisitorModel[]) => {
          const visitorId = res[0].id;
          const request: TicketBuyRequest = { visitorId, eventId: this.control.value?.id || 0 };
          this.TicketService.Create(request).subscribe({
            next: (_) => {
              this.success.set("Ticket comprado con exito");
              this.loading.set(false);
            },
            error: (err) => {
              this.error.set(err.message || "Error al comprar el ticket");
              this.loading.set(false);
            }
          });
        }
      });
      return;
    }

    this.VisitorService.GetByEmail(visitorEmail!).subscribe({
      next: (res: VisitorModel[]) => {
        const visitorId = res[0].id;
        const request: TicketBuyRequest = { visitorId, eventId: 0 };
        this.TicketService.Create(request).subscribe({
          next: (_) => {
            this.success.set("Ticket comprado con exito");
            this.loading.set(false);
          },
          error: (err) => {
            this.error.set(err.message || "Error al comprar el ticket");
            this.loading.set(false);
          }
        });
      },
      error: (err) => {
        this.error.set(err.message || "Error al comprar el ticket");
      }
    });
  }
}