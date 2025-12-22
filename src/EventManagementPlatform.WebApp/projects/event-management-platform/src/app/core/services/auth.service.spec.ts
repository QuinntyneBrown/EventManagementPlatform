import { describe, it, expect, beforeEach, afterEach } from 'vitest';
import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { AuthService } from './auth.service';
import { API_BASE_URL } from '../tokens/api.token';
import { AuthenticateResponse, RegisterResponse, RefreshTokenResponse } from '../models/auth.model';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  const apiBaseUrl = 'http://localhost:5000';

  beforeEach(() => {
    // Clear localStorage before each test
    localStorage.clear();

    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: API_BASE_URL, useValue: apiBaseUrl },
        AuthService
      ]
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  describe('login', () => {
    it('should call POST /api/identity/authenticate', () => {
      const credentials = { username: 'testuser', password: 'password123' };
      const mockResponse: AuthenticateResponse = {
        userId: '123',
        username: 'testuser',
        accessToken: 'mock-access-token',
        refreshToken: 'mock-refresh-token',
        roles: ['Staff']
      };

      service.login(credentials).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/api/identity/authenticate`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(credentials);
      req.flush(mockResponse);
    });

    it('should store tokens and user on successful login', () => {
      const credentials = { username: 'testuser', password: 'password123' };
      const mockResponse: AuthenticateResponse = {
        userId: '123',
        username: 'testuser',
        accessToken: 'mock-access-token',
        refreshToken: 'mock-refresh-token',
        roles: ['Admin', 'Manager']
      };

      service.login(credentials).subscribe(() => {
        expect(localStorage.getItem('eventmanagement:accessToken')).toBe('mock-access-token');
        expect(localStorage.getItem('eventmanagement:refreshToken')).toBe('mock-refresh-token');

        const storedUser = JSON.parse(localStorage.getItem('eventmanagement:currentUser') || '{}');
        expect(storedUser.userId).toBe('123');
        expect(storedUser.username).toBe('testuser');
        expect(storedUser.roles).toEqual(['Admin', 'Manager']);
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/api/identity/authenticate`);
      req.flush(mockResponse);
    });

    it('should update isAuthenticated$ observable on successful login', async () => {
      const credentials = { username: 'testuser', password: 'password123' };
      const mockResponse: AuthenticateResponse = {
        userId: '123',
        username: 'testuser',
        accessToken: 'mock-access-token',
        refreshToken: 'mock-refresh-token',
        roles: ['Staff']
      };

      const loginPromise = firstValueFrom(service.login(credentials));
      const req = httpMock.expectOne(`${apiBaseUrl}/identity/authenticate`);
      req.flush(mockResponse);

      await loginPromise;
      const isAuth = await firstValueFrom(service.isAuthenticated$);
      expect(isAuth).toBe(true);
    });
  });

  describe('register', () => {
    it('should call POST /api/identity/register', () => {
      const registerData = {
        username: 'newuser',
        password: 'password123',
        confirmPassword: 'password123'
      };
      const mockResponse: RegisterResponse = {
        userId: '456',
        username: 'newuser'
      };

      service.register(registerData).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/api/identity/register`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(registerData);
      req.flush(mockResponse);
    });

    it('should NOT auto-login after registration', () => {
      const registerData = {
        username: 'newuser',
        password: 'password123',
        confirmPassword: 'password123'
      };
      const mockResponse: RegisterResponse = {
        userId: '456',
        username: 'newuser'
      };

      service.register(registerData).subscribe(() => {
        // Should NOT store tokens after registration
        expect(localStorage.getItem('eventmanagement:accessToken')).toBeNull();
        expect(localStorage.getItem('eventmanagement:currentUser')).toBeNull();
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/api/identity/register`);
      req.flush(mockResponse);
    });
  });

  describe('refreshToken', () => {
    it('should call POST /api/identity/refresh-token', () => {
      localStorage.setItem('eventmanagement:refreshToken', 'old-refresh-token');

      const mockResponse: RefreshTokenResponse = {
        accessToken: 'new-access-token',
        refreshToken: 'new-refresh-token'
      };

      service.refreshToken().subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/api/identity/refresh-token`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual({ refreshToken: 'old-refresh-token' });
      req.flush(mockResponse);
    });

    it('should update stored tokens on successful refresh', () => {
      localStorage.setItem('eventmanagement:refreshToken', 'old-refresh-token');
      localStorage.setItem('eventmanagement:accessToken', 'old-access-token');

      const mockResponse: RefreshTokenResponse = {
        accessToken: 'new-access-token',
        refreshToken: 'new-refresh-token'
      };

      service.refreshToken().subscribe(() => {
        expect(localStorage.getItem('eventmanagement:accessToken')).toBe('new-access-token');
        expect(localStorage.getItem('eventmanagement:refreshToken')).toBe('new-refresh-token');
      });

      const req = httpMock.expectOne(`${apiBaseUrl}/api/identity/refresh-token`);
      req.flush(mockResponse);
    });
  });

  describe('logout', () => {
    it('should clear all auth data from localStorage', () => {
      localStorage.setItem('eventmanagement:accessToken', 'test-token');
      localStorage.setItem('eventmanagement:refreshToken', 'test-refresh');
      localStorage.setItem('eventmanagement:currentUser', JSON.stringify({ userId: '123' }));

      service.logout();

      expect(localStorage.getItem('eventmanagement:accessToken')).toBeNull();
      expect(localStorage.getItem('eventmanagement:refreshToken')).toBeNull();
      expect(localStorage.getItem('eventmanagement:currentUser')).toBeNull();
    });

    it('should set isAuthenticated$ to false', async () => {
      service.logout();

      const isAuth = await firstValueFrom(service.isAuthenticated$);
      expect(isAuth).toBe(false);
    });

    it('should set currentUser$ to null', async () => {
      service.logout();

      const user = await firstValueFrom(service.currentUser$);
      expect(user).toBeNull();
    });
  });

  describe('getToken', () => {
    it('should return token from localStorage', () => {
      localStorage.setItem('eventmanagement:accessToken', 'test-token');
      expect(service.getToken()).toBe('test-token');
    });

    it('should return null when no token exists', () => {
      expect(service.getToken()).toBeNull();
    });
  });

  describe('getRefreshToken', () => {
    it('should return refresh token from localStorage', () => {
      localStorage.setItem('eventmanagement:refreshToken', 'test-refresh');
      expect(service.getRefreshToken()).toBe('test-refresh');
    });

    it('should return null when no refresh token exists', () => {
      expect(service.getRefreshToken()).toBeNull();
    });
  });

  describe('hasRole', () => {
    it('should return true if user has the specified role', () => {
      const mockResponse: AuthenticateResponse = {
        userId: '123',
        username: 'admin',
        accessToken: 'token',
        refreshToken: 'refresh',
        roles: ['Admin', 'Manager']
      };

      service.login({ username: 'admin', password: 'pass' }).subscribe();
      httpMock.expectOne(`${apiBaseUrl}/api/identity/authenticate`).flush(mockResponse);

      expect(service.hasRole('Admin')).toBe(true);
      expect(service.hasRole('Manager')).toBe(true);
    });

    it('should return false if user does not have the specified role', () => {
      const mockResponse: AuthenticateResponse = {
        userId: '123',
        username: 'user',
        accessToken: 'token',
        refreshToken: 'refresh',
        roles: ['Staff']
      };

      service.login({ username: 'user', password: 'pass' }).subscribe();
      httpMock.expectOne(`${apiBaseUrl}/api/identity/authenticate`).flush(mockResponse);

      expect(service.hasRole('Admin')).toBe(false);
    });
  });

  describe('hasAnyRole', () => {
    it('should return true if user has any of the specified roles', () => {
      const mockResponse: AuthenticateResponse = {
        userId: '123',
        username: 'manager',
        accessToken: 'token',
        refreshToken: 'refresh',
        roles: ['Manager']
      };

      service.login({ username: 'manager', password: 'pass' }).subscribe();
      httpMock.expectOne(`${apiBaseUrl}/api/identity/authenticate`).flush(mockResponse);

      expect(service.hasAnyRole(['Admin', 'Manager'])).toBe(true);
    });

    it('should return false if user has none of the specified roles', () => {
      const mockResponse: AuthenticateResponse = {
        userId: '123',
        username: 'staff',
        accessToken: 'token',
        refreshToken: 'refresh',
        roles: ['Staff']
      };

      service.login({ username: 'staff', password: 'pass' }).subscribe();
      httpMock.expectOne(`${apiBaseUrl}/api/identity/authenticate`).flush(mockResponse);

      expect(service.hasAnyRole(['Admin', 'Manager'])).toBe(false);
    });
  });
});
