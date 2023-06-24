import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth-guard';

const routes: Routes = [
  {
    path: '',
    canActivate: [AuthGuard],
    loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule)
  },
  {
    path: 'system',
    canActivate: [AuthGuard],
    loadChildren: () => import('./system/system.module').then(m => m.SystemModule)
  },
  {
    path: 'users',
    canActivate: [AuthGuard],
    loadChildren: () => import('./users/users.module').then(m => m.UsersModule)
  },
  {
    path: 'roles',
    canActivate: [AuthGuard],
    loadChildren: () => import('./roles/roles.module').then(m => m.RolesModule)
  },
  {
    path: '**',
    loadChildren: () => import('./shared/shared.module').then(m => m.SharedModule)
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
