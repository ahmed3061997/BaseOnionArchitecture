import { Observable } from "rxjs";
import { PageQuery } from "../models/common/page-query";
import { PageResult } from "../models/common/page-result";

export interface IServerSideSource<T> {
    getAll(query: PageQuery): Observable<PageResult<T>>
}