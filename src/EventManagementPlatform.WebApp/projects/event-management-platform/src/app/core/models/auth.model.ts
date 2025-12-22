export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

export interface AuthResponse {
  token: string;
  refreshToken: string;
  expiresAt: Date;
  user: User;
}

export interface User {
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
  roles: UserRole[];
  photoUrl?: string;
}

export type UserRole =
  | 'Admin'
  | 'Manager'
  | 'Staff'
  | 'Customer'
  | 'WarehouseManager'
  | 'MaintenanceTech';

export interface ProfileUpdateRequest {
  firstName: string;
  lastName: string;
  phoneNumber?: string;
}

export interface PasswordChangeRequest {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}
