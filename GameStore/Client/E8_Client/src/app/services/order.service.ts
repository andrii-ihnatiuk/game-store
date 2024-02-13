import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoaderService } from '../componetns/loader-component/loader.service';
import { appConfiguration } from '../configuration/configuration-resolver';
import { OrderDetail } from '../models/order-detail.model';
import { Order } from '../models/order.model';
import { BaseService } from './base.service';
import { BasketInfo } from '../models/basket-info.model';

@Injectable()
export class OrderService extends BaseService {
  constructor(http: HttpClient, loaderService: LoaderService) {
    super(http, loaderService);
  }

  getOrder(id: string): Observable<Order> {
    return this.get<Order>(
      appConfiguration.orderApiUrl.replace(environment.routeIdIdentifier, id)
    );
  }

  updateQuantity(count: number, detailId: string): Observable<any> {
    return this.put(
      appConfiguration.updateOrderDetailCountApiUrl.replace(
        environment.routeIdIdentifier,
        detailId
      ),
      { count }
    );
  }

  deleteOrderDetail(detailId: string): Observable<any> {
    return this.delete(
      appConfiguration.deleteOrderDetailApiUrl.replace(
        environment.routeIdIdentifier,
        detailId
      ),
      {}
    );
  }

  addOrderDetail(gameId: string): Observable<any> {
    return this.post(
      appConfiguration.addOrderDetailApiUrl.replace(
        environment.routeIdIdentifier,
        gameId
      ),
      {}
    );
  }

  getHistory(range: HttpParams): Observable<Order[]> {
    return this.getWithParams<Order[]>(appConfiguration.historyApiUrl, range);
  }

  getOrders(): Observable<Order[]> {
    return this.get<Order[]>(appConfiguration.ordersApiUrl);
  }

  ship(orderId: string): Observable<any> {
    return this.post(
      appConfiguration.shipOrderApiUrl.replace(
        environment.routeIdIdentifier,
        orderId
      ),
      {}
    );
  }

  getMakeOrderInfo(): Observable<{
    paymentMethods: { imageUrl: string; title: string; description: string }[];
  }> {
    return this.get<{
      paymentMethods: {
        imageUrl: string;
        title: string;
        description: string;
      }[];
    }>(appConfiguration.makeOrderInfoApiUrl);
  }

  getOrderDetails(orderId: string): Observable<OrderDetail[]> {
    return this.get<OrderDetail[]>(
      appConfiguration.orderDetailsApiUrl.replace(
        environment.routeIdIdentifier,
        orderId
      )
    );
  }

  getBasket(): Observable<BasketInfo> {
    return this.get<BasketInfo>(appConfiguration.basketApiUrl);
  }

  buyGame(gameKey: string): Observable<any> {
    return this.post(
      appConfiguration.buyGameApiUrl.replace(
        environment.routeKeyIdentifier,
        gameKey
      ),
      {}
    );
  }

  cancelGameBuy(gameKey: string, deleteAll: boolean = false): Observable<any> {
    return this.delete(
      appConfiguration.cancelGameBuyApiUrl.replace(
        environment.routeKeyIdentifier,
        gameKey
      ) + `?deleteAll=${deleteAll}`,
      {}
    );
  }

  getBankFile(method: string): Observable<Blob> {
    return this.getFilePost(appConfiguration.payApiUrl, { method });
  }

  payTerminal(
    method: string
  ): Observable<{ userId: string; orderId: string; sum: number }> {
    return this.post<
      { method: string },
      { userId: string; orderId: string; sum: number }
    >(appConfiguration.payApiUrl, { method });
  }

  payVisa(
    method: string,
    model: {
      holder: string;
      cardNumber: string;
      monthExpire: number;
      yearExpire: number;
      cvv2: number;
    }
  ): Observable<any> {
    return this.post(appConfiguration.payApiUrl, {
      method,
      model,
    });
  }
}
