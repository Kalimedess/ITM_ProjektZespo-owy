<template>
  <div class="p-6 text-white">
    <h2 class="text-2xl font-bold mb-4 text-center text-lime-400">Zarządzanie bitami stołów</h2>

    <div class="mb-6 text-center">
      <label class="mr-2 font-semibold">Wybierz grę:</label>
      <select v-model="selectedGameId" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
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
          class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 mb-4"
        />
        <button
          class="ml-4 px-3 py-1 bg-lime-500 hover:bg-lime-600 text-black font-bold rounded"
        >
          Zapisz
        </button>
      </div>
    </div>

    <div v-else class="text-center text-white">Wybierz grę, aby edytować bity stołów.</div>
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
      { id: 1, name: 'Stół A', bits: 100 },
      { id: 2, name: 'Stół B', bits: 150 },
      { id: 3, name: 'Stół C', bits: 200 },
      { id: 4, name: 'Stół D', bits: 220}
    ]
  },
  {
    id: 2,
    name: 'Gra 2',
    teams: [
      { id: 5, name: 'Stół E', bits: 200 },
      { id: 6, name: 'Stół F', bits: 220 },
      { id: 7, name: 'Stół G', bits: 100 },
      { id: 8, name: 'Stół H', bits: 150 }
    ]
  }
]

const selectedTeams = computed(() => {
  const game = games.find(g => g.id === selectedGameId.value)
  return game ? game.teams : []
})
</script>