import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpHeaders, HTTP_INTERCEPTORS, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, catchError, filter, from, Observable, of, switchMap, take, throwError } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { Router } from '@angular/router';
import { NotificationService } from '../services/notification/notification.service';
import { TranslateService } from '@ngx-translate/core';

const TOKEN_HEADER_KEY = 'Authorization';  // for Spring Boot, .Net back-end
// const TOKEN_HEADER_KEY = 'x-access-token';    // for Node.js Express back-end
@Injectable({ providedIn: 'root' })
export class HttpRequestAuthInterceptor implements HttpInterceptor {
    private isRefreshing = false;
    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

    constructor(
        private router: Router,
        private authService: AuthService,
        private translateService: TranslateService,
        private notification: NotificationService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<Object>> {
        let authReq = req;
        const token = this.authService.getToken();
        if (token != null) {
            authReq = this.addTokenHeader(req, token);
        }

        return next.handle(authReq).pipe(
            catchError(error => {
                if (error instanceof HttpErrorResponse && !authReq.url.includes('auth/login') && !authReq.url.includes('auth/refresh-token') && error.status === 401) {
                    return this.handle401Error(authReq, next);
                }

                return throwError(() => error);
            })) as Observable<HttpEvent<any>>;
    }

    private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
        if (!this.isRefreshing) {
            this.isRefreshing = true;
            this.refreshTokenSubject.next(null);

            const token = this.authService.getRefreshToken();

            if (token)
                return this.authService.refreshToken(token).pipe(
                    switchMap(token => {
                        this.isRefreshing = false;

                        this.authService.saveToken(token.token);
                        this.authService.saveRefreshToken(token.refreshToken);
                        this.refreshTokenSubject.next(token.token);

                        console.info('new access token retreived!')
                        return next.handle(this.addTokenHeader(request, token.token));
                    }),
                    catchError((err) => {
                        this.isRefreshing = false;
                        this.authService.clearSession();
                        this.notification.error(this.translateService.instant('shared.session_expired'))
                        return of({});
                    })
                );
            else
                return from(this.router.navigate(['/users/login']))
        }

        return this.refreshTokenSubject.pipe(
            filter(token => token !== null),
            take(1),
            switchMap((token) => next.handle(this.addTokenHeader(request, token)))
        );
    }

    private addTokenHeader(request: HttpRequest<any>, token: string) {
        /* for Spring Boot, .Net back-end */
        return request.clone({ headers: request.headers.set(TOKEN_HEADER_KEY, 'Bearer ' + token) });

        /* for Node.js Express back-end */
        // return request.clone({ headers: request.headers.set(TOKEN_HEADER_KEY, token) });
    }
}