import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { User } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'gamestore-delete-user',
  templateUrl: './delete-user-page.component.html',
  styleUrls: ['./delete-user-page.component.scss'],
})
export class DeleteUserPageComponent extends BaseComponent implements OnInit {
  user?: User;

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    super();
  }

  get userPageLink(): string | undefined {
    return !!this.user ? `${this.links.get(this.pageRoutes.User)}/${this.user.id}` : undefined;
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'id')
      .pipe(switchMap((id) => this.userService.getUser(id)))
      .subscribe((x) => (this.user = x));
  }

  onDelete(): void {
    this.userService
      .deleteUser(this.user!.id ?? '')
      .subscribe((_) =>
        this.router.navigateByUrl(this.links.get(this.pageRoutes.Users) ?? '')
      );
  }
}
