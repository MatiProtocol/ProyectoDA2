import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../Services/auth.service';

export const RoleGuard: CanActivateFn = (route) => {
  const router = inject(Router);
  const auth = inject(AuthService);
  const rolesString = auth.GetUserRoles();
  const requiredRoles = route.data['roles'];

  if (!rolesString) {
    router.navigate(['/unauthorized']);
    return false;
  }

  const roles = rolesString.split(',');
  const hasRole = requiredRoles.some((role: string) => roles.includes(role));
  if (!hasRole) {
    router.navigate(['/unauthorized']);
    return false;
  }

  return true;
};
