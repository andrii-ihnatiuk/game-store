import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Image } from 'src/app/models/image.model';
import { DialogData } from './dialog-data';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'app-image-select-dialog',
  templateUrl: './image-select-dialog.component.html',
  styleUrls: ['./image-select-dialog.component.scss']
})
export class ImageSelectDialogComponent implements OnInit {

  images: Image[] = [];
  selectedIndices = new Set<number>();

  constructor(
    public dialogRef: MatDialogRef<ImageSelectDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogOpts: DialogData,
  ) { }

  ngOnInit(): void {
    this.refreshImages();
  }

  private refreshImages(): void {
    this.dialogOpts.fetchImages()
      .pipe(untilDestroyed(this))
      .subscribe((images) => {
        this.images = images;
        this.selectedIndices.clear();
      })
  }

  onImageSelect(index: number): void {
    if (this.selectedIndices.has(index)) {
      this.selectedIndices.delete(index);
      return;
    }

    if (!this.dialogOpts.multiSelect) {
      this.selectedIndices.clear();
    }
    
    this.selectedIndices.add(index);     
  }

  handleFileSelect(event: Event) {
    const el = event.target as HTMLInputElement;
    if (!el.files || !el.files.length) {
      return;
    }

    const filesToUpload = Array.from(el.files);
    el.value = '';

    const hasInvalidFile = filesToUpload.some(f => {
      return !this.verifyFileExtension(f) || !this.verifyFileSize(f);
    })

    if (hasInvalidFile) {
      
      return;
    }

    this.uploadImages(filesToUpload);
  }

  closeWithResult(): void {
    const selectedImages = Array.from(this.selectedIndices).map((index) => this.images[index])
    this.dialogRef.close(selectedImages);
  }

  private verifyFileExtension(file: File): boolean {
    let parts = file.name.split('.');
    let extension = parts[parts.length - 1];

    switch (extension) {
      case "jpg":
      case "jpeg":
      case "png":
        return true;
      default:
        alert("Only *.jpg, *.jpeg, *.png files are allowed.");
        return false;
    }
  }

  private verifyFileSize(file: File): boolean {
    const fileSize = file.size / 1024 / 1024;
    if (fileSize > 10) {
      alert("The maximum allowed file size is 10 MB.");
      return false;
    }
    return true;
  }

  private uploadImages(filesToUpload: File[]) {
    const formData = new FormData();
    console.log(filesToUpload);
    filesToUpload.forEach((f) => {
      formData.append("files", f);
    });

    this.dialogOpts.uploadImages(formData)
      .pipe(untilDestroyed(this))
      .subscribe(() => {
        this.refreshImages();
      });
  }
}
