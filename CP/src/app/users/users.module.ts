import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { UsersRoutingModule } from './users-routing.module';
import { SharedModule } from "../shared/shared.module";
import { HomeComponent } from './home/home.component';
import { RolesComponent } from './roles/roles.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';



@NgModule({
    declarations: [
        LoginComponent,
        HomeComponent,
        RolesComponent
    ],
    imports: [
        UsersRoutingModule,
        CommonModule,
        SharedModule,
        TranslateModule.forChild({
            loader: {
                provide: TranslateLoader,
                useFactory: (httpClient: HttpClient) => new TranslateHttpLoader(httpClient),
                deps: [HttpClient]
            }
        })
    ]
})
export class UsersModule { }
