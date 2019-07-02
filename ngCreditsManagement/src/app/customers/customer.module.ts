import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { CustomerListComponent } from './customer-list.component';
import { CustomerDetailComponent } from './customer-detail.component';
import { CustomerDetailGuard } from './customer-detail.guard';
import { CustomerAddComponent } from './customer-add.component';
import { LogCustomerModule } from '../logs/log.module';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { CustomMaterialModule } from 'src/helpers/custommaterial.module';

@NgModule({
  declarations: [
    CustomerListComponent,
    CustomerDetailComponent,
    CustomerAddComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    LogCustomerModule,
    CustomMaterialModule,
    RouterModule.forChild([
      { path: 'customers', component: CustomerListComponent },
      { path: 'customers/:id', canActivate: [ CustomerDetailGuard ], component: CustomerDetailComponent },
      { path: 'customerpost', component: CustomerAddComponent }
    ]),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  exports: []
})
export class CustomerModule { }

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
