import { Routes } from '@angular/router';

export const venueRoutes: Routes = [
  { path: '', loadComponent: () => import('./pages/venue-list/venue-list').then(m => m.VenueList) },
  { path: 'create', loadComponent: () => import('./pages/venue-create/venue-create').then(m => m.VenueCreate) },
  { path: ':venueId', loadComponent: () => import('./pages/venue-detail/venue-detail').then(m => m.VenueDetail) },
  { path: ':venueId/edit', loadComponent: () => import('./pages/venue-edit/venue-edit').then(m => m.VenueEdit) }
];
