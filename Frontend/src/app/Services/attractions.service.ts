import { Injectable } from '@angular/core';
import { AttractionsRepository } from '../Repository/attractions.repository'
import { IService } from './iservice';
import { Observable } from 'rxjs';
import { inject } from '@angular/core';
import { IncidenceModel } from '../Models/incidence.model';
import { AttractionAccessRequest, AttractionAccessResponse } from '../Models/attraction-access.response';
import { AttractionExitsResponse } from '../Models/attraction-exits.response';
import { VisitorsBetweenResponse } from '../Models/visit-entry.response';
import { AttractionModel } from '../Models/attraction.model';
import { AttractionRequest } from '../Models/attraction.request';
import { IncidenceRequest } from '../Models/incidence.request';

@Injectable({
  providedIn: 'root'
})
export class AttractionsService implements IService {
  private repository = inject(AttractionsRepository);

  public GetById(id: number): Observable<AttractionModel> {
    return this.repository.Get(id);
  }

  public GetAll(): Observable<AttractionModel[]> {
    return this.repository.GetAll();
  }

  public Create(data: AttractionRequest): Observable<AttractionModel> {
    return this.repository.Post<AttractionRequest>(data);
  }

  public Update(id: number, data: Partial<AttractionRequest>): Observable<AttractionModel> {
    return this.repository.Put<Partial<AttractionRequest>>(id, data);
  }

  public Delete(id: number): Observable<void> {
    return this.repository.Delete(id);
  }

  public ValidateAttractionAccess(id: number, data: AttractionAccessRequest): Observable<AttractionAccessResponse> {
    return this.repository.ValidateAttractionAccess(id, data);
  }

  public AttractionExits(id: number): Observable<AttractionExitsResponse> {
    return this.repository.AttractionExits(id);
  }

  public RegisterIncident(attractionId: number, data: IncidenceRequest): Observable<IncidenceModel> {
    return this.repository.RegisterIncident(attractionId, data);
  }

  public GetVisitsBetweenDates(attractionId: number, startDate: string, endDate: string): Observable<VisitorsBetweenResponse> {
    return this.repository.GetVisitorsBetween(attractionId, startDate, endDate);
  }

  public ExitAttraction(attractionId: number): Observable<AttractionExitsResponse> {
    return this.repository.ExitAttraction(attractionId);
  }
}
