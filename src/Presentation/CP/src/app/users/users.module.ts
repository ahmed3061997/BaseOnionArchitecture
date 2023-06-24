import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { UsersRoutingModule } from './users-routing.module';
import { SharedModule } from "../shared/shared.module";
import { HomeComponent } from './home/home.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { HttpBackend, HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { RegisterComponent } from './register/register.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { CreateUserComponent } from './create-user/create-user.component';
import { EditUserComponent } from './edit-user/edit-user.component';
import { ViewUserComponent } from './view-user/view-user.component';
import { Select2Module } from 'ng-select2-component';


@NgModule({
    declarations: [
        LoginComponent,
        HomeComponent,
        RegisterComponent,
        CreateUserComponent,
        EditUserComponent,
        ViewUserComponent,
    ],
    imports: [
        UsersRoutingModule,
        CommonModule,
        SharedModule,
        ReactiveFormsModule,
        MatDialogModule,
        MatTableModule,
        MatSortModule,
        MatPaginatorModule,
        MatProgressBarModule,
        Select2Module,
        TranslateModule.forChild({
            loader: {
                provide: TranslateLoader,
                useFactory: (httpHandler: HttpBackend) => new TranslateHttpLoader(new HttpClient(httpHandler)),
                deps: [HttpBackend]
            }
        })
    ]
})
export class UsersModule { }
