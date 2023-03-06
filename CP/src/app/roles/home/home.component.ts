import { Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { TranslateService } from '@ngx-translate/core';
import { merge, tap } from 'rxjs';
import { ServerSideDataSource } from 'src/app/core/common/server-side-data-source';
import { PageQuery, DEFAULT_PAGE_SIZE, PAGE_SIZE_OPTIONS } from 'src/app/core/models/page-query';
import { Role } from 'src/app/core/models/role';
import { RoleService } from 'src/app/core/services/roles/role.service';

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

  constructor(private roleService: RoleService, private translateService: TranslateService) { }

  ngOnInit() {
    this.dataSource = new ServerSideDataSource<Role>(this.roleService)
    this.dataSource.load(this.getQuery())
  }

  ngAfterViewInit() {
    merge(this.paginator.page, this.sort.sortChange, this.translateService.onLangChange)
      .pipe(
        tap(() => this.dataSource.load(this.getQuery()))
      )
      .subscribe()
  }

  private getQuery(): PageQuery {
    return {
      pageIndex: this.paginator?.pageIndex || 0,
      pageSize: this.paginator?.pageSize || this.pageSize,
      sortColumn: this.sort?.active,
      sortDirection: PageQuery.toSortDirection(this.sort?.direction)
    }
  }
}
