import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FooterComponent } from './footer/footer.component';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { SearchBarComponent } from './search-bar/search-bar.component';
import { LayoutComponent } from './layout/layout.component';
import { SharedRoutingModule } from './shared-routing.module';
import { NotFoundComponent } from './not-found/not-found.component';
import { LanguageSelectorComponent } from './language-selector/language-selector.component';
import { AccessDeniedComponent } from './access-denied/access-denied.component';
import { AuthLayoutComponent } from './auth-layout/auth-layout.component';
import { CurrentUserComponent } from './current-user/current-user.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { HttpBackend, HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';



@NgModule({
  declarations: [
    FooterComponent,
    SideMenuComponent,
    SearchBarComponent,
    LayoutComponent,
    NotFoundComponent,
    LanguageSelectorComponent,
    AccessDeniedComponent,
    AuthLayoutComponent,
    CurrentUserComponent
  ],
  imports: [
    SharedRoutingModule,
    CommonModule,
    TranslateModule.forChild({
        loader: {
            provide: TranslateLoader,
            useFactory: (httpHandler: HttpBackend) => new TranslateHttpLoader(new HttpClient(httpHandler)),
            deps: [HttpBackend]
        }
    })
  ],
  exports: [
    LayoutComponent,
    AuthLayoutComponent,
    LanguageSelectorComponent
  ]
})
export class SharedModule { }
