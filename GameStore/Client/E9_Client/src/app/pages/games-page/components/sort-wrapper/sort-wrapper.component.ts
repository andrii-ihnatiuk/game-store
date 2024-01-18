import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { BaseComponent } from 'src/app/componetns/base.component';
import { GameService } from 'src/app/services/game.service';

@Component({
  selector: 'gamestore-sort-wrapper',
  templateUrl: './sort-wrapper.component.html',
  styleUrls: ['./sort-wrapper.component.scss'],
})
export class SortWrapperComponent extends BaseComponent implements OnInit {
  @Input()
  sortControl!: FormControl;

  sortings: { name: string; value: string }[] = [];

  constructor(private gameService: GameService) {
    super();
  }

  ngOnInit(): void {
    this.gameService.getSortingOptions().subscribe((x) => {
      this.sortings = x.map((z) => {
        return { name: z, value: z };
      });
    });
  }
}
