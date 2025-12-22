import { describe, it, expect, beforeEach, afterEach } from 'vitest';
import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { EquipmentService } from './equipment.service';
import { API_BASE_URL } from '../../../core/tokens/api.token';
import { PagedResult } from '../../../core/models/common.model';
import { EquipmentListItemDto, EquipmentDetailDto, CreateEquipmentRequest, EquipmentCategory, EquipmentStatus, EquipmentCondition } from '../models/equipment.model';

/**
 * Equipment Service Unit Tests
 * Tests API contract compliance for Equipment endpoints
 *
 * API Contract Reference:
 * - GET /api/equipment - List equipment with pagination and filters
 * - GET /api/equipment/{equipmentItemId} - Get equipment by ID
 * - POST /api/equipment - Create equipment
 * - PUT /api/equipment/{equipmentItemId} - Update equipment
 * - DELETE /api/equipment/{equipmentItemId} - Delete equipment
 * - GET /api/equipment/available - Get available equipment for date range
 */
describe('EquipmentService', () => {
  let service: EquipmentService;
  let httpMock: HttpTestingController;
  const apiBaseUrl = 'http://localhost:5000';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: API_BASE_URL, useValue: apiBaseUrl },
        EquipmentService
      ]
    });

    service = TestBed.inject(EquipmentService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('getEquipment', () => {
    it('should call GET /api/equipment with pagination parameters', () => {
      const mockResponse: PagedResult<EquipmentListItemDto> = {
        items: [],
        totalCount: 0,
        pageNumber: 1,
        pageSize: 20,
        totalPages: 0,
        hasNextPage: false,
        hasPreviousPage: false
      };

      service.getEquipment({ pageIndex: 0, pageSize: 20 }).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/api/equipment`);
      expect(req.request.method).toBe('GET');
      expect(req.request.params.get('pageIndex')).toBe('0');
      expect(req.request.params.get('pageSize')).toBe('20');
      req.flush(mockResponse);
    });

    it('should include search term when provided', () => {
      service.getEquipment({ pageIndex: 0, pageSize: 10, searchTerm: 'projector' }).subscribe();

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/api/equipment`);
      expect(req.request.params.get('searchTerm')).toBe('projector');
      req.flush({ items: [], totalCount: 0, pageNumber: 1, pageSize: 10, totalPages: 0, hasNextPage: false, hasPreviousPage: false });
    });

    it('should include sort parameters when provided', () => {
      service.getEquipment({ pageIndex: 0, pageSize: 20, sortColumn: 'name', sortDirection: 'desc' }).subscribe();

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/api/equipment`);
      expect(req.request.params.get('sortColumn')).toBe('name');
      expect(req.request.params.get('sortDirection')).toBe('desc');
      req.flush({ items: [], totalCount: 0, pageNumber: 1, pageSize: 20, totalPages: 0, hasNextPage: false, hasPreviousPage: false });
    });
  });

  describe('getEquipmentById', () => {
    it('should call GET /api/equipment/{equipmentId}', () => {
      const equipmentId = '550e8400-e29b-41d4-a716-446655440000';
      const mockEquipment: EquipmentDetailDto = {
        equipmentId,
        name: 'HD Projector',
        description: 'High-definition projector for presentations',
        category: EquipmentCategory.AudioVisual,
        quantity: 5,
        availableQuantity: 3,
        dailyRate: 75,
        status: EquipmentStatus.Available,
        condition: EquipmentCondition.Good,
        serialNumber: 'PRJ-001',
        manufacturer: 'Sony',
        model: 'VPL-VW295ES',
        purchaseDate: new Date('2023-06-01'),
        purchasePrice: 5000,
        lastMaintenanceDate: new Date('2024-01-15'),
        nextMaintenanceDate: new Date('2024-07-15'),
        location: 'Warehouse A',
        notes: 'Includes spare bulb',
        images: [],
        createdAt: new Date(),
        updatedAt: new Date()
      };

      service.getEquipmentById(equipmentId).subscribe(response => {
        expect(response.equipmentId).toBe(equipmentId);
        expect(response.name).toBe('HD Projector');
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/api/equipment/${equipmentId}`);
      expect(req.request.method).toBe('GET');
      req.flush(mockEquipment);
    });
  });

  describe('createEquipment', () => {
    it('should call POST /api/equipment with equipment data', () => {
      const newEquipment: CreateEquipmentRequest = {
        name: 'LED Light Panel',
        description: 'Professional LED lighting panel',
        category: EquipmentCategory.Lighting,
        quantity: 10,
        dailyRate: 50,
        serialNumber: 'LED-010',
        manufacturer: 'Aputure',
        model: 'LS C300d II',
        purchaseDate: new Date('2024-01-01'),
        purchasePrice: 800,
        location: 'Warehouse B',
        notes: 'Battery included'
      };

      const mockResponse: EquipmentDetailDto = {
        ...newEquipment,
        equipmentId: '550e8400-e29b-41d4-a716-446655440001',
        availableQuantity: 10,
        status: EquipmentStatus.Available,
        condition: EquipmentCondition.Excellent,
        lastMaintenanceDate: new Date(),
        nextMaintenanceDate: new Date(),
        images: [],
        createdAt: new Date(),
        updatedAt: new Date()
      };

      service.createEquipment(newEquipment).subscribe(response => {
        expect(response.equipmentId).toBeDefined();
        expect(response.name).toBe('LED Light Panel');
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/api/equipment`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(newEquipment);
      req.flush(mockResponse);
    });
  });

  describe('updateEquipment', () => {
    it('should call PUT /api/equipment/{equipmentId}', () => {
      const equipmentId = '550e8400-e29b-41d4-a716-446655440000';
      const updateData = {
        equipmentId,
        name: 'Updated Projector',
        category: EquipmentCategory.AudioVisual,
        quantity: 6,
        dailyRate: 80,
        status: EquipmentStatus.Available,
        condition: EquipmentCondition.Excellent
      };

      service.updateEquipment(equipmentId, updateData as any).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/api/equipment/${equipmentId}`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual(updateData);
      req.flush({ ...updateData, availableQuantity: 6, images: [], createdAt: new Date(), updatedAt: new Date() });
    });
  });

  describe('deleteEquipment', () => {
    it('should call DELETE /api/equipment/{equipmentId}', () => {
      const equipmentId = '550e8400-e29b-41d4-a716-446655440000';

      service.deleteEquipment(equipmentId).subscribe();

      const req = httpMock.expectOne(`${apiBaseUrl}/api/equipment/${equipmentId}`);
      expect(req.request.method).toBe('DELETE');
      req.flush(null);
    });
  });

  describe('getAvailableEquipment', () => {
    it('should call GET /api/equipment/available with date range parameters', () => {
      const startDate = new Date('2024-06-15');
      const endDate = new Date('2024-06-17');
      const mockEquipment: EquipmentListItemDto[] = [
        {
          equipmentId: '1',
          name: 'HD Projector',
          category: EquipmentCategory.AudioVisual,
          quantity: 5,
          availableQuantity: 3,
          dailyRate: 75,
          status: EquipmentStatus.Available,
          condition: EquipmentCondition.Good
        }
      ];

      service.getAvailableEquipment(startDate, endDate).subscribe(response => {
        expect(response.length).toBe(1);
        expect(response[0].name).toBe('HD Projector');
      });

      const req = httpMock.expectOne(r => r.url === `${apiBaseUrl}/api/equipment/available`);
      expect(req.request.method).toBe('GET');
      expect(req.request.params.get('startDate')).toBe(startDate.toISOString());
      expect(req.request.params.get('endDate')).toBe(endDate.toISOString());
      req.flush(mockEquipment);
    });
  });
});
