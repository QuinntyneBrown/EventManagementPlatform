import { Component, OnInit, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { PageHeader, Breadcrumb } from '../../../../shared/components/page-header';
import { LoadingSpinner } from '../../../../shared/components/loading-spinner';
import { CustomerService } from '../../services/customer.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { CustomerDetailDto } from '../../models/customer.model';
import { HasUnsavedChanges } from '../../../../core/guards/unsaved-changes.guard';

@Component({
  selector: 'app-customer-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatProgressSpinnerModule, PageHeader, LoadingSpinner],
  templateUrl: './customer-edit.html',
  styleUrl: './customer-edit.scss'
})
export class CustomerEdit implements OnInit, HasUnsavedChanges {
  @Input() customerId!: string;
  private readonly fb = inject(FormBuilder);
  private readonly customerService = inject(CustomerService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);

  readonly breadcrumbs: Breadcrumb[] = [{ label: 'Customers', link: '/customers' }, { label: 'Edit Customer' }];
  customer: CustomerDetailDto | null = null;
  isLoading = false;
  isSaving = false;

  form: FormGroup = this.fb.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email]],
    phone: [''], company: [''], address: [''], city: [''], state: [''], zipCode: [''], notes: ['']
  });

  ngOnInit(): void { this.loadCustomer(); }
  hasUnsavedChanges(): boolean { return this.form.dirty; }

  loadCustomer(): void {
    this.isLoading = true;
    this.customerService.getCustomerById(this.customerId).subscribe({
      next: (customer) => {
        this.customer = customer;
        this.form.patchValue(customer);
        this.isLoading = false;
      },
      error: () => { this.isLoading = false; this.router.navigate(['/customers']); }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.isSaving = true;
    this.customerService.updateCustomer(this.customerId, this.form.value).subscribe({
      next: () => {
        this.notificationService.showSuccess('Customer updated successfully');
        this.router.navigate(['/customers', this.customerId]);
      },
      error: () => { this.isSaving = false; }
    });
  }

  onCancel(): void { this.router.navigate(['/customers', this.customerId]); }
}
