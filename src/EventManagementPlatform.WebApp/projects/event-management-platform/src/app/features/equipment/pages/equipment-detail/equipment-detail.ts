import { Component, OnInit, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDialog } from '@angular/material/dialog';
import { PageHeader, Breadcrumb } from '../../../../shared/components/page-header';
import { StatusBadge } from '../../../../shared/components/status-badge';
import { LoadingSpinner } from '../../../../shared/components/loading-spinner';
import { ConfirmDialog } from '../../../../shared/components/confirm-dialog';
import { EquipmentService } from '../../services/equipment.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { EquipmentDetailDto, EquipmentStatus, EquipmentCategoryLabels, EquipmentStatusLabels, EquipmentConditionLabels } from '../../models/equipment.model';

@Component({
  selector: 'app-equipment-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardModule, MatButtonModule, MatIconModule, MatTabsModule, PageHeader, StatusBadge, LoadingSpinner],
  templateUrl: './equipment-detail.html',
  styleUrl: './equipment-detail.scss'
})
export class EquipmentDetail implements OnInit {
  @Input() equipmentId!: string;
  private readonly equipmentService = inject(EquipmentService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);
  private readonly dialog = inject(MatDialog);

  readonly equipmentCategoryLabels = EquipmentCategoryLabels;
  readonly equipmentStatusLabels = EquipmentStatusLabels;
  readonly equipmentConditionLabels = EquipmentConditionLabels;
  breadcrumbs: Breadcrumb[] = [{ label: 'Equipment', link: '/equipment' }, { label: 'Loading...' }];
  equipment: EquipmentDetailDto | null = null;
  isLoading = false;

  ngOnInit(): void { this.loadEquipment(); }

  loadEquipment(): void {
    this.isLoading = true;
    this.equipmentService.getEquipmentById(this.equipmentId).subscribe({
      next: (equipment) => {
        this.equipment = equipment;
        this.breadcrumbs = [{ label: 'Equipment', link: '/equipment' }, { label: equipment.name }];
        this.isLoading = false;
      },
      error: () => { this.isLoading = false; this.router.navigate(['/equipment']); }
    });
  }

  deleteEquipment(): void {
    if (!this.equipment) return;
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: { title: 'Delete Equipment', message: `Are you sure you want to delete "${this.equipment.name}"?`, confirmText: 'Delete', type: 'danger' }
    });
    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.equipmentService.deleteEquipment(this.equipmentId).subscribe({
          next: () => { this.notificationService.showSuccess('Equipment deleted successfully'); this.router.navigate(['/equipment']); },
          error: () => {}
        });
      }
    });
  }

  getStatusColor(status: EquipmentStatus): string {
    const colors: Record<EquipmentStatus, string> = {
      [EquipmentStatus.Available]: 'success', [EquipmentStatus.InUse]: 'info',
      [EquipmentStatus.Reserved]: 'warning', [EquipmentStatus.Maintenance]: 'default', [EquipmentStatus.Retired]: 'danger'
    };
    return colors[status] || 'default';
  }

  formatCurrency(value: number): string { return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(value); }
  formatDate(date: Date): string { return date ? new Date(date).toLocaleDateString() : '-'; }
}
