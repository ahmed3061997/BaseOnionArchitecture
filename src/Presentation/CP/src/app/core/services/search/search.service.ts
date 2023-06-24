import { EventEmitter, Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  private searchTerm: string | undefined
  searchChanged: EventEmitter<string> = new EventEmitter<string>()

  constructor() { }

  setSearchTerm(term: string) {
    this.searchTerm = term
    this.searchChanged.emit(term)
  }

  getSearchTerm() {
    return this.searchTerm
  }
}
