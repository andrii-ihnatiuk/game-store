import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { ReactiveFormsModule } from '@angular/forms';
import { DeleteGenrePageComponent } from './delete-genre-page.component';
import { GenreService } from 'src/app/services/genre.service';

@NgModule({
  declarations: [DeleteGenrePageComponent],
  imports: [CommonModule, CommonComponentsModule, ReactiveFormsModule],
  providers: [GenreService]
})
export class DeleteGenrePageModule {}
