import { CommonModule } from "@angular/common";
import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { Subject, takeUntil } from "rxjs";
import { SearchService } from "../../core/services/search.service";
import { SearchResponse } from "../../core/models/brain.models";

@Component({
    selector: 'app-search',
    templateUrl: 'search.component.html',
    imports: [CommonModule, FormsModule]
})
export class SearchComponent implements OnInit, OnDestroy {
    searchQuery: string = '';
    isLoading = false;
    currentPage: number = 1;
    fromDate: string = '';
    toDate: string = '';
    selectedType: string | null = null;
    results: SearchResponse | null = null;


    private destroy$ = new Subject<void>();

    constructor(private searchService: SearchService) {}

    ngOnInit(): void {}

    onSearch() {
        this.isLoading = true;
        this.currentPage = 1;
        
        this.searchService.search(
            this.searchQuery || undefined, 
            this.fromDate || undefined, 
            this.toDate || undefined, 
            this.selectedType || undefined, 
            undefined, 
            this.currentPage, 10)
            .pipe(takeUntil(this.destroy$))
            .subscribe({
                next: (data) => {
                    this.results = data;
                    this.isLoading = false;
                },
                error: (err) => {
                    console.error('Search failed:', err);
                    this.isLoading = false;
                }
        });
    }

    onReset() {
        this.searchQuery = '';
        this.fromDate = '';
        this.toDate = '';
        this.currentPage = 1;
        this.selectedType = null,
        this.results = null;
    }

    previousPage() {
        if(this.results && this.currentPage > 1){
            this.currentPage--;
            this.onSearch();
        }
    }

    nextPage() {
        if(this.results && this.currentPage < this.results.totalPages)
        {
            this.currentPage++;
            this.onSearch();
        }
    }

    goToPage(page: number) {
        this.currentPage = page;
        this.onSearch();
    }

    getPageNumbers(): number[] {
        if(!this.results)
            return [];

        const pages = [];
        const start = Math.max(1, this.results.page - 2);
        const end = Math.min(this.results.totalPages, this.results.page + 2);

        for (let i = start; i <= end; i++) {
            pages.push(i);
          }
          return pages;
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
}