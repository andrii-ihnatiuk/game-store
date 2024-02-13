import { Route, Routes } from '@angular/router';
import { BasketPageComponent } from '../pages/basket-page/basket-page.component';
import { DeleteGamePageComponent } from '../pages/delete-game-page/delete-game-page.component';
import { DeleteGenrePageComponent } from '../pages/delete-genre-page/delete-genre-page.component';
import { DeletePlatformPageComponent } from '../pages/delete-patform-page/delete-platform-page.component';
import { DeletePublisherPageComponent } from '../pages/delete-publisher-page/delete-publisher-page.component';
import { GamePageComponent } from '../pages/game-page/game-page.component';
import { GamesPageComponent } from '../pages/games-page/games-page.component';
import { GenrePageComponent } from '../pages/genre-page/genre-page.component';
import { GenresPageComponent } from '../pages/genres-page/genres-page.component';
import { MakeOrderPageComponent } from '../pages/make-order-page/make-order-page.component';
import { OrderPageComponent } from '../pages/order-page/order-page.component';
import { HistoryPageComponent } from '../pages/history-page/history-page.component';
import { PlatformPageComponent } from '../pages/platform-page/platform-page.component';
import { PlatformsPageComponent } from '../pages/platforms-page/platforms-page.component';
import { PublisherPageComponent } from '../pages/publisher-page/publisher-page.component';
import { PublishersPageComponent } from '../pages/publishers-page/publishers-page.component';
import { UpdateGamePageComponent } from '../pages/update-game-page/update-game-page.component';
import { UpdateGenrePageComponent } from '../pages/update-genre-page/update-genre-page.component';
import { UpdatePlatformPageComponent } from '../pages/update-platform-page/update-platform-page.component';
import { UpdatePublisherPageComponent } from '../pages/update-publisher-page/update-publisher-page.component';
import { PageRoutes } from './page-routes';
import { UpdateUserPageComponent } from '../pages/update-user-page/update-user-page.component';
import { DeleteUserPageComponent } from '../pages/delete-user-page/delete-user-page.component';
import { UserPageComponent } from '../pages/user-page/user-page.component';
import { UsersPageComponent } from '../pages/users-page/users-page.component';
import { UpdateRolePageComponent } from '../pages/update-role-page/update-role-page.component';
import { DeleteRolePageComponent } from '../pages/delete-role-page/delete-role-page.component';
import { RolePageComponent } from '../pages/role-page/role-page.component';
import { LoginPageComponent } from '../pages/login-page/login-page.component';
import { RolesPageModule } from '../pages/roles-page/roles-page.module';
import { RolesPageComponent } from '../pages/roles-page/roles-page.component';
import { OrdersPageComponent } from '../pages/orders-page/orders-page.component';
import { UpdateOrderPageComponent } from '../pages/update-order-page/update-order-page.component';
import { AuthGuard } from './auth-guard';

export const links = new Map<string, string>();

export async function resolveRoutesAsync(): Promise<Routes> {
  const routeConfig = '/assets/configuration/route-configuration.json';
  const response = await fetch(routeConfig);
  const data = await response.json();
  const routes: Route[] = [];

  Object.keys(PageRoutes).forEach((x) => {
    if (!data[x]) {
      return;
    }

    let link = data[x].toString();
    if (!!link.length && link[0].toString() === '/') {
      link = link.substring(1);
    }

    if (!!link.length && link[link.length - 1].toString() === '/') {
      link = link.substring(0, link.length - 1);
    }

    switch (x) {
      case PageRoutes.Games:
        links.set(PageRoutes.Games, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.Games,
          },
          component: GamesPageComponent,
        });
        break;
      case PageRoutes.Game:
        links.set(PageRoutes.Game, '/' + link);
        routes.push({
          path: link + '/:key',
          pathMatch: 'full',
          component: GamePageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.Game,
            targetIdName: 'key',
          },
        });
        break;
      case PageRoutes.DeleteGame:
        links.set(PageRoutes.DeleteGame, '/' + link);
        routes.push({
          path: link + '/:key',
          pathMatch: 'full',
          component: DeleteGamePageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.DeleteGame,
            targetIdName: 'key',
          },
        });
        break;
      case PageRoutes.UpdateGame:
        links.set(PageRoutes.UpdateGame, '/' + link);
        routes.push({
          path: link + '/:key',
          pathMatch: 'full',
          component: UpdateGamePageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.UpdateGame,
            targetIdName: 'key',
          },
        });
        break;
      case PageRoutes.AddGame:
        links.set(PageRoutes.AddGame, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: UpdateGamePageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.AddGame,
          },
        });
        break;

      case PageRoutes.Genres:
        links.set(PageRoutes.Genres, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: GenresPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.Genres,
          },
        });
        break;
      case PageRoutes.Genre:
        links.set(PageRoutes.Genre, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: GenrePageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.Genre,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.DeleteGenre:
        links.set(PageRoutes.DeleteGenre, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: DeleteGenrePageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.DeleteGenre,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.UpdateGenre:
        links.set(PageRoutes.UpdateGenre, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: UpdateGenrePageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.UpdateGenre,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.AddGenre:
        links.set(PageRoutes.AddGenre, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: UpdateGenrePageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.AddGenre,
          },
        });
        break;

      case PageRoutes.Platforms:
        links.set(PageRoutes.Platforms, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: PlatformsPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.Platforms,
          },
        });
        break;
      case PageRoutes.Platform:
        links.set(PageRoutes.Platform, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: PlatformPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.Platform,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.DeletePlatform:
        links.set(PageRoutes.DeletePlatform, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: DeletePlatformPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.DeletePlatform,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.UpdatePlatform:
        links.set(PageRoutes.UpdatePlatform, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: UpdatePlatformPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.UpdatePlatform,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.AddPlatform:
        links.set(PageRoutes.AddPlatform, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: UpdatePlatformPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.AddPlatform,
          },
        });
        break;

      case PageRoutes.Publishers:
        links.set(PageRoutes.Publishers, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: PublishersPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.Publishers,
          },
        });
        break;
      case PageRoutes.Publisher:
        links.set(PageRoutes.Publisher, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: PublisherPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.Publisher,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.DeletePublisher:
        links.set(PageRoutes.DeletePublisher, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: DeletePublisherPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.DeletePublisher,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.UpdatePublisher:
        links.set(PageRoutes.UpdatePublisher, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: UpdatePublisherPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.UpdatePublisher,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.AddPublisher:
        links.set(PageRoutes.AddPublisher, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: UpdatePublisherPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.AddPublisher,
          },
        });
        break;

      case PageRoutes.Basket:
        links.set(PageRoutes.Basket, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: BasketPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.Basket,
          },
        });
        break;
      case PageRoutes.History:
        links.set(PageRoutes.History, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: HistoryPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.History,
          },
        });
        break;
      case PageRoutes.Orders:
        links.set(PageRoutes.Orders, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: OrdersPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.Orders,
          },
        });
        break;
      case PageRoutes.Order:
        links.set(PageRoutes.Order, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: OrderPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.Order,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.UpdateOrder:
        links.set(PageRoutes.UpdateOrder, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: UpdateOrderPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.UpdateOrder,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.MakeOrder:
        links.set(PageRoutes.MakeOrder, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: MakeOrderPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.MakeOrder,
          },
        });
        break;

      case PageRoutes.Users:
        links.set(PageRoutes.Users, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: UsersPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.Users,
          },
        });
        break;
      case PageRoutes.User:
        links.set(PageRoutes.User, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: UserPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.User,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.DeleteUser:
        links.set(PageRoutes.DeleteUser, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: DeleteUserPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.DeleteUser,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.UpdateUser:
        links.set(PageRoutes.UpdateUser, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: UpdateUserPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.UpdateUser,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.AddUser:
        links.set(PageRoutes.AddUser, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: UpdateUserPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.AddUser,
          },
        });
        break;

      case PageRoutes.Roles:
        links.set(PageRoutes.Roles, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: RolesPageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.Roles,
          },
        });
        break;
      case PageRoutes.Role:
        links.set(PageRoutes.Role, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: RolePageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.Role,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.DeleteRole:
        links.set(PageRoutes.DeleteRole, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: DeleteRolePageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.DeleteRole,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.UpdateRole:
        links.set(PageRoutes.UpdateRole, '/' + link);
        routes.push({
          path: link + '/:id',
          pathMatch: 'full',
          component: UpdateRolePageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.UpdateRole,
            targetIdName: 'id',
          },
        });
        break;
      case PageRoutes.AddRole:
        links.set(PageRoutes.AddRole, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: UpdateRolePageComponent,
          canActivate: [AuthGuard],
          data: {
            targetPage: PageRoutes.AddRole,
          },
        });
        break;

      default:
        links.set(PageRoutes.Login, '/' + link);
        routes.push({
          path: link,
          pathMatch: 'full',
          component: LoginPageComponent,
        });
        break;
    }
  });

  return routes;
}
