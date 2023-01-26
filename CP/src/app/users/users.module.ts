import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { UsersRoutingModule } from './users-routing.module';
import { SharedModule } from "../shared/shared.module";



@NgModule({
    declarations: [
        LoginComponent
    ],
    imports: [
        UsersRoutingModule,
        CommonModule,
        SharedModule
    ]
})
export class UsersModule { }
