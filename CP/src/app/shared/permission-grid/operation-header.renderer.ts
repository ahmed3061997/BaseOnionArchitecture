import { Component } from '@angular/core';
import { IHeaderAngularComp } from 'ag-grid-angular';
import { IHeaderParams } from 'ag-grid-community';

@Component({
    selector: 'app-operation-checkbox',
    template: `
        <div class="ag-cell-label-container" role="presentation">
            <span ref="eMenu" class="ag-header-icon ag-header-cell-menu-button"></span>
            <div ref="eLabel" class="ag-header-cell-label" role="presentation">
                <span ref="eText" class="ag-header-cell-text" role="columnheader">{{this.params.displayName}}</span>
                <span class="btn btn-link ms-1 p-0" (click)="selectAll()" *ngIf="!params.context.disabled"><i class="bx bx-select-multiple"></i></span>
            </div>
        </div>
  `,
})
export class OperationHeaderRenderer implements IHeaderAngularComp {
    params: IHeaderParams

    agInit(params: IHeaderParams): void {
        this.params = params
    }

    refresh(params: IHeaderParams): boolean {
        this.params = params
        return true
    }

    selectAll() {
        var checkboxes = document.querySelectorAll<HTMLElement>('input.operation:not(:checked)')
        checkboxes.forEach(x => x.click())
    }
}
