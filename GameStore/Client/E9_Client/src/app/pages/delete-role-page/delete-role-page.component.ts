import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Role } from 'src/app/models/role.model';
import { RoleService } from 'src/app/services/role.service';

@Component({
  selector: 'gamestore-delete-role',
  templateUrl: './delete-role-page.component.html',
  styleUrls: ['./delete-role-page.component.scss'],
})
export class DeleteRolePageComponent extends BaseComponent implements OnInit {
  role?: Role;

  constructor(
    private roleService: RoleService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    super();
  }

  get rolePageLink(): string | undefined {
    return !!this.role ? `${this.links.get(this.pageRoutes.Role)}/${this.role.id}` : undefined;
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'id')
      .pipe(switchMap((id) => this.roleService.getRole(id)))
      .subscribe((x) => (this.role = x));
  }

  onDelete(): void {
    this.roleService
      .deleteRole(this.role!.id ?? '')
      .subscribe((_) =>
        this.router.navigateByUrl(this.links.get(this.pageRoutes.Roles) ?? '')
      );
  }
}
