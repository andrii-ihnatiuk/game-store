import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-price',
  templateUrl: './price.component.html',
  styleUrls: ['./price.component.scss']
})
export class PriceComponent implements OnInit {

  private _price: number = 0;

  @Input('price')
  set price(value: number | undefined) {
    this._price = value ?? 0;
  }

  get price(): number {
    return this._price;
  }

  @Input('discount')
  discount?: number;

  priceWithDiscount?: number;

  ngOnInit(): void {
    if (!!this.discount) {
      this.priceWithDiscount = this._price * (100 - this.discount) / 100;
    }
  }

}
