import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { DialogService } from 'src/app/core/services/dialogs/dialog.service';
import { MultiLanguageInputComponent } from 'src/app/shared/multi-language-input/multi-language-input.component';

@Component({
  selector: 'app-modules',
  templateUrl: './modules.component.html',
  styleUrls: ['./modules.component.scss']
})
export class ModulesComponent {
  @ViewChild('moduleDialog') moduleDialog: TemplateRef<any>;
  @ViewChild('nameInput') nameInput: MultiLanguageInputComponent;

  submitted: boolean = false;
  private dialogRef: MatDialogRef<any, any>;

  form = new FormGroup({
    code: new FormControl('',
      [
        Validators.required
      ]
    ),
  })

  constructor(private dialog: DialogService) { }

  get f() {
    return this.form.controls
  }

  add() {
    this.dialogRef = this.dialog.open(this.moduleDialog)
    this.dialogRef
      .afterClosed()
      .subscribe(() => {
        this.submitted = false
        this.form.reset()
      })
  }

  save() {
    this.submitted = true
    if (this.form.valid)
      this.dialogRef.close()
  }
}
