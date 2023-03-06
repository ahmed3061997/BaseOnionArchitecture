import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateRoleComponent } from './create-role/create-role.component';
import { EditRoleComponent } from './edit-role/edit-role.component';
import { HomeComponent } from './home/home.component';
import { ViewRoleComponent } from './view-role/view-role.component';


const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'create',
    component: CreateRoleComponent
  },
  {
    path: 'edit',
    component: EditRoleComponent
  },
  {
    path: 'view',
    component: ViewRoleComponent
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RolesRoutingModule { }
