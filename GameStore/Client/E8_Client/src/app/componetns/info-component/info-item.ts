import { ListItem } from "../list-item-component/list-item";

export class InfoItem {
  name?: string;
  value?: string;
  nestedValues?: ListItem[];
  pageLink?: string;
}
