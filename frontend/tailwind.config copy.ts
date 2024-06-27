import type { Config } from "tailwindcss";

const config: Config = {
  content: [
    "./src/pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/components/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/app/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  theme: {
    extend: {
      screens: {
        'xs': '640px',
        // => @media (min-width: 640px) { ... }

      },
      colors: {
        cakery_blue: {
          DEFAULT: "#81cdd2",
          100: "#122f31",
          200: "#245f63",
          300: "#368e94",
          400: "#4eb8bf",
          500: "#81cdd2",
          600: "#99d6da",
          700: "#b3e0e3",
          800: "#cceaed",
          900: "#e6f5f6",
        },
        cakery_white: {
          DEFAULT: "#f1f5f3",
          100: "#283930",
          200: "#507161",
          300: "#7fa491",
          400: "#b7ccc2",
          500: "#f1f5f3",
          600: "#f3f7f5",
          700: "#f6f9f7",
          800: "#f9fbfa",
          900: "#fcfdfc",
        },
        cakery_timberwolf: {
          DEFAULT: "#dddbd4",
          100: "#312e26",
          200: "#615d4c",
          300: "#918b73",
          400: "#b7b3a4",
          500: "#dddbd4",
          600: "#e4e2dd",
          700: "#ebeae5",
          800: "#f2f1ee",
          900: "#f8f8f6",
        },
        cakery_cinereous: {
          DEFAULT: "#81726a",
          100: "#1a1715",
          200: "#342e2a",
          300: "#4d443f",
          400: "#675b54",
          500: "#81726a",
          600: "#9c8e86",
          700: "#b5aaa4",
          800: "#cdc6c2",
          900: "#e6e3e1",
        },
        cakery_wenge: {
          DEFAULT: "#68534d",
          100: "#15100f",
          200: "#29211e",
          300: "#3e312e",
          400: "#52413d",
          500: "#68534d",
          600: "#8d7068",
          700: "#aa938d",
          800: "#c7b7b3",
          900: "#e3dbd9",
        },
      },
      backgroundImage: {
        "gradient-radial": "radial-gradient(var(--tw-gradient-stops))",
        "gradient-conic":
          "conic-gradient(from 180deg at 50% 50%, var(--tw-gradient-stops))",
      },
    },
  },
  plugins: [],
};
export default config;



