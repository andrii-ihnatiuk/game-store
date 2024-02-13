import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../base.component';
import { ListItem } from '../list-item-component/list-item';

@Component({
  selector: 'gamestore-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent extends BaseComponent implements OnInit
{
  mainListItems: ListItem[] = [
    {
      title: this.labels.gamesMenuItem,
      pageLink: this.links.get(this.pageRoutes.Games)
    },
    {
      title: this.labels.genresMenuItem,
      pageLink: this.links.get(this.pageRoutes.Genres)
    },
    {
      title: this.labels.platformsMenuItem,
      pageLink: this.links.get(this.pageRoutes.Platforms)
    },
    {
      title: this.labels.publishersMenuItem,
      pageLink: this.links.get(this.pageRoutes.Publishers)
    },
    {
      title: this.labels.ordersMenuItem,
      pageLink: this.links.get(this.pageRoutes.Orders)
    }
  ] 

  constructor() {
    super();
  }

  ngOnInit(): void {
  }
}
