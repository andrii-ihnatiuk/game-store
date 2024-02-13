import { Component, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Role } from 'src/app/models/role.model';
import { RoleService } from 'src/app/services/role.service';

@Component({
  selector: 'gamestore-update-role',
  templateUrl: './update-role-page.component.html',
  styleUrls: ['./update-role-page.component.scss'],
})
export class UpdateRolePageComponent extends BaseComponent implements OnInit {
  form?: FormGroup;
  rolePageLink?: string;
  permissionItems: string[] = [];

  rolePermissions: string[] = [];

  constructor(
    private roleService: RoleService,
    private route: ActivatedRoute,
    private builder: FormBuilder,
    private router: Router
  ) {
    super();
  }

  getFormControlArray(name: string): FormControl[] {
    return (this.form?.get(name) as FormArray).controls.map(
      (x) => x as FormControl
    );
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'id')
      .pipe(
        switchMap((id) =>
          !!id?.length ? this.roleService.getRole(id) : of(undefined)
        )
      )
      .pipe(
        switchMap((x) =>
          forkJoin({
            permissions: this.roleService.getPermissions(),
            role: of(x),
            rolePermissions: !!x?.id?.length
              ? this.roleService.getRolePermissions(x.id)
              : of([]),
          })
        )
      )
      .subscribe((x) => {
        this.permissionItems = x.permissions;
        this.rolePermissions = x.rolePermissions;

        this.createForm(x.role);
      });
  }

  getFormControl(name: string): FormControl {
    return this.form?.get(name) as FormControl;
  }

  onSave(): void {
    const role: Role = {
      id: this.form!.value.id,
      name: this.form!.value.name,
    };
    const selectedPermissions = this.permissionItems
      .filter((x, i) => !!this.form!.value.permissions[i])
      .map((x) => x ?? '');

    (!!role.id
      ? this.roleService.updateRole(role, selectedPermissions)
      : this.roleService.addRole(role, selectedPermissions)
    ).subscribe((_) =>
      this.router.navigateByUrl(
        !!role.id
          ? this.links.get(this.pageRoutes.Role) + `/${role.id}`
          : this.links.get(this.pageRoutes.Roles) ?? ''
      )
    );
  }

  private createForm(role?: Role): void {
    this.rolePageLink = !!role
      ? `${this.links.get(this.pageRoutes.Role)}/${role.id}`
      : undefined;

    this.form = this.builder.group({
      id: [role?.id ?? ''],
      name: [role?.name ?? '', Validators.required],
      permissions: this.builder.array(
        this.permissionItems.map((x) => this.rolePermissions.includes(x))
      ),
    });
  }
}
