import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LogCustomerComponent } from './log-customer.component';

describe('LogCustomerComponent', () => {
  let component: LogCustomerComponent;
  let fixture: ComponentFixture<LogCustomerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LogCustomerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LogCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
