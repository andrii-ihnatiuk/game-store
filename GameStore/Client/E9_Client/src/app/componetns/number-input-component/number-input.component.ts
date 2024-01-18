import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { InputErrorStateMatcher } from 'src/app/configuration/input-error-matcher';
import { BaseComponent } from '../base.component';

@Component({
  selector: 'gamestore-number-input',
  templateUrl: './number-input.component.html',
  styleUrls: ['./number-input.component.scss'],
})
export class NumberInputComponent extends BaseComponent implements OnInit {
  @Input()
  control!: FormControl;

  @Input()
  name? = '';

  @Input()
  disableValidation = false;

  readonly matcher = new InputErrorStateMatcher();

  ngOnInit(): void {
    this.matcher.disableValidation = this.disableValidation;
  }

  isErrors(): boolean {
    return this.matcher.isErrorState(this.control, null);
  }
}
