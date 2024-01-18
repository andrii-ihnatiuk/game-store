import { BaseModel } from './base.model';

export class Publisher extends BaseModel {
  companyName!: string;
  homePage?: string;
  description?: string;
}
