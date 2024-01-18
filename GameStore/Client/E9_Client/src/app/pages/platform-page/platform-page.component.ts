import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { InfoItem } from 'src/app/componetns/info-component/info-item';
import { Game } from 'src/app/models/game.model';
import { Platform } from 'src/app/models/platform.model';
import { GameService } from 'src/app/services/game.service';
import { PlatformService } from 'src/app/services/platform.service';

@Component({
  selector: 'gamestore-platform',
  templateUrl: './platform-page.component.html',
  styleUrls: ['./platform-page.component.scss'],
})
export class PlatformPageComponent extends BaseComponent implements OnInit {
  platformValue?: Platform;

  platformInfoList: InfoItem[] = [];

  constructor(
    private platformService: PlatformService,
    private route: ActivatedRoute,
    private gameService: GameService
  ) {
    super();
  }

  get deletePlatformLink(): string {
    return `${this.links.get(this.pageRoutes.DeletePlatform)}/${
      this.platformValue?.id
    }`;
  }

  get updatePlatformLink(): string {
    return `${this.links.get(this.pageRoutes.UpdatePlatform)}/${
      this.platformValue?.id
    }`;
  }

  get platform(): Platform | undefined {
    return this.platformValue;
  }

  set platform(value: Platform | undefined) {
    this.platformValue = value;
    this.platformInfoList = [];
    if (!value) {
      return;
    }

    this.platformInfoList.push({
      name: this.labels.platformTypeLabel,
      value: value.type,
    });
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'id')
      .pipe(
        switchMap((id) => this.platformService.getPlatform(id)),
        tap((x) => (this.platform = x)),
        switchMap((x) =>
          !!x?.id?.length
            ? this.gameService.getGamesByPlatfrom(x.id ?? '')
            : of([])
        )
      )
      .subscribe((x) => this.addGamesInfo(x));
  }

  addGamesInfo(games: Game[]): void {
    if (!games?.length) {
      return;
    }

    const gamesInfo: InfoItem = {
      name: this.labels.gamesMenuItem,
      nestedValues: [],
    };
    games.forEach((x) =>
      gamesInfo.nestedValues!.push({
        title: x.name,
        pageLink: `${this.links.get(this.pageRoutes.Game)}/${x.key}`,
      })
    );

    this.platformInfoList.push(gamesInfo);
  }
}
