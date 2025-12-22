import { Component, OnInit, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog } from '@angular/material/dialog';
import { PageHeader, Breadcrumb } from '../../../../shared/components/page-header';
import { StatusBadge, BadgeColor } from '../../../../shared/components/status-badge';
import { LoadingSpinner } from '../../../../shared/components/loading-spinner';
import { ConfirmDialog } from '../../../../shared/components/confirm-dialog';
import { VenueService } from '../../services/venue.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { VenueDetailDto, VenueStatus, VenueTypeLabels, VenueStatusLabels } from '../../models/venue.model';

@Component({
  selector: 'app-venue-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardModule, MatButtonModule, MatIconModule, MatTabsModule, MatChipsModule, PageHeader, StatusBadge, LoadingSpinner],
  templateUrl: './venue-detail.html',
  styleUrl: './venue-detail.scss'
})
export class VenueDetail implements OnInit {
  @Input() venueId!: string;
  private readonly venueService = inject(VenueService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);
  private readonly dialog = inject(MatDialog);

  readonly venueTypeLabels = VenueTypeLabels;
  readonly venueStatusLabels = VenueStatusLabels;
  breadcrumbs: Breadcrumb[] = [{ label: 'Venues', link: '/venues' }, { label: 'Loading...' }];
  venue: VenueDetailDto | null = null;
  isLoading = false;

  ngOnInit(): void { this.loadVenue(); }

  loadVenue(): void {
    this.isLoading = true;
    this.venueService.getVenueById(this.venueId).subscribe({
      next: (venue) => {
        this.venue = venue;
        this.breadcrumbs = [{ label: 'Venues', link: '/venues' }, { label: venue.name }];
        this.isLoading = false;
      },
      error: () => { this.isLoading = false; this.router.navigate(['/venues']); }
    });
  }

  deleteVenue(): void {
    if (!this.venue) return;
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: { title: 'Delete Venue', message: `Are you sure you want to delete "${this.venue.name}"?`, confirmText: 'Delete', type: 'danger' }
    });
    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.venueService.deleteVenue(this.venueId).subscribe({
          next: () => { this.notificationService.showSuccess('Venue deleted successfully'); this.router.navigate(['/venues']); },
          error: () => {}
        });
      }
    });
  }

  getStatusColor(status: VenueStatus): BadgeColor {
    const colors: Record<VenueStatus, BadgeColor> = { [VenueStatus.Active]: 'success', [VenueStatus.Inactive]: 'default', [VenueStatus.UnderMaintenance]: 'warning' };
    return colors[status] || 'default';
  }

  formatCurrency(value: number): string { return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(value); }
}
