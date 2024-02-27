import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { combineLatest } from 'rxjs';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Culture } from 'src/app/models/culture.model';
import { LocalizationService } from 'src/app/services/localization.service';

@UntilDestroy()
@Component({
  selector: 'gamestore-update-game',
  templateUrl: './update-game-page.component.html',
  styleUrls: ['./update-game-page.component.scss'],
})
export class UpdateGamePageComponent extends BaseComponent implements OnInit {

  contentCultures: Culture[] = [];

  gameKey?: string;

  constructor(private localizationService: LocalizationService, private route: ActivatedRoute,) {
    super();
  }

  ngOnInit(): void {
    combineLatest([
      this.localizationService.cultures$,
      this.getRouteParam(this.route, "key")
    ])
    .pipe(untilDestroyed(this))
    .subscribe(([cultures, key]) => {
      this.gameKey = key;
      this.contentCultures = !!key ? cultures : this.takeOnlyDefaultCulture(cultures);
    })
  }

  private takeOnlyDefaultCulture(cultures: Culture[]): Culture[] {
    var defaultCulture = cultures.find(x => x.isDefault);
    return !!defaultCulture ? [defaultCulture] : [];
  }
}
