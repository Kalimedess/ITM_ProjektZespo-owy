<template>
    <div class="w-full max-w-md mx-auto bg-yellow rounded-xl shadow p-4 space-y-4 justify-left">
      <!-- Góra: bity + runda -->
      <div class="flex justify-between items-center">
        <div class="text-xl font-bold text-green-600">
          Bity: {{ bits }}
        </div>
        <div class="text-md font-semibold text-gray-700">
          Runda: {{ round }}
        </div>
      </div>
  
      <!-- Tabela decyzji -->
      <div class="border-t pt-3">
        <h2 class="text-lg font-semibold mb-2 text-white">📋 Decyzje</h2>
        <ul class="space-y-2">
          <li
            v-for="(decision, index) in decisions"
            :key="index"
            class="border pt-2 bg-primary text-white text-left p-2 rounded shadow-sm space-y-2">
            <div><strong>{{ decision.player }}</strong> → {{ decision.choice }}</div>
            <div class="border-t border-gray-500 w-full"></div>
            <div>Wynik: 
              <span class="font-semibold"
              :class="{
                'text-green-400': decision.result === 'Pozytywny',
                'text-red-400': decision.result === 'Negatywny'}">
                {{ decision.result }}
              </span></div>
              <p class="text-sm text-white">
                {{ decision.description }}
              </p>
          </li>
        </ul>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue'
  
  // Props lub dane z logiki gry
  const bits = ref(250)
  const round = ref(3)
  
  const decisions = ref([
    { player: 'Gracz 1', choice: 'Kupno karty', result: 'Pozytywny', description: 'Lorem ipsum dolor sit amet.' },
    { player: 'Gracz 2', choice: 'Pas', result: 'Negatywny', description: 'Lorem ipsum dolor sit amet.' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów', result: 'Pozytywny', description: 'Lorem ipsum dolor sit amet.' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów', result: 'Pozytywny', description: 'Lorem ipsum dolor sit amet.' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów', result: 'Pozytywny', description: 'Lorem ipsum dolor sit amet.' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów', result: 'Pozytywny', description: 'Lorem ipsum dolor sit amet.' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów', result: 'Pozytywny', description: 'Lorem ipsum dolor sit amet.' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów', result: 'Pozytywny', description: 'Lorem ipsum dolor sit amet.' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów', result: 'Pozytywny', description: 'Lorem ipsum dolor sit amet.' },
  ])
  </script>