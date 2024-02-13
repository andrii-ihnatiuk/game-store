import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin, of } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { InfoItem } from 'src/app/componetns/info-component/info-item';
import { Game } from 'src/app/models/game.model';
import { Genre } from 'src/app/models/genre.model';
import { GameService } from 'src/app/services/game.service';
import { GenreService } from 'src/app/services/genre.service';

@Component({
  selector: 'gamestore-genre',
  templateUrl: './genre-page.component.html',
  styleUrls: ['./genre-page.component.scss'],
})
export class GenrePageComponent extends BaseComponent implements OnInit {
  genreValue?: Genre;

  genreInfoList: InfoItem[] = [];

  constructor(
    private genreService: GenreService,
    private route: ActivatedRoute,
    private gameService: GameService
  ) {
    super();
  }

  get deleteGenreLink(): string {
    return `${this.links.get(this.pageRoutes.DeleteGenre)}/${
      this.genreValue?.id
    }`;
  }

  get updateGenreLink(): string {
    return `${this.links.get(this.pageRoutes.UpdateGenre)}/${
      this.genreValue?.id
    }`;
  }

  get genre(): Genre | undefined {
    return this.genreValue;
  }

  set genre(value: Genre | undefined) {
    this.genreValue = value;
    this.genreInfoList = [];
    if (!value) {
      return;
    }

    this.genreInfoList.push({
      name: this.labels.genreNameLabel,
      value: value.name,
    });
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'id')
      .pipe(
        switchMap((id) => this.genreService.getGenre(id)),
        tap((x) => (this.genre = x)),
        switchMap((x) =>
          forkJoin({
            parent: !!x?.parentGenreId
              ? this.genreService.getGenre(x.parentGenreId)
              : of(undefined),
            nested: !!x?.id
              ? this.genreService.getGenresByParent(x.id)
              : of([]),
            games: !!x?.id ? this.gameService.getGamesByGenre(x.id) : of([]),
          })
        )
      )
      .subscribe((x) => {
        if (!!x.parent) {
          this.addParentInfo(x.parent);
        }
        if (!!x.nested?.length) {
          this.addNestedGenresInfo(x.nested);
        }
        if (!!x.games?.length) {
          this.addGamesInfo(x.games);
        }
      });
  }

  addParentInfo(genre: Genre): void {
    const parentInfo: InfoItem = {
      name: this.labels.genreParentLabel,
      value: genre.name,
      pageLink: `${this.links.get(this.pageRoutes.Genre)}/${genre.id}`,
    };

    this.genreInfoList.push(parentInfo);
  }

  addNestedGenresInfo(genres: Genre[]): void {
    if (!genres?.length) {
      return;
    }

    const genresInfo: InfoItem = {
      name: this.labels.genreNestedLabel,
      nestedValues: [],
    };
    genres.forEach((x) =>
      genresInfo.nestedValues!.push({
        title: x.name,
        pageLink: `${this.links.get(this.pageRoutes.Genre)}/${x.id}`,
      })
    );

    this.genreInfoList.push(genresInfo);
  }

  addGamesInfo(games: Game[]): void {
    if (!games?.length) {
      return;
    }

    const gamesInfo: InfoItem = {
      name: this.labels.gamesMenuItem,
      nestedValues: [],
    };
    games.forEach((x) =>
      gamesInfo.nestedValues!.push({
        title: x.name,
        pageLink: `${this.links.get(this.pageRoutes.Game)}/${x.key}`,
      })
    );

    this.genreInfoList.push(gamesInfo);
  }
}
