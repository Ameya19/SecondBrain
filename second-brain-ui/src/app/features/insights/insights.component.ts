import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormsModule } from '@angular/forms';
import { ContradictionsResponse, DecayResponse, GrowthStats } from '../../core/models/brain.models';
import { Subject, takeUntil } from 'rxjs';
import { SearchService } from '../../core/services/search.service';

@Component({
    selector: 'app-insights',
    templateUrl: 'insights.component.html',
    imports: [CommonModule, FormsModule]
})
export class InsightsComponent implements OnInit, OnDestroy {
    contradictions: ContradictionsResponse | null= null;
    decayStats: DecayResponse | null = null;
    growthStats: GrowthStats | null = null;
    growthPeriod: 'week' | 'quarter' | 'month' | 'year' = 'month';

    private destroy$ = new Subject<void>();

    constructor(private searchService: SearchService) {}

    ngOnInit(): void {
        this.onloadGrowth();
    }

    onGrowthPeriodChange() {
        this.onloadGrowth();
        this.onloadDecay();
        this.loadContradictions();
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    onloadGrowth() {
        this.searchService.getGrowthInsights(this.growthPeriod).pipe(takeUntil(this.destroy$)).subscribe({
            next: (data) => {
                this.growthStats = data;
            },
            error: (err) => {
                console.error('Failed to load growth stats:', err);
            }
        });
    }

    onloadDecay(){
        this.searchService.getDecayStats(365).pipe(takeUntil(this.destroy$)).subscribe({
            next: (data) => {
                this.decayStats = data;
            },
            error: (err) => {
                console.error('Failed to load decay stats:', err);
            }
        })
    }

    loadContradictions() {
        this.searchService.getContradictions().pipe(takeUntil(this.destroy$)).subscribe({
            next: (contradictions) => {
                this.contradictions = contradictions;
            },
            error: (err) => {
                console.error('Failed to load contradictions:', err);
            }
        })
    }

    resolveContradiction(id: string){
        this.searchService.resolveContradictions(id, 'Resolved by user').pipe(takeUntil(this.destroy$)).subscribe({
            next: () => {
                this.loadContradictions();
            }
        })
    }

    getMaxGrowth() {
        if(!this.growthStats)
            return 1;

        return Math.max(...this.growthStats.timeline.map(t => t.count), 1);
    }
}