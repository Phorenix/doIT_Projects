import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ICustomer } from './customer';
import { CustomerService } from './customer.service';

@Component({
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})

export class CustomerListComponent implements OnInit {
  customersList: ICustomer[];

  constructor(private customerService: CustomerService, private translateService: TranslateService) { }

  ngOnInit(): void {
    this.customerService.getCustomers().subscribe(
      customersList => this.customersList = customersList
    );
  }
}
