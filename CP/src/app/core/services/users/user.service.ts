import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, tap } from 'rxjs';
import { ApiResponse } from '../../models/api-response';
import { AuthResult } from '../../models/auth-result';
import { User } from '../../models/user';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClient, private authService: AuthService) { }

  register(user: any): Observable<AuthResult> {
    return this.httpClient.post<AuthResult>('/api/auth/register', user)
      .pipe(
        tap(result => {
          this.authService.saveUser(result.user);
          this.authService.saveToken(result.jwt.token);
          this.authService.saveRefreshToken(result.jwt.refreshToken);
        })
      )
  }
}
