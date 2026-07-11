import { OverallStats } from "../../core/models/brain.models";
import { Subject, takeUntil } from "rxjs";
import { SearchService } from "../../core/services/search.service";
import { CommonModule } from "@angular/common";
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { RouterLink } from "@angular/router";

@Component({
    selector: 'app-dashboard',
    templateUrl: 'dashboard.component.html',
    imports: [CommonModule, RouterLink]
})
export class DashboardComponent implements OnInit, OnDestroy {
    stats: OverallStats | null = null;
    isLoading = false;

    private destroy$ = new Subject<void>();

    constructor(private searchService: SearchService,
        private cdr: ChangeDetectorRef
    ) {}

    ngOnInit(): void {
        console.log('Dashboard init - setting isLoading to true');
        this.isLoading = true;
        this.searchService.getOverallStats().pipe(takeUntil(this.destroy$)).subscribe({
            next: (stats) => {
                this.stats = stats;
                console.log('Chunks loaded:', this.stats.TotalChunks);
                console.log('Sources loaded:', this.stats.TotalSources);
                console.log('Queries loaded:', this.stats.TotalQueries);
                console.log('AvgAccessPerChunk loaded:', this.stats.AccessStats?.AvgAccessPerChunk);
                console.log('MostUsedChunk loaded:', this.stats.AccessStats?.MostUsedChunk);
                console.log('SourcesByType loaded:', this.stats.SourceStats);

                console.log('Setting isLoading to false');
                this.isLoading = false;
                this.cdr.markForCheck();
            },
            error: (err) => {
                console.error('Failed to load stats:', err);
                this.isLoading = false;
                this.cdr.markForCheck();
            }
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
}