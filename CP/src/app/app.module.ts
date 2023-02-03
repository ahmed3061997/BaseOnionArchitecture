import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { AuthGuard } from './guards/auth-guard';
import { HttpRequestEndPointInterceptor } from './interceptors/http/endpoint-interceptor';
import { HttpRequestTokenInterceptor } from './interceptors/http/token-interceptor';
import { HttpRequestErrorInterceptor } from './interceptors/http/error-interceptor';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';


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
        ToastrModule.forRoot(),
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: (httpClient: HttpClient) => new TranslateHttpLoader(httpClient),
                deps: [HttpClient]
            }
        })
    ],
    providers: [
        AuthGuard,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: HttpRequestEndPointInterceptor,
            multi: true,
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: HttpRequestTokenInterceptor,
            multi: true,
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: HttpRequestErrorInterceptor,
            multi: true,
        }
    ],
})
export class AppModule { }
