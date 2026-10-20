import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment.development";
import { Observable } from "rxjs";
import { DateTimeResponse } from "../Models/date-time.response";

@Injectable({
  providedIn: 'root'
})
export class DateTimeRepository {
  private endpoint = `${environment.apiBaseUrl}${environment.apiControllers.DateTime}`;
  constructor(private http: HttpClient) {}

  public SetDateTime(data : { dateTime: string }): Observable<DateTimeResponse> {
    return this.http.post<DateTimeResponse>(`${this.endpoint}`, data);
  }

  public GetDateTime(): Observable<DateTimeResponse> {
    return this.http.get<DateTimeResponse>(`${this.endpoint}`);
  }
}
