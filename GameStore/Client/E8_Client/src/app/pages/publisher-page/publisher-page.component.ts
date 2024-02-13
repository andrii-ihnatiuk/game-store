import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin, of } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { InfoItem } from 'src/app/componetns/info-component/info-item';
import { Game } from 'src/app/models/game.model';
import { Publisher } from 'src/app/models/publisher.model';
import { GameService } from 'src/app/services/game.service';
import { PublisherService } from 'src/app/services/publisher.service';

@Component({
  selector: 'gamestore-publisher',
  templateUrl: './publisher-page.component.html',
  styleUrls: ['./publisher-page.component.scss'],
})
export class PublisherPageComponent extends BaseComponent implements OnInit {
  publisherValue?: Publisher;

  publisherInfoList: InfoItem[] = [];

  constructor(
    private publisherService: PublisherService,
    private route: ActivatedRoute,
    private gameService: GameService
  ) {
    super();
  }

  get deletePublisherLink(): string {
    return `${this.links.get(this.pageRoutes.DeletePublisher)}/${
      this.publisherValue?.companyName
    }`;
  }

  get updatePublisherLink(): string {
    return `${this.links.get(this.pageRoutes.UpdatePublisher)}/${
      this.publisherValue?.companyName
    }`;
  }

  get publisher(): Publisher | undefined {
    return this.publisherValue;
  }

  set publisher(value: Publisher | undefined) {
    this.publisherValue = value;
    this.publisherInfoList = [];
    if (!value) {
      return;
    }

    this.publisherInfoList.push({
      name: this.labels.publisherCompanyNameLabel,
      value: value.companyName,
    },
    {
      name: this.labels.publisherDescriptionLabel,
      value: this.publisher?.description
    });
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'id')
      .pipe(
        switchMap((companyName) => this.publisherService.getPublisher(companyName)),
        tap((x) => (this.publisher = x)),
        switchMap((x) =>
          forkJoin({
            games: !!x?.companyName ? this.gameService.getGamesByPublisher(x.companyName) : of([]),
          })
        )
      )
      .subscribe((x) => {
        if (!!x.games?.length) {
          this.addGamesInfo(x.games);
        }
      });
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

    this.publisherInfoList.push(gamesInfo);
  }
}
