import { Component, Input } from '@angular/core';
import { BaseComponent } from '../base.component';

@Component({
  selector: 'gamestore-info-wrapper',
  templateUrl: './info-wrapper.component.html',
  styleUrls: ['./info-wrapper.component.scss'],
})
export class InfoWrapperComponent extends BaseComponent {
  @Input()
  addLink?: string;

  @Input()
  deleteLink?: string;

  @Input()
  updateLink?: string;
}
