import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from '../base.component';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-datepicker',
  templateUrl: './datepicker.component.html',
  styleUrls: ['./datepicker.component.scss']
})
export class DatepickerComponent extends BaseComponent {
  @Input()
  control!: FormControl;

  @Input()
  name!: string;

}
