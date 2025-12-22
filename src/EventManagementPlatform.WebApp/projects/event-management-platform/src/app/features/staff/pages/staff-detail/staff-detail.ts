import { Component, OnInit, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog } from '@angular/material/dialog';
import { PageHeader, Breadcrumb } from '../../../../shared/components/page-header';
import { StatusBadge } from '../../../../shared/components/status-badge';
import { LoadingSpinner } from '../../../../shared/components/loading-spinner';
import { ConfirmDialog } from '../../../../shared/components/confirm-dialog';
import { StaffService } from '../../services/staff.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { StaffDetailDto, StaffStatus, StaffRoleLabels, StaffStatusLabels } from '../../models/staff.model';

@Component({
  selector: 'app-staff-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardModule, MatButtonModule, MatIconModule, MatTabsModule, MatChipsModule, PageHeader, StatusBadge, LoadingSpinner],
  templateUrl: './staff-detail.html',
  styleUrl: './staff-detail.scss'
})
export class StaffDetail implements OnInit {
  @Input() staffId!: string;
  private readonly staffService = inject(StaffService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);
  private readonly dialog = inject(MatDialog);

  readonly staffRoleLabels = StaffRoleLabels;
  readonly staffStatusLabels = StaffStatusLabels;
  breadcrumbs: Breadcrumb[] = [{ label: 'Staff', link: '/staff' }, { label: 'Loading...' }];
  staff: StaffDetailDto | null = null;
  isLoading = false;

  ngOnInit(): void { this.loadStaff(); }

  loadStaff(): void {
    this.isLoading = true;
    this.staffService.getStaffById(this.staffId).subscribe({
      next: (staff) => {
        this.staff = staff;
        this.breadcrumbs = [{ label: 'Staff', link: '/staff' }, { label: `${staff.firstName} ${staff.lastName}` }];
        this.isLoading = false;
      },
      error: () => { this.isLoading = false; this.router.navigate(['/staff']); }
    });
  }

  deleteStaff(): void {
    if (!this.staff) return;
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: { title: 'Delete Staff Member', message: `Are you sure you want to delete "${this.staff.firstName} ${this.staff.lastName}"?`, confirmText: 'Delete', type: 'danger' }
    });
    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.staffService.deleteStaff(this.staffId).subscribe({
          next: () => { this.notificationService.showSuccess('Staff member deleted successfully'); this.router.navigate(['/staff']); },
          error: () => {}
        });
      }
    });
  }

  getStatusColor(status: StaffStatus): string {
    const colors: Record<StaffStatus, string> = { [StaffStatus.Active]: 'success', [StaffStatus.OnLeave]: 'warning', [StaffStatus.Inactive]: 'default', [StaffStatus.Terminated]: 'danger' };
    return colors[status] || 'default';
  }

  formatCurrency(value: number): string { return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(value); }
  formatDate(date: Date): string { return new Date(date).toLocaleDateString(); }
}
