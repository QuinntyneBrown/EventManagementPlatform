export interface CustomerListDto {
  customerId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  company?: string;
  eventCount: number;
  status: CustomerStatus;
}

export interface CustomerDetailDto {
  customerId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  company?: string;
  address?: string;
  city?: string;
  state?: string;
  postalCode?: string;
  notes?: string;
  status: CustomerStatus;
  eventCount: number;
  createdAt: Date;
  modifiedAt?: Date;
}

export interface CreateCustomerDto {
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  company?: string;
  address?: string;
  city?: string;
  state?: string;
  postalCode?: string;
  notes?: string;
}

export interface UpdateCustomerDto extends CreateCustomerDto {}

export type CustomerStatus = 'Active' | 'Inactive' | 'Blocked';

export interface CustomerQueryParams {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  status?: CustomerStatus;
  sortBy?: string;
  sortDirection?: 'asc' | 'desc';
}
