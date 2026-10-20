import { Observable } from "rxjs";

export interface IService {
  GetAll(): Observable<unknown[]>;
  GetById(id: number): Observable<unknown>;
  Create(data: unknown): Observable<unknown>;
  Update(id: number, data: unknown): Observable<unknown>;
  Delete(id: number): Observable<unknown>;
}