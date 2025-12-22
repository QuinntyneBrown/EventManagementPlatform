import { Component, OnInit, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatDialog } from '@angular/material/dialog';
import { PageHeader, Breadcrumb } from '../../../../shared/components/page-header';
import { StatusBadge, BadgeColor } from '../../../../shared/components/status-badge';
import { LoadingSpinner } from '../../../../shared/components/loading-spinner';
import { ConfirmDialog } from '../../../../shared/components/confirm-dialog';
import { CustomerService } from '../../services/customer.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { CustomerDetailDto, CustomerStatus } from '../../models/customer.model';

@Component({
  selector: 'app-customer-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardModule, MatButtonModule, MatIconModule, MatDividerModule, PageHeader, StatusBadge, LoadingSpinner],
  templateUrl: './customer-detail.html',
  styleUrl: './customer-detail.scss'
})
export class CustomerDetail implements OnInit {
  @Input() customerId!: string;
  private readonly customerService = inject(CustomerService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);
  private readonly dialog = inject(MatDialog);

  customer: CustomerDetailDto | null = null;
  isLoading = false;

  readonly breadcrumbs: Breadcrumb[] = [{ label: 'Customers', link: '/customers' }, { label: 'Customer Details' }];

  ngOnInit(): void { this.loadCustomer(); }

  loadCustomer(): void {
    this.isLoading = true;
    this.customerService.getCustomerById(this.customerId).subscribe({
      next: (customer) => { this.customer = customer; this.isLoading = false; },
      error: () => { this.isLoading = false; this.router.navigate(['/customers']); }
    });
  }

  onDelete(): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: { title: 'Delete Customer', message: `Are you sure you want to delete "${this.customer?.firstName} ${this.customer?.lastName}"?`, confirmText: 'Delete', type: 'danger' }
    });
    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed && this.customer) {
        this.customerService.deleteCustomer(this.customer.customerId).subscribe({
          next: () => { this.notificationService.showSuccess('Customer deleted successfully'); this.router.navigate(['/customers']); }
        });
      }
    });
  }

  getStatusColor(status: CustomerStatus): BadgeColor {
    switch (status) { case 'Active': return 'success'; case 'Inactive': return 'neutral'; case 'Blocked': return 'warn'; default: return 'neutral'; }
  }
}
