import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { Culture } from 'src/app/core/models/common/culture';
import { CultureService } from 'src/app/core/services/culture/culture.service';

@Component({
  selector: 'app-multi-language-textarea',
  templateUrl: './multi-language-textarea.component.html',
  styleUrls: ['./multi-language-textarea.component.scss']
})
export class MultiLanguageTextAreaComponent {
  cultures: Culture[] = [];
  subscriptions: Subscription[] = [];
  
  constructor(private cultureService: CultureService) {}
  

  ngOnInit() {
    this.subscriptions.push(
      this.cultureService
        .getCultures()
        .subscribe(result => this.cultures = result),
    )
  }

  ngOnDestroy() {
    this.subscriptions.forEach((subscription) => {
      subscription.unsubscribe();
   });
  }
}
