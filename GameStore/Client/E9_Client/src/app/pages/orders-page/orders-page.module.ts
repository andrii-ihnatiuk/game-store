import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { OrderService } from 'src/app/services/order.service';
import { OrdersPageComponent } from './orders-page.component';

@NgModule({
  declarations: [OrdersPageComponent],
  imports: [CommonModule, CommonComponentsModule],
  providers: [OrderService, DatePipe],
})
export class OrdersPageModule {}
