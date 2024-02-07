import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { InfoItem } from 'src/app/componetns/info-component/info-item';
import { Game } from 'src/app/models/game.model';
import { Genre } from 'src/app/models/genre.model';
import { Platform } from 'src/app/models/platform.model';
import { Publisher } from 'src/app/models/publisher.model';
import { GameService } from 'src/app/services/game.service';
import { GenreService } from 'src/app/services/genre.service';
import { OrderService } from 'src/app/services/order.service';
import { PlatformService } from 'src/app/services/platform.service';
import { PublisherService } from 'src/app/services/publisher.service';
import { UserService } from 'src/app/services/user.service';
import { GameInfo } from './game-info';

@Component({
  selector: 'gamestore-game',
  templateUrl: './game-page.component.html',
  styleUrls: ['./game-page.component.scss'],
})
export class GamePageComponent
  extends BaseComponent
  implements OnInit, AfterViewInit
{
  private file?: Blob;
  private image?: Blob;

  @ViewChild('download')
  downloadLink!: ElementRef;

  gameValue?: Game;

  gameInfo: GameInfo = new GameInfo;

  imageUrl?: SafeUrl;

  canSeeComments = false;
  canBuy = false;
  totalComments = 0;

  constructor(
    private gameService: GameService,
    private genreService: GenreService,
    private platformService: PlatformService,
    private publisherService: PublisherService,
    private orderService: OrderService,
    private route: ActivatedRoute,
    private userService: UserService,
    private router: Router,
    private sanitizer: DomSanitizer
  ) {
    super();
  }

  get deleteGameLink(): string {
    return `${this.links.get(this.pageRoutes.DeleteGame)}/${
      this.gameValue?.key
    }`;
  }

  get updateGameLink(): string {
    return `${this.links.get(this.pageRoutes.UpdateGame)}/${
      this.gameValue?.key
    }`;
  }

  get game(): Game | undefined {
    return this.gameValue;
  }

  set game(value: Game | undefined) {
    this.gameValue = value;
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'key')
      .pipe(
        switchMap((key) => this.gameService.getGame(key)),
        tap((x) => {
          this.game = x;
          if (!!x.fileSize) {
            this.gameInfo.fileInfo = { name: this.labels.gameFileDetailLabel, value: x.fileSize };
          }
        }),
        switchMap((x) =>
          forkJoin({
            genres: this.genreService.getGenresByGameKey(x.key),
            platforms: this.platformService.getPlatformsByGameKey(x.key),
            file: this.gameService.getGameFile(x.key),
            publisher: this.publisherService.getPublisherByGameKey(x.key),
            canSeeComments: this.userService.checkAccess('Comments', x.key),
            canBuy: this.userService.checkAccess('Buy', x.key),
          })
        )
      )
      .subscribe((x) => {
        this.addPlatformsInfo(x.platforms);
        this.addGenresInfo(x.genres);
        this.file = x.file;
        this.addDownloadFile();
        this.addPublisherInfo(x.publisher);
        this.canSeeComments = x.canSeeComments;
        this.canBuy = x.canBuy;
        this.addImage();
      });
  }

  ngAfterViewInit(): void {
    this.addDownloadFile();
  }

  addDownloadFile(): void {
    if (!!this.file && !!this.downloadLink) {
      const downloadURL = window.URL.createObjectURL(this.file);
      (this.downloadLink as any)._elementRef.nativeElement.href = downloadURL;
    }
  }
  
  addImage(): void {
    if (!!this.image) {
      const imageUrl = this.sanitizer.bypassSecurityTrustUrl(
        URL.createObjectURL(this.image)
      );
      this.imageUrl = imageUrl;
    }
  }
  
  addPlatformsInfo(platforms: Platform[]): void {
    if (!platforms?.length) {
      return;
    }

    const platformsInfo: InfoItem = {
      name: this.labels.gamePlatformsDetailLabel,
      nestedValues: [],
    };
    platforms.forEach((x) =>
      platformsInfo.nestedValues!.push({
        title: x.type,
        pageLink: `${this.links.get(this.pageRoutes.Platform)}/${x.id}`,
      })
    );

    this.gameInfo.platformsInfo = platformsInfo;
  }

  addPublisherInfo(publisher: Publisher): void {
    if (!publisher) {
      return;
    }

    this.gameInfo.publisherInfo = {
      name: this.labels.publisherLabel,
      value: publisher.companyName,
      pageLink: !publisher?.id?.length
        ? undefined
        : `${this.links.get(this.pageRoutes.Publisher)}/${
            publisher.id
          }`,
    };
  }

  addGenresInfo(genres: Genre[]): void {
    if (!genres?.length) {
      return;
    }

    const genresInfo: InfoItem = {
      name: this.labels.genresMenuItem,
      nestedValues: [],
    };
    genres.forEach((x) =>
      genresInfo.nestedValues!.push({
        title: x.name,
        pageLink: !x?.id?.length
          ? undefined
          : `${this.links.get(this.pageRoutes.Genre)}/${x.id}`,
      })
    );

    this.gameInfo.genresInfo = genresInfo;
  }

  buy(): void {
    this.orderService
      .buyGame(this.game!.key)
      .subscribe((_) =>
        this.router.navigateByUrl(this.links.get(this.pageRoutes.Basket)!)
      );
  }

  onCommentsLoaded(commentsCount: number) {
    this.totalComments = commentsCount;
  }
}
