import { DatePipe } from '@angular/common';
import { HttpParams, HttpUrlEncodingCodec } from '@angular/common/http';
import { ThrowStmt } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { map, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { ListItem } from 'src/app/componetns/list-item-component/list-item';
import { OrderService } from 'src/app/services/order.service';

@UntilDestroy()
@Component({
  selector: 'gamestore-orders',
  templateUrl: './orders-page.component.html',
  styleUrls: ['./orders-page.component.scss'],
})
export class OrdersPageComponent extends BaseComponent implements OnInit {
  ordersList: ListItem[] = [];

  constructor(private orderService: OrderService, private datePipe: DatePipe) {
    super();
  }

  ngOnInit(): void {
    this.orderService
      .getOrders()
      .pipe(
        map((orders) =>
          orders.map((order) => {
            const orderItem: ListItem = {
              title: !!order.orderDate
                ? `${this.labels.orderCustomerId}:${order.customerId} - ${
                    this.labels.orderDate
                  }:${this.datePipe.transform(order.orderDate, 'yyyy-MM-dd')}`
                : `${this.labels.orderCustomerId}:${order.customerId}`,
                updateLink: `${this.links.get(this.pageRoutes.UpdateOrder)}/${order.id}`,
            };

            return orderItem;
          })
        ),
      )
      .subscribe((x) => (this.ordersList = x ?? []));
  }
}
