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
import { StatusBadge } from '../../../../shared/components/status-badge';
import { LoadingSpinner } from '../../../../shared/components/loading-spinner';
import { EmptyState } from '../../../../shared/components/empty-state';
import { ConfirmDialog } from '../../../../shared/components/confirm-dialog';
import { EquipmentService } from '../../services/equipment.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { EquipmentListItemDto, EquipmentCategory, EquipmentStatus, EquipmentCategoryLabels, EquipmentStatusLabels } from '../../models/equipment.model';
import { QueryParams } from '../../../../core/models/common.model';

@Component({
  selector: 'app-equipment-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, MatTableModule, MatPaginatorModule, MatSortModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, MatSelectModule, MatCardModule, MatMenuModule, PageHeader, StatusBadge, LoadingSpinner, EmptyState],
  templateUrl: './equipment-list.html',
  styleUrl: './equipment-list.scss'
})
export class EquipmentList implements OnInit {
  private readonly equipmentService = inject(EquipmentService);
  private readonly notificationService = inject(NotificationService);
  private readonly dialog = inject(MatDialog);
  private readonly searchSubject = new Subject<string>();

  readonly breadcrumbs: Breadcrumb[] = [{ label: 'Equipment' }];
  readonly displayedColumns = ['name', 'category', 'quantity', 'dailyRate', 'condition', 'status', 'actions'];
  readonly equipmentCategories = Object.values(EquipmentCategory);
  readonly equipmentCategoryLabels = EquipmentCategoryLabels;
  readonly equipmentStatusLabels = EquipmentStatusLabels;

  equipment: EquipmentListItemDto[] = [];
  totalCount = 0;
  isLoading = false;
  searchTerm = '';
  selectedCategory: EquipmentCategory | '' = '';
  query: QueryParams = { pageIndex: 0, pageSize: 10, sortColumn: 'name', sortDirection: 'asc' };

  ngOnInit(): void {
    this.searchSubject.pipe(debounceTime(300), distinctUntilChanged()).subscribe(term => {
      this.searchTerm = term;
      this.query.pageIndex = 0;
      this.loadEquipment();
    });
    this.loadEquipment();
  }

  loadEquipment(): void {
    this.isLoading = true;
    const queryParams = { ...this.query, searchTerm: this.searchTerm };
    this.equipmentService.getEquipment(queryParams).subscribe({
      next: (result) => { this.equipment = result.items; this.totalCount = result.totalCount; this.isLoading = false; },
      error: () => { this.isLoading = false; }
    });
  }

  onSearch(term: string): void { this.searchSubject.next(term); }
  onPageChange(event: PageEvent): void { this.query.pageIndex = event.pageIndex; this.query.pageSize = event.pageSize; this.loadEquipment(); }
  onSortChange(sort: Sort): void { this.query.sortColumn = sort.active; this.query.sortDirection = sort.direction || 'asc'; this.loadEquipment(); }

  deleteEquipment(item: EquipmentListItemDto): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: { title: 'Delete Equipment', message: `Are you sure you want to delete "${item.name}"?`, confirmText: 'Delete', type: 'danger' }
    });
    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.equipmentService.deleteEquipment(item.equipmentId).subscribe({
          next: () => { this.notificationService.showSuccess('Equipment deleted successfully'); this.loadEquipment(); },
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
}
