import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { combineLatest } from 'rxjs';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Culture } from 'src/app/models/culture.model';
import { LocalizationService } from 'src/app/services/localization.service';

@UntilDestroy()
@Component({
  selector: 'gamestore-update-platform',
  templateUrl: './update-platform-page.component.html',
  styleUrls: ['./update-platform-page.component.scss'],
})
export class UpdatePlatformPageComponent extends BaseComponent {

  contentCultures: Culture[] = [];

  platfromId?: string;

  constructor(private localizationService: LocalizationService, private route: ActivatedRoute,) {
    super();
  }
  
  ngOnInit(): void {
    combineLatest([
      this.localizationService.cultures$,
      this.getRouteParam(this.route, "id")
    ])
    .pipe(untilDestroyed(this))
    .subscribe(([cultures, id]) => {
      this.platfromId = id;
      this.contentCultures = !!id ? cultures : this.takeOnlyDefaultCulture(cultures);
    })
  }

  private takeOnlyDefaultCulture(cultures: Culture[]): Culture[] {
    var defaultCulture = cultures.find(x => x.isDefault);
    return !!defaultCulture ? [defaultCulture] : [];
  }
}
