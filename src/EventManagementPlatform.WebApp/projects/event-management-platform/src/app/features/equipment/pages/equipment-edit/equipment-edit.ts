import { Component, OnInit, inject, Input } from '@angular/core';
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
import { LoadingSpinner } from '../../../../shared/components/loading-spinner';
import { EquipmentService } from '../../services/equipment.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { EquipmentDetailDto, EquipmentCategory, EquipmentStatus, EquipmentCondition, EquipmentCategoryLabels, EquipmentStatusLabels, EquipmentConditionLabels } from '../../models/equipment.model';
import { HasUnsavedChanges } from '../../../../core/guards/unsaved-changes.guard';

@Component({
  selector: 'app-equipment-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatDatepickerModule, MatNativeDateModule, MatButtonModule, MatProgressSpinnerModule, PageHeader, LoadingSpinner],
  templateUrl: './equipment-edit.html',
  styleUrl: './equipment-edit.scss'
})
export class EquipmentEdit implements OnInit, HasUnsavedChanges {
  @Input() equipmentId!: string;
  private readonly fb = inject(FormBuilder);
  private readonly equipmentService = inject(EquipmentService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);

  readonly breadcrumbs: Breadcrumb[] = [{ label: 'Equipment', link: '/equipment' }, { label: 'Edit Equipment' }];
  readonly equipmentCategories = Object.values(EquipmentCategory);
  readonly equipmentStatuses = Object.values(EquipmentStatus);
  readonly equipmentConditions = Object.values(EquipmentCondition);
  readonly equipmentCategoryLabels = EquipmentCategoryLabels;
  readonly equipmentStatusLabels = EquipmentStatusLabels;
  readonly equipmentConditionLabels = EquipmentConditionLabels;
  equipment: EquipmentDetailDto | null = null;
  isLoading = false;
  isSaving = false;

  form: FormGroup = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    description: [''],
    category: ['', Validators.required],
    quantity: [1, [Validators.required, Validators.min(1)]],
    dailyRate: ['', [Validators.required, Validators.min(0)]],
    status: ['', Validators.required],
    condition: ['', Validators.required],
    serialNumber: [''],
    manufacturer: [''],
    model: [''],
    purchaseDate: [''],
    purchasePrice: [''],
    location: [''],
    notes: ['']
  });

  ngOnInit(): void { this.loadEquipment(); }
  hasUnsavedChanges(): boolean { return this.form.dirty; }

  loadEquipment(): void {
    this.isLoading = true;
    this.equipmentService.getEquipmentById(this.equipmentId).subscribe({
      next: (equipment) => {
        this.equipment = equipment;
        this.form.patchValue({
          ...equipment,
          purchaseDate: equipment.purchaseDate ? new Date(equipment.purchaseDate) : null
        });
        this.isLoading = false;
      },
      error: () => { this.isLoading = false; this.router.navigate(['/equipment']); }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.isSaving = true;
    const request = { ...this.form.value, equipmentId: this.equipmentId };
    this.equipmentService.updateEquipment(this.equipmentId, request).subscribe({
      next: () => {
        this.notificationService.showSuccess('Equipment updated successfully');
        this.router.navigate(['/equipment', this.equipmentId]);
      },
      error: () => { this.isSaving = false; }
    });
  }

  onCancel(): void { this.router.navigate(['/equipment', this.equipmentId]); }
}
