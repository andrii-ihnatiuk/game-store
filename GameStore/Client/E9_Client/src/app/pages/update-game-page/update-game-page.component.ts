import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BehaviorSubject, combineLatest, of } from 'rxjs';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Culture } from 'src/app/models/culture.model';
import { ImageService } from 'src/app/services/image.service';
import { LocalizationService } from 'src/app/services/localization.service';
import { MatDialog } from '@angular/material/dialog';
import { ImageSelectDialogComponent } from 'src/app/componetns/image-select-dialog-component/image-select-dialog.component';
import { Image } from 'src/app/models/image.model';
import { map, switchMap } from 'rxjs/operators';

@UntilDestroy()
@Component({
  selector: 'gamestore-update-game',
  templateUrl: './update-game-page.component.html',
  styleUrls: ['./update-game-page.component.scss'],
})
export class UpdateGamePageComponent extends BaseComponent implements OnInit {

  private _imagesChange$ = new BehaviorSubject<Image[]>([]);
  imagesChange$ = this._imagesChange$.asObservable();

  contentCultures: Culture[] = [];
  gameKey?: string;

  coverImage?: Image;
  gameImages: Image[] = [];

  reservedImages: Image[] = [];

  constructor(
    private localizationService: LocalizationService,
    private route: ActivatedRoute,
    private imageService: ImageService,
    public imageDialog: MatDialog) {
    super();
  }

  ngOnInit(): void {
    combineLatest([
      this.localizationService.cultures$,
      this.getRouteParam(this.route, "key")
    ])
      .pipe(
        switchMap(([cultures, key]) => {
          this.gameKey = key;
          this.contentCultures = !!key ? cultures : this.takeOnlyDefaultCulture(cultures);
          return !!key ? this.imageService.getImagesByGameKey(key) : of([]);
        }),
        untilDestroyed(this)
      )
      .subscribe((images) => {
        this._imagesChange$.next(images);
        this.setupImages(images); 
      })
  }

  selectCoverImage(): void {
    const onSelectCallback = (images: Image[]) => {
      this.removeFromReservedIfSelectedAgain([images[0]])
      if (!!this.coverImage) {
        this.reservedImages.push(this.coverImage);
      }

      this.coverImage = images[0];
      this._imagesChange$.next(this.getCurrentImages());
    }
    this.openSelectImageDialog(false, onSelectCallback);
  }

  selectAdditionalImage(index?: number): void {
    const onSelectCallback = (images: Image[]) => {
      this.removeFromReservedIfSelectedAgain(images);
      if (index !== undefined) {
        const replaced = this.gameImages.splice(index, 1, ...images);
        this.reservedImages.push(replaced[0]);
      }
      else {
        this.gameImages.push(...images);
      }

      this._imagesChange$.next(this.getCurrentImages());
    }

    this.openSelectImageDialog(true, onSelectCallback);
  }

  deleteImage(index: number) {
    if (index === -1) {
      this.reservedImages.push(this.coverImage!);
      this.coverImage = undefined;
    }
    else {
      this.reservedImages.push(this.gameImages.splice(index, 1)[0]);      
    }

    this._imagesChange$.next(this.getCurrentImages());
  }

  private takeOnlyDefaultCulture(cultures: Culture[]): Culture[] {
    var defaultCulture = cultures.find(x => x.isDefault);
    return !!defaultCulture ? [defaultCulture] : [];
  }

  private setupImages(images: Image[]) {
    this.coverImage = images.find(image => image.isCover)
    this.gameImages = images.filter(image => !image.isCover);
  }

  private removeFromReservedIfSelectedAgain(images: Image[]) {
    images.forEach((image) => {
      let index = this.reservedImages.findIndex(replaced => replaced.id === image.id);
      if (index !== -1) {
        this.reservedImages.splice(index, 1);
      }
    });
  }

  private openSelectImageDialog(selectMultiple: boolean, onSelectCallback: any): void {
    const dialogRef = this.imageDialog.open(ImageSelectDialogComponent, {
      data:
      { 
        fetchImages: () => this.imageService.getAvailableImages().pipe(
          map(images => this.prepareImagesCollection(images))
        ),
        uploadImages: (data: FormData) => this.imageService.uploadImages(data),
        multiSelect: selectMultiple
      },
      width: "calc(100% - 100px)",
      height: "calc(100% - 100px)",
      maxWidth: "1500px"
    });

    dialogRef.afterClosed().subscribe(selectedImages => {
      if (!selectedImages || !selectedImages.length) {
        return;
      }

      onSelectCallback(selectedImages);
    });
  }

  private prepareImagesCollection(images: Image[]): Image[] {
    const filtered = images.filter(image => 
      this.coverImage?.id !== image.id
      && !this.gameImages.some(x => x.id === image.id));

    this.reservedImages = this.reservedImages.filter(image => !filtered.some(x => x.id == image.id));
    filtered.push(...this.reservedImages);
    return filtered;
  }

  private getCurrentImages(): Image[] {
    const images = [...this.gameImages];
    this.gameImages.forEach(image => image.isCover = false);
    if (!!this.coverImage) {
      this.coverImage.isCover = true;
      images.push(this.coverImage);
    }

    return images;
  }
}
