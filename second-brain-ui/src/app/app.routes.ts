import { Routes } from '@angular/router';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { QueryComponent } from './features/query/query.component';
import { IngestComponent } from './features/ingest/ingest.component';
import { InsightsComponent } from './features/insights/insights.component';
import { SourcesComponent } from './features/sources/sources.component';
import { SearchComponent } from './features/search/search.component';

export const routes: Routes = [
    {
        path: '',
        component: DashboardComponent
    },
    {
        path: 'query',
        component: QueryComponent
    },
    {
        path: 'ingest',
        component: IngestComponent
    },
    {
        path: 'sources',
        component: SourcesComponent
    },
    {
        path: 'insights',
        component: InsightsComponent
    },
    {
        path: 'search',
        component: SearchComponent
    }
];
