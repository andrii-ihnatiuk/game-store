import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { MatButtonModule } from '@angular/material/button';
import { GameService } from 'src/app/services/game.service';
import { OrderService } from 'src/app/services/order.service';
import { OrderPageComponent } from './order-page.component';

@NgModule({
  declarations: [OrderPageComponent],
  imports: [
    CommonModule,
    CommonComponentsModule,
    AppRoutingModule,
    MatButtonModule,
  ],
  providers: [OrderService, GameService, DatePipe],
})
export class OrderPageModule {}
