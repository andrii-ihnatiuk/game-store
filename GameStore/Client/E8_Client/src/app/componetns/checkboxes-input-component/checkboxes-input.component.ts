import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { BaseComponent } from '../base.component';

@Component({
  selector: 'gamestore-checkboxes-input',
  templateUrl: './checkboxes-input.component.html',
  styleUrls: ['./checkboxes-input.component.scss'],
})
export class CheckboxesInputComponent extends BaseComponent {
  @Input()
  controls!: FormControl[];

  @Input()
  items!: string[];

  @Input()
  name = '';

  getItem(index: number): string {
    return this.items[index];
  }
}
