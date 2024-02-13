import { Component, Input } from '@angular/core';
import { BaseComponent } from '../base.component';
import { ListItem } from './list-item';

@Component({
    selector: 'gamestore-list-item',
    templateUrl: './list-item.component.html',
    styleUrls: ['./list-item.component.scss']
  })
export class ListItemComponent extends BaseComponent {
  @Input()
  item!: ListItem;
}