import { Component, OnInit, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { PageHeader, Breadcrumb } from '../../../../shared/components/page-header';
import { LoadingSpinner } from '../../../../shared/components/loading-spinner';
import { EventForm } from '../../components/event-form';
import { EventService } from '../../services/event.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { EventDetailDto, UpdateEventDto } from '../../models/event.model';
import { HasUnsavedChanges } from '../../../../core/guards/unsaved-changes.guard';

@Component({
  selector: 'app-event-edit',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    PageHeader,
    LoadingSpinner,
    EventForm
  ],
  templateUrl: './event-edit.html',
  styleUrl: './event-edit.scss'
})
export class EventEdit implements OnInit, HasUnsavedChanges {
  @Input() eventId!: string;

  private readonly eventService = inject(EventService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);

  event: EventDetailDto | null = null;
  isLoading = false;
  isSaving = false;
  private formDirty = false;

  readonly breadcrumbs: Breadcrumb[] = [
    { label: 'Events', link: '/events' },
    { label: 'Edit Event' }
  ];

  ngOnInit(): void {
    this.loadEvent();
  }

  hasUnsavedChanges(): boolean {
    return this.formDirty;
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

  onSubmit(eventData: UpdateEventDto): void {
    this.isSaving = true;
    this.eventService.updateEvent(this.eventId, eventData).subscribe({
      next: () => {
        this.formDirty = false;
        this.notificationService.showSuccess('Event updated successfully');
        this.router.navigate(['/events', this.eventId]);
      },
      error: () => {
        this.isSaving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/events', this.eventId]);
  }
}
