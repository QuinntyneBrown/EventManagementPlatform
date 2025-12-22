import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { unsavedChangesGuard } from './core/guards/unsaved-changes.guard';

export const routes: Routes = [
  // Public routes
  { path: 'login', loadComponent: () => import('./pages/login/login').then(m => m.Login) },
  { path: 'register', loadComponent: () => import('./pages/register/register').then(m => m.Register) },

  // Protected routes with layout
  {
    path: '',
    loadComponent: () => import('./layout/app-layout/app-layout').then(m => m.AppLayout),
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', loadComponent: () => import('./pages/dashboard/dashboard').then(m => m.Dashboard) },
      { path: 'profile', loadComponent: () => import('./pages/profile/profile').then(m => m.Profile), canDeactivate: [unsavedChangesGuard] },

      // Events feature
      {
        path: 'events',
        loadChildren: () => import('./features/events/events.routes').then(m => m.eventRoutes)
      },

      // Customers feature
      {
        path: 'customers',
        loadChildren: () => import('./features/customers/customers.routes').then(m => m.customerRoutes)
      },

      // Venues feature
      {
        path: 'venues',
        loadChildren: () => import('./features/venues/venues.routes').then(m => m.venueRoutes)
      },

      // Staff feature
      {
        path: 'staff',
        loadChildren: () => import('./features/staff/staff.routes').then(m => m.staffRoutes)
      },

      // Equipment feature
      {
        path: 'equipment',
        loadChildren: () => import('./features/equipment/equipment.routes').then(m => m.equipmentRoutes)
      },

      // Error pages
      { path: 'forbidden', loadComponent: () => import('./pages/forbidden/forbidden').then(m => m.Forbidden) }
    ]
  },

  // Catch-all route
  { path: '**', loadComponent: () => import('./pages/not-found/not-found').then(m => m.NotFound) }
];
