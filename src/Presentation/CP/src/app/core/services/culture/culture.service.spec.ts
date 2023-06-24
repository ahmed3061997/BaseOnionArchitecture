import { TestBed } from '@angular/core/testing';

import { CultureService } from './culture.service';

describe('CultureService', () => {
  let service: CultureService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CultureService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
