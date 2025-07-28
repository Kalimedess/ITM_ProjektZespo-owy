// Ten plik zawiera dane dotyczące wyglądu i pozycji węzłów drzewa.
// Docelowo te dane mogłyby pochodzić z API.
// Współrzędne (x,y) są przybliżone na podstawie obrazka.

export const nodesLayout = {
  5: { x: 500, y: 50, type: 'key' },
  37: { x: 200, y: 150, type: 'key' },
  4: { x: 350, y: 150, type: 'normal' },
  42: { x: 500, y: 150, type: 'normal' },
  9: { x: 650, y: 150, type: 'normal' },
  33: { x: 800, y: 150, type: 'normal' },
  40: { x: 50, y: 250, type: 'normal' },
  18: { x: 150, y: 250, type: 'normal' },
  39: { x: 250, y: 250, type: 'normal' },
  22: { x: 350, y: 250, type: 'normal' },
  1: { x: 300, y: 350, type: 'normal' },
  7: { x: 450, y: 350, type: 'normal' },
  2: { x: 600, y: 350, type: 'key' },
  3: { x: 750, y: 350, type: 'normal' },
  41: { x: 300, y: 450, type: 'normal' },
  31: { x: 450, y: 450, type: 'normal' },
  19: { x: 850, y: 450, type: 'key' },
  20: { x: 450, y: 550, type: 'key' },
  10: { x: 650, y: 550, type: 'normal' },
  38: { x: 100, y: 550, type: 'key' },
  8: { x: 300, y: 650, type: 'key' },
  25: { x: 100, y: 750, type: 'normal' },
  21: { x: 250, y: 750, type: 'normal' },
  14: { x: 450, y: 750, type: 'key' },
  16: { x: 650, y: 750, type: 'normal' },
  17: { x: 450, y: 850, type: 'key' },
  15: { x: 300, y: 950, type: 'key' },
  36: { x: 450, y: 950, type: 'normal' },
  13: { x: 650, y: 950, type: 'key' },
  6: { x: 150, y: 1050, type: 'normal' },
  35: { x: 300, y: 1050, type: 'key' },
  27: { x: 250, y: 1150, type: 'key' },
  26: { x: 400, y: 1150, type: 'normal' },
  32: { x: 550, y: 1150, type: 'normal' },
  11: { x: 700, y: 1150, type: 'key' },
  23: { x: 400, y: 1250, type: 'key' },
  28: { x: 300, y: 1250, type: 'normal' },
  34: { x: 200, y: 1350, type: 'normal' },
  46: { x: 800, y: 1250, type: 'key' },
  24: { x: 600, y: 1450, type: 'key' },
  12: { x: 500, y: 1550, type: 'key' },
  29: { x: 700, y: 1550, type: 'key' },
  30: { x: 600, y: 1650, type: 'key' },
};

export const edgeColors = {
  5: '#9333EA',   // Fioletowy
  37: '#0284C7',  // Niebieski
  2: '#16A34A',   // Zielony
  19: '#22D3EE',  // Turkusowy
  8: '#A1A1AA',   // Szary
  17: '#16A34A',  // Zielony
  13: '#EA580C',  // Pomarańczowy
  35: '#A1A1AA',  // Szary
  27: '#EAB308',  // Żółty
  // ... uzupełnij resztę według oryginalnego obrazka
};