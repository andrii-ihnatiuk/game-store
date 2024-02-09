import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
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
import { ImageService } from 'src/app/services/image.service';
import { Image } from 'src/app/models/image.model';
import { GalleryItem } from 'src/app/componetns/image-gallery-component/gallery-item';
import { MatDialog } from '@angular/material/dialog';
import { ImageViewerDialogComponent } from 'src/app/componetns/image-viewer-dialog-component/image-viewer-dialog.component';

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

  @ViewChild('download')
  downloadLink!: ElementRef;

  gameValue?: Game;

  gameInfo: GameInfo = new GameInfo;

  coverImage?: string;
  gameImages?: Image[];
  galleryImages?: GalleryItem[];

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
    private imageService: ImageService,
    private router: Router,
    private imageViewer: MatDialog
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
            images: this.imageService.getImagesByGameKey(x.key)
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
        this.addImages(x.images);
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

  private addImages(images: Image[]) {
    this.gameImages = images;
    this.galleryImages = images.map(i => (
      { id: i.id!, imageUrl: i.small ?? i.large }
   ));
   this.coverImage = images.find(i => i.isCover)?.large ?? 'https://placehold.co/540x445';
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

  onImageSelected(imageId: string): void {
    let index = this.gameImages?.findIndex(i => i.id == imageId)!;
    this.openImageViewer(index);
  }

  private openImageViewer(activeIndex: number) {
    this.imageViewer.open(ImageViewerDialogComponent, {
      panelClass: "dialog-no-padding",
      data: { 
        activeIndex: activeIndex,
        images: this.gameImages,
      }
    });
  }
}
