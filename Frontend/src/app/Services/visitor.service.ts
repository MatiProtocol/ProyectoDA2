import { Injectable } from '@angular/core';
import { IService } from './iservice';
import { Observable } from 'rxjs';
import { VisitorRepository } from '../Repository/visitor.repository';
import { inject } from '@angular/core';
import { VisitorModel } from '../Models/visitor.model';
import { VisitorNfcResponse } from '../Models/visitor-nfc.response';
import { AvailableTicketResponse } from '../Models/availabletTicket.response';
import { VisitorRequest } from '../Models/visitor.request';

@Injectable({
  providedIn: 'root'
})
export class VisitorService implements IService {
  private repository = inject(VisitorRepository);

  public GetById(id: number): Observable<VisitorModel> {
    return this.repository.Get(id);
  }

  public GetAll(): Observable<VisitorModel[]> {
    return this.repository.GetAll();
  }

  public Create(data: VisitorRequest): Observable<VisitorModel> {
    return this.repository.Post(data);
  }

  public Update(id: number, data: Partial<VisitorRequest>): Observable<VisitorModel> {
    return this.repository.Put(id, data);
  }

  public Delete(id: number): Observable<void> {
    return this.repository.Delete(id);
  }

  public GetByEmail(email: string): Observable<VisitorModel[]> {
    return this.repository.GetByEmail(email);
  }

  public GetNfc(id: number): Observable<VisitorNfcResponse> {
    return this.repository.GetVisitorNFC(id);
  }

  public GetAvailableTickets(id: number): Observable<AvailableTicketResponse[]> {
    return this.repository.GetAvailableTickets(id);
  }
}
