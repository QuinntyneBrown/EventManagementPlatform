import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
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
import { StaffService } from '../../services/staff.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { StaffRole, StaffRoleLabels } from '../../models/staff.model';
import { HasUnsavedChanges } from '../../../../core/guards/unsaved-changes.guard';

@Component({
  selector: 'app-staff-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatDatepickerModule, MatNativeDateModule, MatButtonModule, MatProgressSpinnerModule, MatChipsModule, MatIconModule, PageHeader],
  templateUrl: './staff-create.html',
  styleUrl: './staff-create.scss'
})
export class StaffCreate implements HasUnsavedChanges {
  private readonly fb = inject(FormBuilder);
  private readonly staffService = inject(StaffService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);

  readonly breadcrumbs: Breadcrumb[] = [{ label: 'Staff', link: '/staff' }, { label: 'New Staff Member' }];
  readonly staffRoles = Object.values(StaffRole);
  readonly staffRoleLabels = StaffRoleLabels;
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
    address: [''], city: [''], state: [''], postalCode: [''],
    emergencyContact: [''], emergencyPhone: [''],
    notes: ['']
  });

  hasUnsavedChanges(): boolean { return this.form.dirty; }

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
    const request = { ...this.form.value, skills: this.skills, certifications: this.certifications };
    this.staffService.createStaff(request).subscribe({
      next: (staff) => {
        this.notificationService.showSuccess('Staff member created successfully');
        this.form.reset();
        this.router.navigate(['/staff', staff.staffId]);
      },
      error: () => { this.isSaving = false; }
    });
  }

  onCancel(): void { this.router.navigate(['/staff']); }
}
