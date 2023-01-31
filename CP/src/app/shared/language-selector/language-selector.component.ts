import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Culture } from 'src/app/core/models/culture';

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html',
  styleUrls: ['./language-selector.component.scss']
})
export class LanguageSelectorComponent {
  cultures: Culture[] = [];
  currentCulture: Culture = new Culture();

  constructor(private httpClient: HttpClient) {
    httpClient.get<Culture[]>('/system/get-cultures')
      .subscribe(result => {
        this.cultures = result;
        this.currentCulture = result.filter(x => x.isDefault)[0];
      })
  }

  selectLanguage(culture: Culture) {
    this.currentCulture = culture;
  }
}
