/**
 * Authentication Models - Aligned with Backend Identity Spec
 *
 * Backend Endpoints:
 * - POST /api/identity/authenticate
 * - POST /api/identity/register
 * - POST /api/identity/refresh-token
 */

// Request: POST /api/identity/authenticate
export interface LoginRequest {
  username: string;
  password: string;
}

// Response: POST /api/identity/authenticate
export interface AuthenticateResponse {
  userId: string;
  username: string;
  accessToken: string;
  refreshToken: string;
  roles: string[];
}

// Request: POST /api/identity/register
export interface RegisterRequest {
  username: string;
  password: string;
  confirmPassword: string;
}

// Response: POST /api/identity/register
export interface RegisterResponse {
  userId: string;
  username: string;
}

// Request: POST /api/identity/refresh-token
export interface RefreshTokenRequest {
  refreshToken: string;
}

// Response: POST /api/identity/refresh-token
export interface RefreshTokenResponse {
  accessToken: string;
  refreshToken: string;
}

// Current user model (stored in localStorage)
export interface User {
  userId: string;
  username: string;
  roles: string[];
}

// Role type - matches backend roles
export type UserRole =
  | 'SystemAdministrator'
  | 'Admin'
  | 'Manager'
  | 'Staff'
  | 'Customer';
