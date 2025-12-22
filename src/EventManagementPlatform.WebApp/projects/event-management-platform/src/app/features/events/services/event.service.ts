import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/tokens/api.token';
import { PagedResult } from '../../../core/models/common.model';
import {
  EventListDto,
  EventDetailDto,
  CreateEventDto,
  UpdateEventDto,
  EventQueryParams,
  EventStatus
} from '../models/event.model';

@Injectable({ providedIn: 'root' })
export class EventService {
  private readonly baseUrl = inject(API_BASE_URL);
  private readonly http = inject(HttpClient);

  getEvents(params: EventQueryParams = {}): Observable<PagedResult<EventListDto>> {
    let httpParams = new HttpParams();

    if (params.pageNumber) httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
    if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
    if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);
    if (params.status) httpParams = httpParams.set('status', params.status);
    if (params.startDate) httpParams = httpParams.set('startDate', params.startDate.toISOString());
    if (params.endDate) httpParams = httpParams.set('endDate', params.endDate.toISOString());
    if (params.customerId) httpParams = httpParams.set('customerId', params.customerId);
    if (params.venueId) httpParams = httpParams.set('venueId', params.venueId);
    if (params.sortBy) httpParams = httpParams.set('sortBy', params.sortBy);
    if (params.sortDirection) httpParams = httpParams.set('sortDirection', params.sortDirection);

    return this.http.get<PagedResult<EventListDto>>(`${this.baseUrl}/api/events`, { params: httpParams });
  }

  getEventById(eventId: string): Observable<EventDetailDto> {
    return this.http.get<EventDetailDto>(`${this.baseUrl}/api/events/${eventId}`);
  }

  createEvent(event: CreateEventDto): Observable<EventDetailDto> {
    return this.http.post<EventDetailDto>(`${this.baseUrl}/api/events`, event);
  }

  updateEvent(eventId: string, event: UpdateEventDto): Observable<EventDetailDto> {
    return this.http.put<EventDetailDto>(`${this.baseUrl}/api/events/${eventId}`, event);
  }

  deleteEvent(eventId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/api/events/${eventId}`);
  }

  updateStatus(eventId: string, status: EventStatus): Observable<void> {
    return this.http.patch<void>(`${this.baseUrl}/api/events/${eventId}/status`, { status });
  }

  cancelEvent(eventId: string, reason?: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/api/events/${eventId}/cancel`, { reason });
  }

  confirmEvent(eventId: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/api/events/${eventId}/confirm`, {});
  }
}
