import { Routes } from '@angular/router';

export const equipmentRoutes: Routes = [
  { path: '', loadComponent: () => import('./pages/equipment-list/equipment-list').then(m => m.EquipmentList) },
  { path: 'create', loadComponent: () => import('./pages/equipment-create/equipment-create').then(m => m.EquipmentCreate) },
  { path: ':equipmentId', loadComponent: () => import('./pages/equipment-detail/equipment-detail').then(m => m.EquipmentDetail) },
  { path: ':equipmentId/edit', loadComponent: () => import('./pages/equipment-edit/equipment-edit').then(m => m.EquipmentEdit) }
];
