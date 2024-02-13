import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { MatButtonModule } from '@angular/material/button';
import { GenrePageComponent } from './genre-page.component';
import { GenreService } from 'src/app/services/genre.service';
import { GameService } from 'src/app/services/game.service';

@NgModule({
  declarations: [GenrePageComponent],
  imports: [CommonModule, CommonComponentsModule, AppRoutingModule, MatButtonModule],
  providers: [GenreService, GameService]
})
export class GenrePageModule {}
