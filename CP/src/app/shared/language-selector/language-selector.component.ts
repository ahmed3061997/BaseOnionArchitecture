import { Component } from '@angular/core';

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html',
  styleUrls: ['./language-selector.component.scss']
})
export class LanguageSelectorComponent {
  private flags: [string, string][] = [['ar', 'eg'], ['en', 'us']];
  currentFlag: string = 'us';
  currentLang: string = 'en';

  selectLanguage(lang: string) {
    this.currentLang = lang;
    this.currentFlag = this.flags.filter(x => x[0] == lang)[0][1];
  }
}
