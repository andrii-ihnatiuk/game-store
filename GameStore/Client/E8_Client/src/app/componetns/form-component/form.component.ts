import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BaseComponent } from '../base.component';

@Component({
  selector: 'gamestore-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss'],
})
export class FormComponent extends BaseComponent {
  @Input()
  idName?: string;

  @Input()
  pageLink?: string;

  @Input()
  form!: FormGroup;

  @Output()
  save = new EventEmitter<void>();
}
