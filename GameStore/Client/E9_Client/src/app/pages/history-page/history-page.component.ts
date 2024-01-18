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
  selector: 'gamestore-history',
  templateUrl: './history-page.component.html',
  styleUrls: ['./history-page.component.scss'],
})
export class HistoryPageComponent extends BaseComponent implements OnInit {
  private start: Date | null = null;
  private end: Date | null = null;

  range?: FormGroup;
  ordersList: ListItem[] = [];

  constructor(private orderService: OrderService, private datePipe: DatePipe) {
    super();
  }

  ngOnInit(): void {
    this.range = new FormGroup({
      start: new FormControl(null),
      end: new FormControl(null),
    });
    this.loadHistory('', '');

    this.range.valueChanges.pipe(untilDestroyed(this)).subscribe((_) => {
      const currentStart = this.range?.controls['start'].value;
      let currentEnd = this.range?.controls['end'].value;

      if (currentStart === this.start && this.end === currentEnd) {
        return;
      }

      if (this.start != currentStart) {
        currentEnd = null;
      }

      this.start = currentStart;
      this.end = currentEnd;

      if (
        !!this.range?.enabled &&
        !!currentStart &&
        !!currentEnd &&
        currentEnd >= currentStart
      ) {
        this.range?.disable();
        this.loadHistory(currentStart.toString(), currentEnd.toString());
      }
    });
  }

  private loadHistory(start: string, end: string): void {
    this.orderService
      .getHistory(
        new HttpParams({
          fromObject: { start, end },
          encoder: new HttpUrlEncodingCodec(),
        })
      )
      .pipe(
        map((orders) =>
          orders.map((order) => {
            const orderItem: ListItem = {
              title: !!order.orderDate
                ? `${this.labels.orderCustomerId}:${order.customerId} - ${
                    this.labels.orderDate
                  }:${this.datePipe.transform(order.orderDate, 'yyyy-MM-dd')}`
                : `${this.labels.orderCustomerId}:${order.customerId}`,
              pageLink: `${this.links.get(this.pageRoutes.Order)}/${order.id}`,
            };

            return orderItem;
          })
        ),
        tap(() => this.range?.enable())
      )
      .subscribe((x) => (this.ordersList = x ?? []));
  }
}
