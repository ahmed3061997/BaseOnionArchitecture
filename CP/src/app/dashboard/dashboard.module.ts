import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { SharedModule } from "../shared/shared.module";
import { DashboardRoutingModule } from './dashboard-routing.module';



@NgModule({
    declarations: [
        HomeComponent
    ],
    exports: [
        HomeComponent
    ],
    imports: [
        DashboardRoutingModule,
        CommonModule,
        SharedModule
    ]
})
export class DashboardModule { }
