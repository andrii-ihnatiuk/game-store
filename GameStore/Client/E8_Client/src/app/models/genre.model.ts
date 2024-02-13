import { BaseModel } from './base.model';

export class Genre extends BaseModel {
  name!: string;
    
  parentGenreId?: string;
}
