import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged, fromEvent, tap } from 'rxjs';
import { SearchService } from 'src/app/core/services/search/search.service';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss']
})
export class SearchBarComponent {
  seach = new FormControl()

  constructor(private search: SearchService) { }

  ngAfterViewInit() {
    this.seach.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        tap(() => {
          this.search.setSearchTerm(this.seach.value)
        })
      )
      .subscribe();
  }
}
