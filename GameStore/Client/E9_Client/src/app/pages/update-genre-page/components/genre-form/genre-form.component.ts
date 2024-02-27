import { Component, Input, OnChanges } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { of, forkJoin, Subscription } from 'rxjs';
import { finalize, switchMap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Genre } from 'src/app/models/genre.model';
import { GenreService } from 'src/app/services/genre.service';

@Component({
  selector: 'app-genre-form',
  templateUrl: './genre-form.component.html',
  styleUrls: ['./genre-form.component.scss']
})
export class GenreFormComponent extends BaseComponent implements OnChanges {
  form?: FormGroup;
  genrePageLink?: string;

  genres: { name: string; value: string }[] = [{ name: '-', value: '' }];

  @Input()
  culture?: string;

  @Input()
  id?: string;

  private genreSub?: Subscription;

  constructor(
    private genreService: GenreService,
    private builder: FormBuilder,
    private router: Router
  ) {
    super();
  }

  ngOnChanges(): void {
    if (this.genreSub) {
      this.genreSub.unsubscribe();
    }

    if (!this.culture) {
      return;
    }

    localStorage.setItem('overrideLocale', this.culture);

    this.genreSub = of(this.id)
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
        ),
        finalize(() => localStorage.removeItem('overrideLocale'))
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
      ? this.genreService.updateGenre(genre, this.culture!)
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
