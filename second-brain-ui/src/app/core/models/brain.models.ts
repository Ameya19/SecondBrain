export interface KnowledgeChunk {
    id: string;
    content: string;
    sourceId: string;
    ingestedAt: string;
    lastAccessedAt?: string;
    accessCount: number
}

export interface Source {
    id: string;
    title: string;
    type: 'pdf' | 'url' | 'note' | 'youtube';
    url?: string
    publishedAt?: string;
    ingestedAt: string;
    tags?: string[]
    chunkCount: number
}

export interface SourceDetail extends Source {
    ingestionJob?: {
        status: 'pending' | 'processing' | 'done' | 'failed';
        startedAt: string;
        completedAt?: string;
        errorMessage?: string;
        chunksCreated: number;
    }
}

export interface QueryRequest {
    question: string;
    topK: number;
}

export interface SourceRef {
    title: string;
    ingestedAt: string;
    publishedAt?: string;
}

export interface QueryResult {
    answer: string;
    sources: SourceRef[];
    contradiction: string[];
}

export interface IngestRequest {
    content: string;
    title: string;
    type: 'pdf' | 'url' | 'note' | 'youtube';
    url?: string;
    publishedAt?: string;
    tags?: string[]
}

export interface IngestResponse{
    sourceId: string;
}

export interface SearchResult {
    id: string;
    ingestedAt: string;
    accessCount: number;
    preview: string;
    source: Source;
}

export interface SearchResponse {
    query?: string;
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    results: SearchResult[]
}

export interface TimelineItem {
    period: string;
    count: number;
    types: { type: string; count: number; }[];
    sources: string[];
}

export interface GrowthStats {
    period: string;
    totalSources: number;
    avgPerPeriod: number;
    timeline: TimelineItem[]
}

export interface DecayChunk {
    id: string;
    ingestedAt: string;
    lastAccessedAt?: string;
    accessCount: number;
    source: string;
    preview: string;
    daysSinceAdded: number;
    daysSinceLastAccess: number;
}

export interface DecayResponse{
    threshold: string;
    count: number;
    percentageOfTotal: number;
    items: DecayChunk[]
}

export interface OverallStats {
    totalChunks: number;
    totalSources: number;
    totalQueries: number;
    accessStats: {
        totalAccess: number;
        avgAccessPerChunk: number;
        mostUsedChunk: Array<{
            id: string;
            accessCount: number;
            source: string;
        }>;
    }
    sourcesByType: Array<{
        type: string;
        count: number;
        sources: number;
    }>
}

export interface Contradiction {
    id: string;
    detectedAt: string;
    chunkA: {
        id: string;
        ingestedAt: string;
        source: string;
        preview: string;
    };
    chunkB: {
        id: string;
        ingestedAt: string;
        source: string;
        preview: string;
    };
    resolved: boolean,
    resolutionNote?: string;
    daysBetween: number
}

export interface ContradictionsResponse {
    totalContradictions: number;
    unresolved: number;
    items: Contradiction[]
}

