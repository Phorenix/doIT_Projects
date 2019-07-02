import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { ICustomer } from './customer';
import { CustomerService } from './customer.service';

@Component({
  selector: 'app-customer-add',
  templateUrl: './customer-add.component.html',
  styleUrls: ['./customer-add.component.css']
})
export class CustomerAddComponent {
  customer: ICustomer;

  constructor(private customerService: CustomerService, private router: Router) { }

  addCustomer(): void {
    this.customerService.postCustomer(this.customer).subscribe(
      response => console.log("success: ", response),
      error => console.log("error: ", error)
    );
    this.router.navigate(['/customers']);
  }
}
