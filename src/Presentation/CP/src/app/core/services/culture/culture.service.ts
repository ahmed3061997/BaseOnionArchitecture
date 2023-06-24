import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, Observable, catchError, of, shareReplay, tap } from 'rxjs';
import { Culture } from '../../models/common/culture';

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

  onCultureChange: EventEmitter<Culture> = new EventEmitter<Culture>();

  constructor(private httpClient: HttpClient, private translateService: TranslateService) {
    translateService.setDefaultLang(this.fallbackCultures.filter(x => x.isDefault)[0].code)
    this.cultures = this.httpClient.get<Culture[]>('/api/system/get-cultures')
      .pipe(
        catchError(() => {
          return of(this.fallbackCultures)
        }),
        // configure nx-translate & set current or default
        tap(result => {
          translateService.addLangs(result.map(x => x.code))
          var culture = result.filter(x => x.code == localStorage.getItem('culture') || x.isDefault)[0]
          this.changeCulture(culture, false)
        }),
        shareReplay(1),
      )
  }

  getCultures(): Observable<Culture[]> {
    return this.cultures
  }

  getCurrentCultureCode(): string {
    return localStorage.getItem('culture') ?? 'en'
  }

  getCurrentCulture(): Observable<Culture> {
    return this.currentCulture$
  }

  translate(key: string | Array<string>, interpolateParams?: Object): string | any {
    return this.translateService.instant(key, interpolateParams)
  }

  get(key: string | Array<string>, interpolateParams?: Object): Observable<string | any> {
    return this.translateService.get(key, interpolateParams)
  }

  changeCulture(culture: Culture, fireChange: boolean = true) {
    localStorage.setItem('culture', culture.code)
    this.updateDocument(culture.code)
    this.translateService.use(culture.code)
    this.currentCulture$.next(culture)
    if (fireChange)
      this.onCultureChange.emit(culture)
  }

  isRtl() {
    return this.dir() == 'rtl'
  }

  dir() {
    const htmlTag = document.getElementsByTagName("html")[0] as HTMLHtmlElement
    return htmlTag.dir
  }

  private updateDocument(lang: string) {
    const htmlTag = document.getElementsByTagName("html")[0] as HTMLHtmlElement
    htmlTag.lang = lang
    htmlTag.dir = lang === "ar" ? "rtl" : "ltr"
  }
}
