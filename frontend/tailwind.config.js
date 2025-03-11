/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js}",
  ],
  theme: {
    extend: {

      colors: {
        'primary': '#121a2d',
        'secondary': '#141c2f',
        'tertiary': '#1e293b',
        'accent': '#a78bfa',
      },
      fontFamily:  {
        'nasalization': ['Nasalization', 'sans-serif'],
      }
      
    },
  },
  plugins: [],
}

