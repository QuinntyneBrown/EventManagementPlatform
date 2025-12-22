import { test, expect } from '@playwright/test';

test.describe('Authentication Flow', () => {
  test.beforeEach(async ({ page }) => {
    // Clear storage before each test
    await page.context().clearCookies();
    await page.evaluate(() => localStorage.clear());
  });

  test.describe('Login Page', () => {
    test('should display login form', async ({ page }) => {
      await page.goto('/login');

      await expect(page.locator('mat-card-title')).toContainText('Welcome Back');
      await expect(page.locator('input[formcontrolname="username"]')).toBeVisible();
      await expect(page.locator('input[formcontrolname="password"]')).toBeVisible();
      await expect(page.locator('button[type="submit"]')).toContainText('Sign In');
    });

    test('should show validation errors for empty fields', async ({ page }) => {
      await page.goto('/login');

      // Click submit without filling fields
      await page.locator('button[type="submit"]').click();

      await expect(page.locator('mat-error')).toBeVisible();
    });

    test('should show username validation error for short input', async ({ page }) => {
      await page.goto('/login');

      await page.locator('input[formcontrolname="username"]').fill('ab');
      await page.locator('input[formcontrolname="password"]').click();

      await expect(page.locator('mat-error')).toContainText('at least 3 characters');
    });

    test('should have link to register page', async ({ page }) => {
      await page.goto('/login');

      const registerLink = page.locator('a[routerlink="/register"]');
      await expect(registerLink).toBeVisible();
      await registerLink.click();

      await expect(page).toHaveURL(/\/register/);
    });

    test('should toggle password visibility', async ({ page }) => {
      await page.goto('/login');

      const passwordInput = page.locator('input[formcontrolname="password"]');
      await expect(passwordInput).toHaveAttribute('type', 'password');

      // Click visibility toggle button
      await page.locator('button[mat-icon-button]').click();

      await expect(passwordInput).toHaveAttribute('type', 'text');
    });
  });

  test.describe('Register Page', () => {
    test('should display register form', async ({ page }) => {
      await page.goto('/register');

      await expect(page.locator('mat-card-title')).toContainText('Create Account');
      await expect(page.locator('input[formcontrolname="username"]')).toBeVisible();
      await expect(page.locator('input[formcontrolname="password"]')).toBeVisible();
      await expect(page.locator('input[formcontrolname="confirmPassword"]')).toBeVisible();
      await expect(page.locator('button[type="submit"]')).toContainText('Create Account');
    });

    test('should show validation errors for empty fields', async ({ page }) => {
      await page.goto('/register');

      // Click submit without filling fields
      await page.locator('button[type="submit"]').click();

      const errors = page.locator('mat-error');
      await expect(errors.first()).toBeVisible();
    });

    test('should show password mismatch error', async ({ page }) => {
      await page.goto('/register');

      await page.locator('input[formcontrolname="username"]').fill('testuser');
      await page.locator('input[formcontrolname="password"]').fill('password123');
      await page.locator('input[formcontrolname="confirmPassword"]').fill('password456');
      await page.locator('button[type="submit"]').click();

      await expect(page.locator('mat-error')).toContainText('Passwords do not match');
    });

    test('should have link to login page', async ({ page }) => {
      await page.goto('/register');

      const loginLink = page.locator('a[routerlink="/login"]');
      await expect(loginLink).toBeVisible();
      await loginLink.click();

      await expect(page).toHaveURL(/\/login/);
    });

    test('should show username validation for short input', async ({ page }) => {
      await page.goto('/register');

      await page.locator('input[formcontrolname="username"]').fill('ab');
      await page.locator('input[formcontrolname="password"]').click();

      await expect(page.locator('mat-error')).toContainText('at least 3 characters');
    });

    test('should show password validation for short input', async ({ page }) => {
      await page.goto('/register');

      await page.locator('input[formcontrolname="password"]').fill('abc');
      await page.locator('input[formcontrolname="confirmPassword"]').click();

      await expect(page.locator('mat-error')).toContainText('at least 6 characters');
    });
  });

  test.describe('Auth Guard', () => {
    test('should redirect unauthenticated user to login', async ({ page }) => {
      await page.goto('/dashboard');

      await expect(page).toHaveURL(/\/login/);
    });

    test('should redirect unauthenticated user from protected routes', async ({ page }) => {
      await page.goto('/events');

      await expect(page).toHaveURL(/\/login/);
    });
  });
});

test.describe('API Endpoint Integration', () => {
  test('login endpoint should be /api/identity/authenticate', async ({ page }) => {
    let capturedUrl: string | null = null;

    // Intercept API call
    await page.route('**/api/identity/authenticate', (route) => {
      capturedUrl = route.request().url();
      route.fulfill({
        status: 401,
        contentType: 'application/json',
        body: JSON.stringify({ message: 'Invalid credentials' })
      });
    });

    await page.goto('/login');
    await page.locator('input[formcontrolname="username"]').fill('testuser');
    await page.locator('input[formcontrolname="password"]').fill('password123');
    await page.locator('button[type="submit"]').click();

    // Wait a moment for request to be made
    await page.waitForTimeout(1000);

    expect(capturedUrl).toContain('/api/identity/authenticate');
  });

  test('register endpoint should be /api/identity/register', async ({ page }) => {
    let capturedUrl: string | null = null;

    // Intercept API call
    await page.route('**/api/identity/register', (route) => {
      capturedUrl = route.request().url();
      route.fulfill({
        status: 201,
        contentType: 'application/json',
        body: JSON.stringify({ userId: '123', username: 'testuser' })
      });
    });

    await page.goto('/register');
    await page.locator('input[formcontrolname="username"]').fill('testuser');
    await page.locator('input[formcontrolname="password"]').fill('password123');
    await page.locator('input[formcontrolname="confirmPassword"]').fill('password123');
    await page.locator('button[type="submit"]').click();

    // Wait a moment for request to be made
    await page.waitForTimeout(1000);

    expect(capturedUrl).toContain('/api/identity/register');
  });

  test('successful login should store tokens and redirect to dashboard', async ({ page }) => {
    const mockResponse = {
      userId: '123',
      username: 'testuser',
      accessToken: 'mock-access-token',
      refreshToken: 'mock-refresh-token',
      roles: ['Staff']
    };

    await page.route('**/api/identity/authenticate', (route) => {
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify(mockResponse)
      });
    });

    await page.goto('/login');
    await page.locator('input[formcontrolname="username"]').fill('testuser');
    await page.locator('input[formcontrolname="password"]').fill('password123');
    await page.locator('button[type="submit"]').click();

    // Should redirect to dashboard
    await expect(page).toHaveURL(/\/dashboard/);

    // Check localStorage
    const accessToken = await page.evaluate(() => localStorage.getItem('eventmanagement:accessToken'));
    const currentUser = await page.evaluate(() => localStorage.getItem('eventmanagement:currentUser'));

    expect(accessToken).toBe('mock-access-token');
    expect(currentUser).toContain('testuser');
  });

  test('successful registration should redirect to login (not auto-login)', async ({ page }) => {
    const mockResponse = {
      userId: '123',
      username: 'newuser'
    };

    await page.route('**/api/identity/register', (route) => {
      route.fulfill({
        status: 201,
        contentType: 'application/json',
        body: JSON.stringify(mockResponse)
      });
    });

    await page.goto('/register');
    await page.locator('input[formcontrolname="username"]').fill('newuser');
    await page.locator('input[formcontrolname="password"]').fill('password123');
    await page.locator('input[formcontrolname="confirmPassword"]').fill('password123');
    await page.locator('button[type="submit"]').click();

    // Should redirect to login (NOT dashboard, since no auto-login)
    await expect(page).toHaveURL(/\/login/);

    // Check that no tokens were stored
    const accessToken = await page.evaluate(() => localStorage.getItem('eventmanagement:accessToken'));
    expect(accessToken).toBeNull();
  });
});
