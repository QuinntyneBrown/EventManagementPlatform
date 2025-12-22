import { Component, Input, Output, EventEmitter, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CreateEventDto, UpdateEventDto, EventDetailDto } from '../../models/event.model';

@Component({
  selector: 'app-event-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './event-form.html',
  styleUrl: './event-form.scss'
})
export class EventForm implements OnInit {
  @Input() event?: EventDetailDto;
  @Input() isLoading = false;
  @Output() formSubmit = new EventEmitter<CreateEventDto | UpdateEventDto>();
  @Output() formCancel = new EventEmitter<void>();

  private readonly fb = inject(FormBuilder);

  eventForm!: FormGroup;

  ngOnInit(): void {
    this.initForm();

    if (this.event) {
      this.populateForm();
    }
  }

  get isEditMode(): boolean {
    return !!this.event;
  }

  onSubmit(): void {
    if (this.eventForm.invalid) {
      this.eventForm.markAllAsTouched();
      return;
    }

    const formValue = this.eventForm.value;
    const eventData: CreateEventDto | UpdateEventDto = {
      title: formValue.title,
      description: formValue.description || undefined,
      eventDate: formValue.eventDate,
      startTime: formValue.startTime,
      endTime: formValue.endTime,
      venueId: formValue.venueId || undefined,
      customerId: formValue.customerId,
      attendeeCount: formValue.attendeeCount || undefined,
      notes: formValue.notes || undefined
    };

    this.formSubmit.emit(eventData);
  }

  onCancel(): void {
    this.formCancel.emit();
  }

  private initForm(): void {
    this.eventForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', [Validators.maxLength(2000)]],
      eventDate: [null, [Validators.required]],
      startTime: ['', [Validators.required]],
      endTime: ['', [Validators.required]],
      venueId: [''],
      customerId: ['', [Validators.required]],
      attendeeCount: [null, [Validators.min(1)]],
      notes: ['', [Validators.maxLength(2000)]]
    });
  }

  private populateForm(): void {
    if (this.event) {
      this.eventForm.patchValue({
        title: this.event.title,
        description: this.event.description,
        eventDate: new Date(this.event.eventDate),
        startTime: this.event.startTime,
        endTime: this.event.endTime,
        venueId: this.event.venueId,
        customerId: this.event.customerId,
        attendeeCount: this.event.attendeeCount,
        notes: this.event.notes
      });
    }
  }
}
