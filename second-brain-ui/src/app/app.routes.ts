import { Routes } from '@angular/router';
import { DashboardComponent } from './features/dashboard/dashboard.component';

export const routes: Routes = [
    {
        path: '',
        component: DashboardComponent
    },
    /*{
        path: 'query',
        component: QueryComponent
    },
    {
        path: 'ingest',
        component: IngestComponent
    },
    {
        path: 'sources',
        component: SourcesListComponent
    },
    {
        path: 'insights',
        component: InsightsComponent
    }*/
];
