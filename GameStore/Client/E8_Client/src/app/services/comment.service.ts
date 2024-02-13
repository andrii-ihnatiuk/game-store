import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoaderService } from '../componetns/loader-component/loader.service';
import { appConfiguration } from '../configuration/configuration-resolver';
import { Comment } from '../models/comment.model';
import { BaseService } from './base.service';

@Injectable()
export class CommentService extends BaseService {
  constructor(http: HttpClient, loaderService: LoaderService) {
    super(http, loaderService);
  }

  getCommentsByGameKey(gameKey: string): Observable<Comment[]> {
    return this.get<Comment[]>(
      appConfiguration.commentsByGameApiUrl.replace(
        environment.routeKeyIdentifier,
        gameKey
      )
    );
  }

  addComment(
    comment: Comment,
    gameKey: string,
    parentId: string,
    action: string
  ): Observable<Comment[]> {
    return this.post(
      appConfiguration.addCommentApiUrl.replace(
        environment.routeKeyIdentifier,
        gameKey
      ),
      {
        comment,
        parentId,
        action,
      }
    );
  }

  banUser(duration: string, user: string): Observable<any> {
    return this.post(appConfiguration.banUserApiUrl, { user, duration });
  }

  deleteComment(id: string, gameKey: string): Observable<any> {
    return this.delete(
      appConfiguration.deleteCommentApiUrl
        .replace(environment.routeKeyIdentifier, gameKey)
        .replace(environment.routeIdIdentifier, id),
      {}
    );
  }

  getBanDurations(): Observable<string[]> {
    return this.get(appConfiguration.getBanDurationsApiUrl);
  }
}
