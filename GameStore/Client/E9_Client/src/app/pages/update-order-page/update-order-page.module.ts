import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { MatButtonModule } from '@angular/material/button';
import { GameService } from 'src/app/services/game.service';
import { OrderService } from 'src/app/services/order.service';
import { UpdateOrderPageComponent } from './update-order-page.component';
import { MatDialogModule } from '@angular/material/dialog';
import { ChangeCountComponent } from './components/change-count-component/change-count.component';
import { SelectGameComponent } from './components/select-game-component/select-game.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [UpdateOrderPageComponent, ChangeCountComponent,SelectGameComponent],
  imports: [
    CommonModule,
    CommonComponentsModule,
    AppRoutingModule,
    MatButtonModule,
    MatDialogModule,
    ReactiveFormsModule
  ],
  providers: [OrderService, GameService, DatePipe],
})
export class UpdateOrderPageModule {}
