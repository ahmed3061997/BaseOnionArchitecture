import { HttpClient } from '@angular/common/http';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { BehaviorSubject } from 'rxjs';
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

  submitted: boolean = false
  private dialogRef: MatDialogRef<any, any>
  private moduleCodes: [] = []

  form = new FormGroup({
    code: new FormControl('',
      [
        Validators.required
      ]
    ),
  })

  constructor(private dialog: DialogService, private httpClient: HttpClient, private notification: NotificationService) {
  }

  get f() {
    return this.form.controls
  }

  ngOnInit() {
    this.httpClient.get<[]>('/api/modules/get-codes')
      .subscribe(result => this.moduleCodes = result)
      .unsubscribe()
  }

  add() {
    this.dialogRef = this.dialog.open(this.moduleDialog)
    this.dialogRef
      .afterClosed()
      .subscribe(() => {
        this.submitted = false
        this.form.reset()
      })
      .unsubscribe()
  }

  save() {
    this.submitted = true
    if (!this.form.valid) return

    var module = this.form.value as any
    module.names = this.nameInput.getValue()
    this.httpClient.post('/api/modules/create', module).subscribe(() => {
      this.dialogRef.close()
      this.notification.success()
    })
  }
}
