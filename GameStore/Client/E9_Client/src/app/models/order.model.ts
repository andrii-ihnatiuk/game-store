import { BaseModel } from './base.model';

export class Order extends BaseModel {
  customerId!: string;

  orderDate?: Date;
  shippedDate?: Date;
}
