import { test, expect } from '@playwright/test';

test.describe('Event Management Platform', () => {
  test('has title', async ({ page }) => {
    await page.goto('/');
    await expect(page).toHaveTitle(/Event Management Platform/i);
  });

  test('home page loads successfully', async ({ page }) => {
    await page.goto('/');
    await expect(page.locator('body')).toBeVisible();
  });
});
