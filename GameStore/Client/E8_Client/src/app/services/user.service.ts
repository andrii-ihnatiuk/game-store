import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { LoaderService } from '../componetns/loader-component/loader.service';
import { appConfiguration } from '../configuration/configuration-resolver';
import { User } from '../models/user.model';
import { BaseService } from './base.service';

@Injectable()
export class UserService extends BaseService {
  constructor(http: HttpClient, loaderService: LoaderService) {
    super(http, loaderService);
  }

  getUser(id: string): Observable<User> {
    return this.get<User>(
      appConfiguration.userApiUrl.replace(environment.routeIdIdentifier, id)
    );
  }

  getUsers(): Observable<User[]> {
    return this.get<User[]>(appConfiguration.usersApiUrl);
  }

  addUser(user: User, roles: string[], password: string): Observable<any> {
    return this.post(appConfiguration.addUserApiUrl, { user, roles, password });
  }

  login(model: {
    login: string;
    password: string;
    internalAuth: boolean;
  }): Observable<{ token: string }> {
    return this.post<any, { token: string }>(appConfiguration.loginApiUrl, {
      model,
    }).pipe(
      tap((x) => {
        const token = x.token;
        this.clearAuth();
        if (!token?.length) {
          return;
        }

        localStorage.setItem('authKey', token);
      })
    );
  }

  updateUser(user: User, roles: string[], password: string): Observable<any> {
    return this.put(appConfiguration.updateUserApiUrl, {
      user,
      roles,
      password,
    });
  }

  deleteUser(id: string): Observable<any> {
    return this.delete(
      appConfiguration.deleteUserApiUrl.replace(
        environment.routeIdIdentifier,
        id
      ),
      {}
    );
  }

  checkAccess(targetPage: string, targetId?: string): Observable<boolean> {
    return this.post(appConfiguration.checkAccessApiUrl, {
      targetPage,
      targetId,
    });
  }

  clearAuth(): void {
    localStorage.removeItem('authKey');
  }

  isAuth(): boolean {
    return !!localStorage.getItem('authKey')?.length;
  }
}
