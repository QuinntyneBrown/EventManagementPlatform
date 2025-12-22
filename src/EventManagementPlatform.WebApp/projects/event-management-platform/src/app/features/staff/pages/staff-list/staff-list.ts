import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialog } from '@angular/material/dialog';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { PageHeader, Breadcrumb } from '../../../../shared/components/page-header';
import { StatusBadge, BadgeColor } from '../../../../shared/components/status-badge';
import { LoadingSpinner } from '../../../../shared/components/loading-spinner';
import { EmptyState } from '../../../../shared/components/empty-state';
import { ConfirmDialog } from '../../../../shared/components/confirm-dialog';
import { StaffService } from '../../services/staff.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { StaffListItemDto, StaffRole, StaffStatus, StaffRoleLabels, StaffStatusLabels } from '../../models/staff.model';
import { QueryParams } from '../../../../core/models/common.model';

@Component({
  selector: 'app-staff-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, MatTableModule, MatPaginatorModule, MatSortModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, MatSelectModule, MatCardModule, MatMenuModule, PageHeader, StatusBadge, LoadingSpinner, EmptyState],
  templateUrl: './staff-list.html',
  styleUrl: './staff-list.scss'
})
export class StaffList implements OnInit {
  private readonly staffService = inject(StaffService);
  private readonly notificationService = inject(NotificationService);
  private readonly dialog = inject(MatDialog);
  private readonly searchSubject = new Subject<string>();

  readonly breadcrumbs: Breadcrumb[] = [{ label: 'Staff' }];
  readonly displayedColumns = ['name', 'role', 'email', 'phone', 'hourlyRate', 'status', 'actions'];
  readonly staffRoles = Object.values(StaffRole);
  readonly staffRoleLabels = StaffRoleLabels;
  readonly staffStatusLabels = StaffStatusLabels;

  staffMembers: StaffListItemDto[] = [];
  totalCount = 0;
  isLoading = false;
  searchTerm = '';
  selectedRole: StaffRole | '' = '';
  query: QueryParams = { pageIndex: 0, pageSize: 10, sortColumn: 'lastName', sortDirection: 'asc' };

  ngOnInit(): void {
    this.searchSubject.pipe(debounceTime(300), distinctUntilChanged()).subscribe(term => {
      this.searchTerm = term;
      this.query.pageIndex = 0;
      this.loadStaff();
    });
    this.loadStaff();
  }

  loadStaff(): void {
    this.isLoading = true;
    const queryParams = { ...this.query, searchTerm: this.searchTerm };
    this.staffService.getStaff(queryParams).subscribe({
      next: (result) => { this.staffMembers = result.items; this.totalCount = result.totalCount; this.isLoading = false; },
      error: () => { this.isLoading = false; }
    });
  }

  onSearch(term: string): void { this.searchSubject.next(term); }
  onPageChange(event: PageEvent): void { this.query.pageIndex = event.pageIndex; this.query.pageSize = event.pageSize; this.loadStaff(); }
  onSortChange(sort: Sort): void { this.query.sortColumn = sort.active; this.query.sortDirection = sort.direction || 'asc'; this.loadStaff(); }

  deleteStaff(staff: StaffListItemDto): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: { title: 'Delete Staff Member', message: `Are you sure you want to delete "${staff.firstName} ${staff.lastName}"?`, confirmText: 'Delete', type: 'danger' }
    });
    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.staffService.deleteStaff(staff.staffId).subscribe({
          next: () => { this.notificationService.showSuccess('Staff member deleted successfully'); this.loadStaff(); },
          error: () => {}
        });
      }
    });
  }

  getStatusColor(status: StaffStatus): BadgeColor {
    const colors: Record<StaffStatus, BadgeColor> = { [StaffStatus.Active]: 'success', [StaffStatus.OnLeave]: 'warning', [StaffStatus.Inactive]: 'default', [StaffStatus.Terminated]: 'danger' };
    return colors[status] || 'default';
  }

  formatCurrency(value: number): string { return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(value); }
}
