import { Component, OnDestroy, OnInit } from "@angular/core";
import { BrainService } from "../../core/services/brain.service";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { IngestRequest } from "../../core/models/brain.models";
import { Subject, takeUntil } from "rxjs";

@Component({
    selector: 'app-ingest',
    templateUrl: 'ingest.component.html',
    imports: [CommonModule, FormsModule]
})
export class IngestComponent implements OnInit, OnDestroy {
    successMessage = '';
    errorMessage = '';
    publishedDate: string = '';
    activeTab: 'paste' | 'upload' = 'paste';
    isDragging: boolean = false;
    uploadedFileName: string = '';
    isSubmitting = false;
    tagsInput = '';

    formData: IngestRequest = {
        content: '',
        title: '',
        type: 'note',
        url: undefined,
        publishedAt: undefined,
        tags: undefined
    }

    private destroy$ = new Subject<void>();

    constructor(private brainService: BrainService) {}

    ngOnInit(): void {}

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    onDragOver(event: DragEvent) {
        event.preventDefault();
        this.isDragging = true;
    }

    onDragLeave(event: DragEvent) {
        event.preventDefault();
        this.isDragging = false;
    }

    onDrop(event: DragEvent) {
        event.preventDefault();
        this.isDragging = false;

        const files = event.dataTransfer?.files;
        if(files && files.length > 0) {
            this.handleFiles(files[0]);
        }
    }

    handleFiles(file: File) {
        this.uploadedFileName = file.name;
        this.formData.title = file.name.replace(/\.[^/.]+$/, '');
        this.formData.type = "pdf";

        const reader =  new FileReader();
        reader.onload = (e) => {
            this.formData.content = e.target?.result as string;
        }

        reader.readAsText(file);
    }

    onSubmit() {
        if(!this.formData.content.trim() || !this.formData.title.trim())
        {
            this.errorMessage = "Title and Content are required.";
            return;
        }

        this.isSubmitting = true;
        this.errorMessage = '';
        this.successMessage = '';

        //Parse tags
        if(this.tagsInput.trim()) {
            this.formData.tags = this.tagsInput.split(',').map(t => t.trim()).filter(t => t.length > 0);
        }

        //Parse published date
        if(this.publishedDate) {
            this.formData.publishedAt = new Date(this.publishedDate).toISOString() as any;
        }

        this.brainService.ingest(this.formData).pipe(takeUntil(this.destroy$)).subscribe({
            next: (response) => {                    console.log('1. Response received');
                    this.successMessage = `Knowledge ingested successfully`;
                    console.log('2. Success message set');
                    this.resetForm();
                    console.log('3. Form reset');
                    this.isSubmitting = false;
                    console.log('4. isSubmitting set to false');
            },
            error: (err) => {
                this.errorMessage = 'Failed to ingest knowledge. Please try again.';
                console.error('Ingest error:', err);
                this.isSubmitting = false;
            }
        });
    }

    private resetForm() {
        this.formData = {
            content: '',
            title: '',
            type: 'note',
            url: undefined,
            publishedAt: undefined,
            tags: undefined
        };

        this.tagsInput = '';
        this.publishedDate = '';
        this.uploadedFileName = '';
    }

    onFileSelected(event: Event) {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0)
        {
            this.handleFiles(input.files[0]);
        }
    }
}