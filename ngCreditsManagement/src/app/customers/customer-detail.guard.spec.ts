import { TestBed, async, inject } from '@angular/core/testing';

import { CustomerDetailGuard } from './customer-detail.guard';

describe('CustomerDetailGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CustomerDetailGuard]
    });
  });

  it('should ...', inject([CustomerDetailGuard], (guard: CustomerDetailGuard) => {
    expect(guard).toBeTruthy();
  }));
});
