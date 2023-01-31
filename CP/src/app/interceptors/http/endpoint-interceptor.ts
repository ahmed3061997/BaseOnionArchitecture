import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class HttpRequestEndPointInterceptor implements HttpInterceptor {
  private readonly baseUrl: string = 'https://localhost:7060/api';

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    req = req.clone({
      url: this.baseUrl + req.url
    });

    return next.handle(req);
  }
}