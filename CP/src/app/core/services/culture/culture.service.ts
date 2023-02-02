import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, shareReplay, tap } from 'rxjs';
import { Culture } from '../../models/culture';

@Injectable({
  providedIn: 'root'
})
export class CultureService {
  private cultures: Observable<Culture[]>;
  private currentCulture$: BehaviorSubject<Culture> = new BehaviorSubject(new Culture());
  constructor(private httpClient: HttpClient) {
    this.cultures = this.httpClient.get<Culture[]>('/system/get-cultures')
      .pipe(
        tap(result => {
          var culture = result.filter(x => x.code == localStorage.getItem('culture') || x.isDefault)[0];
          this.currentCulture$.next(culture);
        }),
        shareReplay(1),
      )
  }

  getCultures(): Observable<Culture[]> {
    return this.cultures
  }

  getCurrentCulture(): Observable<Culture> {
    return this.currentCulture$
  }

  changeCulture(culture: Culture) {
    localStorage.setItem('culture', culture.code)
    this.currentCulture$.next(culture)
  }
}
