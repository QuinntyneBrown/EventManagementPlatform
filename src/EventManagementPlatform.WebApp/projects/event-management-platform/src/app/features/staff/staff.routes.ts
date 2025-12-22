import { Routes } from '@angular/router';

export const staffRoutes: Routes = [
  { path: '', loadComponent: () => import('./pages/staff-list/staff-list').then(m => m.StaffList) },
  { path: 'create', loadComponent: () => import('./pages/staff-create/staff-create').then(m => m.StaffCreate) },
  { path: ':staffId', loadComponent: () => import('./pages/staff-detail/staff-detail').then(m => m.StaffDetail) },
  { path: ':staffId/edit', loadComponent: () => import('./pages/staff-edit/staff-edit').then(m => m.StaffEdit) }
];
