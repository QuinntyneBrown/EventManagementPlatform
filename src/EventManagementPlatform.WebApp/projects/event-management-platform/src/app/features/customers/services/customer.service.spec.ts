import { describe, it, expect, beforeEach, afterEach } from 'vitest';
import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { CustomerService } from './customer.service';
import { API_BASE_URL } from '../../../core/tokens/api.token';
import { PagedResult } from '../../../core/models/common.model';
import { CustomerListDto, CustomerDetailDto, CreateCustomerDto } from '../models/customer.model';

/**
 * Customer Service Unit Tests
 * Tests API contract compliance for Customer endpoints
 *
 * API Contract Reference:
 * - GET /api/customers - List customers with pagination
 * - GET /api/customers/{customerId} - Get customer by ID
 * - POST /api/customers - Create customer
 * - PUT /api/customers/{customerId} - Update customer
 * - DELETE /api/customers/{customerId} - Delete customer
 */
describe('CustomerService', () => {
  let service: CustomerService;
  let httpMock: HttpTestingController;
  const apiBaseUrl = 'http://localhost:5000';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: API_BASE_URL, useValue: apiBaseUrl },
        CustomerService
      ]
    });

    service = TestBed.inject(CustomerService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('getCustomers', () => {
    it('should call GET /api/customers with correct URL', () => {
      const mockResponse: PagedResult<CustomerListDto> = {
        items: [],
        totalCount: 0,
        pageNumber: 1,
        pageSize: 20,
        totalPages: 0,
        hasNextPage: false,
        hasPreviousPage: false
      };

      service.getCustomers().subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/api/customers`);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
    });

    it('should include pagination parameters when provided', () => {
      const params = { pageNumber: 2, pageSize: 10 };

      service.getCustomers(params).subscribe();

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/api/customers`);
      expect(req.request.params.get('pageNumber')).toBe('2');
      expect(req.request.params.get('pageSize')).toBe('10');
      req.flush({ items: [], totalCount: 0, pageNumber: 2, pageSize: 10, totalPages: 0, hasNextPage: false, hasPreviousPage: true });
    });

    it('should include search term when provided', () => {
      const params = { searchTerm: 'Acme Corp' };

      service.getCustomers(params).subscribe();

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/api/customers`);
      expect(req.request.params.get('searchTerm')).toBe('Acme Corp');
      req.flush({ items: [], totalCount: 0, pageNumber: 1, pageSize: 20, totalPages: 0, hasNextPage: false, hasPreviousPage: false });
    });

    it('should include status filter when provided', () => {
      const params = { status: 'Active' as const };

      service.getCustomers(params).subscribe();

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/api/customers`);
      expect(req.request.params.get('status')).toBe('Active');
      req.flush({ items: [], totalCount: 0, pageNumber: 1, pageSize: 20, totalPages: 0, hasNextPage: false, hasPreviousPage: false });
    });
  });

  describe('getCustomerById', () => {
    it('should call GET /api/customers/{customerId}', () => {
      const customerId = '550e8400-e29b-41d4-a716-446655440000';
      const mockCustomer: CustomerDetailDto = {
        customerId,
        firstName: 'John',
        lastName: 'Doe',
        email: 'john.doe@example.com',
        phone: '555-1234',
        company: 'Acme Corp',
        address: '123 Main St',
        city: 'New York',
        state: 'NY',
        postalCode: '10001',
        status: 'Active',
        eventCount: 5,
        createdAt: new Date()
      };

      service.getCustomerById(customerId).subscribe(response => {
        expect(response.customerId).toBe(customerId);
        expect(response.email).toBe('john.doe@example.com');
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/api/customers/${customerId}`);
      expect(req.request.method).toBe('GET');
      req.flush(mockCustomer);
    });
  });

  describe('createCustomer', () => {
    it('should call POST /api/customers with customer data', () => {
      const newCustomer: CreateCustomerDto = {
        firstName: 'Jane',
        lastName: 'Smith',
        email: 'jane.smith@example.com',
        phone: '555-5678',
        company: 'Tech Inc',
        address: '456 Oak Ave',
        city: 'San Francisco',
        state: 'CA',
        postalCode: '94102'
      };

      const mockResponse: CustomerDetailDto = {
        ...newCustomer,
        customerId: '550e8400-e29b-41d4-a716-446655440001',
        status: 'Active',
        eventCount: 0,
        createdAt: new Date()
      };

      service.createCustomer(newCustomer).subscribe(response => {
        expect(response.customerId).toBeDefined();
        expect(response.email).toBe('jane.smith@example.com');
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/api/customers`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(newCustomer);
      req.flush(mockResponse);
    });

    it('should send postalCode (not zipCode) in request body', () => {
      const customerWithPostalCode: CreateCustomerDto = {
        firstName: 'Test',
        lastName: 'User',
        email: 'test@example.com',
        postalCode: '12345'
      };

      service.createCustomer(customerWithPostalCode).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/api/customers`);
      expect(req.request.body.postalCode).toBe('12345');
      expect(req.request.body.zipCode).toBeUndefined();
      req.flush({ customerId: '123', ...customerWithPostalCode, status: 'Active', eventCount: 0, createdAt: new Date() });
    });
  });

  describe('updateCustomer', () => {
    it('should call PUT /api/customers/{customerId}', () => {
      const customerId = '550e8400-e29b-41d4-a716-446655440000';
      const updateData = {
        firstName: 'Jane',
        lastName: 'Doe',
        email: 'jane.doe@example.com',
        phone: '555-9999',
        postalCode: '10002'
      };

      service.updateCustomer(customerId, updateData).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/api/customers/${customerId}`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual(updateData);
      req.flush({ customerId, ...updateData, status: 'Active', eventCount: 5, createdAt: new Date() });
    });
  });

  describe('deleteCustomer', () => {
    it('should call DELETE /api/customers/{customerId}', () => {
      const customerId = '550e8400-e29b-41d4-a716-446655440000';

      service.deleteCustomer(customerId).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/api/customers/${customerId}`);
      expect(req.request.method).toBe('DELETE');
      req.flush(null);
    });
  });
});
