import { BaseModel } from "./base.model";
import { OrderDetail } from "./order-detail.model";

export class BasketInfo extends BaseModel{
    subtotal!: number;
    total!: number;
    taxes!: number;

    details!: OrderDetail[];
}