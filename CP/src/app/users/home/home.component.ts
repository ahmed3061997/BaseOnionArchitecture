
import { Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { TranslateService } from '@ngx-translate/core';
import { merge, tap } from 'rxjs';
import { ServerSideDataSource } from 'src/app/core/common/server-side-data-source';
import { PageQuery, DEFAULT_PAGE_SIZE, PAGE_SIZE_OPTIONS } from 'src/app/core/models/page-query';
import { User } from 'src/app/core/models/user';
import { UserService } from 'src/app/core/services/users/user.service';
import { SearchService } from 'src/app/core/services/search/search.service';

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
  displayedColumns: string[] = ['fullName', 'username', 'email', 'isActive', 'actions'];
  dataSource: ServerSideDataSource<User>

  constructor(
    private userService: UserService,
    private searchService: SearchService,
    private translateService: TranslateService) { }

  ngOnInit() {
    this.dataSource = new ServerSideDataSource<User>(this.userService)
    this.dataSource.load(this.getQuery())
  }

  ngAfterViewInit() {
    merge(this.paginator.page, this.sort.sortChange, this.searchService.searchChanged, this.translateService.onLangChange)
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
      sortDirection: PageQuery.toSortDirection(this.sort?.direction),
      searchTerm: this.searchService.getSearchTerm()
    }
  }
}
