import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Genre } from 'src/app/models/genre.model';
import { GenreService } from 'src/app/services/genre.service';

@Component({
  selector: 'gamestore-update-genre',
  templateUrl: './update-genre-page.component.html',
  styleUrls: ['./update-genre-page.component.scss'],
})
export class UpdateGenrePageComponent extends BaseComponent implements OnInit {
  form?: FormGroup;
  genrePageLink?: string;

  genres: { name: string; value: string }[] = [{ name: '-', value: '' }];

  constructor(
    private genreService: GenreService,
    private route: ActivatedRoute,
    private builder: FormBuilder,
    private router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'id')
      .pipe(
        switchMap((id) =>
          !!id?.length ? this.genreService.getGenre(id) : of(undefined)
        )
      )
      .pipe(
        switchMap((x) =>
          forkJoin({
            genres: this.genreService.getGenres(),
            genre: of(x),
          })
        )
      )
      .subscribe((x) => {
        x.genres.forEach((genre) =>
          this.genres.push({ name: genre.name, value: genre.id ?? '' })
        );
        this.createForm(x.genre);
      });
  }

  getFormControl(name: string): FormControl {
    return this.form?.get(name) as FormControl;
  }

  onSave(): void {
    const genre: Genre = this.form!.value;

    (!!genre.id
      ? this.genreService.updateGenre(genre)
      : this.genreService.addGenre(genre)
    ).subscribe((_) =>
      this.router.navigateByUrl(
        !!genre.id
          ? this.links.get(this.pageRoutes.Genre) + `/${genre.id}`
          : this.links.get(this.pageRoutes.Genres) ?? ''
      )
    );
  }

  private createForm(genre?: Genre): void {
    this.genrePageLink = !!genre
      ? `${this.links.get(this.pageRoutes.Genre)}/${genre.id}`
      : undefined;

    this.form = this.builder.group({
      id: [genre?.id ?? ''],
      parentGenreId: [genre?.parentGenreId ?? ''],
      name: [genre?.name ?? '', Validators.required],
    });
  }
}
