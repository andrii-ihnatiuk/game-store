import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Subject } from 'rxjs';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Comment } from 'src/app/models/comment.model';
import { CommentAction } from '../comment-component/comment-action';

@UntilDestroy()
@Component({
  selector: 'gamestore-add-comment',
  templateUrl: './add-comment.component.html',
  styleUrls: ['./add-comment.component.scss'],
})
export class AddCommentComponent extends BaseComponent implements OnInit {
  currentCommentValue?: Comment;
  action?: CommentAction;

  @Input()
  set currentComment(
    value: { comment: Comment; action: CommentAction } | undefined
  ) {
    this.action = value?.action;
    this.currentCommentValue = value?.comment;

    this.createForm();
  }

  @Input()
  clearForm = new Subject();

  @Output()
  selectCancel = new EventEmitter();

  @Output()
  saveComment = new EventEmitter<Comment>();

  form?: FormGroup;

  constructor(private builder: FormBuilder) {
    super();
  }

  get actionLabel(): string {
    switch (this.action) {
      case CommentAction.Reply:
        return this.labels.replyButtonLabel;
      case CommentAction.Quote:
        return this.labels.quoteButtonLabel;
      default:
        return '';
    }
  }

  ngOnInit(): void {
    this.createForm();
    this?.clearForm
      .pipe(untilDestroyed(this))
      .subscribe((_) => this.createForm());
  }

  getFormControl(name: string): FormControl {
    return this.form?.get(name) as FormControl;
  }

  onSave(): void {
    const comment: Comment = this.form!.value;
    this.saveComment.emit(comment);
  }

  private createForm(): void {
    this.form = this.builder.group({
      id: [''],
      name: ['', Validators.required],
      body: ['', Validators.required],
    });
  }

  onSelectCancel(): void {
    this.selectCancel.emit();
    this.currentComment = undefined;
  }
}
