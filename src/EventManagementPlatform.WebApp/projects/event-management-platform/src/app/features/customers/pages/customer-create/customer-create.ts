import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { PageHeader, Breadcrumb } from '../../../../shared/components/page-header';
import { CustomerService } from '../../services/customer.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { HasUnsavedChanges } from '../../../../core/guards/unsaved-changes.guard';

@Component({
  selector: 'app-customer-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatProgressSpinnerModule, PageHeader],
  templateUrl: './customer-create.html',
  styleUrl: './customer-create.scss'
})
export class CustomerCreate implements HasUnsavedChanges {
  private readonly fb = inject(FormBuilder);
  private readonly customerService = inject(CustomerService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);

  readonly breadcrumbs: Breadcrumb[] = [{ label: 'Customers', link: '/customers' }, { label: 'New Customer' }];
  isLoading = false;

  form: FormGroup = this.fb.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email]],
    phone: [''],
    company: [''],
    address: [''],
    city: [''],
    state: [''],
    zipCode: [''],
    notes: ['']
  });

  hasUnsavedChanges(): boolean { return this.form.dirty; }

  onSubmit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.isLoading = true;
    this.customerService.createCustomer(this.form.value).subscribe({
      next: (customer) => {
        this.notificationService.showSuccess('Customer created successfully');
        this.router.navigate(['/customers', customer.customerId]);
      },
      error: () => { this.isLoading = false; }
    });
  }

  onCancel(): void { this.router.navigate(['/customers']); }
}
