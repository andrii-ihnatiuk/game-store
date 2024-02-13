import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { ListItem } from 'src/app/componetns/list-item-component/list-item';
import { RoleService } from 'src/app/services/role.service';

@Component({
  selector: 'gamestore-roles',
  templateUrl: './roles-page.component.html',
  styleUrls: ['./roles-page.component.scss'],
})
export class RolesPageComponent extends BaseComponent implements OnInit {
  rolesList: ListItem[] = [];

  constructor(private roleService: RoleService) {
    super();
  }

  ngOnInit(): void {
    this.roleService
      .getRoles()
      .pipe(
        map((roles) =>
          roles.map((role) => {
            const roleItem: ListItem = {
              title: role.name,
              pageLink: `${this.links.get(this.pageRoutes.Role)}/${role.id}`,
              updateLink: `${this.links.get(this.pageRoutes.UpdateRole)}/${role.id}`,
              deleteLink: `${this.links.get(this.pageRoutes.DeleteRole)}/${role.id}`,
            };

            return roleItem;
          })
        )
      )
      .subscribe((x) => (this.rolesList = x ?? []));
  }
}
