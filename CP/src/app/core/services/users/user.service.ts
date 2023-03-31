import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, tap } from 'rxjs';
import { IServerSideSource } from '../../common/server-side-source';
import { LoadingOverlayHelper } from '../../helpers/loading-overlay/loading-overlay';
import { ApiResponse } from '../../models/api-response';
import { AuthResult } from '../../models/auth-result';
import { PageQuery } from '../../models/page-query';
import { PageResult } from '../../models/page-result';
import { User } from '../../models/user';
import { AuthService } from '../auth/auth.service';
import { NotificationService } from '../notification/notification.service';

@Injectable({
  providedIn: 'root'
})
export class UserService implements IServerSideSource<User> {

  constructor(
    private httpClient: HttpClient,
    private authService: AuthService,
    private notification: NotificationService) { }

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

  getAll(query: PageQuery) {
    return this.httpClient.post<PageResult<User>>('/api/users/get-all', query)
  }

  get(id: string) {
    LoadingOverlayHelper.showLoading()
    return this.httpClient
      .get<User>(`/api/users/get?id=${id}`)
      .pipe(
        tap(() => {
          LoadingOverlayHelper.hideLoading()
        })
      )
  }

  create(user: User) {
    LoadingOverlayHelper.showLoading()
    return this.httpClient
      .post<any>('/api/users/create', user)
      .pipe(
        tap(() => {
          LoadingOverlayHelper.hideLoading()
          this.notification.success()
        })
      )
  }

  edit(user: User) {
    LoadingOverlayHelper.showLoading()
    return this.httpClient
      .post<any>('/api/users/edit', user)
      .pipe(
        tap(() => {
          LoadingOverlayHelper.hideLoading()
          this.notification.success()
        })
      )
  }

  delete(id: string) {
    LoadingOverlayHelper.showLoading()
    return this.httpClient
      .post<any>(`/api/users/delete?id=${id}`, null)
      .pipe(
        tap(() => {
          LoadingOverlayHelper.hideLoading()
          this.notification.success()
        })
      )
  }
}
