import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin, of } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { InfoItem } from 'src/app/componetns/info-component/info-item';
import { Role } from 'src/app/models/role.model';
import { RoleService } from 'src/app/services/role.service';

@Component({
  selector: 'gamestore-role',
  templateUrl: './role-page.component.html',
  styleUrls: ['./role-page.component.scss'],
})
export class RolePageComponent extends BaseComponent implements OnInit {
  roleValue?: Role;

  roleInfoList: InfoItem[] = [];

  constructor(private roleService: RoleService, private route: ActivatedRoute) {
    super();
  }

  get deleteRoleLink(): string {
    return `${this.links.get(this.pageRoutes.DeleteRole)}/${
      this.roleValue?.id
    }`;
  }

  get updateRoleLink(): string {
    return `${this.links.get(this.pageRoutes.UpdateRole)}/${
      this.roleValue?.id
    }`;
  }

  get role(): Role | undefined {
    return this.roleValue;
  }

  set role(value: Role | undefined) {
    this.roleValue = value;
    this.roleInfoList = [];
    if (!value) {
      return;
    }

    this.roleInfoList.push({
      name: this.labels.roleNameLabel,
      value: value.name,
    });
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'id')
      .pipe(
        switchMap((id) => this.roleService.getRole(id)),
        tap((x) => (this.role = x)),
        switchMap((x) =>
          forkJoin({
            permissions: !!x?.id
              ? this.roleService.getRolePermissions(x.id)
              : of([]),
          })
        )
      )
      .subscribe((x) => {
        if (!!x.permissions?.length) {
          this.addPermissionsInfo(x.permissions);
        }
      });
  }

  addPermissionsInfo(permissions: string[]): void {
    if (!permissions?.length) {
      return;
    }

    const permissionsInfo: InfoItem = {
      name: this.labels.permissionsLabel,
      nestedValues: [],
    };
    permissions.forEach((x) =>
      permissionsInfo.nestedValues!.push({
        title: x,
      })
    );

    this.roleInfoList.push(permissionsInfo);
  }
}
