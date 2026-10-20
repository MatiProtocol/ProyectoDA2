import { Injectable } from '@angular/core';
import { AuthRepository } from '../Repository/auth.repository';
import { inject } from '@angular/core';
import { WorkerService } from './worker.service';
import { Observable } from 'rxjs';
import { tap, switchMap, map } from 'rxjs/operators';
import { AuthLoginResponse } from '../Models/auth-login.response';
import { WorkerModel } from '../Models/worker.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private repository = inject(AuthRepository);
  private workerService = inject(WorkerService);
  private tokenKey: string = 'auth_token';
  private emailKey: string = 'auth_email';
  private userTypeKey: string = 'auth_user_type';
  private userRolesKey: string = 'auth_user_roles';
  
  VisitorLogin(data: { email: string; password: string }) : Observable<AuthLoginResponse> {
    return this.repository.VisitorLogin(data).pipe(
      tap((res: AuthLoginResponse) => {
        this.SaveToken(res.token);
        this.SaveEmail(data.email);
        this.SaveUserType('Visitor');
        // 👇 NUEVO: Guarda 'Visitor' como rol también
        this.SaveUserRoles('Visitor');
      })
    );
  }

  WorkerLogin(data: { email: string; password: string }) : Observable<AuthLoginResponse> {
    return this.repository.WorkerLogin(data).pipe(
      tap((res) => {
        this.SaveToken(res.token);
        this.SaveEmail(data.email);
        this.SaveUserType('Worker');
      }),
      switchMap((res) => this.SaveRoles(data.email).pipe(map(() => res)))
    );
  }

  private SaveRoles(email: string) : Observable<WorkerModel[]> {
    return this.workerService.GetByEmail(email).pipe(
      tap((workerData: WorkerModel[]) => {
        const worker = workerData[0];
        this.SaveUserRoles(worker.roles);
      })
    );
  }
  
  GetUserRoles(): string | null {
    return localStorage.getItem(this.userRolesKey);
  }

  GetToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  GetUserType(): string | null {
    return localStorage.getItem(this.userTypeKey);
  }

  GetEmail(): string | null {
    return localStorage.getItem(this.emailKey);
  }

  Logout() {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.emailKey);
    localStorage.removeItem(this.userTypeKey);
    localStorage.removeItem(this.userRolesKey);
  }

  IsLoggedIn(): boolean {
    return this.GetToken() !== null;
  }

  IsRole(role: string): boolean {
    const roles = this.GetUserRoles();
    const userType = this.GetUserType();
    
    if (roles && roles.includes(role)) {
      return true;
    }
    
    if (userType === role) {
      return true;
    }
    
    return false;
  }
  
  private SaveToken(token: string) {
    localStorage.setItem(this.tokenKey, token);
  }

  private SaveEmail(email: string) {
    localStorage.setItem(this.emailKey, email);
  }

  private SaveUserType(userType: string) {
    localStorage.setItem(this.userTypeKey, userType);
  }

  private SaveUserRoles(userRoles: string) {
    localStorage.setItem(this.userRolesKey, userRoles);
  }
}