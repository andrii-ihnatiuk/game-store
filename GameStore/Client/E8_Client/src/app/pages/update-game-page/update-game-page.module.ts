import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { GameService } from 'src/app/services/game.service';
import { ReactiveFormsModule } from '@angular/forms';
import { UpdateGamePageComponent } from './update-game-page.component';
import { GenreService } from 'src/app/services/genre.service';
import { PlatformService } from 'src/app/services/platform.service';
import { PublisherService } from 'src/app/services/publisher.service';

@NgModule({
  declarations: [UpdateGamePageComponent],
  imports: [CommonModule, CommonComponentsModule, ReactiveFormsModule],
  providers: [GameService, GenreService, PlatformService, PublisherService],
})
export class UpdateGamePageModule {}
