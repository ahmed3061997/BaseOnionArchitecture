import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, switchMap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { LoadingOverlayHelper } from '../helpers/loading-overlay/loading-overlay';

@Injectable({ providedIn: 'root' })
export class HttpRequestErrorInterceptor implements HttpInterceptor {

    constructor(private translateService: TranslateService, private toastr: ToastrService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req)
            .pipe(
                catchError((res: HttpErrorResponse) => {
                    return this.translateService.get(['shared.error', 'shared.undefined_error']).pipe(
                        switchMap(translate => {
                            let errorMsg = res.error?.errors?.join('\n') || translate['shared.undefined_error']
                            this.toastr.error(errorMsg, translate['shared.error'])
                            LoadingOverlayHelper.hideLoading()
                            return throwError(() => res)
                        })
                    )
                })
            ) as Observable<HttpEvent<any>>
    }
}