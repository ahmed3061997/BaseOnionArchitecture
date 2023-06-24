import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DEFAULT_OPTIONS, MatDialogModule } from '@angular/material/dialog';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MatDialogModule,
  ],
  providers: [
    { provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: { disableClose: true, panelClass: 'dialog-md' } }
  ]
})
export class CoreModule { }
