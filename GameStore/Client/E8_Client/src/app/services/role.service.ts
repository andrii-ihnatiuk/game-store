import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoaderService } from '../componetns/loader-component/loader.service';
import { appConfiguration } from '../configuration/configuration-resolver';
import { Role } from '../models/role.model';
import { BaseService } from './base.service';

@Injectable()
export class RoleService extends BaseService {
  constructor(http: HttpClient, loaderService: LoaderService) {
    super(http, loaderService);
  }

  getRole(id: string): Observable<Role> {
    return this.get<Role>(
      appConfiguration.roleApiUrl.replace(environment.routeIdIdentifier, id)
    );
  }

  getRoles(): Observable<Role[]> {
    return this.get<Role[]>(appConfiguration.rolesApiUrl);
  }

  getUserRoles(userId: string): Observable<Role[]> {
    return this.get<Role[]>(
      appConfiguration.userRolesApiUrl.replace(
        environment.routeIdIdentifier,
        userId
      )
    );
  }

  getRolePermissions(roleId: string): Observable<string[]> {
    if (!appConfiguration.rolePermissionsApiUrl?.length) {
      return of([]);
    }

    return this.get<string[]>(
      appConfiguration.rolePermissionsApiUrl.replace(
        environment.routeIdIdentifier,
        roleId
      )
    );
  }

  getPermissions(): Observable<string[]> {
    if (!appConfiguration.permissionsApiUrl?.length) {
      return of([]);
    }

    return this.get<string[]>(appConfiguration.permissionsApiUrl);
  }

  addRole(role: Role, permissions: string[]): Observable<any> {
    return this.post(appConfiguration.addRoleApiUrl, { role, permissions });
  }

  updateRole(role: Role, permissions: string[]): Observable<any> {
    return this.put(appConfiguration.updateRoleApiUrl, { role, permissions });
  }

  deleteRole(id: string): Observable<any> {
    return this.delete(
      appConfiguration.deleteRoleApiUrl.replace(
        environment.routeIdIdentifier,
        id
      ),
      {}
    );
  }
}
