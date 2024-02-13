import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { BaseComponent } from 'src/app/componetns/base.component';
import { CommentService } from 'src/app/services/comment.service';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'gamestore-ban-user',
  templateUrl: './ban-user.component.html',
  styleUrls: ['./ban-user.component.scss'],
})
export class BanUserComponent extends BaseComponent implements OnInit {
  banDurations?: { name: string; value: string }[];
  control?: FormControl;

  @Input()
  name!: string;

  @Output()
  ban = new EventEmitter<string>();

  constructor(private commentService: CommentService) {
    super();
  }

  ngOnInit(): void {
    this.commentService.getBanDurations().subscribe(
      (x) =>
        (this.banDurations = x.map((z) => {
          return { name: z, value: z };
        }))
    );

    this.control = new FormControl('', Validators.required);
  }

  onBan(): void {
    if(!this.control?.value){
      return;
    }

    this.ban.emit(this.control.value.toString());
  }
}
