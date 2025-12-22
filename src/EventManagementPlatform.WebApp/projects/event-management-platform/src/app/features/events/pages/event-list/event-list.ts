import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSortModule, MatSort, Sort } from '@angular/material/sort';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatMenuModule } from '@angular/material/menu';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog } from '@angular/material/dialog';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { PageHeader } from '../../../../shared/components/page-header';
import { StatusBadge, BadgeColor } from '../../../../shared/components/status-badge';
import { LoadingSpinner } from '../../../../shared/components/loading-spinner';
import { EmptyState } from '../../../../shared/components/empty-state';
import { ConfirmDialog } from '../../../../shared/components/confirm-dialog';
import { EventService } from '../../services/event.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { EventListDto, EventStatus, EventQueryParams } from '../../models/event.model';

@Component({
  selector: 'app-event-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatMenuModule,
    MatChipsModule,
    PageHeader,
    StatusBadge,
    LoadingSpinner,
    EmptyState
  ],
  templateUrl: './event-list.html',
  styleUrl: './event-list.scss'
})
export class EventList implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  private readonly eventService = inject(EventService);
  private readonly notificationService = inject(NotificationService);
  private readonly dialog = inject(MatDialog);

  displayedColumns = ['title', 'eventDate', 'venueName', 'customerName', 'status', 'actions'];
  dataSource = new MatTableDataSource<EventListDto>([]);

  isLoading = false;
  totalCount = 0;
  pageSize = 10;
  pageIndex = 0;

  searchTerm = '';
  statusFilter: EventStatus | '' = '';

  private searchSubject = new Subject<string>();

  readonly statusOptions: { value: EventStatus | ''; label: string }[] = [
    { value: '', label: 'All Statuses' },
    { value: 'Draft', label: 'Draft' },
    { value: 'Scheduled', label: 'Scheduled' },
    { value: 'Confirmed', label: 'Confirmed' },
    { value: 'InProgress', label: 'In Progress' },
    { value: 'Completed', label: 'Completed' },
    { value: 'Cancelled', label: 'Cancelled' }
  ];

  ngOnInit(): void {
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(() => {
      this.pageIndex = 0;
      this.loadEvents();
    });

    this.loadEvents();
  }

  loadEvents(): void {
    this.isLoading = true;

    const params: EventQueryParams = {
      pageNumber: this.pageIndex + 1,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm || undefined,
      status: this.statusFilter || undefined,
      sortBy: this.sort?.active,
      sortDirection: this.sort?.direction as 'asc' | 'desc' || undefined
    };

    this.eventService.getEvents(params).subscribe({
      next: (result) => {
        this.dataSource.data = result.items;
        this.totalCount = result.totalCount;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  onSearch(value: string): void {
    this.searchSubject.next(value);
  }

  onStatusChange(): void {
    this.pageIndex = 0;
    this.loadEvents();
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadEvents();
  }

  onSortChange(_sort: Sort): void {
    this.loadEvents();
  }

  onDelete(event: EventListDto): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: {
        title: 'Delete Event',
        message: `Are you sure you want to delete "${event.title}"? This action cannot be undone.`,
        confirmText: 'Delete',
        type: 'danger'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.eventService.deleteEvent(event.eventId).subscribe({
          next: () => {
            this.notificationService.showSuccess('Event deleted successfully');
            this.loadEvents();
          }
        });
      }
    });
  }

  getStatusColor(status: EventStatus): BadgeColor {
    switch (status) {
      case 'Draft':
        return 'neutral';
      case 'Scheduled':
        return 'info';
      case 'Confirmed':
        return 'primary';
      case 'InProgress':
        return 'accent';
      case 'Completed':
        return 'success';
      case 'Cancelled':
        return 'warn';
      default:
        return 'neutral';
    }
  }
}
