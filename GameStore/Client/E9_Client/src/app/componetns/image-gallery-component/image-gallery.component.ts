import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { GalleryItem } from './gallery-item';

@Component({
  selector: 'app-image-gallery',
  templateUrl: './image-gallery.component.html',
  styleUrls: ['./image-gallery.component.scss']
})
export class ImageGalleryComponent implements OnInit {

  @ViewChild("imgGallery")
  gallery!: ElementRef;

  @Input()
  images: GalleryItem[] = [];

  @Output("select")
  select = new EventEmitter<string>();


  constructor() { }

  ngOnInit(): void {
  }

  scrollLeft(): void {
    this.gallery.nativeElement.scrollLeft -= 300; 
  }

  scrollRight(): void {
    this.gallery.nativeElement.scrollLeft += 300; 
  }

  onImageSelection(id: string): void {
    this.select.emit(id);
  }

}
