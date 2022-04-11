import { TestBed } from '@angular/core/testing';

import { EmailVerDataService } from './email-ver-data.service';

describe('EmailVerDataService', () => {
  let service: EmailVerDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmailVerDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
