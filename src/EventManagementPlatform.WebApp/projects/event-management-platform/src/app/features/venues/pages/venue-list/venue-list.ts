import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialog } from '@angular/material/dialog';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { PageHeader, Breadcrumb } from '../../../../shared/components/page-header';
import { StatusBadge } from '../../../../shared/components/status-badge';
import { LoadingSpinner } from '../../../../shared/components/loading-spinner';
import { EmptyState } from '../../../../shared/components/empty-state';
import { ConfirmDialog } from '../../../../shared/components/confirm-dialog';
import { VenueService } from '../../services/venue.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { VenueListItemDto, VenueType, VenueStatus, VenueTypeLabels, VenueStatusLabels } from '../../models/venue.model';
import { QueryParams } from '../../../../core/models/common.model';

@Component({
  selector: 'app-venue-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, MatTableModule, MatPaginatorModule, MatSortModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, MatSelectModule, MatCardModule, MatMenuModule, PageHeader, StatusBadge, LoadingSpinner, EmptyState],
  templateUrl: './venue-list.html',
  styleUrl: './venue-list.scss'
})
export class VenueList implements OnInit {
  private readonly venueService = inject(VenueService);
  private readonly notificationService = inject(NotificationService);
  private readonly dialog = inject(MatDialog);
  private readonly searchSubject = new Subject<string>();

  readonly breadcrumbs: Breadcrumb[] = [{ label: 'Venues' }];
  readonly displayedColumns = ['name', 'venueType', 'city', 'capacity', 'hourlyRate', 'status', 'actions'];
  readonly venueTypes = Object.values(VenueType);
  readonly venueTypeLabels = VenueTypeLabels;
  readonly venueStatusLabels = VenueStatusLabels;

  venues: VenueListItemDto[] = [];
  totalCount = 0;
  isLoading = false;
  searchTerm = '';
  selectedType: VenueType | '' = '';
  query: QueryParams = { pageIndex: 0, pageSize: 10, sortColumn: 'name', sortDirection: 'asc' };

  ngOnInit(): void {
    this.searchSubject.pipe(debounceTime(300), distinctUntilChanged()).subscribe(term => {
      this.searchTerm = term;
      this.query.pageIndex = 0;
      this.loadVenues();
    });
    this.loadVenues();
  }

  loadVenues(): void {
    this.isLoading = true;
    const queryParams = { ...this.query, searchTerm: this.searchTerm };
    this.venueService.getVenues(queryParams).subscribe({
      next: (result) => { this.venues = result.items; this.totalCount = result.totalCount; this.isLoading = false; },
      error: () => { this.isLoading = false; }
    });
  }

  onSearch(term: string): void { this.searchSubject.next(term); }
  onPageChange(event: PageEvent): void { this.query.pageIndex = event.pageIndex; this.query.pageSize = event.pageSize; this.loadVenues(); }
  onSortChange(sort: Sort): void { this.query.sortColumn = sort.active; this.query.sortDirection = sort.direction || 'asc'; this.loadVenues(); }

  deleteVenue(venue: VenueListItemDto): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: { title: 'Delete Venue', message: `Are you sure you want to delete "${venue.name}"?`, confirmText: 'Delete', type: 'danger' }
    });
    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.venueService.deleteVenue(venue.venueId).subscribe({
          next: () => { this.notificationService.showSuccess('Venue deleted successfully'); this.loadVenues(); },
          error: () => {}
        });
      }
    });
  }

  getStatusColor(status: VenueStatus): string {
    const colors: Record<VenueStatus, string> = { [VenueStatus.Active]: 'success', [VenueStatus.Inactive]: 'default', [VenueStatus.UnderMaintenance]: 'warning' };
    return colors[status] || 'default';
  }

  formatCurrency(value: number): string { return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(value); }
}
