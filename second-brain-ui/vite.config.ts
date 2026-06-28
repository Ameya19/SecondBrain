import angular from '@analogjs/vite-plugin-angular';
// vite.config.ts
import { defineConfig } from 'vite';

export default defineConfig({
  plugins: [angular()],
  optimizeDeps: {
    exclude: [
      '@angular/core',
      '@angular/common',
      '@angular/platform-browser',
      '@angular/platform-browser-dynamic',
      '@angular/router',
      '@angular/common/http',
      '@angular/platform-browser/animations'
    ]
  }
});