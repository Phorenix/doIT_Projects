import { TestBed, async, inject } from '@angular/core/testing';

import { LogCustomerGuard } from './log-customer.guard';

describe('LogCustomerGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LogCustomerGuard]
    });
  });

  it('should ...', inject([LogCustomerGuard], (guard: LogCustomerGuard) => {
    expect(guard).toBeTruthy();
  }));
});
