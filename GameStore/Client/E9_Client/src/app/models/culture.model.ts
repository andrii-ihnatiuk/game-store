import { BaseModel } from "./base.model";

export class Culture extends BaseModel {
    name!: string;
    displayName!: string;
    isDefault!: boolean;
}
