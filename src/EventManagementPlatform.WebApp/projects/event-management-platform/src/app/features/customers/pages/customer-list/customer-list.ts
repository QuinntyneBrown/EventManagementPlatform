import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSortModule, MatSort, Sort } from '@angular/material/sort';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialog } from '@angular/material/dialog';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { PageHeader } from '../../../../shared/components/page-header';
import { StatusBadge, BadgeColor } from '../../../../shared/components/status-badge';
import { LoadingSpinner } from '../../../../shared/components/loading-spinner';
import { EmptyState } from '../../../../shared/components/empty-state';
import { ConfirmDialog } from '../../../../shared/components/confirm-dialog';
import { CustomerService } from '../../services/customer.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { CustomerListDto, CustomerStatus, CustomerQueryParams } from '../../models/customer.model';

@Component({
  selector: 'app-customer-list',
  standalone: true,
  imports: [
    CommonModule, RouterModule, FormsModule, MatTableModule, MatPaginatorModule,
    MatSortModule, MatCardModule, MatButtonModule, MatIconModule, MatInputModule,
    MatFormFieldModule, MatSelectModule, MatMenuModule, PageHeader, StatusBadge,
    LoadingSpinner, EmptyState
  ],
  templateUrl: './customer-list.html',
  styleUrl: './customer-list.scss'
})
export class CustomerList implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  private readonly customerService = inject(CustomerService);
  private readonly notificationService = inject(NotificationService);
  private readonly dialog = inject(MatDialog);

  displayedColumns = ['name', 'email', 'phone', 'company', 'eventCount', 'status', 'actions'];
  dataSource = new MatTableDataSource<CustomerListDto>([]);

  isLoading = false;
  totalCount = 0;
  pageSize = 10;
  pageIndex = 0;
  searchTerm = '';
  statusFilter: CustomerStatus | '' = '';

  private searchSubject = new Subject<string>();

  readonly statusOptions = [
    { value: '', label: 'All Statuses' },
    { value: 'Active', label: 'Active' },
    { value: 'Inactive', label: 'Inactive' },
    { value: 'Blocked', label: 'Blocked' }
  ];

  ngOnInit(): void {
    this.searchSubject.pipe(debounceTime(300), distinctUntilChanged()).subscribe(() => {
      this.pageIndex = 0;
      this.loadCustomers();
    });
    this.loadCustomers();
  }

  loadCustomers(): void {
    this.isLoading = true;
    const params: CustomerQueryParams = {
      pageNumber: this.pageIndex + 1,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm || undefined,
      status: this.statusFilter || undefined,
      sortBy: this.sort?.active,
      sortDirection: this.sort?.direction as 'asc' | 'desc' || undefined
    };

    this.customerService.getCustomers(params).subscribe({
      next: (result) => {
        this.dataSource.data = result.items;
        this.totalCount = result.totalCount;
        this.isLoading = false;
      },
      error: () => { this.isLoading = false; }
    });
  }

  onSearch(value: string): void { this.searchSubject.next(value); }
  onStatusChange(): void { this.pageIndex = 0; this.loadCustomers(); }
  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadCustomers();
  }
  onSortChange(sort: Sort): void { this.loadCustomers(); }

  onDelete(customer: CustomerListDto): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: {
        title: 'Delete Customer',
        message: `Are you sure you want to delete "${customer.firstName} ${customer.lastName}"?`,
        confirmText: 'Delete', type: 'danger'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.customerService.deleteCustomer(customer.customerId).subscribe({
          next: () => {
            this.notificationService.showSuccess('Customer deleted successfully');
            this.loadCustomers();
          }
        });
      }
    });
  }

  getStatusColor(status: CustomerStatus): BadgeColor {
    switch (status) {
      case 'Active': return 'success';
      case 'Inactive': return 'neutral';
      case 'Blocked': return 'warn';
      default: return 'neutral';
    }
  }
}
