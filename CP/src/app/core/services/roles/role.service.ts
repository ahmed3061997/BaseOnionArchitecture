import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { IServerSideSource } from '../../common/server-side-source';
import { LoadingOverlayHelper } from '../../helpers/loading-overlay/loading-overlay';
import { PageQuery } from '../../models/page-query';
import { PageResult } from '../../models/page-result';
import { Role } from '../../models/role';
import { NotificationService } from '../notification/notification.service';

@Injectable({
  providedIn: 'root'
})
export class RoleService implements IServerSideSource<Role> {

  constructor(private httpClient: HttpClient, private notification: NotificationService) { }

  getAll(query: PageQuery) {
    return this.httpClient.post<PageResult<Role>>('/api/roles/get-all', query);
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
}
