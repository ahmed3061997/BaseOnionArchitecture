import { Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { TranslateService } from '@ngx-translate/core';
import { merge, tap } from 'rxjs';
import { ServerSideDataSource } from 'src/app/core/common/server-side-data-source';
import { PageQuery, DEFAULT_PAGE_SIZE, PAGE_SIZE_OPTIONS } from 'src/app/core/models/page-query';
import { Role } from 'src/app/core/models/role';
import { DialogService } from 'src/app/core/services/dialogs/dialog.service';
import { RoleService } from 'src/app/core/services/roles/role.service';
import { SearchService } from 'src/app/core/services/search/search.service';
import { AlertDialogComponent } from 'src/app/shared/alert-dialog/alert-dialog.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  pageSize = DEFAULT_PAGE_SIZE;
  pageSizes = PAGE_SIZE_OPTIONS;
  displayedColumns: string[] = ['name', 'isActive', 'actions'];
  dataSource: ServerSideDataSource<Role>

  constructor(
    private roleService: RoleService,
    private searchService: SearchService,
    private dialog: DialogService,
    private translateService: TranslateService) { }

  ngOnInit() {
    this.dataSource = new ServerSideDataSource<Role>(this.roleService)
    this.refreshTable()
  }

  ngAfterViewInit() {
    merge(this.paginator.page, this.sort.sortChange, this.searchService.searchChanged, this.translateService.onLangChange)
      .pipe(
        tap(() => this.refreshTable())
      )
      .subscribe()
  }

  private refreshTable() {
    this.dataSource.load(this.getQuery())
  }

  private getQuery(): PageQuery {
    return {
      pageIndex: this.paginator?.pageIndex || 0,
      pageSize: this.paginator?.pageSize || this.pageSize,
      sortColumn: this.sort?.active,
      sortDirection: PageQuery.toSortDirection(this.sort?.direction),
      searchTerm: this.searchService.getSearchTerm()
    }
  }

  confirmDelete(id: string) {
    this.dialog.open(AlertDialogComponent,
      {
        panelClass: 'dialog-sm',
        data: {
          title: this.translateService.instant('shared.confirm'),
          message: this.translateService.instant('shared.confirm_delete_msg'),
          confirmBtnText: this.translateService.instant('shared.confirm'),
          cancelBtnText: this.translateService.instant('shared.cancel'),
          iconClass: 'bx bx-x-circle text-danger',
          btnClass: 'danger',
          confirmFunc: () => {
            this.delete(id)
          }
        }
      })
  }

  private delete(id: string) {
    this.roleService.delete(id).subscribe(() => this.refreshTable())
  }

  toggleActive(id: string, target: any) {
    if (target.checked)
      this.roleService.activate(id).subscribe()
    else
      this.roleService.stop(id).subscribe()
  }
}
