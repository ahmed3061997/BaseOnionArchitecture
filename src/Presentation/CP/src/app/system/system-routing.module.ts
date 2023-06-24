import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ModulesComponent } from './modules/modules.component';
import { PagesComponent } from './pages/pages.component';
import { OperationsComponent } from './operations/operations.component';

const routes: Routes = [
  {
    path: 'modules',
    component: ModulesComponent
  },
  {
    path: 'pages',
    component: PagesComponent
  },
  {
    path: 'operations',
    component: OperationsComponent
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemRoutingModule { }
