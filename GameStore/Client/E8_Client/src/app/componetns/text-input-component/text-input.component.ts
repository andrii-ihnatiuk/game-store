import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { InputErrorStateMatcher } from 'src/app/configuration/input-error-matcher';
import { BaseComponent } from '../base.component';

@Component({
  selector: 'gamestore-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss'],
})
export class TextInputComponent extends BaseComponent implements OnInit{
  @Input()
  control!: FormControl;

  @Input()
  name? = '';

  @Input()
  disableValidation = false;

  @Input()
  hide = false;

  readonly matcher = new InputErrorStateMatcher();

  ngOnInit(): void {
    this.matcher.disableValidation = this.disableValidation;
  }

  isErrors(): boolean {
    return this.matcher.isErrorState(this.control, null);
  }
}
