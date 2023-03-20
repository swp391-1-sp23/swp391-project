import react from "@vitejs/plugin-react-swc";
import { defineConfig } from "vite";
import svgr from "vite-plugin-svgr";
import WindiCSS from "vite-plugin-windicss";
import tsconfigPaths from "vite-tsconfig-paths";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react(), tsconfigPaths(), WindiCSS(), svgr()],
  server: {
    port: 3000,
  },
  envDir: "env",
});
