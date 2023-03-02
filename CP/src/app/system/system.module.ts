import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SystemRoutingModule } from './system-routing.module';
import { SharedModule } from '../shared/shared.module';
import { OperationsComponent } from './operations/operations.component';
import { ModulesComponent } from './modules/modules.component';
import { PagesComponent } from './pages/pages.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { HttpBackend, HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    ModulesComponent,
    PagesComponent,
    OperationsComponent,
  ],
  imports: [
    CommonModule,
    SystemRoutingModule,
    ReactiveFormsModule,
    SharedModule,
    MatDialogModule,
    MatTableModule,
    MatProgressBarModule,
    TranslateModule.forChild({
      loader: {
        provide: TranslateLoader,
        useFactory: (httpHandler: HttpBackend) => new TranslateHttpLoader(new HttpClient(httpHandler)),
        deps: [HttpBackend]
      }
    })
  ],
})
export class SystemModule { }
