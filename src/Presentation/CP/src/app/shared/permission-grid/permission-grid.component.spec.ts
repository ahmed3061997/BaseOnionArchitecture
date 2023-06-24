import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PermissionGridComponent } from './permission-grid.component';

describe('PermissionGridComponent', () => {
  let component: PermissionGridComponent;
  let fixture: ComponentFixture<PermissionGridComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PermissionGridComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PermissionGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
