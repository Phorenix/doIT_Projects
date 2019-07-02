import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ICustomer } from './customer';
import { CustomerService } from './customer.service';

@Component({
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.css']
})
export class CustomerDetailComponent implements OnInit {
  showInputText: boolean = false;

  customer: ICustomer;
  // customerToPut: ICustomer;

  constructor(private customerService: CustomerService, private route: ActivatedRoute, private router: Router) { }

  toggleInputText(): void {
    if (!this.showInputText) {
      this.showInputText = !this.showInputText;
    } else {
      this.customerService.putCustomer(this.customer).subscribe(
        response => console.log("success: ", response),
        error => console.log("error: ", error)
      );
      // this.customer = this.customerToPut;
      this.showInputText = !this.showInputText;
    }
  }

  ngOnInit() {
    let id = +this.route.snapshot.paramMap.get('id');
    this.customerService.getCustomer(id).subscribe(
      customer => this.customer = customer
    );
    // this.customerToPut = this.customer;
  }

  onBack(): void {
    this.router.navigate(['/customers']);
  }
}
