import { defineConfig } from 'vitest/config';

export default defineConfig({
  test: {
    include: ['projects/**/*.spec.ts'],
    exclude: [
      'node_modules/**',
      'dist/**',
      '**/e2e/**',
      '**/*.e2e.spec.ts'
    ],
    environment: 'jsdom',
    globals: true,
    setupFiles: ['./test-setup.ts'],
    deps: {
      inline: [/@angular/, /@ngrx/]
    }
  }
});
