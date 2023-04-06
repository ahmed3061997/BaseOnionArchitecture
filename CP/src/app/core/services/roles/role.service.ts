import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { IServerSideSource } from '../../common/server-side-source';
import { LoadingOverlayHelper } from '../../helpers/loading-overlay/loading-overlay';
import { PageQuery } from '../../models/common/page-query';
import { PageResult } from '../../models/common/page-result';
import { Role } from '../../models/users/role';
import { NotificationService } from '../notification/notification.service';

@Injectable({
  providedIn: 'root'
})
export class RoleService implements IServerSideSource<Role> {

  constructor(private httpClient: HttpClient, private notification: NotificationService) { }

  getDrop() {
    LoadingOverlayHelper.showLoading()
    return this.httpClient
    .get<Role[]>('/api/roles/get-drop')
    .pipe(
      tap(() => {
        LoadingOverlayHelper.hideLoading()
      })
    )
  }

  getAll(query: PageQuery) {
    return this.httpClient.post<PageResult<Role>>('/api/roles/get-all', query)
  }

  get(id: string) {
    LoadingOverlayHelper.showLoading()
    return this.httpClient
      .get<Role>(`/api/roles/get?id=${id}`)
      .pipe(
        tap(() => {
          LoadingOverlayHelper.hideLoading()
        })
      )
  }

  create(role: Role) {
    LoadingOverlayHelper.showLoading()
    return this.httpClient
      .post<any>('/api/roles/create', role)
      .pipe(
        tap(() => {
          LoadingOverlayHelper.hideLoading()
          this.notification.success()
        })
      )
  }

  edit(role: Role) {
    LoadingOverlayHelper.showLoading()
    return this.httpClient
      .post<any>('/api/roles/edit', role)
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
      .post<any>(`/api/roles/delete?id=${id}`, null)
      .pipe(
        tap(() => {
          LoadingOverlayHelper.hideLoading()
          this.notification.success()
        })
      )
  }

  activate(id: string) {
    LoadingOverlayHelper.showLoading()
    return this.httpClient
      .post<any>(`/api/roles/activate?id=${id}`, null)
      .pipe(
        tap(() => {
          LoadingOverlayHelper.hideLoading()
          this.notification.success()
        })
      )
  }

  stop(id: string) {
    LoadingOverlayHelper.showLoading()
    return this.httpClient
      .post<any>(`/api/roles/stop?id=${id}`, null)
      .pipe(
        tap(() => {
          LoadingOverlayHelper.hideLoading()
          this.notification.success()
        })
      )
  }
}
