import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://localhost:5025', // Адрес вашего запущенного API
        changeOrigin: true,
        secure: false,          // Отключаем проверку SSL для локального сертификата
      }
    }
  }
})
