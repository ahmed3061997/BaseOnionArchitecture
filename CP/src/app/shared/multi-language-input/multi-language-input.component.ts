import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { Culture } from 'src/app/core/models/culture';
import { CultureService } from 'src/app/core/services/culture/culture.service';

@Component({
  selector: 'app-multi-language-input',
  templateUrl: './multi-language-input.component.html',
  styleUrls: ['./multi-language-input.component.scss']
})
export class MultiLanguageInputComponent {
  @Input() disabled: boolean = false;
  @Input() form: FormGroup;
  @Input() submitted: boolean;

  cultures: Culture[] = [];
  subscriptions: Subscription[] = [];

  constructor(private cultureService: CultureService) { }

  get f(): any {
    return this.form.controls
  }

  ngOnInit() {
    this.subscriptions.push(
      this.cultureService
        .getCultures()
        .subscribe(result => {
          this.cultures = result
          result.forEach(x => this.form?.addControl(`${x.code}Input`, new FormControl('',
            [
              Validators.required
            ]
          )))
        }),
    )
  }

  ngOnDestroy() {
    this.subscriptions.forEach((subscription) => {
      subscription.unsubscribe();
    });
  }

  getValue() {
    var inputs = document.querySelectorAll('.language-input input')
    return Array.from(inputs).map(x => ({
      culture: x.parentElement!.getAttribute('data-culture'),
      value: (x as any).value
    }))
  }
}