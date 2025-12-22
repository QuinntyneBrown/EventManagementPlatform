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
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { PageHeader, Breadcrumb } from '../../../../shared/components/page-header';
import { LoadingSpinner } from '../../../../shared/components/loading-spinner';
import { StaffService } from '../../services/staff.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { StaffDetailDto, StaffRole, StaffRoleLabels } from '../../models/staff.model';
import { HasUnsavedChanges } from '../../../../core/guards/unsaved-changes.guard';

@Component({
  selector: 'app-staff-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatDatepickerModule, MatNativeDateModule, MatButtonModule, MatProgressSpinnerModule, MatChipsModule, MatIconModule, PageHeader, LoadingSpinner],
  templateUrl: './staff-edit.html',
  styleUrl: './staff-edit.scss'
})
export class StaffEdit implements OnInit, HasUnsavedChanges {
  @Input() staffId!: string;
  private readonly fb = inject(FormBuilder);
  private readonly staffService = inject(StaffService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);

  readonly breadcrumbs: Breadcrumb[] = [{ label: 'Staff', link: '/staff' }, { label: 'Edit Staff Member' }];
  readonly staffRoles = Object.values(StaffRole);
  readonly staffRoleLabels = StaffRoleLabels;
  staff: StaffDetailDto | null = null;
  isLoading = false;
  isSaving = false;
  skills: string[] = [];
  certifications: string[] = [];
  newSkill = '';
  newCertification = '';

  form: FormGroup = this.fb.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email]],
    phone: [''],
    role: ['', Validators.required],
    hireDate: ['', Validators.required],
    hourlyRate: ['', [Validators.required, Validators.min(0)]],
    address: [''], city: [''], state: [''], zipCode: [''],
    emergencyContact: [''], emergencyPhone: [''],
    notes: ['']
  });

  ngOnInit(): void { this.loadStaff(); }
  hasUnsavedChanges(): boolean { return this.form.dirty; }

  loadStaff(): void {
    this.isLoading = true;
    this.staffService.getStaffById(this.staffId).subscribe({
      next: (staff) => {
        this.staff = staff;
        this.skills = staff.skills || [];
        this.certifications = staff.certifications || [];
        this.form.patchValue({ ...staff, hireDate: new Date(staff.hireDate) });
        this.isLoading = false;
      },
      error: () => { this.isLoading = false; this.router.navigate(['/staff']); }
    });
  }

  addSkill(): void {
    const skill = this.newSkill.trim();
    if (skill && !this.skills.includes(skill)) { this.skills.push(skill); this.newSkill = ''; this.form.markAsDirty(); }
  }

  removeSkill(skill: string): void { this.skills = this.skills.filter(s => s !== skill); this.form.markAsDirty(); }

  addCertification(): void {
    const cert = this.newCertification.trim();
    if (cert && !this.certifications.includes(cert)) { this.certifications.push(cert); this.newCertification = ''; this.form.markAsDirty(); }
  }

  removeCertification(cert: string): void { this.certifications = this.certifications.filter(c => c !== cert); this.form.markAsDirty(); }

  onSubmit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.isSaving = true;
    const request = { ...this.form.value, staffId: this.staffId, skills: this.skills, certifications: this.certifications };
    this.staffService.updateStaff(this.staffId, request).subscribe({
      next: () => {
        this.notificationService.showSuccess('Staff member updated successfully');
        this.router.navigate(['/staff', this.staffId]);
      },
      error: () => { this.isSaving = false; }
    });
  }

  onCancel(): void { this.router.navigate(['/staff', this.staffId]); }
}
