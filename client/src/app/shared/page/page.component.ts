import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class PageComponent {
  @Input() totalCounts?: number;
  @Input() pageSize?: number;
  
  @Output() pageChanges = new EventEmitter<number>();

  onPagerChanged(event: any) {
    this.pageChanges.emit(event.page);
  }
}
