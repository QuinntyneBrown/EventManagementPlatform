import { Component, OnInit, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { PageHeader, Breadcrumb } from '../../../../shared/components/page-header';
import { LoadingSpinner } from '../../../../shared/components/loading-spinner';
import { VenueService } from '../../services/venue.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { VenueDetailDto, VenueType, VenueTypeLabels } from '../../models/venue.model';
import { HasUnsavedChanges } from '../../../../core/guards/unsaved-changes.guard';

@Component({
  selector: 'app-venue-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, MatProgressSpinnerModule, MatChipsModule, MatIconModule, PageHeader, LoadingSpinner],
  templateUrl: './venue-edit.html',
  styleUrl: './venue-edit.scss'
})
export class VenueEdit implements OnInit, HasUnsavedChanges {
  @Input() venueId!: string;
  private readonly fb = inject(FormBuilder);
  private readonly venueService = inject(VenueService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);

  readonly breadcrumbs: Breadcrumb[] = [{ label: 'Venues', link: '/venues' }, { label: 'Edit Venue' }];
  readonly venueTypes = Object.values(VenueType);
  readonly venueTypeLabels = VenueTypeLabels;
  venue: VenueDetailDto | null = null;
  isLoading = false;
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
    zipCode: ['', Validators.required],
    phone: [''],
    email: ['', Validators.email],
    website: [''],
    contactPerson: [''],
    notes: ['']
  });

  ngOnInit(): void { this.loadVenue(); }
  hasUnsavedChanges(): boolean { return this.form.dirty; }

  loadVenue(): void {
    this.isLoading = true;
    this.venueService.getVenueById(this.venueId).subscribe({
      next: (venue) => {
        this.venue = venue;
        this.amenities = venue.amenities || [];
        this.form.patchValue(venue);
        this.isLoading = false;
      },
      error: () => { this.isLoading = false; this.router.navigate(['/venues']); }
    });
  }

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
    const request = { ...this.form.value, venueId: this.venueId, amenities: this.amenities };
    this.venueService.updateVenue(this.venueId, request).subscribe({
      next: () => {
        this.notificationService.showSuccess('Venue updated successfully');
        this.router.navigate(['/venues', this.venueId]);
      },
      error: () => { this.isSaving = false; }
    });
  }

  onCancel(): void { this.router.navigate(['/venues', this.venueId]); }
}
