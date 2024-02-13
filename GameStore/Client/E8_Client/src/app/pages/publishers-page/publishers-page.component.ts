import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { ListItem } from 'src/app/componetns/list-item-component/list-item';
import { PublisherService } from 'src/app/services/publisher.service';

@Component({
  selector: 'gamestore-publishers',
  templateUrl: './publishers-page.component.html',
  styleUrls: ['./publishers-page.component.scss'],
})
export class PublishersPageComponent extends BaseComponent implements OnInit {
  publishersList: ListItem[] = [];

  constructor(private publisherService: PublisherService) {
    super();
  }

  ngOnInit(): void {
    this.publisherService
      .getPublishers()
      .pipe(
        map((publishers) =>
          publishers.map((publisher) => {
            const publisherItem: ListItem = {
              title: publisher.companyName,
              pageLink: `${this.links.get(this.pageRoutes.Publisher)}/${publisher.companyName}`,
              updateLink: `${this.links.get(this.pageRoutes.UpdatePublisher)}/${publisher.companyName}`,
              deleteLink: `${this.links.get(this.pageRoutes.DeletePublisher)}/${publisher.companyName}`,
            };

            return publisherItem;
          })
        )
      )
      .subscribe((x) => (this.publishersList = x ?? []));
  }
}
