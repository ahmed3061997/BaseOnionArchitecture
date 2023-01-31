import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Injectable({ providedIn: 'root' })
export class HttpRequestErrorInterceptor implements HttpInterceptor {
    constructor(private toastr: ToastrService) {

    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req)
            .pipe(
                catchError((error: HttpErrorResponse) => {
                    let errorMsg = 'Unknown error!';
                    this.toastr.error('Error', errorMsg);
                    console.log(error);
                    return throwError(() => error);
                })
            ) as Observable<HttpEvent<any>>
    }
}