import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/tokens/api.token';
import { PagedResult } from '../../../core/models/common.model';
import {
  CustomerListDto,
  CustomerDetailDto,
  CreateCustomerDto,
  UpdateCustomerDto,
  CustomerQueryParams
} from '../models/customer.model';

@Injectable({ providedIn: 'root' })
export class CustomerService {
  private readonly baseUrl = inject(API_BASE_URL);
  private readonly http = inject(HttpClient);

  getCustomers(params: CustomerQueryParams = {}): Observable<PagedResult<CustomerListDto>> {
    let httpParams = new HttpParams();
    if (params.pageNumber) httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
    if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
    if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);
    if (params.status) httpParams = httpParams.set('status', params.status);
    if (params.sortBy) httpParams = httpParams.set('sortBy', params.sortBy);
    if (params.sortDirection) httpParams = httpParams.set('sortDirection', params.sortDirection);

    return this.http.get<PagedResult<CustomerListDto>>(`${this.baseUrl}/customers`, { params: httpParams });
  }

  getCustomerById(customerId: string): Observable<CustomerDetailDto> {
    return this.http.get<CustomerDetailDto>(`${this.baseUrl}/customers/${customerId}`);
  }

  createCustomer(customer: CreateCustomerDto): Observable<CustomerDetailDto> {
    return this.http.post<CustomerDetailDto>(`${this.baseUrl}/customers`, customer);
  }

  updateCustomer(customerId: string, customer: UpdateCustomerDto): Observable<CustomerDetailDto> {
    return this.http.put<CustomerDetailDto>(`${this.baseUrl}/customers/${customerId}`, customer);
  }

  deleteCustomer(customerId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/customers/${customerId}`);
  }
}
