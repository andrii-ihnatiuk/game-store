import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BaseComponent } from '../base.component';

@Component({
  selector: 'gamestore-delete-wrapper',
  templateUrl: './delete-wrapper.component.html',
  styleUrls: ['./delete-wrapper.component.scss'],
})
export class DeleteWrapperComponent extends BaseComponent {
  @Input()
  name!: string;

  @Input()
  pageLink?: string;

  @Output()
  delete = new EventEmitter<void>();
}
