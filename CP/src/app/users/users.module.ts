import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { UsersRoutingModule } from './users-routing.module';
import { SharedModule } from "../shared/shared.module";
import { HomeComponent } from './home/home.component';
import { RolesComponent } from './roles/roles.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { HttpBackend, HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { RegisterComponent } from './register/register.component';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
    declarations: [
        LoginComponent,
        HomeComponent,
        RolesComponent,
        RegisterComponent
    ],
    imports: [
        UsersRoutingModule,
        CommonModule,
        SharedModule,
        ReactiveFormsModule,
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
