import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from 'src/app/componetns/base.component';
import { OrderService } from 'src/app/services/order.service';
import { PaymentMethods } from './payment-methods.enum';
import { BasketInfo } from 'src/app/models/basket-info.model';
import { OrderDetail } from 'src/app/models/order-detail.model';

@Component({
  selector: 'gamestore-make-order',
  templateUrl: './make-order-page.component.html',
  styleUrls: ['./make-order-page.component.scss'],
})
export class MakeOrderPageComponent extends BaseComponent implements OnInit {
  terminalResult: any;
  paymentViaVisa = false;

  totalForPay?: number;
  paymentMethods: {
    imageUrl: string;
    title: string;
    description: string;
    method: PaymentMethods;
  }[] = [];

  constructor(private orderService: OrderService, private router: Router) {
    super();
  }

  ngOnInit(): void {
    this.orderService.getBasket().subscribe((x: BasketInfo) => {
      if (!x.details?.length) {
        return;
      }
      let total = 0;
      x.details.forEach(
        (z: OrderDetail) =>
          (total +=
            (z.price ?? 0) * (z.quantity ?? 0) -
            ((z.discount ?? 0) * (z.price ?? 0) * (z.quantity ?? 0)) / 100)
      );
      this.totalForPay = Number.parseInt(total.toString());
    });

    this.orderService.getMakeOrderInfo().subscribe(
      (x) =>
        (this.paymentMethods =
          x?.paymentMethods
            ?.filter(
              (z) =>
                !!z?.title &&
                Object.values(PaymentMethods).some((y) => y === z.title)
            )
            ?.map((z) => {
              return {
                title: z.title,
                description: z.description,
                imageUrl: z.imageUrl,
                method:
                  z.title === PaymentMethods.Visa
                    ? PaymentMethods.Visa
                    : z.title === PaymentMethods.Terminal
                    ? PaymentMethods.Terminal
                    : PaymentMethods.Bank,
              };
            }) ?? [])
    );
  }

  onPay(method: PaymentMethods): void {
    switch (method) {
      case PaymentMethods.Bank:
        this.payViaBank();
        break;
      case PaymentMethods.Terminal:
        this.payViaTerminal();
        break;
      case PaymentMethods.Visa:
        this.payVisa();
        break;
    }
  }

  payViaBank() {
    this.orderService.getBankFile(PaymentMethods.Bank).subscribe((data) => {
      const downloadURL = window.URL.createObjectURL(data);
      window.open(downloadURL);
      this.router.navigateByUrl('');
    });
  }

  payViaTerminal() {
    this.orderService.payTerminal(PaymentMethods.Terminal).subscribe((data) => {
      this.terminalResult = data;
    });
  }

  payVisa() {
    this.paymentViaVisa = true;
  }

  onPayVisa(model: any) {
    this.orderService.payVisa(PaymentMethods.Visa, model).subscribe((_) => {
      this.router.navigateByUrl('');
    });
  }
}
