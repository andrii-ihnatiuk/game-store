import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { MatButtonModule } from '@angular/material/button';
import { PublisherPageComponent } from './publisher-page.component';
import { PublisherService } from 'src/app/services/publisher.service';
import { GameService } from 'src/app/services/game.service';

@NgModule({
  declarations: [PublisherPageComponent],
  imports: [CommonModule, CommonComponentsModule, AppRoutingModule, MatButtonModule],
  providers: [PublisherService, GameService]
})
export class PublisherPageModule {}
