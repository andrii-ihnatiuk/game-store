import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { MatButtonModule } from '@angular/material/button';
import { OrderService } from 'src/app/services/order.service';
import { MakeOrderPageComponent } from './make-order-page.component';
import { TerminalResultComponent } from './components/terminal-result-component/terminal-result.component';
import { VisaPaymentComponent } from './components/visa-payment-component/visa-payment.component';

@NgModule({
  declarations: [MakeOrderPageComponent, TerminalResultComponent, VisaPaymentComponent],
  imports: [
    CommonModule,
    CommonComponentsModule,
    AppRoutingModule,
    MatButtonModule,
  ],
  providers: [OrderService],
})
export class MakeOrderPageModule {}
