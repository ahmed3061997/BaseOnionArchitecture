import { Injectable, TemplateRef } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ComponentType } from 'ngx-toastr';
import { CultureService } from '../culture/culture.service';
import { Direction } from '@angular/cdk/bidi';

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog: MatDialog, private cultureService: CultureService) { }

  open<T>(template: ComponentType<T> | TemplateRef<T>, config: MatDialogConfig | undefined) {
    return this.dialog.open(template, {
      ...config,
      direction:  this.cultureService.dir() as Direction
    })
  }
}
