import { TestBed } from '@angular/core/testing';

import { KweetService } from './kweet.service';

describe('KweetService', () => {
  let service: KweetService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(KweetService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
