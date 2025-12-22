import { describe, it, expect, beforeEach, afterEach } from 'vitest';
import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { StaffService } from './staff.service';
import { API_BASE_URL } from '../../../core/tokens/api.token';
import { PagedResult } from '../../../core/models/common.model';
import { StaffListItemDto, StaffDetailDto, CreateStaffRequest, StaffRole, StaffStatus } from '../models/staff.model';

/**
 * Staff Service Unit Tests
 * Tests API contract compliance for Staff endpoints
 *
 * API Contract Reference:
 * - GET /api/staff - List staff with pagination and filters
 * - GET /api/staff/{staffMemberId} - Get staff member by ID
 * - POST /api/staff - Create staff member
 * - PUT /api/staff/{staffMemberId} - Update staff member
 * - DELETE /api/staff/{staffMemberId} - Delete staff member
 * - GET /api/staff/available - Get available staff for a date
 */
describe('StaffService', () => {
  let service: StaffService;
  let httpMock: HttpTestingController;
  const apiBaseUrl = 'http://localhost:5000';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: API_BASE_URL, useValue: apiBaseUrl },
        StaffService
      ]
    });

    service = TestBed.inject(StaffService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('getStaff', () => {
    it('should call GET /api/staff with pagination parameters', () => {
      const mockResponse: PagedResult<StaffListItemDto> = {
        items: [],
        totalCount: 0,
        pageNumber: 1,
        pageSize: 20,
        totalPages: 0,
        hasNextPage: false,
        hasPreviousPage: false
      };

      service.getStaff({ pageIndex: 0, pageSize: 20 }).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/api/staff`);
      expect(req.request.method).toBe('GET');
      expect(req.request.params.get('pageIndex')).toBe('0');
      expect(req.request.params.get('pageSize')).toBe('20');
      req.flush(mockResponse);
    });

    it('should include search term when provided', () => {
      service.getStaff({ pageIndex: 0, pageSize: 10, searchTerm: 'John' }).subscribe();

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/api/staff`);
      expect(req.request.params.get('searchTerm')).toBe('John');
      req.flush({ items: [], totalCount: 0, pageNumber: 1, pageSize: 10, totalPages: 0, hasNextPage: false, hasPreviousPage: false });
    });

    it('should include sort parameters when provided', () => {
      service.getStaff({ pageIndex: 0, pageSize: 20, sortColumn: 'lastName', sortDirection: 'asc' }).subscribe();

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/api/staff`);
      expect(req.request.params.get('sortColumn')).toBe('lastName');
      expect(req.request.params.get('sortDirection')).toBe('asc');
      req.flush({ items: [], totalCount: 0, pageNumber: 1, pageSize: 20, totalPages: 0, hasNextPage: false, hasPreviousPage: false });
    });
  });

  describe('getStaffById', () => {
    it('should call GET /api/staff/{staffId}', () => {
      const staffId = '550e8400-e29b-41d4-a716-446655440000';
      const mockStaff: StaffDetailDto = {
        staffId,
        firstName: 'John',
        lastName: 'Smith',
        email: 'john.smith@example.com',
        phone: '555-1234',
        role: StaffRole.Coordinator,
        status: StaffStatus.Active,
        hireDate: new Date('2023-01-15'),
        hourlyRate: 25,
        address: '123 Staff Lane',
        city: 'Boston',
        state: 'MA',
        postalCode: '02101',
        emergencyContact: 'Jane Smith',
        emergencyPhone: '555-5678',
        skills: ['Event Planning', 'Customer Service'],
        certifications: ['First Aid'],
        notes: '',
        totalEventsAssigned: 15,
        createdAt: new Date(),
        updatedAt: new Date()
      };

      service.getStaffById(staffId).subscribe(response => {
        expect(response.staffId).toBe(staffId);
        expect(response.firstName).toBe('John');
        expect(response.postalCode).toBe('02101');
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/api/staff/${staffId}`);
      expect(req.request.method).toBe('GET');
      req.flush(mockStaff);
    });
  });

  describe('createStaff', () => {
    it('should call POST /api/staff with staff data', () => {
      const newStaff: CreateStaffRequest = {
        firstName: 'Jane',
        lastName: 'Doe',
        email: 'jane.doe@example.com',
        phone: '555-9999',
        role: StaffRole.Technician,
        hireDate: new Date('2024-02-01'),
        hourlyRate: 30,
        address: '456 Worker St',
        city: 'Denver',
        state: 'CO',
        postalCode: '80202',
        emergencyContact: 'John Doe',
        emergencyPhone: '555-8888',
        skills: ['Technical Support'],
        certifications: [],
        notes: ''
      };

      const mockResponse: StaffDetailDto = {
        ...newStaff,
        staffId: '550e8400-e29b-41d4-a716-446655440001',
        status: StaffStatus.Active,
        totalEventsAssigned: 0,
        createdAt: new Date(),
        updatedAt: new Date()
      };

      service.createStaff(newStaff).subscribe(response => {
        expect(response.staffId).toBeDefined();
        expect(response.firstName).toBe('Jane');
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/api/staff`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(newStaff);
      req.flush(mockResponse);
    });

    it('should send postalCode (not zipCode) in request body', () => {
      const staffWithPostalCode: CreateStaffRequest = {
        firstName: 'Test',
        lastName: 'User',
        email: 'test@example.com',
        phone: '555-0000',
        role: StaffRole.General,
        hireDate: new Date(),
        hourlyRate: 20,
        address: '123 Test St',
        city: 'Test City',
        state: 'TC',
        postalCode: '12345',
        emergencyContact: 'Emergency Contact',
        emergencyPhone: '555-9999',
        skills: [],
        certifications: [],
        notes: ''
      };

      service.createStaff(staffWithPostalCode).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/api/staff`);
      expect(req.request.body.postalCode).toBe('12345');
      expect((req.request.body as any).zipCode).toBeUndefined();
      req.flush({ staffId: '123', ...staffWithPostalCode, status: StaffStatus.Active, totalEventsAssigned: 0, createdAt: new Date(), updatedAt: new Date() });
    });
  });

  describe('updateStaff', () => {
    it('should call PUT /api/staff/{staffId}', () => {
      const staffId = '550e8400-e29b-41d4-a716-446655440000';
      const updateData = {
        staffId,
        firstName: 'John',
        lastName: 'Smith Jr.',
        email: 'john.smith.jr@example.com',
        phone: '555-4321',
        role: StaffRole.Manager,
        hireDate: new Date('2023-01-15'),
        hourlyRate: 35,
        address: '123 Staff Lane',
        city: 'Boston',
        state: 'MA',
        postalCode: '02102',
        emergencyContact: 'Jane Smith',
        emergencyPhone: '555-5678',
        skills: ['Management', 'Event Planning'],
        certifications: ['First Aid'],
        notes: 'Updated notes'
      };

      service.updateStaff(staffId, updateData).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/api/staff/${staffId}`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual(updateData);
      req.flush({ ...updateData, status: StaffStatus.Active, totalEventsAssigned: 15, createdAt: new Date(), updatedAt: new Date() });
    });
  });

  describe('deleteStaff', () => {
    it('should call DELETE /api/staff/{staffId}', () => {
      const staffId = '550e8400-e29b-41d4-a716-446655440000';

      service.deleteStaff(staffId).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/api/staff/${staffId}`);
      expect(req.request.method).toBe('DELETE');
      req.flush(null);
    });
  });

  describe('getAvailableStaff', () => {
    it('should call GET /api/staff/available with date parameter', () => {
      const eventDate = new Date('2024-06-15');
      const mockStaff: StaffListItemDto[] = [
        {
          staffId: '1',
          firstName: 'John',
          lastName: 'Doe',
          email: 'john@example.com',
          phone: '555-1111',
          role: StaffRole.Coordinator,
          status: StaffStatus.Active,
          hireDate: new Date('2023-01-15'),
          hourlyRate: 25,
          totalEventsAssigned: 10
        }
      ];

      service.getAvailableStaff(eventDate).subscribe(response => {
        expect(response.length).toBe(1);
        expect(response[0].firstName).toBe('John');
      });

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/api/staff/available`);
      expect(req.request.method).toBe('GET');
      expect(req.request.params.get('date')).toBe(eventDate.toISOString());
      req.flush(mockStaff);
    });
  });
});
