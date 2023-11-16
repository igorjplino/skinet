import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './paging-header/paging-header.component';
import { PageComponent } from './page/page.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    PagingHeaderComponent,
    PageComponent
  ],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    FormsModule
  ],
  exports: [
    PaginationModule,
    PagingHeaderComponent,
    PageComponent,
    FormsModule
  ]
})
export class SharedModule { }
