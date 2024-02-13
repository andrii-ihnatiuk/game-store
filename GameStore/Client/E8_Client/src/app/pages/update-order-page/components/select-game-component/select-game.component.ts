import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { BaseComponent } from 'src/app/componetns/base.component';
import { CommentService } from 'src/app/services/comment.service';
import { GameService } from 'src/app/services/game.service';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'gamestore-select-game',
  templateUrl: './select-game.component.html',
  styleUrls: ['./select-game.component.scss'],
})
export class SelectGameComponent extends BaseComponent implements OnInit {
  games?: { name: string; value: string }[];
  control?: FormControl;

  @Output()
  save = new EventEmitter<string>();

  constructor(private gameService: GameService) {
    super();
  }

  ngOnInit(): void {
    this.gameService.getAllGames().subscribe(
      (x) =>
        (this.games = x.map((z) => {
          return { name: z.name ?? '', value: z.id ?? '' };
        }))
    );

    this.control = new FormControl('', Validators.required);
  }

  onSave(): void {
    if (!this.control?.value) {
      return;
    }

    this.save.emit(this.control.value.toString());
  }
}
