import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams } from 'ag-grid-community';

@Component({
  selector: 'app-operation-checkbox',
  template: `
    <div class="form-check">
      <input class="form-check-input operation" type="checkbox" style="margin-top: 0.9rem;" [checked]="checked" (change)="togglePermission($event)" [disabled]="params.context.disabled">
    </div>
  `,
})
export class OperationCheckboxRenderer implements ICellRendererAngularComp {
  params: ICellRendererParams
  checked: boolean

  agInit(params: ICellRendererParams): void {
    this.params = params
    this.checked = this.isChecked()
  }

  refresh(params: ICellRendererParams): boolean {
    this.params = params
    this.checked = this.isChecked()
    return true
  }

  getValue() {
    var operation = this.params.data.operations.filter((x: any) => x.value.indexOf(this.params.colDef!.field))[0]
    return operation?.value
  }

  isChecked() {
    var value = this.getValue()
    return this.params.context.claims.filter((x: any) => x == value).length != 0
  }

  togglePermission(event: any) {
    var checked = event.currentTarget.checked
    var value = this.getValue()
    if (checked) {
      if (this.params.context.claims.indexOf(value) == -1)
        this.params.context.claims.push(value);
    } else {
      this.params.context.claims = this.params.context.claims.filter((x: any) => x != value);
    }
  }
}
