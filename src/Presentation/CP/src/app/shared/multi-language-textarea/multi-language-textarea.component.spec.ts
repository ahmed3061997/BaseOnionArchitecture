import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiLanguageTextareaComponent } from './multi-language-textarea.component';

describe('MultiLanguageTextareaComponent', () => {
  let component: MultiLanguageTextareaComponent;
  let fixture: ComponentFixture<MultiLanguageTextareaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MultiLanguageTextareaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MultiLanguageTextareaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
