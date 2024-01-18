import { Component, Input } from '@angular/core';
import { BaseComponent } from '../base.component';
import { InfoItem } from './info-item';

@Component({
  selector: 'gamestore-info',
  templateUrl: './info.component.html',
  styleUrls: ['./info.component.scss'],
})
export class InfoComponent extends BaseComponent {
  @Input()
  infoList: InfoItem[] = [];
}
