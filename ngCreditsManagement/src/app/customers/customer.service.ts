import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';

import { ICustomer } from './customer';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private customerUrl = 'http://localhost:60734/api/customers';

  constructor(private http: HttpClient) {}

  getCustomers(): Observable<ICustomer[]> {
    return this.http.get<ICustomer[]>(this.customerUrl).pipe(
      tap(data => console.log("All: " + JSON.stringify(data)))
    );
  }

  getCustomer(id: number): Observable<ICustomer | undefined> {
    return this.http.get<ICustomer>(`${this.customerUrl}/${id}`).pipe(
      tap(data => console.log("Customer: " + JSON.stringify(data)))
    );
  }

  postCustomer(customerToPost: ICustomer): Observable<any> {
    return this.http.post(`${this.customerUrl}`, customerToPost, {responseType: 'text'});
  }

  putCustomer(customerToPut: ICustomer): Observable<any> {
    return this.http.put(`${this.customerUrl}/${customerToPut.id}`, customerToPut, {responseType: 'text'});
  }
}
