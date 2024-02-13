import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Genre } from 'src/app/models/genre.model';
import { GenreService } from 'src/app/services/genre.service';

@Component({
  selector: 'gamestore-delete-genre',
  templateUrl: './delete-genre-page.component.html',
  styleUrls: ['./delete-genre-page.component.scss'],
})
export class DeleteGenrePageComponent extends BaseComponent implements OnInit {
  genre?: Genre;

  constructor(
    private genreService: GenreService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    super();
  }

  get genrePageLink(): string | undefined {
    return !!this.genre ? `${this.links.get(this.pageRoutes.Genre)}/${this.genre.id}` : undefined;
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'id')
      .pipe(switchMap((id) => this.genreService.getGenre(id)))
      .subscribe((x) => (this.genre = x));
  }

  onDelete(): void {
    this.genreService
      .deleteGenre(this.genre!.id ?? '')
      .subscribe((_) =>
        this.router.navigateByUrl(this.links.get(this.pageRoutes.Genres) ?? '')
      );
  }
}
