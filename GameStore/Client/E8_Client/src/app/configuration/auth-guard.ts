import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserService } from '../services/user.service';
import { PageRoutes } from './page-routes';
import { links } from './routes-resolver';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private userService: UserService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    const targetPage: PageRoutes = route.data.targetPage;
    const targetIdName: string | undefined = route.data.targetIdName;
    let targetId: string | undefined = undefined;

    if (!!targetIdName?.length) {
        targetId = route.params[targetIdName];
    }

    return this.userService.checkAccess(targetPage, targetId).pipe(
      map((x) => {
        if (x) {
          return true;
        }

        this.router.navigateByUrl(links.get(PageRoutes.Login) ?? '');

        return false;
      })
    );
  }
}
