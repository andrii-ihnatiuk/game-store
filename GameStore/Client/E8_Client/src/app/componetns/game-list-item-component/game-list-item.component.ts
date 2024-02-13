import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from '../base.component';
import { GameListItem } from './game-list-item';

@Component({
  selector: 'app-game-list-item',
  templateUrl: './game-list-item.component.html',
  styleUrls: ['./game-list-item.component.scss']
})
export class GameListItemComponent extends BaseComponent {

  @Input()
  item!: GameListItem;
}
