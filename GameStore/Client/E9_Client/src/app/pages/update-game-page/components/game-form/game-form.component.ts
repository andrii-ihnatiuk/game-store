import { Component, Input, OnChanges } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, Subscription, forkJoin, of } from 'rxjs';
import { finalize, switchMap, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { InputValidator } from 'src/app/configuration/input-validator';
import { Game } from 'src/app/models/game.model';
import { Genre } from 'src/app/models/genre.model';
import { Image } from 'src/app/models/image.model';
import { Platform } from 'src/app/models/platform.model';
import { Publisher } from 'src/app/models/publisher.model';
import { GameService } from 'src/app/services/game.service';
import { GenreService } from 'src/app/services/genre.service';
import { PlatformService } from 'src/app/services/platform.service';
import { PublisherService } from 'src/app/services/publisher.service';

@Component({
  selector: 'app-game-form',
  templateUrl: './game-form.component.html',
  styleUrls: ['./game-form.component.scss']
})
export class GameFormComponent extends BaseComponent implements OnChanges {
  form?: FormGroup;
  genreItems: string[] = [];
  platformItems: string[] = [];
  gamePageLink?: string;

  game?: Game;

  genres: Genre[] = [];
  publishers: { name: string; value: string }[] = [{ name: '-', value: '' }];
  platforms: Platform[] = [];

  gameGenres: Genre[] = [];
  gamePublisher?: Publisher;
  gamePlatforms: Platform[] = [];
  gameImages: Image[] = [];

  @Input()
  culture?: string;

  @Input()
  key?: string;

  @Input()
  onImagesChange?: Observable<Image[]>;

  private gameSub?: Subscription;
  private imagesSub?: Subscription;

  constructor(
    private gameService: GameService,
    private genreService: GenreService,
    private platformService: PlatformService,
    private publisherService: PublisherService,
    private builder: FormBuilder,
    private router: Router  ) {
    super();
  }

  ngOnChanges(): void {
    if (this.gameSub) {
      this.gameSub.unsubscribe();
    }

    if (this.imagesSub) {
      this.imagesSub.unsubscribe();
    }

    if (!this.culture) {
      return;
    }

    localStorage.setItem('overrideLocale', this.culture);

    this.gameSub = of(this.key)
      .pipe(
        switchMap((key) =>
          !!key?.length ? this.gameService.getGame(key) : of(undefined)
        ),
        tap((x) => (this.game = x)),
        switchMap((x) =>
          forkJoin({
            gameGenres: !!x?.key?.length
              ? this.genreService.getGenresByGameKey(x.key)
              : of([]),
            gamePlatforms: !!x?.key?.length
              ? this.platformService.getPlatformsByGameKey(x.key)
              : of([]),
            gamePublisher: !!x?.key?.length
              ? this.publisherService.getPublisherByGameKey(x.key)
              : of(undefined),
            genres: this.genreService.getGenres(),
            platforms: this.platformService.getPlatforms(),
            publishers: this.publisherService.getPublishers(),
          })
        ),
        finalize(() => localStorage.removeItem('overrideLocale'))
      )
      .subscribe((x) => {
        this.platforms = x.platforms;
        x.publishers.forEach((publisher) =>
          this.publishers.push({
            name: publisher.companyName,
            value: publisher.id ?? '',
          })
        );
        this.genres = x.genres;
        this.gameGenres = x.gameGenres;
        this.gamePlatforms = x.gamePlatforms;
        this.gamePublisher = x.gamePublisher;
        this.createForm();
      });

      this.imagesSub = this.onImagesChange?.subscribe((images) => {
        this.gameImages = images;
      });
  }

  getFormControl(name: string): FormControl {
    return this.form?.get(name) as FormControl;
  }

  getFormControlArray(name: string): FormControl[] {
    return (this.form?.get(name) as FormArray).controls.map(
      (x) => x as FormControl
    );
  }

  onSave(): void {
    if (!!this.gameImages.length && !this.gameImages.some(img => img.isCover)) {
      alert("Cover image is required when selecting additional images!");
      return;
    }

    const game: Game = {
      id: this.form!.value.id,
      name: this.form!.value.name,
      type: this.form!.value.type,
      fileSize: this.form!.value.fileSize,
      description: this.form!.value.description,
      key: this.form!.value.key,
      unitInStock: this.form!.value.unitInStock,
      price: this.form!.value.price,
      discount: this.form!.value.discount,
      publishDate: this.form!.value.publishDate || null,
      discontinued: this.form!.value.discontinued,
    };

    const selectedGenres = this.genres
      .filter((x, i) => !!this.form!.value.genres[i])
      .map((x) => x.id ?? '');
    const selectedPlatforms = this.platforms
      .filter((x, i) => !!this.form!.value.platforms[i])
      .map((x) => x.id ?? '');

    const selectedPublisher = this.form!.value.publisher || null;

    (!!game.id
      ? this.gameService.updateGame(
          game,
          selectedGenres,
          selectedPlatforms,
          selectedPublisher,
          this.culture!,
          this.gameImages.map(image => ({ id: image.id, isCover: image.isCover }))
        )
      : this.gameService.addGame(
          game,
          selectedGenres,
          selectedPlatforms,
          selectedPublisher,
        )
    ).subscribe((_) =>
      this.router.navigateByUrl(
        !!game.id
          ? this.links.get(this.pageRoutes.Game) + `/${game.key}`
          : this.links.get(this.pageRoutes.Games) ?? ''
      )
    );
  }

  private createForm(): void {
    this.gamePageLink = !!this.game
      ? `${this.links.get(this.pageRoutes.Game)}/${this.game.key}`
      : undefined;

    this.form = this.builder.group({
      id: [this.game?.id ?? ''],
      name: [this.game?.name ?? '', Validators.required],
      key: [this.game?.key ?? ''],
      type: [this.game?.type ?? ''],
      fileSize: [this.game?.fileSize ?? ''],
      description: [this.game?.description ?? ''],
      unitInStock: [
        this.game?.unitInStock ?? '',
        [Validators.required, InputValidator.getNumberValidator()],
      ],
      price: [
        this.game?.price ?? '',
        [Validators.required, Validators.min(0)],
      ],
      discount: [
        this.game?.discount ?? 0,
        [Validators.required, Validators.min(0), Validators.max(100)]
      ],
      discontinued: [
        this.game?.discontinued ?? false,
        [Validators.required],
      ],
      publishDate: [
        this.game?.publishDate ?? ''
      ],
      publisher: [this.gamePublisher?.id ?? ''],
      genres: this.builder.array(
        this.genres.map((x) => this.gameGenres.some((z) => z.id === x.id))
      ),
      platforms: this.builder.array(
        this.platforms.map((x) => this.gamePlatforms.some((z) => z.id === x.id))
      ),
    });

    this.genreItems = this.genres.map((x) => x.name);
    this.platformItems = this.platforms.map((x) => x.type);
  }
}
