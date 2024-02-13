import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Platform } from 'src/app/models/platform.model';
import { PlatformService } from 'src/app/services/platform.service';

@Component({
  selector: 'gamestore-delete-platform',
  templateUrl: './delete-platform-page.component.html',
  styleUrls: ['./delete-platform-page.component.scss'],
})
export class DeletePlatformPageComponent extends BaseComponent implements OnInit {
  platform?: Platform;

  constructor(
    private platformService: PlatformService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    super();
  }

  get platformPageLink(): string | undefined {
    return !!this.platform ? `${this.links.get(this.pageRoutes.Platform)}/${this.platform.id}` : undefined;
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'id')
      .pipe(switchMap((id) => this.platformService.getPlatform(id)))
      .subscribe((x) => (this.platform = x));
  }

  onDelete(): void {
    this.platformService
      .deletePlatform(this.platform!.id ?? '')
      .subscribe((_) =>
        this.router.navigateByUrl(this.links.get(this.pageRoutes.Platforms) ?? '')
      );
  }
}
