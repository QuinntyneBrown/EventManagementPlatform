import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { PageHeader, Breadcrumb } from '../../../../shared/components/page-header';
import { EquipmentService } from '../../services/equipment.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { EquipmentCategory, EquipmentCategoryLabels } from '../../models/equipment.model';
import { HasUnsavedChanges } from '../../../../core/guards/unsaved-changes.guard';

@Component({
  selector: 'app-equipment-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatDatepickerModule, MatNativeDateModule, MatButtonModule, MatProgressSpinnerModule, PageHeader],
  templateUrl: './equipment-create.html',
  styleUrl: './equipment-create.scss'
})
export class EquipmentCreate implements HasUnsavedChanges {
  private readonly fb = inject(FormBuilder);
  private readonly equipmentService = inject(EquipmentService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);

  readonly breadcrumbs: Breadcrumb[] = [{ label: 'Equipment', link: '/equipment' }, { label: 'New Equipment' }];
  readonly equipmentCategories = Object.values(EquipmentCategory);
  readonly equipmentCategoryLabels = EquipmentCategoryLabels;
  isSaving = false;

  form: FormGroup = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    description: [''],
    category: ['', Validators.required],
    quantity: [1, [Validators.required, Validators.min(1)]],
    dailyRate: ['', [Validators.required, Validators.min(0)]],
    serialNumber: [''],
    manufacturer: [''],
    model: [''],
    purchaseDate: [''],
    purchasePrice: [''],
    location: [''],
    notes: ['']
  });

  hasUnsavedChanges(): boolean { return this.form.dirty; }

  onSubmit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.isSaving = true;
    this.equipmentService.createEquipment(this.form.value).subscribe({
      next: (equipment) => {
        this.notificationService.showSuccess('Equipment created successfully');
        this.form.reset();
        this.router.navigate(['/equipment', equipment.equipmentId]);
      },
      error: () => { this.isSaving = false; }
    });
  }

  onCancel(): void { this.router.navigate(['/equipment']); }
}
