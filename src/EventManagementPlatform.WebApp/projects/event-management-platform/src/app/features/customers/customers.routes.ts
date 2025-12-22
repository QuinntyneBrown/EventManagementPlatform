import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { unsavedChangesGuard } from '../../core/guards/unsaved-changes.guard';

export const customerRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/customer-list').then(m => m.CustomerList),
    title: 'Customers'
  },
  {
    path: 'create',
    loadComponent: () => import('./pages/customer-create').then(m => m.CustomerCreate),
    title: 'Create Customer',
    canActivate: [authGuard],
    canDeactivate: [unsavedChangesGuard]
  },
  {
    path: ':customerId',
    loadComponent: () => import('./pages/customer-detail').then(m => m.CustomerDetail),
    title: 'Customer Details'
  },
  {
    path: ':customerId/edit',
    loadComponent: () => import('./pages/customer-edit').then(m => m.CustomerEdit),
    title: 'Edit Customer',
    canActivate: [authGuard],
    canDeactivate: [unsavedChangesGuard]
  }
];
