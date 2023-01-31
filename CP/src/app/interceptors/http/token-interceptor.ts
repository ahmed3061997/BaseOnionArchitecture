import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({ providedIn: 'root' })
export class HttpRequestTokenInterceptor implements HttpInterceptor {
    private readonly baseUrl: string = 'https://localhost:7060/api';

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        var token = '';
        req = req.clone({
            // headers: new HttpHeaders({
            //     ...req.headers,
            //     'Authorization': 'Bearer ' + token
            // })
        });

        return next.handle(req);
    }
}