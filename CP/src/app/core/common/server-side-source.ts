import { Observable } from "rxjs";
import { PageQuery } from "../models/page-query";
import { PageResult } from "../models/page-result";

export interface IServerSideSource<T> {
    getAll(query: PageQuery): Observable<PageResult<T>>
}