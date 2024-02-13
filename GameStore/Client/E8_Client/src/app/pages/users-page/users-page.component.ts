import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { ListItem } from 'src/app/componetns/list-item-component/list-item';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'gamestore-users',
  templateUrl: './users-page.component.html',
  styleUrls: ['./users-page.component.scss'],
})
export class UsersPageComponent extends BaseComponent implements OnInit {
  usersList: ListItem[] = [];

  constructor(private userService: UserService) {
    super();
  }

  ngOnInit(): void {
    this.userService
      .getUsers()
      .pipe(
        map((users) =>
          users.map((user) => {
            const userItem: ListItem = {
              title: user.name,
              pageLink: `${this.links.get(this.pageRoutes.User)}/${user.id}`,
              updateLink: `${this.links.get(this.pageRoutes.UpdateUser)}/${user.id}`,
              deleteLink: `${this.links.get(this.pageRoutes.DeleteUser)}/${user.id}`,
            };

            return userItem;
          })
        )
      )
      .subscribe((x) => (this.usersList = x ?? []));
  }
}
