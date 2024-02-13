import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { ListItem } from 'src/app/componetns/list-item-component/list-item';
import { GenreService } from 'src/app/services/genre.service';

@Component({
  selector: 'gamestore-genres',
  templateUrl: './genres-page.component.html',
  styleUrls: ['./genres-page.component.scss'],
})
export class GenresPageComponent extends BaseComponent implements OnInit {
  genresList: ListItem[] = [];

  constructor(private genreService: GenreService) {
    super();
  }

  ngOnInit(): void {
    this.genreService
      .getGenres()
      .pipe(
        map((genres) =>
          genres.map((genre) => {
            const genreItem: ListItem = {
              title: genre.name,
              pageLink: `${this.links.get(this.pageRoutes.Genre)}/${genre.id}`,
              updateLink: `${this.links.get(this.pageRoutes.UpdateGenre)}/${genre.id}`,
              deleteLink: `${this.links.get(this.pageRoutes.DeleteGenre)}/${genre.id}`,
            };

            return genreItem;
          })
        )
      )
      .subscribe((x) => (this.genresList = x ?? []));
  }
}
