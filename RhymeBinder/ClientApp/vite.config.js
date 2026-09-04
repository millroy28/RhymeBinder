import vue from '@vitejs/plugin-vue'
import { defineConfig } from 'vite'

// https://vite.dev/config/
//export default { plugins: [vue()], build: { outDir: '../wwwroot/dist/list-texts', emptyOutDir: true }, base: '/dist/list-texts/' }

export default defineConfig({
    // ...your existing config
    plugins: [vue()],
    build: {
        manifest: true,
        outDir: '../wwwroot/dist', // adjust to wherever your Razor app serves static files from
        rollupOptions: {
            input: 'src/main.js' // your actual entry point
        }
    }
})