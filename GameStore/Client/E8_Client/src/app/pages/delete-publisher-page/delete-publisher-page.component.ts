import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Publisher } from 'src/app/models/publisher.model';
import { PublisherService } from 'src/app/services/publisher.service';

@Component({
  selector: 'gamestore-delete-publisher',
  templateUrl: './delete-publisher-page.component.html',
  styleUrls: ['./delete-publisher-page.component.scss'],
})
export class DeletePublisherPageComponent extends BaseComponent implements OnInit {
  publisher?: Publisher;

  constructor(
    private publisherService: PublisherService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    super();
  }

  get publisherPageLink(): string | undefined {
    return !!this.publisher ? `${this.links.get(this.pageRoutes.Publisher)}/${this.publisher.companyName}` : undefined;
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'id')
      .pipe(switchMap((companyName) => this.publisherService.getPublisher(companyName)))
      .subscribe((x) => (this.publisher = x));
  }

  onDelete(): void {
    this.publisherService
      .deletePublisher(this.publisher!.id ?? '')
      .subscribe((_) =>
        this.router.navigateByUrl(this.links.get(this.pageRoutes.Publishers) ?? '')
      );
  }
}
