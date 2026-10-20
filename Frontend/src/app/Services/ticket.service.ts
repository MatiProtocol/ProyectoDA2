import { Injectable } from '@angular/core';
import { IService } from './iservice';
import { Observable } from 'rxjs';
import { inject } from '@angular/core';
import { TicketRepository } from '../Repository/ticket.repository';
import { TicketModel } from '../Models/ticket.model';
import { TicketBuyRequest } from '../Models/ticket-buy.request';

@Injectable({
  providedIn: 'root'
})
export class TicketService implements IService {
  private repository = inject(TicketRepository);

  public GetById(id: number): Observable<TicketModel> {
    return this.repository.Get(id);
  }

  public GetAll(): Observable<TicketModel[]> {
    return this.repository.GetAll();
  }

  public Create(data: TicketBuyRequest): Observable<TicketModel> {
    return this.repository.Post<TicketBuyRequest>(data);
  }

  public Update(id: number, data: Partial<TicketModel>): Observable<TicketModel> {
    return this.repository.Put(id, data);
  }

  public Delete(id: number): Observable<void> {
    return this.repository.Delete(id);
  }

  public GetTicketByQR(qr: string): Observable<TicketModel> {
    return this.repository.GetTicketByQR(qr);
  }
}
