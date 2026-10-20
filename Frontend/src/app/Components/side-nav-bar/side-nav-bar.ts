import { Component} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSidenavModule} from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import { inject } from '@angular/core';
import { OnInit } from '@angular/core';
import { AuthService } from '../../Services/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { Settings } from '../settings-menu/settings';

@Component({
  selector: 'app-side-nav-bar',
  standalone: true,
  imports: [
    CommonModule,
    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatListModule,
    MatButtonModule
  ],
  templateUrl: './side-nav-bar.html',
  styleUrls: ['./side-nav-bar.css']
})
export class SideNavBar implements OnInit {

  private rout = inject(Router);
  private dialog = inject(MatDialog);
  private authService = inject(AuthService);

  menuItems = [
    { label: 'Inicio', icon: 'home', route: '/home' },
    { label: 'Perfil', icon: 'person', route: '/profile' },
  ];

  ngOnInit() {
  const userType = this.authService.GetUserType();
  const userRoles = this.authService.GetUserRoles();

  this.menuItems = [
    { label: 'Inicio', icon: 'home', route: '/home' },
    { label: 'Perfil', icon: 'person', route: '/profile' },
  ];

  if(userType === 'Visitor') {
    this.menuItems.push(
      { label: 'Tickets', icon: 'confirmation_number', route: '/tickets' },
      { label: 'Catálogo de Recompensas', icon: 'card_giftcard', route: '/rewards' },
      { label: 'Mis Recompensas', icon: 'redeem', route: '/my-rewards' },
      { label: 'Historial de Puntos', icon: 'history', route: '/points-history' }
    );
  }

  if (['ParkAdmin', 'Operator'].some(role => userRoles?.includes(role))) {
    this.menuItems.push(
      { label: 'Atracciones', icon: 'star', route: '/attractions/menu' },
      { label: 'Usuarios', icon: 'group', route: '/users/menu' }
    );
  }

  if(userRoles?.includes('ParkAdmin')) {
    this.menuItems.push(
      { label: 'Eventos Especiales', icon: 'festival', route: '/special-events' },
      { label: 'Ranking Diario', icon: 'leaderboard', route: '/daily-ranking' },
      { label: 'Trabajadores', icon: 'construction_worker', route: '/workers/menu' }
    );

    if (userType !== 'Visitor') {
      this.menuItems.push({ label: 'Gestión de Recompensas', icon: 'card_giftcard', route: '/rewards' });
    }
  }

  if(userRoles?.includes('Operator') && !userRoles?.includes('ParkAdmin') && userType !== 'Visitor') {
    this.menuItems.push({ label: 'Catálogo de Recompensas', icon: 'card_giftcard', route: '/rewards' });
  }
}

  onNavigate(item: any) {
    this.navigate(item.route);
  }

  onLogout() {
    this.authService.Logout();
    this.rout.navigate(['/login']);
  }

  navigate(route: string) {
    this.rout.navigate([route]);
  }

  onSettings() {
    this.dialog.open(Settings, {
      width: '400px',
      disableClose: false,
    });
  }
}
