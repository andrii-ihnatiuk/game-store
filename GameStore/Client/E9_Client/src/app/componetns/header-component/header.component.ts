import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { gameCountSubject } from 'src/app/configuration/shared-info';
import { UserService } from 'src/app/services/user.service';
import { BaseComponent } from '../base.component';
import { ListItem } from '../list-item-component/list-item';

@UntilDestroy()
@Component({
  selector: 'gamestore-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent extends BaseComponent implements OnInit {
  gameCount: string | null = null;
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
      title: this.labels.usersMenuItem,
      pageLink: this.links.get(this.pageRoutes.Users)
    },
    {
      title: this.labels.rolesMenuItem,
      pageLink: this.links.get(this.pageRoutes.Roles)
    },
    {
      title: this.labels.ordersMenuItem,
      pageLink: this.links.get(this.pageRoutes.Orders)
    },
    {
      title: this.labels.historyMenuItem,
      pageLink: this.links.get(this.pageRoutes.History)
    },
  ]
  
    locales = [
      { name: 'En', value: 'en' },
      { name: 'Ua', value: 'ua' },
      { name: 'Ru', value: 'ru' },
    ];

  control = new FormControl(localStorage.getItem('locale') ?? 'en');

  constructor(private userService: UserService, private router: Router) {
    super();
  }

  get isAuth(): boolean {
    return this.userService.isAuth();
  }

  ngOnInit(): void {
    gameCountSubject
      .pipe(untilDestroyed(this))
      .subscribe((x) => (this.gameCount = x));

    this.control.valueChanges.pipe(untilDestroyed(this)).subscribe((x) => {
      localStorage.setItem('locale', x.toString());
      this.router.navigateByUrl('').then(() => window.location.reload());
    });
  }
}
