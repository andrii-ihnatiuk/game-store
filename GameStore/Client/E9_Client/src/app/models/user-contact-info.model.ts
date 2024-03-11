import { BaseModel } from "./base.model";
import { NotificationMethod } from "./notification-method.model";

export class UserContactInfo extends BaseModel {
    name!: string;
    email?: string;
    phoneNumber?: string;
    notificationMethods: NotificationMethod[] = [];
  }