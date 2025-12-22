export interface EventListDto {
  eventId: string;
  title: string;
  eventDate: Date;
  startTime: string;
  endTime: string;
  status: EventStatus;
  venueId?: string;
  venueName?: string;
  customerId: string;
  customerName: string;
  attendeeCount?: number;
}

export interface EventDetailDto {
  eventId: string;
  title: string;
  description?: string;
  eventDate: Date;
  startTime: string;
  endTime: string;
  status: EventStatus;
  venueId?: string;
  venue?: VenueDto;
  customerId: string;
  customer?: CustomerDto;
  attendeeCount?: number;
  notes?: string;
  createdAt: Date;
  modifiedAt?: Date;
}

export interface CreateEventDto {
  title: string;
  description?: string;
  eventDate: Date;
  startTime: string;
  endTime: string;
  venueId?: string;
  customerId: string;
  attendeeCount?: number;
  notes?: string;
}

export interface UpdateEventDto {
  title: string;
  description?: string;
  eventDate: Date;
  startTime: string;
  endTime: string;
  venueId?: string;
  customerId: string;
  attendeeCount?: number;
  notes?: string;
}

export interface VenueDto {
  venueId: string;
  name: string;
  address: string;
  city: string;
  capacity: number;
}

export interface CustomerDto {
  customerId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
}

export type EventStatus =
  | 'Draft'
  | 'Scheduled'
  | 'Confirmed'
  | 'InProgress'
  | 'Completed'
  | 'Cancelled';

export interface EventQueryParams {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  status?: EventStatus;
  startDate?: Date;
  endDate?: Date;
  customerId?: string;
  venueId?: string;
  sortBy?: string;
  sortDirection?: 'asc' | 'desc';
}
