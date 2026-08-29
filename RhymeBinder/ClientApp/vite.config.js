import vue from '@vitejs/plugin-vue'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default { plugins: [vue()], build: { outDir: '../wwwroot/dist/list-texts', emptyOutDir: true }, base: '/dist/list-texts/' }