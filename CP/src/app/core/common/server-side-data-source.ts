import { CollectionViewer, DataSource } from "@angular/cdk/collections";
import { BehaviorSubject, catchError, finalize, Observable, of } from "rxjs";
import { PageQuery } from "../models/page-query";
import { PageResult } from "../models/page-result";
import { IServerSideSource } from "./server-side-source";

export class ServerSideDataSource<T> implements DataSource<T> {

    private dataSubject = new BehaviorSubject<T[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private totalCountSubject = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();
    public totalCount$ = this.totalCountSubject.asObservable();

    constructor(private source: IServerSideSource<T>) { }

    connect(collectionViewer: CollectionViewer): Observable<T[]> {
        return this.dataSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
        this.dataSubject.complete();
        this.loadingSubject.complete();
    }

    load(query: PageQuery) {

        this.loadingSubject.next(true);

        this.source.getAll(query).pipe(
            finalize(() => this.loadingSubject.next(false))
        )
            .subscribe({
                next: result => {
                    this.dataSubject.next(result.items)
                    this.totalCountSubject.next(result.totalCount)
                },
                error: () => []
            });
    }
}