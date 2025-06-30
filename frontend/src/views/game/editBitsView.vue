<template>
  <div class="p-6 text-white">
    <h2 class="text-2xl font-bold mb-4 text-center text-lime-400">Zarządzanie bitami drużyn</h2>

    <div class="mb-6 text-center">
      <label class="mr-2 font-semibold">Wybierz grę:</label>
      <select v-model="selectedGameId" class="p-2 rounded text-black">
        <option disabled value="">-- Wybierz grę --</option>
        <option v-for="game in games" :key="game.id" :value="game.id">
          {{ game.name }}
        </option>
      </select>
    </div>

    <div v-if="selectedTeams.length">
      <div
        v-for="team in selectedTeams"
        :key="team.id"
        class="mb-4 flex items-center justify-between bg-secondary p-3 rounded"
      >
        <div class="font-semibold">{{ team.name }}</div>
        <input
          v-model.number="team.bits"
          type="number"
          class="w-24 p-1 rounded text-black"
        />
        <button
          class="ml-4 px-3 py-1 bg-lime-500 hover:bg-lime-600 text-black font-bold rounded"
        >
          Zapisz
        </button>
      </div>
    </div>

    <div v-else class="text-center text-white">Wybierz grę, aby edytować bity drużyn.</div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const selectedGameId = ref('')

const games = [
  {
    id: 1,
    name: 'Gra 1',
    teams: [
      { id: 1, name: 'Drużyna A', bits: 100 },
      { id: 2, name: 'Drużyna B', bits: 150 },
      { id: 3, name: 'Drużyna C', bits: 200 },
      { id: 4, name: 'Drużyna D', bits: 220}
    ]
  },
  {
    id: 2,
    name: 'Gra 2',
    teams: [
      { id: 5, name: 'Drużyna E', bits: 200 },
      { id: 6, name: 'Drużyna F', bits: 220 },
      { id: 7, name: 'Drużyna G', bits: 100 },
      { id: 8, name: 'Drużyna H', bits: 150 }
    ]
  }
]

const selectedTeams = computed(() => {
  const game = games.find(g => g.id === selectedGameId.value)
  return game ? game.teams : []
})
</script>