import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, Observable, catchError, of, shareReplay, tap } from 'rxjs';
import { Culture } from '../../models/culture';

@Injectable({
  providedIn: 'root'
})
export class CultureService {
  private readonly fallbackCultures: Culture[] = [
    new Culture('ar', 'عربي', 'eg', false),
    new Culture('en', 'English', 'us', true),
  ]
  private cultures: Observable<Culture[]>;
  private currentCulture$: BehaviorSubject<Culture> = new BehaviorSubject(new Culture());

  constructor(private httpClient: HttpClient, private translate: TranslateService) {
    translate.setDefaultLang(this.fallbackCultures.filter(x => x.isDefault)[0].code)
    this.cultures = this.httpClient.get<Culture[]>('/api/system/get-cultures')
      .pipe(
        catchError(() => {
          return of(this.fallbackCultures)
        }),
        // configure nx-translate & set current or default
        tap(result => {
          translate.addLangs(result.map(x => x.code))
          var culture = result.filter(x => x.code == localStorage.getItem('culture') || x.isDefault)[0]
          this.changeCulture(culture)
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
    this.changeDirection(culture.code)
    this.translate.use(culture.code)
    this.currentCulture$.next(culture)
  }

  dir() {
    const htmlTag = document.getElementsByTagName("html")[0] as HTMLHtmlElement
    return htmlTag.dir
  }

  private changeDirection(lang: string) {
    const htmlTag = document.getElementsByTagName("html")[0] as HTMLHtmlElement
    htmlTag.dir = lang === "ar" ? "rtl" : "ltr"
  }
}
