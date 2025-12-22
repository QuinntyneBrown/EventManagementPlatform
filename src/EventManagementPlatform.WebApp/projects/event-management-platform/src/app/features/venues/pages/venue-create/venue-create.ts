import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { PageHeader, Breadcrumb } from '../../../../shared/components/page-header';
import { VenueService } from '../../services/venue.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { VenueType, VenueTypeLabels } from '../../models/venue.model';
import { HasUnsavedChanges } from '../../../../core/guards/unsaved-changes.guard';

@Component({
  selector: 'app-venue-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, MatProgressSpinnerModule, MatChipsModule, MatIconModule, PageHeader],
  templateUrl: './venue-create.html',
  styleUrl: './venue-create.scss'
})
export class VenueCreate implements HasUnsavedChanges {
  private readonly fb = inject(FormBuilder);
  private readonly venueService = inject(VenueService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);

  readonly breadcrumbs: Breadcrumb[] = [{ label: 'Venues', link: '/venues' }, { label: 'New Venue' }];
  readonly venueTypes = Object.values(VenueType);
  readonly venueTypeLabels = VenueTypeLabels;
  isSaving = false;
  amenities: string[] = [];
  newAmenity = '';

  form: FormGroup = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    description: [''],
    venueType: ['', Validators.required],
    capacity: ['', [Validators.required, Validators.min(1)]],
    hourlyRate: ['', [Validators.required, Validators.min(0)]],
    address: ['', Validators.required],
    city: ['', Validators.required],
    state: ['', Validators.required],
    postalCode: ['', Validators.required],
    phone: [''],
    email: ['', Validators.email],
    website: [''],
    contactPerson: [''],
    notes: ['']
  });

  hasUnsavedChanges(): boolean { return this.form.dirty; }

  addAmenity(): void {
    const amenity = this.newAmenity.trim();
    if (amenity && !this.amenities.includes(amenity)) {
      this.amenities.push(amenity);
      this.newAmenity = '';
      this.form.markAsDirty();
    }
  }

  removeAmenity(amenity: string): void {
    this.amenities = this.amenities.filter(a => a !== amenity);
    this.form.markAsDirty();
  }

  onSubmit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.isSaving = true;
    const request = { ...this.form.value, amenities: this.amenities };
    this.venueService.createVenue(request).subscribe({
      next: (venue) => {
        this.notificationService.showSuccess('Venue created successfully');
        this.form.reset();
        this.router.navigate(['/venues', venue.venueId]);
      },
      error: () => { this.isSaving = false; }
    });
  }

  onCancel(): void { this.router.navigate(['/venues']); }
}
