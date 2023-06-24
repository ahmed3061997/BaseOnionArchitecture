import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, TemplateRef, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Observable, Subscription } from 'rxjs';
import { Helper } from 'src/app/core/helpers/helper';
import { LoadingOverlayHelper } from 'src/app/core/helpers/loading-overlay/loading-overlay';
import { CultureService } from 'src/app/core/services/culture/culture.service';
import { DialogService } from 'src/app/core/services/dialogs/dialog.service';
import { NotificationService } from 'src/app/core/services/notification/notification.service';
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
    private dialog: DialogService,
    private httpClient: HttpClient,
    private notification: NotificationService,
    private cultureService: CultureService
  ) {

    this.pageCodes = httpClient.get<any>('/api/pages/get-codes')
    this.modules = httpClient.get<any>('/api/modules/get-all')
    this.operations = httpClient.get<any>('/api/operations/get-all')
  }

  get f() {
    return this.form.controls
  }

  ngOnInit() {
    this.subscriptions.push(this.cultureService
      .onCultureChange
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

  export() {
    LoadingOverlayHelper.showLoading()
    this.httpClient.get('/api/pages/get-all')
      .subscribe(result => {
        LoadingOverlayHelper.hideLoading()
        Helper.saveAsFile('pages.json', JSON.stringify(result))
      })
  }

  openImport() {
    this.dialog.import((file: File) => {
      LoadingOverlayHelper.showLoading()
      var formData = new FormData()
      formData.append('jsonFile', file, file.name)
      this.httpClient.post('/api/modules/import', formData)
        .subscribe(() => {
          LoadingOverlayHelper.hideLoading()
          this.notification.success()
          this.refreshTable()
        })
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
    this.dialog.confirmDelete(() => {
      this.delete(id)
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
