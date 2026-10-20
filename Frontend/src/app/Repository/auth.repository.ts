import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import { AuthLoginResponse } from "../Models/auth-login.response";

@Injectable({
  providedIn: 'root'
})
export class AuthRepository {
  private endpoint = `${environment.apiBaseUrl}${environment.apiControllers.Auth}`;
  constructor(private http: HttpClient) {}

  public VisitorLogin(data: { email: string; password: string }): Observable<AuthLoginResponse> {
    return this.http.post<AuthLoginResponse>(`${this.endpoint}/Visitor/Login`, data);
  }

  public WorkerLogin(data: { email: string; password: string }): Observable<AuthLoginResponse> {
    return this.http.post<AuthLoginResponse>(`${this.endpoint}/Worker/Login`, data);
  }
}
