import { describe, it, expect, beforeEach, afterEach } from 'vitest';
import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { VenueService } from './venue.service';
import { API_BASE_URL } from '../../../core/tokens/api.token';
import { PagedResult } from '../../../core/models/common.model';
import { VenueListItemDto, VenueDetailDto, CreateVenueRequest, VenueType, VenueStatus } from '../models/venue.model';

/**
 * Venue Service Unit Tests
 * Tests API contract compliance for Venue endpoints
 *
 * API Contract Reference:
 * - GET /api/venues - List venues with pagination and filters
 * - GET /api/venues/{venueId} - Get venue by ID
 * - POST /api/venues - Create venue
 * - PUT /api/venues/{venueId} - Update venue
 * - DELETE /api/venues/{venueId} - Delete venue
 */
describe('VenueService', () => {
  let service: VenueService;
  let httpMock: HttpTestingController;
  const apiBaseUrl = 'http://localhost:5000';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: API_BASE_URL, useValue: apiBaseUrl },
        VenueService
      ]
    });

    service = TestBed.inject(VenueService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('getVenues', () => {
    it('should call GET /api/venues with pagination parameters', () => {
      const mockResponse: PagedResult<VenueListItemDto> = {
        items: [],
        totalCount: 0,
        pageNumber: 1,
        pageSize: 20,
        totalPages: 0,
        hasNextPage: false,
        hasPreviousPage: false
      };

      service.getVenues({ pageIndex: 0, pageSize: 20 }).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/api/venues`);
      expect(req.request.method).toBe('GET');
      expect(req.request.params.get('pageIndex')).toBe('0');
      expect(req.request.params.get('pageSize')).toBe('20');
      req.flush(mockResponse);
    });

    it('should include search term when provided', () => {
      service.getVenues({ pageIndex: 0, pageSize: 10, searchTerm: 'Convention' }).subscribe();

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/api/venues`);
      expect(req.request.params.get('searchTerm')).toBe('Convention');
      req.flush({ items: [], totalCount: 0, pageNumber: 1, pageSize: 10, totalPages: 0, hasNextPage: false, hasPreviousPage: false });
    });

    it('should include sort parameters when provided', () => {
      service.getVenues({ pageIndex: 0, pageSize: 20, sortColumn: 'name', sortDirection: 'asc' }).subscribe();

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/api/venues`);
      expect(req.request.params.get('sortColumn')).toBe('name');
      expect(req.request.params.get('sortDirection')).toBe('asc');
      req.flush({ items: [], totalCount: 0, pageNumber: 1, pageSize: 20, totalPages: 0, hasNextPage: false, hasPreviousPage: false });
    });
  });

  describe('getVenueById', () => {
    it('should call GET /api/venues/{venueId}', () => {
      const venueId = '550e8400-e29b-41d4-a716-446655440000';
      const mockVenue: VenueDetailDto = {
        venueId,
        name: 'Grand Ballroom',
        description: 'Elegant event space',
        venueType: VenueType.BanquetHall,
        capacity: 500,
        hourlyRate: 150,
        address: '100 Event Plaza',
        city: 'Chicago',
        state: 'IL',
        postalCode: '60601',
        phone: '555-1000',
        email: 'info@grandballroom.com',
        website: 'https://grandballroom.com',
        amenities: ['Parking', 'Catering', 'WiFi'],
        images: [],
        contactPerson: 'Event Manager',
        notes: '',
        status: VenueStatus.Active,
        rating: 4.5,
        totalEvents: 25,
        createdAt: new Date(),
        updatedAt: new Date()
      };

      service.getVenueById(venueId).subscribe(response => {
        expect(response.venueId).toBe(venueId);
        expect(response.name).toBe('Grand Ballroom');
        expect(response.postalCode).toBe('60601');
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/api/venues/${venueId}`);
      expect(req.request.method).toBe('GET');
      req.flush(mockVenue);
    });
  });

  describe('createVenue', () => {
    it('should call POST /api/venues with venue data', () => {
      const newVenue: CreateVenueRequest = {
        name: 'New Conference Center',
        description: 'Modern meeting space',
        venueType: VenueType.ConferenceCenter,
        capacity: 200,
        hourlyRate: 100,
        address: '200 Business Way',
        city: 'Seattle',
        state: 'WA',
        postalCode: '98101',
        phone: '555-2000',
        email: 'contact@newconf.com',
        website: 'https://newconf.com',
        amenities: ['WiFi', 'Projector'],
        contactPerson: 'Conference Manager',
        notes: ''
      };

      const mockResponse: VenueDetailDto = {
        ...newVenue,
        venueId: '550e8400-e29b-41d4-a716-446655440001',
        status: VenueStatus.Active,
        rating: 0,
        totalEvents: 0,
        images: [],
        createdAt: new Date(),
        updatedAt: new Date()
      };

      service.createVenue(newVenue).subscribe(response => {
        expect(response.venueId).toBeDefined();
        expect(response.name).toBe('New Conference Center');
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/api/venues`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(newVenue);
      req.flush(mockResponse);
    });

    it('should send postalCode (not zipCode) in request body', () => {
      const venueWithPostalCode: CreateVenueRequest = {
        name: 'Test Venue',
        description: 'Test description',
        venueType: VenueType.Other,
        capacity: 100,
        hourlyRate: 50,
        address: '123 Test St',
        city: 'Test City',
        state: 'TC',
        postalCode: '54321',
        phone: '555-0000',
        email: 'test@venue.com',
        website: '',
        amenities: [],
        contactPerson: 'Test Manager',
        notes: ''
      };

      service.createVenue(venueWithPostalCode).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/api/venues`);
      expect(req.request.body.postalCode).toBe('54321');
      expect((req.request.body as any).zipCode).toBeUndefined();
      req.flush({ venueId: '123', ...venueWithPostalCode, status: VenueStatus.Active, rating: 0, totalEvents: 0, images: [], createdAt: new Date(), updatedAt: new Date() });
    });
  });

  describe('updateVenue', () => {
    it('should call PUT /api/venues/{venueId}', () => {
      const venueId = '550e8400-e29b-41d4-a716-446655440000';
      const updateData = {
        venueId,
        name: 'Updated Ballroom',
        description: 'Updated elegant event space',
        venueType: VenueType.BanquetHall,
        capacity: 600,
        hourlyRate: 175,
        address: '100 Event Plaza',
        city: 'Chicago',
        state: 'IL',
        postalCode: '60602',
        phone: '555-1000',
        email: 'info@updatedballroom.com',
        website: 'https://updatedballroom.com',
        amenities: ['Parking', 'Catering', 'WiFi', 'Stage'],
        contactPerson: 'Senior Event Manager',
        notes: 'Renovated in 2024'
      };

      service.updateVenue(venueId, updateData).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/api/venues/${venueId}`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual(updateData);
      req.flush({ ...updateData, status: VenueStatus.Active, rating: 4.5, totalEvents: 25, images: [], createdAt: new Date(), updatedAt: new Date() });
    });
  });

  describe('deleteVenue', () => {
    it('should call DELETE /api/venues/{venueId}', () => {
      const venueId = '550e8400-e29b-41d4-a716-446655440000';

      service.deleteVenue(venueId).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/api/venues/${venueId}`);
      expect(req.request.method).toBe('DELETE');
      req.flush(null);
    });
  });
});
