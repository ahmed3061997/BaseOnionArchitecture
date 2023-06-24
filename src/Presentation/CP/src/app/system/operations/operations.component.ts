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
  selector: 'app-operations',
  templateUrl: './operations.component.html',
  styleUrls: ['./operations.component.scss']
})
export class OperationsComponent {
  @ViewChild('operationDialog') operationDialog: TemplateRef<any>
  @ViewChild('nameInput') nameInput: MultiLanguageInputComponent

  private subscriptions: Subscription[] = []
  private dialogRef: MatDialogRef<any, any>
  operationCodes: Observable<any>
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
    this.operationCodes = httpClient.get<any>('/api/operations/get-codes')
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
    this.httpClient.get<[]>('/api/operations/get-all')
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
    this.httpClient.get('/api/operations/get-all')
      .subscribe(result => {
        LoadingOverlayHelper.hideLoading()
        Helper.saveAsFile('operations.json', JSON.stringify(result))
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
    this.httpClient.get<any>(`/api/operations/get?id=${id}`)
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
    this.httpClient.post(`/api/operations/delete?id=${id}`, null)
      .subscribe(() => {
        LoadingOverlayHelper.hideLoading()
        this.notification.success()
        this.refreshTable()
      })
  }

  private showDialog(data?: any) {
    this.dialogRef = this.dialog.open(this.operationDialog, { data })
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

    var operation: any = {}
    operation.id = this.currentId
    operation.code = +this.f['code'].value!
    operation.names = this.nameInput.getValue()
    console.log(operation)

    LoadingOverlayHelper.showLoading()
    this.httpClient.post(this.currentId == null ? '/api/operations/create' : '/api/operations/edit', operation)
      .subscribe(() => {
        LoadingOverlayHelper.hideLoading()
        this.notification.success()
        this.dialogRef.close()
        this.refreshTable()
      })
  }
}
