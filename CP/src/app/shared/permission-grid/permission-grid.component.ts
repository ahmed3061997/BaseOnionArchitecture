import { HttpClient } from '@angular/common/http';
import { Component, Input, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { first, tap } from 'rxjs';
import { Helper } from 'src/app/core/helpers/helper';
import { OperationCheckboxRenderer } from './operation-checkbox.renderer';
import { OperationHeaderRenderer } from './operation-header.renderer';

@Component({
  selector: 'app-permission-grid',
  templateUrl: './permission-grid.component.html',
  styleUrls: ['./permission-grid.component.scss']
})
export class PermissionGridComponent {
  @Input() disabled: boolean = false;
  @ViewChild(AgGridAngular) agGrid!: AgGridAngular;

  context: { claims: string[], disabled: boolean }
  rowData: any[] = []
  colDefs: ColDef[] = []
  enableRtl: boolean

  constructor(
    private httpClient: HttpClient,
    private translate: TranslateService) { }

  getSelectedClaims() {
    return this.context.claims
  }

  setSelectedClaims(claims: string[]) {
    this.context.claims = claims || []
    this.agGrid.api.redrawRows()
  }

  ngOnInit() {
    this.enableRtl = this.translate.currentLang == 'ar'
    this.context = { claims: [], disabled: this.disabled }
    this.loadClaims().subscribe()
  }

  ngAfterViewInit() {
    this.translate.onLangChange.subscribe(result => {
      this.loadClaims().subscribe(() => {
        this.enableRtl = result.lang == 'ar'
      })
    })
  }

  onRowDataUpdated() {
    this.agGrid.api.sizeColumnsToFit()
  }

  private loadClaims() {
    return this.httpClient.get<any[]>('/api/system/get-claims')
      .pipe(
        first(),
        tap(result => this.initAgGrid(result))
      )
  }

  private initAgGrid(result: any[]) {

    var operations = result.map(x => x.operationName);
    operations = operations.filter((x, i) => operations.indexOf(x) == i);

    var pages = Helper.groupBy(result, 'pageName');
    this.rowData = Object.keys(pages).map(x => ({
      moduleName: pages[x][0].moduleName,
      pageName: pages[x][0].pageName,
      operations: pages[x].map((y: any) => ({
        operationName: y.operationName,
        value: y.value
      }))
    }));

    this.colDefs = [
      {
        field: 'moduleName',
        headerName: this.translate.instant('roles.module_name'),
        tooltipField: this.translate.instant('roles.module_name'),
      },
      {
        field: 'pageName',
        headerName: this.translate.instant('roles.page_name'),
        tooltipField: this.translate.instant('roles.page_name'),
      },
      ...operations.map(x => ({
        field: x,
        tooltipField: x,
        cellRenderer: OperationCheckboxRenderer,
        headerComponent: OperationHeaderRenderer
      })),
    ]
  }
}
