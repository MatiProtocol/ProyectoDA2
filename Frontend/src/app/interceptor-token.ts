import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './Services/auth.service';

const skipVisitorCreate = (url: string) => /\/Visitors\/Create(\/|$)/i.test(url);
const skipWorkerCreate = (url: string) => /\/Workers\/Create(\/|$)/i.test(url);

export const TokenInterceptor: HttpInterceptorFn = (req, next) => {
  if (skipVisitorCreate(req.url)) return next(req);
  if (skipWorkerCreate(req.url)) return next(req);

  const authService = inject(AuthService);
  const token = authService.GetToken();
  if (!token) return next(req);

  const authReq = req.clone({
    setHeaders: { Authorization: `Bearer ${token}` },
  });
  return next(authReq);
};