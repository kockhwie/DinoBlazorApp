/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Components/**/*.razor",
    "./Components/**/*.razor.css",
    "./wwwroot/**/*.html"
  ],
  safelist: [
    { pattern: /dino-(level|badge)-./ }
  ],
  theme: {
    extend: {
      colors: {
        'dino-bg':     '#0f0d0a',
        'dino-surf':   '#181510',
        'dino-surf2':  '#211e16',
        'dino-surf3':  '#2a261c',
        'dino-bdr':    '#38321f',
        'dino-bdr2':   '#50483a',
        'dino-amber':  '#f0a030',
        'dino-ambers': '#c07820',
        'dino-green':  '#72c49a',
        'dino-rose':   '#e8836a',
        'dino-cream':  '#f2e8d5',
        'dino-creamd': '#c9b898',
        'dino-muted':  '#8c7c62',
        'dino-muted2': '#6a5a42',
      },
      fontFamily: {
        display: ['Fraunces', 'Georgia', 'serif'],
        body:    ['DM Sans', 'system-ui', 'sans-serif'],
      },
      borderRadius: {
        'dino': '12px',
        'dino-lg': '18px',
      },
      keyframes: {
        dinoFloat: {
          '0%, 100%': { transform: 'translateY(0) rotate(-3deg)' },
          '50%':      { transform: 'translateY(-7px) rotate(3deg)' },
        },
        slideUp: {
          from: { opacity: '0', transform: 'translateY(13px)' },
          to:   { opacity: '1', transform: 'translateY(0)' },
        },
        blink: {
          '0%, 100%': { opacity: '1' },
          '50%':      { opacity: '0' },
        },
        dotBounce: {
          '0%, 80%, 100%': { transform: 'scale(0.75)', opacity: '0.4' },
          '40%':           { transform: 'scale(1.1)',  opacity: '1' },
        },
        dinoBreathe: {
          '0%, 100%': { transform: 'scale(1)', opacity: '1' },
          '50%':      { transform: 'scale(1.05)', opacity: '0.9' },
        },
      },
      animation: {
        'dino-float':   'dinoFloat 3.5s ease-in-out infinite',
        'slide-up':     'slideUp 0.45s ease both',
        'blink':        'blink 0.8s ease infinite',
        'dot-bounce':   'dotBounce 1.1s ease infinite',
        'dino-breathe': 'dinoBreathe 3s ease-in-out infinite',
      },
    },
  },
  plugins: [
    require('daisyui'),
  ],
  daisyui: {
    themes: [
      {
        dino: {
          "primary":         "#f0a030",
          "primary-content": "#180f00",
          "secondary":       "#72c49a",
          "accent":          "#e8836a",
          "neutral":         "#211e16",
          "base-100":        "#0f0d0a",
          "base-200":        "#181510",
          "base-300":        "#211e16",
          "base-content":    "#f2e8d5",
          "info":            "#3b82f6",
          "success":         "#10b981",
          "warning":         "#f59e0b",
          "error":           "#ef4444",
        },
      },
    ],
    darkTheme: "dino",
  },
}
