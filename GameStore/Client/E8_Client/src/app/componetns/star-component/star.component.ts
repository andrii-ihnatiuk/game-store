import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BaseComponent } from '../base.component';
import { StarSize } from './star-size';

const DEFAULT_COLOR = '#D8D8D8';
const STARS_PADDING = 6;

@Component({
  selector: 'app-rating-star',
  templateUrl: './star.component.html',
  styleUrls: ['./star.component.scss']
})
export class RatingStarComponent extends BaseComponent implements OnInit {
  private _svgColor = DEFAULT_COLOR;

  starSizePx!: string;
  starsPadding: number = 6;

  @Input()
  readonly: boolean = false;

  @Input()
  size: StarSize = StarSize.Normal;

  @Input()
  set svgColor(color: string | undefined) {
    this._svgColor = !!color ? color : DEFAULT_COLOR;
  }

  get svgColor() {
    return this._svgColor;
  }

  get btnStyle() {
    const sizeWithPadding = `${this.size + STARS_PADDING}px`;
    return {
      'width': sizeWithPadding,
      'height': sizeWithPadding,
      'line-height': sizeWithPadding
    };
  }

  @Output()
  onSelection = new EventEmitter<void>();

  ngOnInit(): void {
    this.starSizePx = `${this.size}px`;
  }
}
