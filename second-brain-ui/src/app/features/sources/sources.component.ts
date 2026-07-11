import { Source, SourceDetail } from './../../core/models/brain.models';
import { CommonModule } from "@angular/common";
import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { Subject, takeUntil } from 'rxjs';
import { BrainService } from '../../core/services/brain.service';

@Component({
    selector: 'app-sources',
    templateUrl: 'sources.component.html',
    imports: [CommonModule, FormsModule]
})
export class SourcesComponent implements OnInit, OnDestroy {
    searchQuery: string = '';
    filterType: null | 'note' | 'pdf' | 'youtube' | 'url' = null;
    sources: Source[] = [];
    selectedSource: SourceDetail | null = null;

    private destroy$ = new Subject<void>();

    constructor(private brainService: BrainService) {}

    ngOnInit(): void {
        this.loadSources();
    }

    loadSources() {
        this.brainService.getSources().pipe(takeUntil(this.destroy$)).subscribe({
            next: (sources) => {
                this.sources = sources;
            },
            error: (err) => {
                console.error('Failed to load sources:', err);
            }
        })
    }

    onDelete(id: string) {
        if(confirm('Are you sure you want to delete this source and all its chunks?')) {
            this.brainService.deleteSourceById(id).pipe(takeUntil(this.destroy$)).subscribe({
                next: () => {
                    this.loadSources();
                },
                error: (err) => {
                    console.log('Failed to delete source: ', err);
                }
            });
        }
    }

    onViewDetails(id: string) {
        this.brainService.getSourceById(id).pipe(takeUntil(this.destroy$)).subscribe({
            next: (source) => {
                this.selectedSource = source;
            },
            error: (err) => {
                console.log('Failed to load source details: ', err);
            }
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    get filteredSources(): Source[] {
        return this.sources.filter(source => {
            const matchesType = !this.filterType || source.type == this.filterType;
            const matchesSearch = !this.searchQuery || source.title.toLowerCase().includes(this.searchQuery.toLowerCase());
            return matchesType && matchesSearch;
        });
    }
}