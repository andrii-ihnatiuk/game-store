import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { GameService } from 'src/app/services/game.service';
import { GamePageComponent } from './game-page.component';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { MatButtonModule } from '@angular/material/button';
import { GenreService } from 'src/app/services/genre.service';
import { PlatformService } from 'src/app/services/platform.service';
import { PublisherService } from 'src/app/services/publisher.service';
import { OrderService } from 'src/app/services/order.service';
import { AddCommentComponent } from './components/add-comment-component/add-comment.component';
import { CommentComponent } from './components/comment-component/comment.component';
import { CommentsComponent } from './components/comments-component/comments.component';
import { CommentService } from 'src/app/services/comment.service';
import { BanUserComponent } from './components/ban-user-component/ban-user.component';
import { UserService } from 'src/app/services/user.service';

@NgModule({
  declarations: [
    GamePageComponent,
    AddCommentComponent,
    CommentsComponent,
    CommentComponent,
    BanUserComponent
  ],
  imports: [
    CommonModule,
    CommonComponentsModule,
    AppRoutingModule,
    MatButtonModule,
  ],
  providers: [
    GameService,
    GenreService,
    PlatformService,
    PublisherService,
    OrderService,
    CommentService,
    UserService
  ],
})
export class GamePageModule {}
