<template>
  <div class="w-full max-w-md mx-auto bg-yellow rounded-xl shadow p-4 space-y-4 border border-white">

    <div class="p-2 rounded relative">
      <h2 class="text-xl font-semibold">📝 Dodaj decyzję</h2>
    </div>

    <div class="p-2 rounded relative">
      <select class="w-full p-2 rounded border border-gray-300 text-black">
        <option>Decyzja 1</option>
        <option>Decyzja 2</option>
        <option>Decyzja 3</option>
      </select>
    </div>

    <div class="p-2 rounded relative">
      <div class="flex justify-between items-center">
        <h2 class="text-xl font-semibold">📂 Podjęte decyzje</h2>
        <select v-model="selectedTeam" class="p-2 rounded border border-gray-300 text-black">
          <option>Wszystkie drużyny</option>
          <option>Drużyna 1</option>
          <option>Drużyna 2</option>
        </select>
      </div>
    </div>

    <div class="p-2 rounded relative space-y-3">
      <ul class="space-y-2">
        <li
          v-for="(decision, index) in filteredDecisions"
          :key="index"
          class="bg-primary text-white text-left p-2 rounded shadow-sm space-y-2"
        >
          <div class="text-sm italic text-gray-200">{{ decision.team }}</div>
          <div><strong>{{ decision.player }}</strong> → {{ decision.idchoice }} {{ decision.choice }}</div>
          <div class="border-t border-gray-300 w-full"></div>
          <div>
            Wynik:
            <span
              class="font-semibold"
              :class="{
                'text-green-400': decision.result === 'Pozytywny',
                'text-red-400': decision.result === 'Negatywny'
              }"
            >
              {{ decision.result }}
            </span>
          </div>
          <p class="text-sm text-white">
            {{ decision.description }}
          </p>
          <button class="text-xs bg-red-600 text-white px-2 py-1 rounded">Usuń</button>
        </li>
      </ul>
      
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const selectedTeam = ref('Wszystkie drużyny')

const decisions = ref([
  {
    team: 'Drużyna 1',
    player: 'Gracz 1',
    idchoice: '1',
    choice: 'Kupno karty',
    result: 'Pozytywny',
    description: 'Lorem ipsum dolor sit amet.'
  },
  {
    team: 'Drużyna 2',
    player: 'Gracz 2',
    idchoice: '2',
    choice: 'Pas',
    result: 'Negatywny',
    description: 'Consectetur adipiscing elit.'
  },
  {
    team: 'Drużyna 1',
    player: 'Gracz 3',
    idchoice: '3',
    choice: 'Sprzedaż zasobów',
    result: 'Pozytywny',
    description: 'Sed do eiusmod tempor.'
  }
])

const filteredDecisions = computed(() => {
  if (selectedTeam.value === 'Wszystkie drużyny') {
    return decisions.value
  }
  return decisions.value.filter(d => d.team === selectedTeam.value)
})
</script>
