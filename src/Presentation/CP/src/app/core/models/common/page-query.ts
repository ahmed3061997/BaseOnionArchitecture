import { SortDirection } from "@angular/material/sort";

export class PageQuery {
    public pageSize: number;
    public pageIndex: number;
    public sortColumn?: string;
    public sortDirection?: number;
    public searchColumn?: string;
    public searchTerm?: string;
    
    public static toSortDirection(direction: SortDirection) {
        if (direction == 'asc')
            return 1;
        else if (direction == 'desc')
            return 2;
        return 0;
    }
}

export const DEFAULT_PAGE_SIZE: number = 10;
export const PAGE_SIZE_OPTIONS: number[] = [5, 10, 50, 100];