import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { Culture } from 'src/app/core/models/culture';
import { CultureService } from 'src/app/core/services/culture/culture.service';

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html',
  styleUrls: ['./language-selector.component.scss']
})
export class LanguageSelectorComponent {
  cultures: Culture[] = [];
  currentCulture: Culture = new Culture();
  subscriptions: Subscription[] = [];

  constructor(private cultureService: CultureService) {

  }

  ngOnInit() {
    this.subscriptions.push(
      this.cultureService
        .getCultures()
        .subscribe(result => this.cultures = result),
      this.cultureService
        .getCurrentCulture()
        .subscribe(result => this.currentCulture = result)
    )
  }

  ngOnDestroy() {
    this.subscriptions.forEach((subscription) => {
      subscription.unsubscribe();
   });
  }

  selectLanguage(culture: Culture) {
    this.cultureService.changeCulture(culture);
  }
}
