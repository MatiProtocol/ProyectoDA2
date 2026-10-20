import { Injectable } from '@angular/core';
import { IService } from './iservice';
import { Observable } from 'rxjs';
import { IncidenceRepository } from '../Repository/incidence.repository';
import { inject } from '@angular/core';
import { IncidenceModel } from '../Models/incidence.model';
import { IncidenceRequest } from '../Models/incidence.request';

@Injectable({
  providedIn: 'root'
})
export class IncidenceService implements IService {
  private repository = inject(IncidenceRepository);
  
  GetAll(): Observable<IncidenceModel[]> {
    return this.repository.GetAll();
  }
  GetById(id: number): Observable<IncidenceModel> {
    return this.repository.Get(id);
  }
  
  GetByAttractionId(attractionId: number): Observable<IncidenceModel[]> {
    return this.repository.GetByAttractionId(attractionId);
  }

  Create(data: IncidenceRequest): Observable<IncidenceModel> {
    return this.repository.Post<IncidenceRequest>(data);
  }
  Update(id: number, data: Partial<IncidenceRequest>): Observable<IncidenceModel> {
    return this.repository.Put<Partial<IncidenceRequest>>(id, data);
  }
  Delete(id: number): Observable<void> {
    return this.repository.Delete(id);
  }

  GetActiveIncidents(): Observable<IncidenceModel[]> {
    return this.repository.GetActiveIncidences();
  }
  
  DeactivateIncidence(id: number): Observable<IncidenceModel> {
    return this.repository.DeactivateIncidence(id);
  }
}
