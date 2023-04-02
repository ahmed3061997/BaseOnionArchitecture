import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, TemplateRef, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { Observable, Subscription } from 'rxjs';
import { Helper } from 'src/app/core/helpers/helper';
import { LoadingOverlayHelper } from 'src/app/core/helpers/loading-overlay/loading-overlay';
import { DialogService } from 'src/app/core/services/dialogs/dialog.service';
import { NotificationService } from 'src/app/core/services/notification/notification.service';
import { AlertDialogComponent } from 'src/app/shared/alert-dialog/alert-dialog.component';
import { ImportDialogComponent } from 'src/app/shared/import-dialog/import-dialog.component';
import { MultiLanguageInputComponent } from 'src/app/shared/multi-language-input/multi-language-input.component';

@Component({
  selector: 'app-modules',
  templateUrl: './modules.component.html',
  styleUrls: ['./modules.component.scss']
})
export class ModulesComponent {
  @ViewChild('moduleDialog') moduleDialog: TemplateRef<any>
  @ViewChild('nameInput') nameInput: MultiLanguageInputComponent

  private subscriptions: Subscription[] = []
  private dialogRef: MatDialogRef<any, any>
  moduleCodes: Observable<any>
  submitted: boolean = false
  loading: boolean = false
  currentId: string | null = null
  displayedColumns: string[] = ['code', 'name', 'actions'];
  data: [] = []

  form = new FormGroup({
    code: new FormControl<number>(0,
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
    this.moduleCodes = httpClient.get<any>('/api/modules/get-codes')
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
    this.loading = true
    this.httpClient.get<[]>('/api/modules/get-all')
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
    this.httpClient.get('/api/modules/get-all')
      .subscribe(result => {
        LoadingOverlayHelper.hideLoading()
        Helper.saveAsFile('modules.json', JSON.stringify(result))
      })
  }

  openImport() {
    this.dialogRef = this.dialog.open(ImportDialogComponent,
      {
        data: {
          importFunc: (file: File) => {
            LoadingOverlayHelper.showLoading()
            var formData = new FormData()
            formData.append('jsonFile', file, file.name)
            this.httpClient.post('/api/modules/import', formData)
              .subscribe(() => {
                LoadingOverlayHelper.hideLoading()
                this.notification.success()
                this.refreshTable()
              })
          }
        }
      })
  }

  add() {
    this.showDialog()
  }

  edit(id: string) {
    LoadingOverlayHelper.showLoading()
    this.httpClient.get<any>(`/api/modules/get?id=${id}`)
      .subscribe(result => {
        LoadingOverlayHelper.hideLoading()
        this.showDialog(result)
        this.currentId = id
        this.f['code'].setValue(result.code)
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
    this.httpClient.post(`/api/modules/delete?id=${id}`, null)
      .subscribe(() => {
        LoadingOverlayHelper.hideLoading()
        this.notification.success()
        this.refreshTable()
      })
  }

  private showDialog(data?: any) {
    this.dialogRef = this.dialog.open(this.moduleDialog, { data })
    this.dialogRef
      .afterClosed()
      .subscribe(() => {
        this.currentId = null
        this.submitted = false
        this.form.reset()
      })
  }

  save() {
    this.submitted = true
    if (!this.form.valid) return

    var module: any = {}
    module.id = this.currentId
    module.code = +this.f['code'].value!
    module.names = this.nameInput.getValue()
    console.log(module)

    LoadingOverlayHelper.showLoading()
    this.httpClient.post(this.currentId == null ? '/api/modules/create' : '/api/modules/edit', module)
      .subscribe(() => {
        LoadingOverlayHelper.hideLoading()
        this.notification.success()
        this.dialogRef.close()
        this.refreshTable()
      })
  }
}