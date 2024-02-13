import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Comment } from 'src/app/models/comment.model';
import { CommentService } from 'src/app/services/comment.service';
import { CommentAction } from '../comment-component/comment-action';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { DeleteWrapperComponent } from 'src/app/componetns/delete-wrapper-component/delete-wrapper.component';
import { map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { BanUserComponent } from '../ban-user-component/ban-user.component';
import { UserService } from 'src/app/services/user.service';
import { AddCommentComponent } from '../add-comment-component/add-comment.component';

@Component({
  selector: 'gamestore-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss'],
})
export class CommentsComponent extends BaseComponent implements OnInit {
  @Input()
  gameKey!: string;

  @Output()
  onCommentsCountChanged: EventEmitter<number> = new EventEmitter();

  comments: Comment[] = [];

  commentsCount: number = 0;

  canAdd = false;

  selectedComment?: { comment: Comment; action: CommentAction };

  constructor(
    private commentsService: CommentService,
    private userService: UserService,
    private dialog: MatDialog
  ) {
    super();
  }

  ngOnInit(): void {
    this.userService
      .checkAccess('AddComment', this.gameKey)
      .subscribe((x) => (this.canAdd = x));
    this.loadComments();
  }

  onSelect(selected: { comment: Comment; action: CommentAction }): void {
    if (selected.action === CommentAction.Like) {
      selected.comment.likesCount += 1;
    }
    else {
      this.selectedComment = selected;
      this.onCreateComment();
    }
  }

  onSelectCancel(): void {
    this.selectedComment = undefined;
  }

  onDeleteComment(comment: Comment): void {
    const deleteDialog = this.dialog.open(DeleteWrapperComponent);
    deleteDialog.componentInstance.name = comment.author;

    this.handleDialog(
      deleteDialog,
      deleteDialog.componentInstance.delete,
      (_) =>
        this.commentsService
          .deleteComment(comment?.id ?? '', this.gameKey)
          .pipe(tap(() => this.loadComments()))
    );
  }

  onBanComment(comment: Comment): void {
    const banDialog = this.dialog.open(BanUserComponent);
    banDialog.componentInstance.name = comment.author;

    this.handleDialog(banDialog, banDialog.componentInstance.ban, (x) =>
      this.commentsService
        .banUser(x, comment.author)
        .pipe(tap(() => this.loadComments()))
    );
  }

  private handleDialog<TComponent, TEvent, TResponse>(
    dialog: MatDialogRef<TComponent, any>,
    event: EventEmitter<TEvent>,
    eventHandler: (content: TEvent) => Observable<TResponse>
  ) {
    const closed = new Subject();
    event
      .pipe(
        takeUntil(closed),
        tap((_) => dialog.close()),
        switchMap((x) => eventHandler(x))
      )
      .subscribe();

    dialog.afterClosed().subscribe((_) => {
      closed.next();
      closed.complete();
    });
  }

  private loadComments(): void {
    this.commentsService
      .getCommentsByGameKey(this.gameKey)
      .pipe(tap(x => this.setZeroLikes(x)))
      .subscribe((x) => {
        this.comments = x;
        this.commentsCount = this.countComments(x);
        this.onCommentsCountChanged.emit(this.commentsCount);
      });
  }

  private setZeroLikes(comments: Comment[]): void {
    comments.map(comment => {
      comment.likesCount = 0;
      if (!!comment.childComments) {
        this.setZeroLikes(comment.childComments);
      }
    })
  }

  private countComments(comments: Comment[]): number {
    let total = 0;
    comments.forEach((comment) => {
      total++;
      if (comment.childComments) {
        total += this.countComments(comment.childComments);
      }
    });
    return total;
  }

  onCreateComment(): void {
    const dialogRef = this.dialog.open(AddCommentComponent);
    dialogRef.componentInstance.currentComment = this.selectedComment;
    dialogRef.afterClosed().subscribe(() => this.onSelectCancel());
    dialogRef.componentInstance.selectCancel.subscribe(() => this.onSelectCancel());

    this.handleDialog(dialogRef, dialogRef.componentInstance.saveComment, (comment) => {
      return this.commentsService
        .addComment(
          comment,
          this.gameKey,
          this.selectedComment?.comment?.id ?? '',
          this.selectedComment?.action ?? ''
        )
        .pipe(tap((x) => {
          this.onSelectCancel();
          if (!!x?.length) {
            this.setZeroLikes(x);
            this.comments = x;
          } else {
            this.loadComments();
          }
        }));
    });
  }
}
