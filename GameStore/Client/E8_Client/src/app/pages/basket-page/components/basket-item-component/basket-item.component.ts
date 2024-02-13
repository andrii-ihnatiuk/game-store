import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Game } from 'src/app/models/game.model';
import { OrderDetail } from 'src/app/models/order-detail.model';
import { QuantityOperation } from './quantity-operation';

@Component({
  selector: 'app-basket-item',
  templateUrl: './basket-item.component.html',
  styleUrls: ['./basket-item.component.scss']
})
export class BasketItemComponent extends BaseComponent {

  @Input('item')
  item!: { game: Game; detail: OrderDetail; gamePageLink: string; };

  @Output('onDelete')
  onDelete = new EventEmitter();

  @Output('onAdd')
  onAdd = new EventEmitter<string>();


  onItemAdd(): void {
    this.onAdd.emit(this.item.game.key);
  }

  onItemDelete(deleteAll: boolean): void {
    this.onDelete.emit({ "key": this.item.game.key, "deleteAll": deleteAll });
  }

}
