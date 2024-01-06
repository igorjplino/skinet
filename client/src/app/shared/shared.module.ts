import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './paging-header/paging-header.component';
import { PageComponent } from './page/page.component';
import { FormsModule } from '@angular/forms';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { OrderTotalsComponent } from './order-totals/order-totals.component';

@NgModule({
  declarations: [
    PagingHeaderComponent,
    PageComponent,
    OrderTotalsComponent
  ],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    CarouselModule.forRoot(),
    FormsModule
  ],
  exports: [
    PaginationModule,
    PagingHeaderComponent,
    PageComponent,
    FormsModule,
    CarouselModule,
    OrderTotalsComponent
  ]
})
export class SharedModule { }
