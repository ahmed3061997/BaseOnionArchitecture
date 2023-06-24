import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CopyFromRoleComponent } from './copy-from-role.component';

describe('CopyFromRoleComponent', () => {
  let component: CopyFromRoleComponent;
  let fixture: ComponentFixture<CopyFromRoleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CopyFromRoleComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CopyFromRoleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
