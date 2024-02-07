import { BaseModel } from "./base.model";

export class Image extends BaseModel {
    large!: string;
    small?: string;
    isCover!: boolean;
}