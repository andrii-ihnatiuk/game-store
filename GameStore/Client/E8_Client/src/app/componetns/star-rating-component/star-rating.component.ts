import { Component, Input, OnInit } from '@angular/core';
import { StarSize } from '../star-component/star-size';

@Component({
  selector: 'app-star-rating',
  templateUrl: './star-rating.component.html',
  styleUrls: ['./star-rating.component.scss']
})
export class StarRatingComponent implements OnInit {
  @Input('rating')
  rating: number = 3;

  @Input('starCount')
  starCount: number = 5;

  @Input('showRatingNumber')
  showRatingNumber: boolean = false

  @Input('starSize')
  starSize: StarSize = StarSize.Normal;

  @Input('readonly')
  readonly = false;

  highlightColor: string = "#FF7E2A";
  ratingNumberStyle = {
    'color': this.highlightColor,
    'font-size': `${this.starSize}px`,
    'margin-left': '12px'
  }

  ratingArr: number[] = [];

  constructor() { }

  ngOnInit(): void {
    for (let index = 0; index < this.starCount; index++) {
      this.ratingArr.push(index);
    }
  }

  onRatingSelection(ratingId: number) {
    this.rating = ratingId + 1;
  }

  getStarColor(starId: number) {
    return starId < this.rating ? this.highlightColor : undefined
  }

}
