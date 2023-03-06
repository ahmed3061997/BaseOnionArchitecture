import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS, HttpBackend } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { HttpRequestApiEndPointInterceptor } from './core/interceptors/api-interceptor';
import { HttpRequestAuthInterceptor } from './core/interceptors/token-interceptor';
import { HttpRequestErrorInterceptor } from './core/interceptors/error-interceptor';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpRequestLanguageInterceptor } from './core/interceptors/language-interceptor';
import { CustomToastrComponent } from './shared/custom-toastr/custom-toastr.component';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { CustomMatPaginatorIntl } from './core/common/custom-mat-paginator-intl';

const InterceptorProviders = [
    { provide: HTTP_INTERCEPTORS, useClass: HttpRequestApiEndPointInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: HttpRequestLanguageInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: HttpRequestAuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: HttpRequestErrorInterceptor, multi: true },
];

const MatPaginatorIntlProvider = { provide: MatPaginatorIntl, useClass: CustomMatPaginatorIntl };



@NgModule({
    declarations: [
        AppComponent,
    ],
    bootstrap: [AppComponent],
    imports: [
        BrowserModule,
        RouterModule,
        HttpClientModule,
        BrowserAnimationsModule,
        AppRoutingModule,
        CoreModule,
        ToastrModule.forRoot({
            toastClass: '',
            toastComponent: CustomToastrComponent,
        }),
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: (httpHandler: HttpBackend) => new TranslateHttpLoader(new HttpClient(httpHandler)),
                deps: [HttpBackend]
            }
        })
    ],
    providers: [
        InterceptorProviders,
        MatPaginatorIntlProvider
    ],
})
export class AppModule { }
