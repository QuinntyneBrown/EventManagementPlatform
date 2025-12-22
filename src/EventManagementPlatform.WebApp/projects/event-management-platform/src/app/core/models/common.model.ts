export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface QueryParams {
  pageIndex: number;
  pageSize: number;
  sortColumn?: string;
  sortDirection?: 'asc' | 'desc' | '';
  searchTerm?: string;
}

export interface ApiError {
  message: string;
  errors?: Record<string, string[]>;
  statusCode: number;
}

export interface ConfirmDialogData {
  title: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
  type?: 'info' | 'warning' | 'danger';
}

export interface SelectOption<T = string> {
  value: T;
  label: string;
  disabled?: boolean;
}
