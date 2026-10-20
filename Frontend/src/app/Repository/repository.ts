import { Observable } from "rxjs";
import { HttpClient } from '@angular/common/http';
import { environment } from "../../environments/environment.development";

export abstract class Repository<T> {
  public Endpoint: string;

  constructor(public http: HttpClient, private endpoint: string) {
    this.Endpoint = `${environment.apiBaseUrl}${endpoint}`;
  }

  public Get(id: number): Observable<T> {
    return this.http.get<T>(`${this.Endpoint}/${id}`);
  }

  public GetAll(): Observable<T[]> {
    return this.http.get<T[]>(this.Endpoint);
  }

  public Post<TRequest = Partial<T>>(data: TRequest): Observable<T> {
    return this.http.post<T>(this.Endpoint, data);
  }

  public Put<TRequest = Partial<T>>(id: number, data: TRequest): Observable<T> {
    return this.http.put<T>(`${this.Endpoint}/${id}`, data);
  }

  public Delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.Endpoint}/${id}`);
  }
}
