import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable, switchMap } from 'rxjs';
import { CultureService } from '../services/culture/culture.service';

@Injectable({ providedIn: 'root' })
export class HttpRequestLanguageInterceptor implements HttpInterceptor {

    constructor(private cultureService: CultureService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (req.url.endsWith('/api/system/get-cultures'))
            return next.handle(req)

        return this.cultureService.getCurrentCulture().pipe(
            switchMap(culture => {
                req = req.clone({
                    headers: req.headers.set('accept-language', culture.code)
                });

                return next.handle(req);
            })
        )
    }
}