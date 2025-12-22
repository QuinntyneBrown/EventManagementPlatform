import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, map } from 'rxjs';
import { API_BASE_URL } from '../tokens/api.token';
import {
  User,
  LoginRequest,
  RegisterRequest,
  RegisterResponse,
  AuthenticateResponse,
  RefreshTokenRequest,
  RefreshTokenResponse,
  UserRole
} from '../models/auth.model';

/**
 * LocalStorage Keys
 * Per Identity Frontend Spec (Appendix B)
 */
const ACCESS_TOKEN_KEY = 'eventmanagement:accessToken';
const REFRESH_TOKEN_KEY = 'eventmanagement:refreshToken';
const CURRENT_USER_KEY = 'eventmanagement:currentUser';

/**
 * AuthService - Aligned with Backend Identity Spec
 *
 * Backend Endpoints (Phase A):
 * - POST /api/identity/authenticate
 * - POST /api/identity/register
 * - POST /api/identity/refresh-token
 */
@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly baseUrl = inject(API_BASE_URL);
  private readonly http = inject(HttpClient);

  private currentUserSubject = new BehaviorSubject<User | null>(this.getStoredUser());
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasValidToken());

  currentUser$ = this.currentUserSubject.asObservable();
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  /**
   * Authenticate user with username and password
   * POST /api/identity/authenticate
   */
  login(credentials: LoginRequest): Observable<AuthenticateResponse> {
    return this.http.post<AuthenticateResponse>(`${this.baseUrl}/identity/authenticate`, credentials).pipe(
      tap(response => this.handleAuthenticateResponse(response))
    );
  }

  /**
   * Register new user account
   * POST /api/identity/register
   * Note: Does NOT auto-login after registration per identity spec
   */
  register(request: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.baseUrl}/identity/register`, request);
  }

  /**
   * Refresh access token using refresh token
   * POST /api/identity/refresh-token
   */
  refreshToken(): Observable<RefreshTokenResponse> {
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY);
    const request: RefreshTokenRequest = { refreshToken: refreshToken || '' };

    return this.http.post<RefreshTokenResponse>(`${this.baseUrl}/identity/refresh-token`, request).pipe(
      tap(response => this.handleRefreshTokenResponse(response))
    );
  }

  /**
   * Logout - Clear all authentication state
   */
  logout(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(CURRENT_USER_KEY);
    this.currentUserSubject.next(null);
    this.isAuthenticatedSubject.next(false);
  }

  /**
   * Get current access token
   */
  getToken(): string | null {
    return localStorage.getItem(ACCESS_TOKEN_KEY);
  }

  /**
   * Get current refresh token
   */
  getRefreshToken(): string | null {
    return localStorage.getItem(REFRESH_TOKEN_KEY);
  }

  /**
   * Get current user synchronously
   */
  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  /**
   * Set current user (used for session restoration)
   */
  setCurrentUser(user: User): void {
    localStorage.setItem(CURRENT_USER_KEY, JSON.stringify(user));
    this.currentUserSubject.next(user);
    this.isAuthenticatedSubject.next(true);
  }

  /**
   * Clear current user (used by JWT interceptor on auth failure)
   */
  clearCurrentUser(): void {
    this.logout();
  }

  /**
   * Check if user has specific role
   */
  hasRole(role: UserRole): boolean {
    const user = this.currentUserSubject.value;
    return user?.roles?.includes(role) ?? false;
  }

  /**
   * Check if user has any of the specified roles
   */
  hasAnyRole(roles: UserRole[]): boolean {
    const user = this.currentUserSubject.value;
    return roles.some(role => user?.roles?.includes(role));
  }

  /**
   * Observable check if user has specific role
   */
  hasRole$(role: UserRole): Observable<boolean> {
    return this.currentUser$.pipe(
      map(user => user?.roles?.includes(role) ?? false)
    );
  }

  /**
   * Observable check if user has any of the specified roles
   */
  hasAnyRole$(roles: UserRole[]): Observable<boolean> {
    return this.currentUser$.pipe(
      map(user => roles.some(role => user?.roles?.includes(role)))
    );
  }

  /**
   * Try to initialize user session from stored tokens
   * Called on app startup
   */
  tryToLogin(): Observable<User | null> {
    const accessToken = this.getToken();
    const storedUser = this.getStoredUser();

    if (accessToken && storedUser) {
      this.currentUserSubject.next(storedUser);
      this.isAuthenticatedSubject.next(true);
    }

    return this.currentUser$;
  }

  /**
   * Handle successful authentication response
   */
  private handleAuthenticateResponse(response: AuthenticateResponse): void {
    // Store tokens
    localStorage.setItem(ACCESS_TOKEN_KEY, response.accessToken);
    localStorage.setItem(REFRESH_TOKEN_KEY, response.refreshToken);

    // Create user object from response
    const user: User = {
      userId: response.userId,
      username: response.username,
      roles: response.roles
    };

    // Store and emit user
    localStorage.setItem(CURRENT_USER_KEY, JSON.stringify(user));
    this.currentUserSubject.next(user);
    this.isAuthenticatedSubject.next(true);
  }

  /**
   * Handle successful token refresh response
   */
  private handleRefreshTokenResponse(response: RefreshTokenResponse): void {
    localStorage.setItem(ACCESS_TOKEN_KEY, response.accessToken);
    localStorage.setItem(REFRESH_TOKEN_KEY, response.refreshToken);
  }

  /**
   * Get stored user from localStorage
   */
  private getStoredUser(): User | null {
    const userJson = localStorage.getItem(CURRENT_USER_KEY);
    if (userJson) {
      try {
        return JSON.parse(userJson);
      } catch {
        return null;
      }
    }
    return null;
  }

  /**
   * Check if valid access token exists
   */
  private hasValidToken(): boolean {
    return !!localStorage.getItem(ACCESS_TOKEN_KEY);
  }
}
