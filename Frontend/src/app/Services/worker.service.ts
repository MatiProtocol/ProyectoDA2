import { Injectable } from '@angular/core';
import { IService } from './iservice';
import { Observable } from 'rxjs';
import { WorkerRepository } from '../Repository/worker.repository';
import { inject } from '@angular/core';
import { WorkerModel } from '../Models/worker.model';
import { WorkerRequest } from '../Models/worker.request';

@Injectable({
  providedIn: 'root'
})
export class WorkerService implements IService {
  private repository = inject(WorkerRepository);

  public GetAll(): Observable<WorkerModel[]> {
    return this.repository.GetAll();
  }

  public GetById(id: number): Observable<WorkerModel> {
    return this.repository.Get(id);
  }

  public Create(data: WorkerRequest): Observable<WorkerModel> {
    return this.repository.Post<WorkerRequest>(data);
  }

  public Update(id: number, data: Partial<WorkerRequest>): Observable<WorkerModel> {
    return this.repository.Put<Partial<WorkerRequest>>(id, data);
  }

  public Delete(id: number): Observable<void> {
    return this.repository.Delete(id);
  }

  public GetByEmail(email: string): Observable<WorkerModel[]> {
    return this.repository.GetByEmail(email);
  }
}
