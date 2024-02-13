import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
import { InputErrorStateMatcher } from 'src/app/configuration/input-error-matcher';
import { BaseComponent } from '../base.component';

@Component({
  selector: 'gamestore-textarea-input',
  templateUrl: './textarea-input.component.html',
  styleUrls: ['./textarea-input.component.scss'],
})
export class TextareaInputComponent extends BaseComponent {
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
