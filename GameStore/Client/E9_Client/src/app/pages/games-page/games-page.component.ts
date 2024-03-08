import { HttpParams } from '@angular/common/http';
import { Component } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { GameListItem } from 'src/app/componetns/game-list-item-component/game-list-item';
import { GameService } from 'src/app/services/game.service';

@Component({
  selector: 'gamestore-games',
  templateUrl: './games-page.component.html',
  styleUrls: ['./games-page.component.scss'],
})
export class GamesPageComponent extends BaseComponent {
  gamesList: GameListItem[] = [];
  pagesSubject = new BehaviorSubject<{ totalPages: number; page: number }>({
    totalPages: 1,
    page: 1,
  });

  pageTitle = "Games";

  gamesLoadSubject = new Subject<void>();

  constructor(private gameService: GameService) {
    super();
  }

  loadGames(filterParams: HttpParams): void {
    this.gameService
      .getGames(filterParams)
      .pipe(
        tap((x) => {
          this.pagesSubject.next({
            totalPages: !!x.totalPages ? x.totalPages : 1,
            page: !!x.currentPage ? x.currentPage : 1,
          });
        }),
        map((gamesInfo) =>
          gamesInfo.games.map((game) => {
            const gameItem: GameListItem = {
              price: game.price,
              discount: game.discount,
              title: game.name,
              type: game.type,
              pageLink: `${this.links.get(this.pageRoutes.Game)}/${game.key}`,
              updateLink: `${this.links.get(this.pageRoutes.UpdateGame)}/${game.key}`,
              deleteLink: `${this.links.get(this.pageRoutes.DeleteGame)}/${game.key}`,
              previewImgUrl: game.previewImgUrl,
            };

            return gameItem;
          })
        )
      )
      .subscribe((x) => this.onGamesLoaded(x), () => this.onGamesLoaded([]));
  }

  private onGamesLoaded(games: GameListItem[]): void {
    this.gamesList = games ?? [];
    this.gamesLoadSubject.next();
  }
}
