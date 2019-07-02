import { MatButtonModule, MatCheckboxModule, MatSelectModule } from '@angular/material';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
    imports: [
        BrowserAnimationsModule,
        MatButtonModule, 
        MatCheckboxModule,
        MatSelectModule
    ],
    exports: [
        BrowserAnimationsModule,
        MatButtonModule, 
        MatCheckboxModule,
        MatSelectModule
    ]
  })
  export class CustomMaterialModule { }