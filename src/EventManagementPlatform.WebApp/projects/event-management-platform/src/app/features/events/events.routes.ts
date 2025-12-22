import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { unsavedChangesGuard } from '../../core/guards/unsaved-changes.guard';

export const eventRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/event-list').then(m => m.EventList),
    title: 'Events'
  },
  {
    path: 'create',
    loadComponent: () => import('./pages/event-create').then(m => m.EventCreate),
    title: 'Create Event',
    canActivate: [authGuard],
    canDeactivate: [unsavedChangesGuard]
  },
  {
    path: ':eventId',
    loadComponent: () => import('./pages/event-detail').then(m => m.EventDetail),
    title: 'Event Details'
  },
  {
    path: ':eventId/edit',
    loadComponent: () => import('./pages/event-edit').then(m => m.EventEdit),
    title: 'Edit Event',
    canActivate: [authGuard],
    canDeactivate: [unsavedChangesGuard]
  }
];
