import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { catchError, Observable, tap, throwError } from "rxjs";
import { ContradictionsResponse, DecayResponse, GrowthStats, OverallStats, SearchResponse } from "../models/brain.models";

@Injectable({ providedIn: 'root' })
export class SearchService {
    private readonly apiUrl = "http://localhost:5163/api";

    constructor(private http: HttpClient) {}

    search(q?: string, from?: string, to?: string, type?:string, tag?: string, page: number = 1, pageSize: number = 10) : Observable<SearchResponse> {
        let params = new HttpParams().set('page', page.toString()).set('pageSize', pageSize.toString());

        if(q) params = params.set('q', q);
        if(from) params = params.set('from', from);
        if(to) params = params.set('to', to);
        if(type) params = params.set('type', type);
        if(tag) params = params.set('tag', tag);

        return this.http.get<SearchResponse>(`${this.apiUrl}/search`, {params})
    }

    getGrowthInsights(period: 'quarter' | 'month' | 'year' | 'week' = 'month'): Observable<GrowthStats> {
        return this.http.get<GrowthStats>(`${this.apiUrl}/insights/growth`, { params : {period} });
    }

    getDecayStats(days: number = 365): Observable<DecayResponse> {
        return this.http.get<DecayResponse>(`${this.apiUrl}/insights/decay`, { params : { days: days.toString()}})
    }

    getOverallStats(): Observable<OverallStats> {
        return this.http.get<OverallStats>(`${this.apiUrl}/insights/stats`).pipe(
            tap(response => {
                console.log('Overall stats response:', response);
            }),
            catchError(error => {
                console.error('Overall stats error:', error);
                return throwError(() => error);
            })
        );
    }

    getContradictions(): Observable<ContradictionsResponse> {
        return this.http.get<ContradictionsResponse>(`${this.apiUrl}/insights/contradictions`);
    }

    resolveContradictions(id: string, note?:string): Observable<{ message: string; id: string }> {
        return this.http.patch<{ message: string; id: string }>(`${this.apiUrl}/insights/contradictions/${id}/resolve`, {note});
    }
}