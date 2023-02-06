import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { LoadingOverlayHelper } from '../helpers/loading-overlay/loading-overlay';
import { NotificationService } from '../services/notification/notification.service';

@Injectable({ providedIn: 'root' })
export class HttpRequestErrorInterceptor implements HttpInterceptor {

    constructor(private notification: NotificationService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req)
            .pipe(
                catchError((res: HttpErrorResponse) => {
                    let errorMsg = res.error?.errors?.join('\n')
                    this.notification.error(errorMsg)
                    LoadingOverlayHelper.hideLoading()
                    return throwError(() => res)
                })
            ) as Observable<HttpEvent<any>>
    }
}