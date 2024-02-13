import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/componetns/base.component';
import { InfoItem } from 'src/app/componetns/info-component/info-item';

@Component({
  selector: 'gamestore-terminal-result',
  templateUrl: './terminal-result.component.html',
  styleUrls: ['./terminal-result.component.scss'],
})
export class TerminalResultComponent extends BaseComponent {
  resultInfoList: InfoItem[] = [];

  @Input()
  set terminalResult(value: {
    userId: string;
    orderId: string;
    sum: number;
  } | undefined) {
    this.resultInfoList = [];
    if (!value) {
      return;
    }

    this.resultInfoList.push(
      {
        name: 'User Id',
        value: value.userId,
      },
      {
        name: 'Order Id',
        value: value.orderId,
      },      {
        name: 'Sum',
        value: value.sum.toString(),
      }
    );
  }
}
