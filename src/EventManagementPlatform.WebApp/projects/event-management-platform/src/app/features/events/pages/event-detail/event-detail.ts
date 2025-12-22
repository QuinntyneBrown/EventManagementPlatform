import { Component, OnInit, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDividerModule } from '@angular/material/divider';
import { MatDialog } from '@angular/material/dialog';
import { PageHeader, Breadcrumb } from '../../../../shared/components/page-header';
import { StatusBadge, BadgeColor } from '../../../../shared/components/status-badge';
import { LoadingSpinner } from '../../../../shared/components/loading-spinner';
import { ConfirmDialog } from '../../../../shared/components/confirm-dialog';
import { EventService } from '../../services/event.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { EventDetailDto, EventStatus } from '../../models/event.model';

@Component({
  selector: 'app-event-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatTabsModule,
    MatDividerModule,
    PageHeader,
    StatusBadge,
    LoadingSpinner
  ],
  templateUrl: './event-detail.html',
  styleUrl: './event-detail.scss'
})
export class EventDetail implements OnInit {
  @Input() eventId!: string;

  private readonly eventService = inject(EventService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);
  private readonly dialog = inject(MatDialog);

  event: EventDetailDto | null = null;
  isLoading = false;

  readonly breadcrumbs: Breadcrumb[] = [
    { label: 'Events', link: '/events' },
    { label: 'Event Details' }
  ];

  ngOnInit(): void {
    this.loadEvent();
  }

  loadEvent(): void {
    this.isLoading = true;
    this.eventService.getEventById(this.eventId).subscribe({
      next: (event) => {
        this.event = event;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.router.navigate(['/events']);
      }
    });
  }

  onConfirm(): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: {
        title: 'Confirm Event',
        message: 'Are you sure you want to confirm this event?',
        confirmText: 'Confirm',
        type: 'info'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed && this.event) {
        this.eventService.confirmEvent(this.event.eventId).subscribe({
          next: () => {
            this.notificationService.showSuccess('Event confirmed successfully');
            this.loadEvent();
          }
        });
      }
    });
  }

  onCancel(): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: {
        title: 'Cancel Event',
        message: 'Are you sure you want to cancel this event? This action cannot be undone.',
        confirmText: 'Cancel Event',
        type: 'danger'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed && this.event) {
        this.eventService.cancelEvent(this.event.eventId).subscribe({
          next: () => {
            this.notificationService.showSuccess('Event cancelled successfully');
            this.loadEvent();
          }
        });
      }
    });
  }

  onDelete(): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: {
        title: 'Delete Event',
        message: `Are you sure you want to delete "${this.event?.title}"? This action cannot be undone.`,
        confirmText: 'Delete',
        type: 'danger'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed && this.event) {
        this.eventService.deleteEvent(this.event.eventId).subscribe({
          next: () => {
            this.notificationService.showSuccess('Event deleted successfully');
            this.router.navigate(['/events']);
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
