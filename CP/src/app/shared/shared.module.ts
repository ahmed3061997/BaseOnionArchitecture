import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FooterComponent } from './footer/footer.component';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { SearchBarComponent } from './search-bar/search-bar.component';
import { LayoutComponent } from './layout/layout.component';
import { SharedRoutingModule } from './shared-routing.module';
import { NotFoundComponent } from './not-found/not-found.component';
import { LanguageSelectorComponent } from './language-selector/language-selector.component';



@NgModule({
  declarations: [
    FooterComponent,
    SideMenuComponent,
    SearchBarComponent,
    LayoutComponent,
    NotFoundComponent,
    LanguageSelectorComponent
  ],
  imports: [
    SharedRoutingModule,
    CommonModule,
  ],
  exports: [
    LayoutComponent
  ]
})
export class SharedModule { }
