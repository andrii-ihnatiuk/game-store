import { Component, OnInit } from '@angular/core';
import { forkJoin, of } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Game } from 'src/app/models/game.model';
import { OrderDetail } from 'src/app/models/order-detail.model';
import { GameService } from 'src/app/services/game.service';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'gamestore-basket',
  templateUrl: './basket-page.component.html',
  styleUrls: ['./basket-page.component.scss'],
})
export class BasketPageComponent extends BaseComponent implements OnInit {

  orderInfoList?: { game: Game; detail: OrderDetail; gamePageLink: string }[];
  subtotal: number = 0;
  taxes: number = 10;
  total: number = 0;

  constructor(
    private orderService: OrderService,
    private gameService: GameService
  ) {
    super();
  }

  ngOnInit(): void {
    this.refreshBasketItems();
  }

  refreshBasketItems(): void {
    this.orderService
      .getBasket()
      .pipe(
        tap(x => {
          this.subtotal = x.subtotal;
          this.taxes = x.taxes;
          this.total = x.total;
        }),
        switchMap((x) =>
          forkJoin(
            !!x.details.length
              ? x.details.map((z: OrderDetail) =>
                  this.gameService.getGameById(z.productId).pipe(
                    map((y) => {
                      return { orderDetail: z, game: y };
                    })
                  )
                )
              : of([])
          )
        )
      )
      .subscribe((x) => {
        this.addDetailsInfo(x);
      });
  }

  addDetailsInfo(details: { orderDetail: OrderDetail; game: Game }[]): void {
    this.orderInfoList = [];
    if (!details?.length) {
      return;
    }

    details.forEach((x) => {
      if(!x?.game?.key){
        return;
      }

      this.orderInfoList?.push({
        game: x.game,
        detail: x.orderDetail,
        gamePageLink: `${this.links.get(this.pageRoutes.Game)}/${x.game.key}`,
      });
    });
  }

  onItemAdd(gameKey: string) {
    this.orderService
      .buyGame(gameKey)
      .subscribe((_) => this.refreshBasketItems());
  }

  onItemDelete(action: any) {
    this.orderService
      .cancelGameBuy(action.key, action.deleteAll)
      .subscribe((_) => this.refreshBasketItems());
  }
}
