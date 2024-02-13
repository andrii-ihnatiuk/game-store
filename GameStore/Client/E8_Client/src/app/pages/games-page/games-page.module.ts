import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { GameService } from 'src/app/services/game.service';
import { GamesPageComponent } from './games-page.component';
import { FilterWrapperComponent } from './components/filter-wrapper/filter-wrapper.component';
import { PlatformService } from 'src/app/services/platform.service';
import { GenreService } from 'src/app/services/genre.service';
import { PublisherService } from 'src/app/services/publisher.service';
import { SortWrapperComponent } from './components/sort-wrapper/sort-wrapper.component';
import { PaggingWrapperComponent } from './components/pagging-wrapper/pagging-wrapper.component';
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { AppRoutingModule } from 'src/app/app-routing.module';

@NgModule({
  declarations: [
    GamesPageComponent,
    FilterWrapperComponent,
    SortWrapperComponent,
    PaggingWrapperComponent,
  ],
  imports: [AppRoutingModule, CommonModule, CommonComponentsModule, MatButtonModule, MatExpansionModule],
  providers: [GameService, PlatformService, GenreService, PublisherService],
})
export class GamesPageModule { }
