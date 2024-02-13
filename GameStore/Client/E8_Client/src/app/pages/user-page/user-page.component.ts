import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin, of } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { InfoItem } from 'src/app/componetns/info-component/info-item';
import { Game } from 'src/app/models/game.model';
import { Role } from 'src/app/models/role.model';
import { User } from 'src/app/models/user.model';
import { GameService } from 'src/app/services/game.service';
import { RoleService } from 'src/app/services/role.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'gamestore-user',
  templateUrl: './user-page.component.html',
  styleUrls: ['./user-page.component.scss'],
})
export class UserPageComponent extends BaseComponent implements OnInit {
  userValue?: User;

  userInfoList: InfoItem[] = [];

  constructor(
    private userService: UserService,
    private roleService: RoleService,
    private route: ActivatedRoute
  ) {
    super();
  }

  get deleteUserLink(): string {
    return `${this.links.get(this.pageRoutes.DeleteUser)}/${
      this.userValue?.id
    }`;
  }

  get updateUserLink(): string {
    return `${this.links.get(this.pageRoutes.UpdateUser)}/${
      this.userValue?.id
    }`;
  }

  get user(): User | undefined {
    return this.userValue;
  }

  set user(value: User | undefined) {
    this.userValue = value;
    this.userInfoList = [];
    if (!value) {
      return;
    }

    this.userInfoList.push({
      name: this.labels.userNameLabel,
      value: value.name,
    });
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'id')
      .pipe(
        switchMap((id) => this.userService.getUser(id)),
        tap((x) => (this.user = x)),
        switchMap((x) =>
          forkJoin({
            roles: !!x?.id ? this.roleService.getUserRoles(x.id) : of([]),
          })
        )
      )
      .subscribe((x) => {
        if (!!x.roles?.length) {
          this.addRolesInfo(x.roles);
        }
      });
  }

  addRolesInfo(roles: Role[]): void {
    if (!roles?.length) {
      return;
    }

    const rolesInfo: InfoItem = {
      name: this.labels.rolesMenuItem,
      nestedValues: [],
    };
    roles.forEach((x) =>
      rolesInfo.nestedValues!.push({
        title: x.name,
        pageLink: `${this.links.get(this.pageRoutes.Role)}/${x.id}`,
      })
    );

    this.userInfoList.push(rolesInfo);
  }
}
