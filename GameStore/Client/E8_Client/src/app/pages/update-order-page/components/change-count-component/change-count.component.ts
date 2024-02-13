import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { BaseComponent } from 'src/app/componetns/base.component';
import { CommentService } from 'src/app/services/comment.service';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'gamestore-change-count',
  templateUrl: './change-count.component.html',
  styleUrls: ['./change-count.component.scss'],
})
export class ChangeCountComponent extends BaseComponent implements OnInit {
  control?: FormControl;

  @Input()
  name!: string;

  @Output()
  save = new EventEmitter<number>();

  ngOnInit(): void {
    this.control = new FormControl('', Validators.required);
  }

  onSave(): void {
    if (!this.control?.value) {
      return;
    }

    this.save.emit(Number.parseInt(this.control.value.toString()));
  }
}
