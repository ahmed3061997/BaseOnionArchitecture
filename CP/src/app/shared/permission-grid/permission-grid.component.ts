import { HttpClient } from '@angular/common/http';
import { Component, Input, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { Subscription, first, tap } from 'rxjs';
import { Helper } from 'src/app/core/helpers/helper';
import { OperationCheckboxRenderer } from './operation-checkbox.renderer';
import { OperationHeaderRenderer } from './operation-header.renderer';
import { AutoUnsubscribe } from 'src/app/core/decorators/auto-unsubscribe.decorator';
import { CultureService } from 'src/app/core/services/culture/culture.service';

@AutoUnsubscribe()
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
  onLangChange$: Subscription;

  constructor(
    private httpClient: HttpClient,
    private cultureService: CultureService) { }

  getSelectedClaims() {
    return this.context.claims
  }

  setSelectedClaims(claims: string[]) {
    this.context.claims = claims || []
    this.agGrid.api.redrawRows()
  }

  ngOnInit() {
    this.enableRtl = this.cultureService.getCurrentCultureCode() == 'ar'
    this.context = { claims: [], disabled: this.disabled }
    this.loadClaims().subscribe()
  }

  ngAfterViewInit() {
    this.onLangChange$ = this.cultureService.onCultureChange.subscribe(result => {
      this.loadClaims().subscribe(() => {
        this.enableRtl = result.code == 'ar'
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
    var operations = result
      .map(x => ({ id: x.value.split('.')[2], name: x.operationName }))
      .reduce((acc: any[], curr: any) => {
        if (!acc.find(x => x.id == curr.id))
          acc.push(curr)
        return acc;
      }, [])


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
        headerName: this.cultureService.translate('roles.module_name'),
        tooltipField: this.cultureService.translate('roles.module_name'),
      },
      {
        field: 'pageName',
        headerName: this.cultureService.translate('roles.page_name'),
        tooltipField: this.cultureService.translate('roles.page_name'),
      },
      ...operations.map(x => ({
        field: x.id,
        headerName: x.name,
        tooltipField: x.name,
        cellRenderer: OperationCheckboxRenderer,
        headerComponent: OperationHeaderRenderer
      })),
    ]
  }
}
