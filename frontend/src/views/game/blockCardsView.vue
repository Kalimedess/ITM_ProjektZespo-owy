<template>
  <div class="max-w-xl mx-auto bg-secondary text-white p-6 rounded shadow-md mt-10">
    <h2 class="text-2xl font-bold mb-4 text-center">Zarządzanie zablokowanymi kartami</h2>

    <!-- Wybór stołu -->
    <select v-model="selectedTable" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
      <option disabled value="">-- Wybierz stół --</option>
      <option v-for="table in tables" :key="table" :value="table">Stół {{ table }}</option>
    </select>

    <!-- Wybór karty (tylko jeśli wybrano stół) -->
    <div v-if="selectedTable">
      <select v-model="selectedCardId" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
        <option disabled value="">-- Wybierz kartę --</option>
        <option
          v-for="card in blockedCardsForTable"
          :key="card.id"
          :value="card.id"
        >
          {{ card.name }}
        </option>
      </select>

      <!-- Przycisk odblokowywania -->
      <div class="text-center">
        <button
          v-if="selectedCardId"
          @click="unblockCard(selectedCardId)"
          class="px-4 py-2 rounded font-bold bg-red-500 hover:bg-red-600"
        >
          Odblokuj kartę
        </button>
        <p v-else class="text-sm text-gray-300">Brak wybranej karty do odblokowania.</p>
      </div>
    </div>

    <p v-else class="text-center text-gray-300 mt-4">Wybierz stół, aby zarządzać blokadami kart.</p>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'

const tables = [1, 2, 3, 4]
const selectedTable = ref('')
const selectedCardId = ref('')
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

const blockedCardsForTable = computed(() => {
  if (!selectedTable.value) return []
  const blockedIds = blockedCards.value[selectedTable.value] || []
  return cards.filter(card => blockedIds.includes(card.id))
})

function unblockCard(cardId) {
  const tableId = selectedTable.value
  const index = blockedCards.value[tableId]?.indexOf(cardId)

  if (index !== -1) {
    blockedCards.value[tableId].splice(index, 1)
    localStorage.setItem('blockedCards', JSON.stringify(blockedCards.value))
    selectedCardId.value = ''
  }
}
</script>