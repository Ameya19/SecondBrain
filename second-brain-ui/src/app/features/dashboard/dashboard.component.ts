import { Component, OnDestroy, OnInit } from "@angular/core";
import { OverallStats } from "../../core/models/brain.models";
import { Subject, takeUntil } from "rxjs";
import { SearchService } from "../../core/services/search.service";

@Component({
    selector: 'app-dashboard',
    templateUrl: 'dashboard.component.html'
})
export class DashboardComponent implements OnInit, OnDestroy {
    stats: OverallStats | null = null;
    private destroy$ = new Subject<void>();

    constructor(private searchService: SearchService) {}

    ngOnInit(): void {
        this.searchService.getOverallStats().pipe(takeUntil(this.destroy$)).subscribe({
            next: (stats) => {
                this.stats = stats;
            },
            error: (err) => {
                console.error('Failed to load stats:', err);
            }
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
}