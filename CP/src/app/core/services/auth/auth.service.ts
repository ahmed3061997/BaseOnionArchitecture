import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, mergeMap, tap } from 'rxjs';
import { AuthResult } from '../../models/auth-result';
import { JwtToken } from '../../models/jwt-token';
import { User } from '../../models/user';
import { UserRole } from '../../models/user-role';

const TOKEN_KEY = 'access-token';
const REFRESHTOKEN_KEY = 'refresh-token';
const USER_KEY = 'current-user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private httpClient: HttpClient) { }

  login(username: string | null | undefined, password: string | null | undefined): Observable<any> {
    return this.httpClient.post<AuthResult>('/api/auth/login', { username, password })
      .pipe(
        tap(result => {
          this.saveUser(result.user);
          this.saveToken(result.jwt.token);
          this.saveRefreshToken(result.jwt.refreshToken);
        }),
        mergeMap(result => {
          return this.httpClient.get<string[]>(`/api/users/get-roles?id=${result.user.id}`)
            .pipe(
              tap(result => this.saveRoles(result)),
              mergeMap(() => this.httpClient.get<string[]>(`/api/users/get-claims?id=${result.user.id}`)),
              tap(result => this.saveClaims(result)),
            )
        }),
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

  isInRole(role: string): boolean {
    var user = this.getUser();
    return user.roles?.findIndex(x => x.name == role) != -1 ?? false;
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

  saveUser(user: User): void {
    localStorage.setItem(USER_KEY, JSON.stringify(user));
  }

  getUser(): User {
    const user = localStorage.getItem(USER_KEY);
    if (user) {
      return JSON.parse(user);
    }
    return new User();
  }

  saveRoles(claims: string[]) {
    var user = this.getUser();
    user.roles = claims.map(x => ({ name: x })) as UserRole[];
    this.saveUser(user);
  }

  saveClaims(claims: string[]) {
    var user = this.getUser();
    user.claims = claims;
    this.saveUser(user);
  }
}
