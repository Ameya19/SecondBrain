import { Subject, takeUntil } from 'rxjs';
import { BrainService } from '../../core/services/brain.service';
import { SourceRef } from './../../core/models/brain.models';
import { Component, OnDestroy, OnInit } from "@angular/core";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-query',
    templateUrl: 'query.component.html',
    imports: [CommonModule, FormsModule]
})
export class QueryComponent implements OnInit, OnDestroy {
    question: string = ''
    answer = '';
    sources: SourceRef[] = [];
    contradictions: string[] = [];
    isLoading = false;

    private destroy$ = new Subject<void>();

    constructor(private brainService: BrainService) {}

    ngOnInit(): void {}

    onQuery() {
        console.log("Button Clicked");
        console.log("Question: ", this.question);
        if(!this.question.trim())
            return;

        this.isLoading = true;
        this.answer = '';
        this.sources = [];
        this.contradictions = [];

        this.brainService.query({ question: this.question, topK: 5 }).pipe(takeUntil(this.destroy$)).subscribe({
            next: (result) => {
                this.answer = result.answer;
                this.sources = result.sources;
                this.contradictions = result.contradictions;
                this.isLoading = false;
            },
            error: (err) => {
                console.log("Query Failed;", err);
                this.answer = 'Error querying knowledge base. Please try again.';
                this.isLoading = false;
            }
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
}