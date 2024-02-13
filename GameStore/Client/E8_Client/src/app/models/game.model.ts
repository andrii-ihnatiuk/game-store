import { BaseModel } from './base.model';

export class Game extends BaseModel {
  key!: string;
  name!: string;
  type?: string;
  fileSize?: string;
  description?: string;
  publishDate?: Date;

  price?: number;
  discount?: number;
  unitInStock?: number;
  discontinued?: boolean;
}
