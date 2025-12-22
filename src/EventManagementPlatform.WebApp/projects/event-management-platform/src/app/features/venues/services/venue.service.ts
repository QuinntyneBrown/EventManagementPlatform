import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/tokens/api.token';
import { PagedResult, QueryParams } from '../../../core/models/common.model';
import { VenueListItemDto, VenueDetailDto, CreateVenueRequest, UpdateVenueRequest } from '../models/venue.model';

@Injectable({ providedIn: 'root' })
export class VenueService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getVenues(query: QueryParams): Observable<PagedResult<VenueListItemDto>> {
    let params = new HttpParams()
      .set('pageIndex', query.pageIndex.toString())
      .set('pageSize', query.pageSize.toString());
    if (query.searchTerm) params = params.set('searchTerm', query.searchTerm);
    if (query.sortColumn) params = params.set('sortColumn', query.sortColumn);
    if (query.sortDirection) params = params.set('sortDirection', query.sortDirection);
    return this.http.get<PagedResult<VenueListItemDto>>(`${this.baseUrl}/api/venues`, { params });
  }

  getVenueById(id: string): Observable<VenueDetailDto> {
    return this.http.get<VenueDetailDto>(`${this.baseUrl}/api/venues/${id}`);
  }

  createVenue(request: CreateVenueRequest): Observable<VenueDetailDto> {
    return this.http.post<VenueDetailDto>(`${this.baseUrl}/api/venues`, request);
  }

  updateVenue(id: string, request: UpdateVenueRequest): Observable<VenueDetailDto> {
    return this.http.put<VenueDetailDto>(`${this.baseUrl}/api/venues/${id}`, request);
  }

  deleteVenue(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/api/venues/${id}`);
  }

  getVenueTypes(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/api/venues/types`);
  }
}
