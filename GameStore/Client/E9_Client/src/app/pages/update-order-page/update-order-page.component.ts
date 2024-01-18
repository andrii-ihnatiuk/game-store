import { DatePipe } from '@angular/common';
import { Component, EventEmitter, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { forkJoin, Observable, of, Subject } from 'rxjs';
import { map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { DeleteWrapperComponent } from 'src/app/componetns/delete-wrapper-component/delete-wrapper.component';
import { Game } from 'src/app/models/game.model';
import { OrderDetail } from 'src/app/models/order-detail.model';
import { Order } from 'src/app/models/order.model';
import { GameService } from 'src/app/services/game.service';
import { OrderService } from 'src/app/services/order.service';
import { UserService } from 'src/app/services/user.service';
import { ChangeCountComponent } from './components/change-count-component/change-count.component';
import { SelectGameComponent } from './components/select-game-component/select-game.component';

@Component({
  selector: 'gamestore-update-order',
  templateUrl: './update-order-page.component.html',
  styleUrls: ['./update-order-page.component.scss'],
})
export class UpdateOrderPageComponent extends BaseComponent implements OnInit {
  order?: Order;
  orderDetails: { detail: OrderDetail; game: Game }[] = [];

  canShip = false;

  constructor(
    private orderService: OrderService,
    private route: ActivatedRoute,
    private gameService: GameService,
    private userSerive: UserService,
    private dialog: MatDialog
  ) {
    super();
  }

  ngOnInit(): void {
    this.loadOrder();
  }

  loadOrder(): void {
    this.order = undefined;
    this.orderDetails = [];
    this.getRouteParam(this.route, 'id')
      .pipe(
        switchMap((x) =>
          forkJoin({
            order: this.orderService.getOrder(x),
            orderDetails: this.orderService.getOrderDetails(x),
            canShip: this.userSerive.checkAccess('ShipOrder', x),
          })
        ),
        tap((x) => {
          this.order = x.order;
          this.canShip = x.canShip;
        }),
        switchMap((x) =>
          !x.orderDetails?.length
            ? of([])
            : forkJoin(
                x.orderDetails.map((z) =>
                  this.gameService.getGameById(z.productId).pipe(
                    map((y) => {
                      return { detail: z, game: y };
                    })
                  )
                )
              )
        )
      )
      .subscribe((x) => {
        if (!!x?.length) {
          this.orderDetails = x;
        }
      });
  }

  onDeleteDetail(orderDetail: { detail: OrderDetail; game: Game }): void {
    const deleteDialog = this.dialog.open(DeleteWrapperComponent);
    deleteDialog.componentInstance.name = orderDetail.game.name;

    this.handleDialog(
      deleteDialog,
      deleteDialog.componentInstance.delete,
      (_) =>
        this.orderService
          .deleteOrderDetail(orderDetail.detail.id ?? '')
          .pipe(tap(() => this.loadOrder()))
    );
  }

  onAddDetail(): void {
    const deleteDialog = this.dialog.open(SelectGameComponent);

    this.handleDialog(deleteDialog, deleteDialog.componentInstance.save, (x) =>
      this.orderService.addOrderDetail(x).pipe(tap(() => this.loadOrder()))
    );
  }

  onShip(): void {
    this.orderService
      .ship(this.order?.id ?? '')
      .subscribe((_) => this.loadOrder());
  }

  onUpdateCount(orderDetail: { detail: OrderDetail; game: Game }): void {
    const banDialog = this.dialog.open(ChangeCountComponent);
    banDialog.componentInstance.name = orderDetail.game.name;

    this.handleDialog(banDialog, banDialog.componentInstance.save, (x) =>
      this.orderService
        .updateQuantity(x, orderDetail.detail.id ?? '')
        .pipe(tap(() => this.loadOrder()))
    );
  }

  private handleDialog<TComponent, TEvent, TResponse>(
    dialog: MatDialogRef<TComponent, any>,
    event: EventEmitter<TEvent>,
    eventHandler: (content: TEvent) => Observable<TResponse>
  ) {
    const closed = new Subject();
    event
      .pipe(
        takeUntil(closed),
        tap((_) => dialog.close()),
        switchMap((x) => eventHandler(x))
      )
      .subscribe();

    dialog.afterClosed().subscribe((_) => {
      closed.next();
      closed.complete();
    });
  }
}
