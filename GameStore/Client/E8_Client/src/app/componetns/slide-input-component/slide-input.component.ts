import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from '../base.component';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-slide-input-component',
  templateUrl: './slide-input.component.html',
  styleUrls: ['./slide-input.component.scss']
})
export class SlideInputComponentComponent extends BaseComponent {
  @Input()
  control!: FormControl;

  @Input()
  name!: string;

}
