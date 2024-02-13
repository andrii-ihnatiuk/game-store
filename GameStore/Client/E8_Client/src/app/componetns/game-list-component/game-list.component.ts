import { Component, Input } from '@angular/core';
import { BaseComponent } from '../base.component';
import { GameListItem } from '../game-list-item-component/game-list-item';

@Component({
  selector: 'app-game-list',
  templateUrl: './game-list.component.html',
  styleUrls: ['./game-list.component.scss']
})
export class GameListComponent extends BaseComponent {
  @Input()
  listItems: GameListItem[] = [];
}
