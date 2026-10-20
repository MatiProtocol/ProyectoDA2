import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../Services/auth.service';
import { inject } from '@angular/core';

export const AuthGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if (auth.IsLoggedIn()) return true;
 
  router.navigate(['/login']);
  return false;
};