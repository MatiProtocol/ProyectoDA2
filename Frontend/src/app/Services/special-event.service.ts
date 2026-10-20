import { Injectable } from '@angular/core';
import { SpecialEventRepository } from '../Repository/special-event.repository';
import { Observable } from 'rxjs';
import { inject } from '@angular/core';
import { SpecialEvent } from '../Models/special-event.model';
import { SpecialEventRequest } from '../Models/special-event.request';

@Injectable({
  providedIn: 'root'
})
export class SpecialEventService {
  private repository = inject(SpecialEventRepository);

  public GetAll(): Observable<SpecialEvent[]> {
    return this.repository.GetAll();
  }

  public GetById(id: number): Observable<SpecialEvent> {
    return this.repository.Get(id);
  }

  public Create(data: SpecialEventRequest): Observable<SpecialEvent> {
    return this.repository.Post<SpecialEventRequest>(data);
  }

  public Update(id: number, data: Partial<SpecialEventRequest>): Observable<SpecialEvent> {
    return this.repository.Put<Partial<SpecialEventRequest>>(id, data);
  }

  public Delete(id: number): Observable<void> {
    return this.repository.Delete(id);
  }
  
  public GetActiveEvents(): Observable<SpecialEvent[]> {
    return this.repository.GetActiveEvents();
  }
}
