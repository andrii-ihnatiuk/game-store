import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { BaseComponent } from 'src/app/componetns/base.component';
import { GameService } from 'src/app/services/game.service';

@Component({
  selector: 'gamestore-pagging-wrapper',
  templateUrl: './pagging-wrapper.component.html',
  styleUrls: ['./pagging-wrapper.component.scss'],
})
export class PaggingWrapperComponent extends BaseComponent implements OnInit {
  @Input()
  pageCountControl!: FormControl;

  @Input()
  pageControl!: FormControl;

  @Input()
  totalPages: number = 1;

  paggings: { name: string; value: string }[] = [];

  constructor(private gameService: GameService) {
    super();
  }

  get currentPage(): number {
    const pageFromControl = Number.parseInt(this.pageControl.value ?? '1');
    return !!pageFromControl ? pageFromControl : 1;
  }

  ngOnInit(): void {
    this.gameService.getPaggingOptions().subscribe((x) => {
      this.paggings = x.map((z) => {
        return { name: z, value: z };
      });
    });
  }

  onPrevChange(): void {
    if (this.currentPage <= 1) {
      return;
    }

    this.pageControl.setValue(this.currentPage - 1);
  }

  onNextChange(): void {
    if (this.currentPage >= this.totalPages) {
      return;
    }

    this.pageControl.setValue(this.currentPage + 1);
  }
}
