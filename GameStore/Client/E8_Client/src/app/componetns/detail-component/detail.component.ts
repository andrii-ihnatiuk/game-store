import { Component, Input, OnInit } from '@angular/core';
import { InfoItem } from '../info-component/info-item';
import { BaseComponent } from '../base.component';
import { MatChipsModule } from '@angular/material/chips';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.scss']
})
export class DetailComponent extends BaseComponent {

  @Input()
  infoItem: InfoItem = new InfoItem;

  @Input()
  displayChips: boolean = false;
}
