import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { MatButtonModule } from '@angular/material/button';
import { PlatformPageComponent } from './platform-page.component';
import { PlatformService } from 'src/app/services/platform.service';
import { GameService } from 'src/app/services/game.service';

@NgModule({
  declarations: [PlatformPageComponent],
  imports: [
    CommonModule,
    CommonComponentsModule,
    AppRoutingModule,
    MatButtonModule,
  ],
  providers: [PlatformService, GameService],
})
export class PlatformPageModule {}
