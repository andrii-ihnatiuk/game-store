import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BaseComponent } from 'src/app/componetns/base.component';

@Component({
  selector: 'gamestore-visa-payment',
  templateUrl: './visa-payment.component.html',
  styleUrls: ['./visa-payment.component.scss'],
})
export class VisaPaymentComponent extends BaseComponent implements OnInit {
  form?: FormGroup;

  @Output()
  payVisa = new EventEmitter<any>();

  constructor(private builder: FormBuilder) {
    super();
  }

  getFormControl(name: string): FormControl {
    return this.form?.get(name) as FormControl;
  }

  ngOnInit(): void {
    this.form = this.builder.group({
      holder: [''],
      cardNumber: [''],
      monthExpire: [''],
      yearExpire: [''],
      cvv2: [''],
    });
  }
}
