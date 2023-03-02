import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { AuthResult } from '../../models/auth-result';
import { JwtToken } from '../../models/jwt-token';

const TOKEN_KEY = 'access-token';
const REFRESHTOKEN_KEY = 'refresh-token';
const USER_KEY = 'current-user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private httpClient: HttpClient) { }

  login(username: string | null | undefined, password: string | null | undefined): Observable<AuthResult> {
    return this.httpClient.post<AuthResult>('/api/auth/login', { username, password })
      .pipe(
        tap(result => {
          this.saveUser(result.user);
          this.saveToken(result.jwt.token);
          this.saveRefreshToken(result.jwt.refreshToken);
        })
      )
  }

  refreshToken(token: string): Observable<JwtToken> {
    return this.httpClient.post<JwtToken>('/api/auth/refresh-token', { refreshToken: token });
  }

  logout(): Observable<boolean> {
    return this.httpClient.get<boolean>('/api/auth/logout').pipe(
      tap(this.clearSession)
    );
  }

  clearSession(): void {
    localStorage.removeItem(USER_KEY);
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESHTOKEN_KEY);
  }

  isAuthenticated(): boolean {
    return this.getToken() != null;
  }

  saveToken(token: string): void {
    localStorage.setItem(TOKEN_KEY, token);
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  saveRefreshToken(token: string): void {
    localStorage.setItem(REFRESHTOKEN_KEY, token);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(REFRESHTOKEN_KEY);
  }

  saveUser(user: any): void {
    localStorage.setItem(USER_KEY, JSON.stringify(user));
  }

  getUser(): any {
    const user = localStorage.getItem(USER_KEY);
    if (user) {
      return JSON.parse(user);
    }
    return {};
  }
}
