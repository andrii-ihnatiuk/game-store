import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/componetns/base.component';

@Component({
  selector: 'app-item-quantity',
  templateUrl: './item-quantity.component.html',
  styleUrls: ['./item-quantity.component.scss']
})
export class ItemQuantityComponent extends BaseComponent implements OnInit {

  private _quantity: number = 0;

  @Input('quantity')
  set quantity(value: number | undefined) {
    this._quantity = value ?? 0;
  }

  get quantity(): number {
    return this._quantity;
  }

  @Input('min')
  minQuantity: number = 0;

  @Input('max')
  maxQuantity: number = Number.MAX_VALUE;

  @Output()
  onIncrement = new EventEmitter();

  @Output()
  onDecrement = new EventEmitter();

  @ViewChild('btnMinus', { static: true })
  btnMinus!: ElementRef<HTMLButtonElement>;

  @ViewChild('btnPlus', { static: true })
  btnPlus!: ElementRef<HTMLButtonElement>;

  ngOnInit(): void {
    if (this.minQuantity == this.quantity) {
      this.btnMinus.nativeElement.disabled = true;
    }
    else if (this.maxQuantity == this.quantity) {
      this.btnPlus.nativeElement.disabled = true;
    }
  }


  decrementQuantity(): void {
    if (this._quantity > this.minQuantity) {
      this.onDecrement.emit();
    }
  }

  incrementQuantity(): void {
    if (this._quantity < this.maxQuantity) {
      this.onIncrement.emit();
    }
  }

}
