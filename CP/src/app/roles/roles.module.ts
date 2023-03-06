import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { RolesRoutingModule } from './roles-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { HttpBackend, HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { CreateRoleComponent } from './create-role/create-role.component';
import { EditRoleComponent } from './edit-role/edit-role.component';
import { ViewRoleComponent } from './view-role/view-role.component';



@NgModule({
  declarations: [
    HomeComponent,
    CreateRoleComponent,
    EditRoleComponent,
    ViewRoleComponent
  ],
  imports: [
      RolesRoutingModule,
      CommonModule,
      SharedModule,
      ReactiveFormsModule,
      MatDialogModule,
      MatTableModule,
      MatSortModule,
      MatPaginatorModule,
      MatProgressBarModule,
      TranslateModule.forChild({
          loader: {
              provide: TranslateLoader,
              useFactory: (httpHandler: HttpBackend) => new TranslateHttpLoader(new HttpClient(httpHandler)),
              deps: [HttpBackend]
          }
      })
  ]
})
export class RolesModule { }
