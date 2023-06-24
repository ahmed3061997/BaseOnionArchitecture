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
    private dialog: DialogService,
    private httpClient: HttpClient,
    private notification: NotificationService,
    private cultureService: CultureService
  ) {
    this.moduleCodes = httpClient.get<any>('/api/modules/get-codes')
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
    this.httpClient.get<any>(`/api/modules/get?id=${id}`)
      .subscribe(result => {
        LoadingOverlayHelper.hideLoading()
        this.showDialog(result)
        this.currentId = id
        this.f['code'].setValue(result.code)
      })
  }

  confirmDelete(id: string) {
    this.dialog.confirmDelete(() => {
      this.delete(id)
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