import { Component, Input } from '@angular/core';
import { BaseComponent } from '../base.component';
import { ListItem } from '../list-item-component/list-item';

@Component({
  selector: 'gamestore-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss'],
})
export class ListComponent extends BaseComponent {
  @Input()
  addLink?: string;

  @Input()
  listItems: ListItem[] = [];
}
