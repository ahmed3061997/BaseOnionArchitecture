import { Injectable, OnDestroy } from '@angular/core';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { Subject, takeUntil } from 'rxjs';
import { CultureService } from '../services/culture/culture.service';

@Injectable()
export class CustomMatPaginatorIntl extends MatPaginatorIntl implements OnDestroy {
    unsubscribe: Subject<void> = new Subject<void>();
    of_label = 'of';

    constructor(private cultureService: CultureService) {
        super();

        this.cultureService.onCultureChange
            .pipe(
                takeUntil(this.unsubscribe)
            )
            .subscribe(() => {
                this.getAndInitTranslations();
            });

        this.getAndInitTranslations();
    }

    ngOnDestroy() {
        this.unsubscribe.next();
        this.unsubscribe.complete();
    }

    getAndInitTranslations() {
        this.cultureService
            .get([
                'paginator.items_per_page',
                'paginator.next_page',
                'paginator.previous_page',
                'paginator.of_label',
            ])
            .pipe(
                takeUntil(this.unsubscribe)
            )
            .subscribe(translation => {
                this.itemsPerPageLabel =
                    translation['paginator.items_per_page'];
                this.nextPageLabel = translation['paginator.next_page'];
                this.previousPageLabel =
                    translation['paginator.previous_page'];
                this.of_label = translation['paginator.of_label'];
                this.changes.next();
            });
    }

    override getRangeLabel = (
        page: number,
        pageSize: number,
        length: number,
    ) => {
        if (length === 0 || pageSize === 0) {
            return `0 ${this.of_label} ${length}`;
        }
        length = Math.max(length, 0);
        const startIndex = page * pageSize;
        const endIndex =
            startIndex < length
                ? Math.min(startIndex + pageSize, length)
                : startIndex + pageSize;
        return `${startIndex + 1} - ${endIndex} ${this.of_label
            } ${length}`;
    };
}