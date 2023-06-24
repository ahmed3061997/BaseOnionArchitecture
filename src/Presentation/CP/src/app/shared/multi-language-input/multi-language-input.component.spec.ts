import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiLanguageInputComponent } from './multi-language-input.component';

describe('MultiLanguageInputComponent', () => {
  let component: MultiLanguageInputComponent;
  let fixture: ComponentFixture<MultiLanguageInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MultiLanguageInputComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MultiLanguageInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
