import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
import { BaseComponent } from '../base.component';

@Component({
  selector: 'gamestore-selector-input',
  templateUrl: './selector-input.component.html',
  styleUrls: ['./selector-input.component.scss'],
})
export class SelectorInputComponent extends BaseComponent {
  @Input()
  control!: FormControl;

  @Input()
  name? = '';

  @Input()
  values: { name: string; value: string }[] = [];
}
