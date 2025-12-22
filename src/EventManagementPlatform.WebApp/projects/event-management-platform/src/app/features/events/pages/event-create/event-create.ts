import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { PageHeader, Breadcrumb } from '../../../../shared/components/page-header';
import { EventForm } from '../../components/event-form';
import { EventService } from '../../services/event.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { CreateEventDto } from '../../models/event.model';
import { HasUnsavedChanges } from '../../../../core/guards/unsaved-changes.guard';

@Component({
  selector: 'app-event-create',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    PageHeader,
    EventForm
  ],
  templateUrl: './event-create.html',
  styleUrl: './event-create.scss'
})
export class EventCreate implements HasUnsavedChanges {
  private readonly eventService = inject(EventService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);

  isLoading = false;
  private formDirty = false;

  readonly breadcrumbs: Breadcrumb[] = [
    { label: 'Events', link: '/events' },
    { label: 'New Event' }
  ];

  hasUnsavedChanges(): boolean {
    return this.formDirty;
  }

  onSubmit(eventData: CreateEventDto): void {
    this.isLoading = true;
    this.eventService.createEvent(eventData).subscribe({
      next: (event) => {
        this.formDirty = false;
        this.notificationService.showSuccess('Event created successfully');
        this.router.navigate(['/events', event.eventId]);
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/events']);
  }
}
