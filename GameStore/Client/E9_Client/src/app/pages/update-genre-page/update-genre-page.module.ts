import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { ReactiveFormsModule } from '@angular/forms';
import { UpdateGenrePageComponent } from './update-genre-page.component';
import { GenreService } from 'src/app/services/genre.service';

@NgModule({
  declarations: [UpdateGenrePageComponent],
  imports: [CommonModule, CommonComponentsModule, ReactiveFormsModule],
  providers: [GenreService]
})
export class UpdateGenrePageModule {}
