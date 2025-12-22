import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/tokens/api.token';
import { PagedResult, QueryParams } from '../../../core/models/common.model';
import { EquipmentListItemDto, EquipmentDetailDto, CreateEquipmentRequest, UpdateEquipmentRequest } from '../models/equipment.model';

@Injectable({ providedIn: 'root' })
export class EquipmentService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getEquipment(query: QueryParams): Observable<PagedResult<EquipmentListItemDto>> {
    let params = new HttpParams()
      .set('pageIndex', query.pageIndex.toString())
      .set('pageSize', query.pageSize.toString());
    if (query.searchTerm) params = params.set('searchTerm', query.searchTerm);
    if (query.sortColumn) params = params.set('sortColumn', query.sortColumn);
    if (query.sortDirection) params = params.set('sortDirection', query.sortDirection);
    return this.http.get<PagedResult<EquipmentListItemDto>>(`${this.baseUrl}/api/equipment`, { params });
  }

  getEquipmentById(id: string): Observable<EquipmentDetailDto> {
    return this.http.get<EquipmentDetailDto>(`${this.baseUrl}/api/equipment/${id}`);
  }

  createEquipment(request: CreateEquipmentRequest): Observable<EquipmentDetailDto> {
    return this.http.post<EquipmentDetailDto>(`${this.baseUrl}/api/equipment`, request);
  }

  updateEquipment(id: string, request: UpdateEquipmentRequest): Observable<EquipmentDetailDto> {
    return this.http.put<EquipmentDetailDto>(`${this.baseUrl}/api/equipment/${id}`, request);
  }

  deleteEquipment(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/api/equipment/${id}`);
  }

  getAvailableEquipment(startDate: Date, endDate: Date): Observable<EquipmentListItemDto[]> {
    return this.http.get<EquipmentListItemDto[]>(`${this.baseUrl}/api/equipment/available`, {
      params: { startDate: startDate.toISOString(), endDate: endDate.toISOString() }
    });
  }
}
