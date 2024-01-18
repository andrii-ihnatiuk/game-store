import { APP_INITIALIZER, ErrorHandler, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HeaderComponent } from './componetns/header-component/header.component';
import { MainPageModule } from './pages/main-page/main-page.module';
import { resolveConfigurations } from './configuration/configuration-resolver';
import { ToastrModule } from 'ngx-toastr';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { GlobalHttpInterceptorService } from './configuration/http-interceptor';
import { GlobalErrorHandlerService } from './configuration/error-handler';
import { GamesPageModule } from './pages/games-page/games-page.module';
import { GamePageModule } from './pages/game-page/game-page.module';
import { UpdateGamePageModule } from './pages/update-game-page/update-game-page.module';
import { DeleteGamePageModule } from './pages/delete-game-page/delete-game-page.module';
import { MatButtonModule } from '@angular/material/button';
import { GenrePageModule } from './pages/genre-page/genre-page.module';
import { GenresPageModule } from './pages/genres-page/genres-page.module';
import { UpdateGenrePageModule } from './pages/update-genre-page/update-genre-page.module';
import { DeleteGenrePageModule } from './pages/delete-genre-page/delete-genre-page.module';
import { PlatformsPageModule } from './pages/platforms-page/platforms-page.module';
import { PlatformPageModule } from './pages/platform-page/platform-page.module';
import { UpdatePlatformPageModule } from './pages/update-platform-page/update-platform-page.module';
import { DeletePlatformPageModule } from './pages/delete-patform-page/delete-platform-page.module';
import { DeletePublisherPageModule } from './pages/delete-publisher-page/delete-publisher-page.module';
import { UpdatePublisherPageModule } from './pages/update-publisher-page/update-publisher-page.module';
import { PublisherPageModule } from './pages/publisher-page/publisher-page.module';
import { PublishersPageModule } from './pages/publishers-page/publishers-page.module';
import { HistoryPageModule } from './pages/history-page/history-page.module';
import { OrderPageModule } from './pages/order-page/order-page.module';
import { BasketPageModule } from './pages/basket-page/basket-page.module';
import { MakeOrderPageModule } from './pages/make-order-page/make-order-page.module';
import { UpdateUserPageModule } from './pages/update-user-page/update-user-page.module';
import { UserPageModule } from './pages/user-page/user-page.module';
import { DeleteUserPageModule } from './pages/delete-user-page/delete-user-page.module';
import { UsersPageModule } from './pages/users-page/users-page.module';
import { RolePageModule } from './pages/role-page/role-page.module';
import { RolesPageModule } from './pages/roles-page/roles-page.module';
import { UpdateRolePageModule } from './pages/update-role-page/update-role-page.module';
import { DeleteRolePageModule } from './pages/delete-role-page/delete-role-page.module';
import { UpdateLoginPageModule } from './pages/login-page/login-page.module';
import { OrdersPageModule } from './pages/orders-page/orders-page.module';
import { UpdateOrderPageModule } from './pages/update-order-page/update-order-page.module';
import { AuthGuard } from './configuration/auth-guard';
import { FooterComponent } from './componetns/footer-component/footer.component';
import { CommonComponentsModule } from "./componetns/common-components.module";

@NgModule({
    declarations: [AppComponent, HeaderComponent, FooterComponent],
    bootstrap: [AppComponent],
    providers: [
        {
            provide: APP_INITIALIZER,
            useFactory: () => resolveConfigurations,
            multi: true,
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: GlobalHttpInterceptorService,
            multi: true,
        },
        { provide: ErrorHandler, useClass: GlobalErrorHandlerService },
        AuthGuard
    ],
    imports: [
        MainPageModule,
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        ToastrModule.forRoot(),
        HttpClientModule,
        GamesPageModule,
        GamePageModule,
        UpdateGamePageModule,
        DeleteGamePageModule,
        MatButtonModule,
        GenrePageModule,
        GenresPageModule,
        UpdateGenrePageModule,
        DeleteGenrePageModule,
        PlatformsPageModule,
        PlatformPageModule,
        UpdatePlatformPageModule,
        DeletePlatformPageModule,
        DeletePublisherPageModule,
        UpdatePublisherPageModule,
        PublisherPageModule,
        PublishersPageModule,
        HistoryPageModule,
        OrderPageModule,
        BasketPageModule,
        MakeOrderPageModule,
        UpdateUserPageModule,
        UserPageModule,
        DeleteUserPageModule,
        UsersPageModule,
        RolePageModule,
        RolesPageModule,
        UpdateRolePageModule,
        DeleteRolePageModule,
        UpdateLoginPageModule,
        OrdersPageModule,
        UpdateOrderPageModule,
        CommonComponentsModule
    ]
})
export class AppModule {}
