/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js}",
  ],
  theme: {
    extend: {
      textShadow: {
        outline: '0 0 1px black, 0 0 1px black',
      },
      colors: {
        'primary': '#121a2d',
        'secondary': '#141c2f',
        'tertiary': '#1e293b',
        'accent': '#a78bfa',
        'lgray-blue': "#1E293B",
        "lgray-accent": "#2B374B",
      },
      fontFamily:  {
        'nasalization': ['Nasalization', 'sans-serif'],
      }
      
    },
  },
  plugins: [
    require('tailwindcss-animated'),
    require('tailwindcss-textshadow')
  ],
}

