import { Routes } from '@angular/router';
import { Home } from './Components/home/home';
import { Auth } from './auth/auth';
import { PunctuationService } from './Services/punctuation.service';
import { AttractionMenu } from './Components/attraction-menu/attraction.menu';
import { Profile } from './Components/profile/profile';
import { AuthGuard } from './guards/auth-guard';
import { RoleGuard } from './guards/role-guard';
import { Unauthorized } from './Components/unauthorized/unauthorized';
import { TicketVisitorMenu } from './Components/ticket-visitor-menu/ticket-visitor.menu';
import { VisitorListMenu } from './Components/visitor-list-menu/visitor-list.menu';
import { DailyRanking } from './Components/daily-ranking/daily-ranking';
import { SpecialEvents } from './Components/special-events/special-events';
import { RewardListMenu } from './Components/reward-list-menu/reward-list-menu';
import { MyRewards } from './Components/my-rewards/my-rewards';
import { Register } from './Components/register/register';
import { WorkerListMenu } from './Components/worker-list-menu/worker-list-menu';
import { PointsHistoryList } from './Components/points-history-list/points-history-list';

export const routes: Routes = [
  { path: 'login', component: Auth },
  { path: 'home', component: Home, canActivate: [AuthGuard] },
  { path: 'attractions/menu', component: AttractionMenu, canActivate: [AuthGuard, RoleGuard], data: { roles: ['ParkAdmin', 'Operator'] } },
  { path: 'profile', component: Profile, canActivate: [AuthGuard] },
  { path: 'unauthorized', component: Unauthorized },
  { path: 'tickets', component: TicketVisitorMenu, canActivate: [AuthGuard] },
  { path: 'users/menu', component: VisitorListMenu, canActivate: [AuthGuard] },
  { path: 'punctuation', component: PunctuationService, canActivate: [AuthGuard] },
  { path: 'daily-ranking', component: DailyRanking, canActivate: [AuthGuard, RoleGuard], data: { roles: ['ParkAdmin'] } },
  { path: 'workers/menu', component: WorkerListMenu , canActivate: [AuthGuard] },
  { path: 'special-events', component: SpecialEvents, canActivate: [AuthGuard, RoleGuard], data: { roles: ['ParkAdmin'] } },
  { path: 'rewards', component: RewardListMenu, canActivate: [AuthGuard] },
  { path: 'my-rewards', component: MyRewards, canActivate: [AuthGuard], data: { roles: ['Visitor'] } },
  { path: 'register', component: Register },
  { path: 'points-history', component: PointsHistoryList, canActivate: [AuthGuard]},
  { path: '', pathMatch: 'full', redirectTo: 'login' }
];
