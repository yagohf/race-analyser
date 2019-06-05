import { Paging } from "./paging";

export class Listing<T> {
    list: T[];
    paging: Paging;
}