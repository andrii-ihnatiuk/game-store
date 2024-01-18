import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { BaseComponent } from '../base.component';

@Component({
  selector: 'gamestore-radio-input',
  templateUrl: './radio-input.component.html',
  styleUrls: ['./radio-input.component.scss'],
})
export class RadioInputComponent extends BaseComponent {
  @Input()
  control!: FormControl;

  @Input()
  items!: { value: string; title: string }[];

  @Input()
  name = '';
}
