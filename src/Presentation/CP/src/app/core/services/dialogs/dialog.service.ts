import { Injectable, TemplateRef } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ComponentType } from 'ngx-toastr';
import { CultureService } from '../culture/culture.service';
import { Direction } from '@angular/cdk/bidi';
import { AlertDialogComponent } from 'src/app/shared/alert-dialog/alert-dialog.component';
import { ImportDialogComponent } from 'src/app/shared/import-dialog/import-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog: MatDialog, private cultureService: CultureService) { }

  open<T>(template: ComponentType<T> | TemplateRef<T>, config?: MatDialogConfig | undefined) {
    return this.dialog.open(template, {
      ...config,
      direction:  this.cultureService.dir() as Direction
    })
  }

  confirmDelete(confirmCallback: () => void) {
    this.open(AlertDialogComponent,
      {
        panelClass: 'dialog-sm',
        data: {
          title: this.cultureService.translate('shared.confirm'),
          message: this.cultureService.translate('shared.confirm_delete_msg'),
          confirmBtnText: this.cultureService.translate('shared.confirm'),
          cancelBtnText: this.cultureService.translate('shared.cancel'),
          iconClass: 'bx bx-x-circle text-danger',
          btnClass: 'danger',
          confirmFunc: confirmCallback
        }
      })
  }

  import(fileCallback: (file: File) => void) {
     this.dialog.open(ImportDialogComponent,
      {
        data: {
          importFunc: fileCallback
        }
      })
  }
}
