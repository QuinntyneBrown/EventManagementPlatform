import { describe, it, expect, beforeEach, afterEach } from 'vitest';
import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { EventService } from './event.service';
import { API_BASE_URL } from '../../../core/tokens/api.token';
import { PagedResult } from '../../../core/models/common.model';
import { EventListDto, EventDetailDto, CreateEventDto, EventStatus } from '../models/event.model';

/**
 * Event Service Unit Tests
 * Tests API contract compliance for Event endpoints
 *
 * API Contract Reference:
 * - GET /api/events - List events with pagination and filters
 * - GET /api/events/{eventId} - Get event by ID
 * - POST /api/events - Create event
 * - PUT /api/events/{eventId} - Update event
 * - DELETE /api/events/{eventId} - Delete event
 * - PATCH /api/events/{eventId}/status - Update event status
 */
describe('EventService', () => {
  let service: EventService;
  let httpMock: HttpTestingController;
  const apiBaseUrl = 'http://localhost:5000';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: API_BASE_URL, useValue: apiBaseUrl },
        EventService
      ]
    });

    service = TestBed.inject(EventService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('getEvents', () => {
    it('should call GET /events with correct URL', () => {
      const mockResponse: PagedResult<EventListDto> = {
        items: [],
        totalCount: 0,
        pageNumber: 1,
        pageSize: 20,
        totalPages: 0,
        hasNextPage: false,
        hasPreviousPage: false
      };

      service.getEvents().subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/events`);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
    });

    it('should include pagination parameters when provided', () => {
      service.getEvents({ pageNumber: 2, pageSize: 10 }).subscribe();

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/events`);
      expect(req.request.params.get('pageNumber')).toBe('2');
      expect(req.request.params.get('pageSize')).toBe('10');
      req.flush({ items: [], totalCount: 0, pageNumber: 2, pageSize: 10, totalPages: 0, hasNextPage: false, hasPreviousPage: true });
    });

    it('should include status filter when provided', () => {
      service.getEvents({ status: 'Scheduled' }).subscribe();

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/events`);
      expect(req.request.params.get('status')).toBe('Scheduled');
      req.flush({ items: [], totalCount: 0, pageNumber: 1, pageSize: 20, totalPages: 0, hasNextPage: false, hasPreviousPage: false });
    });

    it('should include customerId filter when provided', () => {
      const customerId = '550e8400-e29b-41d4-a716-446655440000';
      service.getEvents({ customerId }).subscribe();

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/events`);
      expect(req.request.params.get('customerId')).toBe(customerId);
      req.flush({ items: [], totalCount: 0, pageNumber: 1, pageSize: 20, totalPages: 0, hasNextPage: false, hasPreviousPage: false });
    });

    it('should include venueId filter when provided', () => {
      const venueId = '550e8400-e29b-41d4-a716-446655440001';
      service.getEvents({ venueId }).subscribe();

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/events`);
      expect(req.request.params.get('venueId')).toBe(venueId);
      req.flush({ items: [], totalCount: 0, pageNumber: 1, pageSize: 20, totalPages: 0, hasNextPage: false, hasPreviousPage: false });
    });

    it('should include date range filters when provided', () => {
      const startDate = new Date('2024-01-01');
      const endDate = new Date('2024-12-31');
      service.getEvents({ startDate, endDate }).subscribe();

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/events`);
      expect(req.request.params.get('startDate')).toBe(startDate.toISOString());
      expect(req.request.params.get('endDate')).toBe(endDate.toISOString());
      req.flush({ items: [], totalCount: 0, pageNumber: 1, pageSize: 20, totalPages: 0, hasNextPage: false, hasPreviousPage: false });
    });
  });

  describe('getEventById', () => {
    it('should call GET /events/{eventId}', () => {
      const eventId = '550e8400-e29b-41d4-a716-446655440000';
      const mockEvent: EventDetailDto = {
        eventId,
        title: 'Annual Gala',
        description: 'Annual company celebration',
        eventDate: new Date('2024-06-15'),
        startTime: '18:00',
        endTime: '23:00',
        status: 'Scheduled',
        customerId: 'cust-123',
        createdAt: new Date()
      };

      service.getEventById(eventId).subscribe(response => {
        expect(response.eventId).toBe(eventId);
        expect(response.title).toBe('Annual Gala');
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/events/${eventId}`);
      expect(req.request.method).toBe('GET');
      req.flush(mockEvent);
    });
  });

  describe('createEvent', () => {
    it('should call POST /events with event data', () => {
      const newEvent: CreateEventDto = {
        title: 'Product Launch',
        description: 'New product announcement',
        eventDate: new Date('2024-08-20'),
        startTime: '14:00',
        endTime: '17:00',
        customerId: 'cust-456',
        venueId: 'venue-123',
        attendeeCount: 150
      };

      const mockResponse: EventDetailDto = {
        ...newEvent,
        eventId: '550e8400-e29b-41d4-a716-446655440001',
        status: 'Draft',
        createdAt: new Date()
      };

      service.createEvent(newEvent).subscribe(response => {
        expect(response.eventId).toBeDefined();
        expect(response.title).toBe('Product Launch');
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/events`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(newEvent);
      req.flush(mockResponse);
    });
  });

  describe('updateEvent', () => {
    it('should call PUT /events/{eventId}', () => {
      const eventId = '550e8400-e29b-41d4-a716-446655440000';
      const updateData = {
        title: 'Updated Gala',
        eventDate: new Date('2024-06-16'),
        startTime: '19:00',
        endTime: '00:00',
        customerId: 'cust-123'
      };

      service.updateEvent(eventId, updateData).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/events/${eventId}`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual(updateData);
      req.flush({ eventId, ...updateData, status: 'Scheduled', createdAt: new Date() });
    });
  });

  describe('deleteEvent', () => {
    it('should call DELETE /events/{eventId}', () => {
      const eventId = '550e8400-e29b-41d4-a716-446655440000';

      service.deleteEvent(eventId).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/events/${eventId}`);
      expect(req.request.method).toBe('DELETE');
      req.flush(null);
    });
  });

  describe('updateStatus', () => {
    it('should call PATCH /events/{eventId}/status', () => {
      const eventId = '550e8400-e29b-41d4-a716-446655440000';
      const status: EventStatus = 'Confirmed';

      service.updateStatus(eventId, status).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/events/${eventId}/status`);
      expect(req.request.method).toBe('PATCH');
      expect(req.request.body).toEqual({ status });
      req.flush(null);
    });
  });

  describe('cancelEvent', () => {
    it('should call POST /events/{eventId}/cancel', () => {
      const eventId = '550e8400-e29b-41d4-a716-446655440000';
      const reason = 'Weather conditions';

      service.cancelEvent(eventId, reason).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/events/${eventId}/cancel`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual({ reason });
      req.flush(null);
    });
  });

  describe('confirmEvent', () => {
    it('should call POST /events/{eventId}/confirm', () => {
      const eventId = '550e8400-e29b-41d4-a716-446655440000';

      service.confirmEvent(eventId).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/events/${eventId}/confirm`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual({});
      req.flush(null);
    });
  });
});
