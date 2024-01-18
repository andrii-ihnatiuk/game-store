import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin, of } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { InfoItem } from 'src/app/componetns/info-component/info-item';
import { Game } from 'src/app/models/game.model';
import { OrderDetail } from 'src/app/models/order-detail.model';
import { Order } from 'src/app/models/order.model';
import { GameService } from 'src/app/services/game.service';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'gamestore-order',
  templateUrl: './order-page.component.html',
  styleUrls: ['./order-page.component.scss'],
})
export class OrderPageComponent extends BaseComponent implements OnInit {
  orderValue?: Order;

  orderInfoList: InfoItem[] = [];

  constructor(
    private orderService: OrderService,
    private route: ActivatedRoute,
    private gameService: GameService,
    private datePipe: DatePipe
  ) {
    super();
  }

  get order(): Order | undefined {
    return this.orderValue;
  }

  set order(value: Order | undefined) {
    this.orderValue = value;
    this.orderInfoList = [];
    if (!value) {
      return;
    }

    this.orderInfoList.push(
      {
        name: this.labels.orderCustomerId,
        value: value.customerId,
      },
      {
        name: this.labels.orderDate,
        value: !!value.orderDate
          ? this.datePipe.transform(value.orderDate, 'yyyy-MM-dd') ?? ''
          : '',
      },
      {
        name: this.labels.orderShipDate,
        value: !!value.shippedDate
          ? this.datePipe.transform(value.shippedDate, 'yyyy-MM-dd') ?? ''
          : '',
      }
    );
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'id')
      .pipe(
        switchMap((x) =>
          forkJoin({
            order: this.orderService.getOrder(x),
            orderDetails: this.orderService.getOrderDetails(x),
          })
        ),
        tap((x) => (this.order = x.order)),
        switchMap((x) =>
          forkJoin(
            x.orderDetails.map((z) =>
              this.gameService.getGameById(z.productId).pipe(
                map((y) => {
                  return { orderDetail: z, game: y };
                })
              )
            )
          )
        )
      )
      .subscribe((x) => {
        if (!!x?.length) {
          this.addDetailsInfo(x);
        }
      });
  }

  addDetailsInfo(details: { orderDetail: OrderDetail; game: Game }[]): void {
    if (!details?.length) {
      return;
    }

    const detailsInfo: InfoItem = {
      name: this.labels.orderDetailsLabel,
      nestedValues: [],
    };
    this.orderInfoList.push(detailsInfo);

    details.forEach((x) => {
      const detailInfo: InfoItem = {
        nestedValues: [],
      };
      detailInfo.nestedValues!.push({
        title: x.game.name,
        pageLink: `${this.links.get(this.pageRoutes.Game)}/${x.game.key}`,
      });
      detailInfo.nestedValues!.push({
        title:
          x.orderDetail.price === undefined
            ? ''
            : `${this.labels.orderDetailsPriceLabel}: ${x.orderDetail.price}`,
      });
      detailInfo.nestedValues!.push({
        title:
          x.orderDetail.discount === undefined
            ? ''
            : `${this.labels.orderDetailsDiscountLabel}: ${x.orderDetail.discount}`,
      });
      detailInfo.nestedValues!.push({
        title:
          x.orderDetail.quantity === undefined
            ? ''
            : `${this.labels.orderDetailsQuantityLabel}: ${x.orderDetail.quantity}`,
      });
      this.orderInfoList.push(detailInfo);
    });
  }
}
