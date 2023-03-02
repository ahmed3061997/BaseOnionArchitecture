import { HttpClient } from '@angular/common/http';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { Observable, Subscription } from 'rxjs';
import { LoadingOverlayHelper } from 'src/app/core/helpers/loading-overlay/loading-overlay';
import { CultureService } from 'src/app/core/services/culture/culture.service';
import { DialogService } from 'src/app/core/services/dialogs/dialog.service';
import { NotificationService } from 'src/app/core/services/notification/notification.service';
import { AlertDialogComponent } from 'src/app/shared/alert-dialog/alert-dialog.component';
import { MultiLanguageInputComponent } from 'src/app/shared/multi-language-input/multi-language-input.component';

@Component({
  selector: 'app-pages',
  templateUrl: './pages.component.html',
  styleUrls: ['./pages.component.scss']
})
export class PagesComponent {
  @ViewChild('pageDialog') pageDialog: TemplateRef<any>
  @ViewChild('nameInput') nameInput: MultiLanguageInputComponent

  private subscriptions: Subscription[] = []
  private dialogRef: MatDialogRef<any, any>
  pageCodes: Observable<any>
  modules: Observable<any>
  operations: Observable<any>
  submitted: boolean = false
  loading: boolean = false
  currentId: string | null = null
  displayedColumns: string[] = ['code', 'name', 'actions'];
  data: [] = []

  form = new FormGroup({
    code: new FormControl('',
      [
        Validators.required
      ]
    ),
    moduleId: new FormControl('',
      [
        Validators.required
      ]
    ),
    url: new FormControl('',
      [
        Validators.required
      ]
    ),
  })

  constructor(
    private translateService: TranslateService,
    private dialog: DialogService,
    private httpClient: HttpClient,
    private notification: NotificationService) {

    this.pageCodes = httpClient.get<any>('/api/pages/get-codes')
    this.modules = httpClient.get<any>('/api/modules/get-all')
    this.operations = httpClient.get<any>('/api/operations/get-all')
  }

  get f() {
    return this.form.controls
  }

  ngOnInit() {
    this.subscriptions.push(this.translateService
      .onLangChange
      .subscribe(() => this.refreshTable())
    )
    this.refreshTable()
  }

  ngOnDestroy() {
    this.subscriptions.forEach((subscription) => {
      subscription.unsubscribe();
    });
  }

  refreshTable() {
    this.loading = true;
    this.httpClient.get<[]>('/api/pages/get-all')
      .subscribe({
        next: result => {
          this.loading = false
          this.data = result
        },
        error: () => {
          this.loading = false
        }
      })
  }

  add() {
    this.showDialog()
  }

  edit(id: string) {
    LoadingOverlayHelper.showLoading()
    this.httpClient.get<any>(`/api/pages/get?id=${id}`)
      .subscribe(result => {
        LoadingOverlayHelper.hideLoading()
        this.showDialog(result)
        this.currentId = id
        this.form.patchValue(result)
      })
  }

  confirmDelete(id: string) {
    this.dialog.open(AlertDialogComponent,
      {
        panelClass: 'dialog-sm',
        data: {
          title: this.translateService.instant('shared.confirm'),
          message: this.translateService.instant('shared.confirm_delete_msg'),
          confirmBtnText: this.translateService.instant('shared.confirm'),
          cancelBtnText: this.translateService.instant('shared.cancel'),
          iconClass: 'bx bx-x-circle text-danger',
          btnClass: 'danger',
          confirmFunc: () => {
            this.delete(id)
          }
        }
      })
  }

  private delete(id: string) {
    LoadingOverlayHelper.showLoading()
    this.httpClient.post(`/api/pages/delete?id=${id}`, null)
      .subscribe(() => {
        LoadingOverlayHelper.hideLoading()
        this.notification.success()
        this.refreshTable()
      })
  }

  private showDialog(data?: any) {
    this.dialogRef = this.dialog.open(this.pageDialog, { data })
    this.dialogRef
      .afterClosed()
      .subscribe(() => {
        this.currentId = null
        this.submitted = false
        this.form.reset()
      })
  }

  checkOp(data: any, id: string) {
    return (data?.operations as any[])?.filter(x => x.operationId == id).length > 0
  }

  save() {
    this.submitted = true
    if (!this.form.valid) return

    var page: any = {}
    page.id = this.currentId
    page.code = +this.f['code'].value!
    page.moduleId = this.f['moduleId'].value
    page.url = this.f['url'].value
    page.names = this.nameInput.getValue()
    page.operations = Array.from(document.querySelectorAll('.operation:checked')).map(x => ({ operationId: x.id }))

    LoadingOverlayHelper.showLoading()
    this.httpClient.post(this.currentId == null ? '/api/pages/create' : '/api/pages/edit', page)
      .subscribe(() => {
        LoadingOverlayHelper.hideLoading()
        this.notification.success()
        this.dialogRef.close()
        this.refreshTable()
      })
  }
}
