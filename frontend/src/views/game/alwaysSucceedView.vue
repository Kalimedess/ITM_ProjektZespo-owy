<template>
  <div class="max-w-xl mx-auto bg-secondary text-white p-6 rounded shadow-md mt-10">
    <h2 class="text-2xl font-bold mb-4 text-center">Tryb: Zagrana karta zawsze się powodzi</h2>

    <div class="flex flex-col space-y-4">
      <!-- Wybór stołu -->
      <select v-model="selectedTable" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
        <option disabled value="">-- Wybierz stół --</option>
        <option v-for="table in tables" :key="table" :value="table">
          Stół {{ table }}
        </option>
      </select>

      <!-- Status -->
      <p v-if="selectedTable" class="text-center text-lg">
        Status dla Stołu {{ selectedTable }}: 
        <span :class="isEnabled(selectedTable) ? 'text-green-400' : 'text-red-400'">
          {{ isEnabled(selectedTable) ? 'WŁĄCZONY' : 'WYŁĄCZONY' }}
        </span>
      </p>

      <!-- Przycisk toggle -->
      <button
        v-if="selectedTable"
        @click="toggle(selectedTable)"
        class="bg-lime-500 hover:bg-lime-600 text-black font-bold py-2 px-4 rounded"
      >
        {{ isEnabled(selectedTable) ? 'Wyłącz tryb' : 'Włącz tryb' }}
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const tables = [1, 2, 3, 4]
const selectedTable = ref('')
const enabledMap = ref({})

onMounted(() => {
  const saved = localStorage.getItem('alwaysSucceedMap')
  enabledMap.value = saved ? JSON.parse(saved) : {}
})

function isEnabled(tableId) {
  return enabledMap.value[tableId] === true
}

function toggle(tableId) {
  if (!tableId) return

  const current = enabledMap.value[tableId] === true
  enabledMap.value[tableId] = !current

  localStorage.setItem('alwaysSucceedMap', JSON.stringify(enabledMap.value))
}
</script>