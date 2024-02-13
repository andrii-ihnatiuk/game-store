import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { GenresPageComponent } from './genres-page.component';
import { GenreService } from 'src/app/services/genre.service';

@NgModule({
  declarations: [GenresPageComponent],
  imports: [CommonModule, CommonComponentsModule],
  providers: [GenreService]
})
export class GenresPageModule {}
