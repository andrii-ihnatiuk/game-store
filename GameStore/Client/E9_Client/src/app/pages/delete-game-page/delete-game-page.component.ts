import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Game } from 'src/app/models/game.model';
import { GameService } from 'src/app/services/game.service';

@Component({
  selector: 'gamestore-delete-game',
  templateUrl: './delete-game-page.component.html',
  styleUrls: ['./delete-game-page.component.scss'],
})
export class DeleteGamePageComponent extends BaseComponent implements OnInit {
  game?: Game;

  constructor(
    private gameService: GameService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    super();
  }

  get gamePageLink(): string | undefined {
    return !!this.game ? `${this.links.get(this.pageRoutes.Game)}/${this.game.key}` : undefined;
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'key')
      .pipe(switchMap((key) => this.gameService.getGame(key)))
      .subscribe((x) => (this.game = x));
  }

  onDelete(): void {
    this.gameService
      .deleteGame(this.game!.key)
      .subscribe((_) =>
        this.router.navigateByUrl(this.links.get(this.pageRoutes.Games) ?? '')
      );
  }
}
