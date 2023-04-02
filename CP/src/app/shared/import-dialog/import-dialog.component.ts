import { Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-import-dialog',
  templateUrl: './import-dialog.component.html',
  styleUrls: ['./import-dialog.component.scss']
})
export class ImportDialogComponent {
  @ViewChild('importFile') importFile: ElementRef

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) { }

  import() {
    var file = this.importFile.nativeElement.files[0]
    this.data.importFunc && this.data.importFunc(file)
  }
}
