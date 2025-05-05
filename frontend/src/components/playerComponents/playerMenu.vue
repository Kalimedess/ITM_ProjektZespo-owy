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
        <h2 class="text-lg font-semibold mb-2">Decyzje</h2>
        <div class="max-h-[500px] overflow-y-auto pr-1">
          <ul class="space-y-2">
            <li
              v-for="(decision, index) in decisions"
              :key="index"
              class="border p-2 rounded shadow-sm bg-gray-100"
            >
              <strong>{{ decision.player }}</strong> → {{ decision.choice }}
            </li>
          </ul>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue'
  
  // Props lub dane z logiki gry
  const bits = ref(250)
  const round = ref(3)
  
  const decisions = ref([
    { player: 'Gracz 1', choice: 'Kupno karty' },
    { player: 'Gracz 1', choice: 'Pas' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
    { player: 'Gracz 3', choice: 'Sprzedaż zasobów' },
  ])
  </script>