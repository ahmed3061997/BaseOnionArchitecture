import { Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Subscription, merge, tap } from 'rxjs';
import { ServerSideDataSource } from 'src/app/core/common/server-side-data-source';
import { PageQuery, DEFAULT_PAGE_SIZE, PAGE_SIZE_OPTIONS } from 'src/app/core/models/common/page-query';
import { User } from 'src/app/core/models/users/user';
import { UserService } from 'src/app/core/services/users/user.service';
import { SearchService } from 'src/app/core/services/search/search.service';
import { AutoUnsubscribe } from 'src/app/core/decorators/auto-unsubscribe.decorator';
import { DialogService } from 'src/app/core/services/dialogs/dialog.service';
import { CultureService } from 'src/app/core/services/culture/culture.service';

@AutoUnsubscribe()
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  subscription: Subscription
  pageSize = DEFAULT_PAGE_SIZE;
  pageSizes = PAGE_SIZE_OPTIONS;
  displayedColumns: string[] = ['fullName', 'username', 'email', 'isActive', 'actions'];
  dataSource: ServerSideDataSource<User>

  constructor(
    private userService: UserService,
    private searchService: SearchService,
    private dialog: DialogService,
    private cultureService: CultureService) { }

  ngOnInit() {
    this.dataSource = new ServerSideDataSource<User>(this.userService)
    this.refreshTable()
  }

  ngAfterViewInit() {
    this.subscription = merge(this.paginator.page, this.sort.sortChange, this.searchService.searchChanged, this.cultureService.onCultureChange)
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
    this.dialog.confirmDelete(() => {
      this.delete(id)
    })
  }

  private delete(id: string) {
    this.userService.delete(id).subscribe(() => this.refreshTable())
  }

  toggleActive(id: string, target: any) {
    if (target.checked)
      this.userService.activate(id).subscribe()
    else
      this.userService.stop(id).subscribe()
  }
}
