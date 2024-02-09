import { Component, Inject, Input, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ImageViewerData } from './image-viewer-data';
import { Image } from 'src/app/models/image.model';

@Component({
  selector: 'app-image-viewer-dialog',
  templateUrl: './image-viewer-dialog.component.html',
  styleUrls: ['./image-viewer-dialog.component.scss']
})
export class ImageViewerDialogComponent {

  activeIndex: number;
  loadedIndices = new Set<number>();

  private imagesCount: number;

  constructor(
    @Inject(MAT_DIALOG_DATA) public dialogData: ImageViewerData
  ) {
    this.imagesCount = dialogData.images.length;
    this.activeIndex = dialogData.activeIndex ?? 0;
    this.loadedIndices.add(this.activeIndex);
  }

  selectPrevious(): void {
    if (this.activeIndex - 1 >= 0) {
      this.activeIndex -= 1;
      this.loadedIndices.add(this.activeIndex);
    }
  }

  selectNext(): void {
    if (this.activeIndex + 1 <= this.imagesCount - 1) {
      this.activeIndex += 1;
      this.loadedIndices.add(this.activeIndex);
    }
  }

  getImageUrl(image: Image): string | undefined {
    return this.dialogData.showLarge === false ? image.small : image.large
  }
}
