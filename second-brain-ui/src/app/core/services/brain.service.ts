import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { IngestRequest, IngestResponse, QueryRequest, QueryResult, Source } from "../models/brain.models";
import { Observable } from "rxjs";

@Injectable({providedIn: 'root'})
export class BrainService {

    private readonly apiUrl = 'http://localhost:5163/api';

    constructor(private http: HttpClient) {}

    query(request: QueryRequest) : Observable<QueryResult> {
        return this.http.post<QueryResult>(`${this.apiUrl}/brain/query`, request);
    }

    queryStream(question: string): Observable<string> {
        return new Observable(observer => {
            const eventSource = new EventSource(`${this.apiUrl}/brain/query/stream?question=${encodeURIComponent(question)}`);

            eventSource.onmessage = (event) => {
                if(event.data === '[DONE]')
                {
                    eventSource.close();
                    observer.complete();
                }
                else{
                    observer.next(event.data);
                }
            };

            eventSource.onerror = (error) => {
                eventSource.close();
                observer.error(error);
            };

            return () => eventSource.close();
        });
    }

    ingest(request: IngestRequest): Observable<IngestResponse> {
        return this.http.post<IngestResponse>(`${this.apiUrl}/brain/ingest`, request);
    }

    getSources(): Observable<Source[]> {
        return this.http.get<Source[]>(`${this.apiUrl}/sources`);
    }

    getSourceById(id: string): Observable<Source>{
        return this.http.get<Source>(`${this.apiUrl}/sources/${id}`);
    }

    deleteSourceById(id: string): Observable<{message: string}> {
        return this.http.delete<{ message: string }>(`${this.apiUrl}/sources/${id}`);
    }

    updateSourceTags(id: string, tags: string[]): Observable<{ sourceId: string; tags: string[]; }> {
        return this.http.patch<{ sourceId: string; tags: string[] }>(`${this.apiUrl}/sources/${id}/tags`, tags);
    }
}