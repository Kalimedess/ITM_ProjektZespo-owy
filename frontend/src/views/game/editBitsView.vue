<template>
  <div class="p-6 text-white">
    <h2 class="text-2xl font-bold mb-4 text-center text-lime-400">Zarządzanie bitami stołów</h2>

    <!-- Wybór stołu -->
    <div class="mb-6 text-center">
      <label class="mr-2 font-semibold">Wybierz stół:</label>
      <select v-model="selectedTableId" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
        <option disabled value="">-- Wybierz stół --</option>
        <option v-for="table in tables" :key="table.id" :value="table.id">
          {{ table.name }}
        </option>
      </select>
    </div>

    <!-- Edycja bitów -->
    <div v-if="selectedTable">
      <div class="mb-4 flex items-center justify-center gap-4 bg-secondary p-3 rounded">
        <input
          v-model.number="selectedTable.bits"
          type="number"
          class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-32 text-center"
        />
        <button
          class="px-4 py-2 bg-lime-500 hover:bg-lime-600 text-black font-bold rounded"
          @click="saveBits"
        >
          Zapisz
        </button>
      </div>
    </div>

    <div v-else class="text-center text-white">Wybierz stół, aby edytować bity.</div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useToast } from 'vue-toastification'

const toast = useToast()

const selectedTableId = ref('')

// Lista stołów
const tables = ref([
  { id: 1, name: 'Stół 1', bits: 100 },
  { id: 2, name: 'Stół 2', bits: 150 },
  { id: 3, name: 'Stół 3', bits: 200 },
  { id: 4, name: 'Stół 4', bits: 220 }
])

const selectedTable = computed(() => {
  return tables.value.find(t => t.id === selectedTableId.value)
})

function saveBits() {
  toast.success(`Zapisano ${selectedTable.value.bits} bitów dla ${selectedTable.value.name}`)
}
</script>