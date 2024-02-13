import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { ListItem } from 'src/app/componetns/list-item-component/list-item';
import { PlatformService } from 'src/app/services/platform.service';

@Component({
  selector: 'gamestore-platforms',
  templateUrl: './platforms-page.component.html',
  styleUrls: ['./platforms-page.component.scss'],
})
export class PlatformsPageComponent extends BaseComponent implements OnInit {
  platformsList: ListItem[] = [];

  constructor(private platformService: PlatformService) {
    super();
  }

  ngOnInit(): void {
    this.platformService
      .getPlatforms()
      .pipe(
        map((platforms) =>
          platforms.map((platform) => {
            const platformItem: ListItem = {
              title: platform.type,
              pageLink: `${this.links.get(this.pageRoutes.Platform)}/${platform.id}`,
              updateLink: `${this.links.get(this.pageRoutes.UpdatePlatform)}/${platform.id}`,
              deleteLink: `${this.links.get(this.pageRoutes.DeletePlatform)}/${platform.id}`,
            };

            return platformItem;
          })
        )
      )
      .subscribe((x) => (this.platformsList = x ?? []));
  }
}
