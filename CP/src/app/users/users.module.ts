import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { UsersRoutingModule } from './users-routing.module';
import { SharedModule } from "../shared/shared.module";
import { HomeComponent } from './home/home.component';
import { RolesComponent } from './roles/roles.component';



@NgModule({
    declarations: [
        LoginComponent,
        HomeComponent,
        RolesComponent
    ],
    imports: [
        UsersRoutingModule,
        CommonModule,
        SharedModule
    ]
})
export class UsersModule { }
