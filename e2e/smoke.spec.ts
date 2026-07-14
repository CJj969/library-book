import { test, expect } from '@playwright/test';

test.describe('用户端主流程烟雾测试', () => {
  test('首页可访问并显示统计', async ({ page }) => {
    await page.goto('/');
    await expect(page.locator('h2')).toContainText('座位预约');
    await expect(page.locator('.display-6')).toHaveCount(3);
  });

  test('座位列表显示至少20个座位并可按区域切换', async ({ page }) => {
    await page.goto('/Seat');
    await expect(page.locator('.seat-card')).toHaveCount(20);
    const tabs = page.locator('.nav-tabs .nav-link');
    const tabCount = await tabs.count();
    expect(tabCount).toBeGreaterThanOrEqual(3);
    await tabs.first().click();
    await expect(page.locator('.seat-card').first()).toBeVisible();
  });

  test('点击空闲座位进入详情页', async ({ page }) => {
    await page.goto('/Seat');
    const freeCard = page.locator('.seat-free a').first();
    if (await freeCard.count() > 0) {
      await freeCard.click();
      await expect(page.locator('h4').first()).toBeVisible();
    }
  });

  test('未登录访问我的预约跳转到首页', async ({ page }) => {
    await page.goto('/Reservation/MyReservations');
    await expect(page).toHaveURL(/\/$/);
  });

  test('切换体验账号后可访问我的预约', async ({ page }) => {
    await page.goto('/');
    const dropdown = page.locator('#userDropdown');
    await dropdown.click();
    await page.locator('button[value="1"]').first().click();
    await page.waitForTimeout(500);
    await page.goto('/Reservation/MyReservations');
    await expect(page.locator('h2')).toContainText('我的预约');
  });
});

test.describe('管理端主流程烟雾测试', () => {
  test('登录页可访问', async ({ page }) => {
    await page.goto('/Admin/Login');
    await expect(page.locator('h4')).toContainText('管理员登录');
  });

  test('未登录访问管理页跳转到登录页', async ({ page }) => {
    await page.goto('/Admin/Reservations');
    await expect(page).toHaveURL(/\/Admin\/Login/);
  });

  test('管理员登录后可访问预约管理和统计', async ({ page }) => {
    await page.goto('/Admin/Login');
    await page.fill('input[name="Username"]', '管理员');
    await page.fill('input[name="Password"]', '123456');
    await page.click('button[type="submit"]');
    await expect(page).toHaveURL(/\/Admin\/Reservations/);
    await page.goto('/Admin/Statistics');
    await expect(page.locator('h4')).toContainText('数据统计');
  });
});

test.describe('404页面测试', () => {
  test('访问不存在路由显示404页', async ({ page }) => {
    const resp = await page.goto('/nonexistent-page');
    expect(resp?.status()).toBe(404);
    await expect(page.locator('body')).toContainText('页面未找到');
  });
});
