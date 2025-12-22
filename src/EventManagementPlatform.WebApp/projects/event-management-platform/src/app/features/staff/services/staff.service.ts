import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/tokens/api.token';
import { PagedResult, QueryParams } from '../../../core/models/common.model';
import { StaffListItemDto, StaffDetailDto, CreateStaffRequest, UpdateStaffRequest } from '../models/staff.model';

@Injectable({ providedIn: 'root' })
export class StaffService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getStaff(query: QueryParams): Observable<PagedResult<StaffListItemDto>> {
    let params = new HttpParams()
      .set('pageIndex', query.pageIndex.toString())
      .set('pageSize', query.pageSize.toString());
    if (query.searchTerm) params = params.set('searchTerm', query.searchTerm);
    if (query.sortColumn) params = params.set('sortColumn', query.sortColumn);
    if (query.sortDirection) params = params.set('sortDirection', query.sortDirection);
    return this.http.get<PagedResult<StaffListItemDto>>(`${this.baseUrl}/api/staff`, { params });
  }

  getStaffById(id: string): Observable<StaffDetailDto> {
    return this.http.get<StaffDetailDto>(`${this.baseUrl}/api/staff/${id}`);
  }

  createStaff(request: CreateStaffRequest): Observable<StaffDetailDto> {
    return this.http.post<StaffDetailDto>(`${this.baseUrl}/api/staff`, request);
  }

  updateStaff(id: string, request: UpdateStaffRequest): Observable<StaffDetailDto> {
    return this.http.put<StaffDetailDto>(`${this.baseUrl}/api/staff/${id}`, request);
  }

  deleteStaff(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/api/staff/${id}`);
  }

  getAvailableStaff(eventDate: Date): Observable<StaffListItemDto[]> {
    return this.http.get<StaffListItemDto[]>(`${this.baseUrl}/api/staff/available`, { params: { date: eventDate.toISOString() } });
  }
}
