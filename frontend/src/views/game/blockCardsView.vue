<template>
  <div class="max-w-xl mx-auto bg-secondary text-white p-6 rounded shadow-md mt-10">
    <h2 class="text-2xl font-bold mb-4 text-center">Zarządzanie zablokowanymi kartami</h2>

    <!-- Wybór stołu -->
    <select v-model="selectedTable" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
      <option disabled value="">-- Wybierz stół --</option>
      <option v-for="table in tables" :key="table" :value="table">Stół {{ table }}</option>
    </select>

    <div v-if="selectedTable">
      <div
        v-for="card in cards"
        :key="card.id"
        class="flex justify-between items-center bg-secondary-light p-2 my-2 rounded"
      >
        <span>{{ card.name }}</span>
        <button
          @click="toggleCard(card.id)"
          class="px-3 py-1 rounded font-bold"
          :class="isBlocked(card.id) ? 'bg-red-500 hover:bg-red-600' : 'bg-lime-500 hover:bg-lime-600'"
        >
          {{ isBlocked(card.id) ? 'Odblokuj' : 'Zablokuj' }}
        </button>
      </div>
    </div>

    <p v-else class="text-center text-gray-300 mt-4">Wybierz stół, aby zarządzać blokadami kart.</p>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'

const tables = [1, 2, 3, 4]
const selectedTable = ref('')
const blockedCards = ref({})

const cards = [
  { id: 1, name: "Stworzenie profilu organizacji" },
  { id: 2, name: "Przeprowadź szkolenie techniczne" },
  { id: 3, name: "Zarządzanie danymi" }
]

onMounted(() => {
  const saved = localStorage.getItem('blockedCards')
  blockedCards.value = saved ? JSON.parse(saved) : {}
})

function toggleCard(cardId) {
  if (!selectedTable.value) return

  const tableId = selectedTable.value
  if (!blockedCards.value[tableId]) {
    blockedCards.value[tableId] = []
  }

  const index = blockedCards.value[tableId].indexOf(cardId)
  if (index === -1) {
    blockedCards.value[tableId].push(cardId)
  } else {
    blockedCards.value[tableId].splice(index, 1)
  }

  localStorage.setItem('blockedCards', JSON.stringify(blockedCards.value))
}

function isBlocked(cardId) {
  if (!selectedTable.value) return false
  return blockedCards.value[selectedTable.value]?.includes(cardId)
}
</script>