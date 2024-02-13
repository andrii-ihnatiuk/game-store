import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { OrderService } from 'src/app/services/order.service';
import { HistoryPageComponent } from './history-page.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [HistoryPageComponent],
  imports: [
    CommonModule,
    CommonComponentsModule,
    MatDatepickerModule,
    MatInputModule,
    MatNativeDateModule,
    ReactiveFormsModule
  ],
  providers: [OrderService, DatePipe],
})
export class HistoryPageModule {}
