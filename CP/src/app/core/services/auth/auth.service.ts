import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, mergeMap, tap } from 'rxjs';
import { AuthResult } from '../../models/auth/auth-result';
import { JwtToken } from '../../models/auth/jwt-token';
import { User } from '../../models/users/user';
import { UserRole } from '../../models/users/user-role';
import { LoadingOverlayHelper } from '../../helpers/loading-overlay/loading-overlay';

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
          return this.httpClient.get<UserRole[]>(`/api/users/get-roles?id=${result.user.id}`)
            .pipe(
              tap(result => this.saveRoles(result)),
              mergeMap(() => this.httpClient.get<string[]>(`/api/users/get-claims?id=${result.user.id}`)),
              tap(result => this.saveClaims(result)),
            )
        }),
      )
  }

  refreshClaims() {
    var user = this.getUser();
    LoadingOverlayHelper.showLoading();
    return this.httpClient.get<string[]>(`/api/users/get-claims?id=${user.id}`)
      .pipe(
        tap(result => {
          LoadingOverlayHelper.hideLoading();
          this.saveClaims(result);
        })
      );
  }

  refreshRoles() {
    var user = this.getUser();
    LoadingOverlayHelper.showLoading();
    return this.httpClient.get<UserRole[]>(`/api/users/get-roles?id=${user.id}`)
      .pipe(
        tap(result => {
          LoadingOverlayHelper.hideLoading();
          this.saveRoles(result);
        })
      );
  }

  refreshToken(token: string): Observable<JwtToken> {
    return this.httpClient.post<JwtToken>('/api/auth/refresh-token', { refreshToken: token });
  }

  logout(): Observable<boolean> {
    LoadingOverlayHelper.showLoading()
    return this.httpClient.get<boolean>('/api/auth/logout').pipe(
      tap(() => {
        this.clearSession();
        LoadingOverlayHelper.hideLoading();
      })
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
    return user.roles?.findIndex(x => x.id == role || x.code == role) != -1 ?? false;
  }

  hasClaim(claim: string): boolean {
    var claims = this.getUser().claims || [];
    return claims.filter(x => x == claim).length != 0;
  }

  hasClaimContains(claim: string): boolean {
    var claims = this.getUser().claims || [];
    return claims.filter(x => x.indexOf(claim) != -1).length != 0;
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

  saveRoles(roles: UserRole[]) {
    var user = this.getUser();
    user.roles = roles;
    this.saveUser(user);
  }

  saveClaims(claims: string[]) {
    var user = this.getUser();
    user.claims = claims;
    this.saveUser(user);
  }
}
