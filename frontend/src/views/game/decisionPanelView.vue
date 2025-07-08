<template>
  <div class="flex flex-col md:flex-row bg-secondary p-4 rounded-md text-white w-full max-w-7xl mx-auto space-y-6 md:space-y-0 md:space-x-6">
    
    <!-- Lewa kolumna: wybór karty i stołu -->
    <div class="flex-1 p-4 bg-secondary-light rounded-md shadow-md min-h-[500px]">
      <h2 class="text-xl font-bold mb-4 text-center">Wybierz stół i kartę</h2>

      <select v-model="selectedTableId" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
        <option disabled value="">-- Wybierz stół --</option>
        <option v-for="table in tables" :key="table" :value="table">Stół {{ table }}</option>
      </select>

      <select v-model="selectedCardId" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
        <option disabled value="">-- Wybierz kartę --</option>
        <option v-for="card in cards" :key="card.id" :value="card.id">{{ card.name }}</option>
      </select>

      <p v-if="selectedCardId && isCardBlocked(parseInt(selectedCardId))" class="text-red-400 text-sm mt-2 text-center">
        Ta karta jest zablokowana i nie może zostać zagrana.
      </p>

      <div class="flex justify-center mt-4">
        <button
          :disabled="!selectedCardId || !selectedTableId || isCardBlocked(parseInt(selectedCardId))"
          @click="playCard"
          class="px-4 py-2 bg-lime-500 text-black font-bold rounded hover:bg-lime-600 disabled:opacity-50"
        >
          Zagraj kartę
        </button>
      </div>

      <!-- Plansza -->
      <div class="mt-6 w-full flex justify-center ">
      
      <GameBoard
      :config="formData"
      :game-mode="true"
      :pos-x="7"
      :pos-y="7"
      :pawn-color="'#000000'"
      />
    </div>
    </div>

    <!-- Prawa kolumna: historia decyzji -->
    <div class="flex-1 p-4 bg-secondary-light rounded-md shadow-md min-h-[500px]">
      <h2 class="text-xl font-bold mb-4 text-center">Historia decyzji</h2>

      <select v-model="selectedHistoryTable" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
        <option value="all">Wszystkie stoły</option>
        <option v-for="table in tables" :key="table" :value="table">Stół {{ table }}</option>
      </select>

      <div
        v-for="(entry, index) in filteredDecisions"
        :key="entry.cardId + '-' + entry.tableId + '-' + entry.timestamp"
        class="mb-4 border border-gray-600 rounded p-3 bg-secondary relative"
      >
        <p><strong>Stół {{ entry.tableId }}</strong> – Karta ID: {{ entry.cardId }}</p>
        <p :class="entry.result === 'Pozytywny' ? 'text-green-400' : 'text-red-400'">
          <strong>Wynik:</strong> {{ entry.result }}
        </p>
        <p class="text-sm mt-1">{{ entry.feedback }}</p>
        <p class="text-xs text-gray-400 mt-1">Zagrano: {{ formatDate(entry.timestamp) }}</p>

        <button
          class="absolute top-2 right-2 text-sm text-red-400 hover:text-red-200"
          @click="undoDecision(index)"
        >
          Cofnij
        </button>
      </div>

      <p v-if="filteredDecisions.length === 0" class="text-center text-gray-400">Brak decyzji do wyświetlenia.</p>
    </div>

  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch, reactive } from 'vue'
import { useToast } from 'vue-toastification'
import GameBoard from '@/components/game/gameBoard.vue'

const toast = useToast()

const selectedCardId = ref('')
const selectedTableId = ref('')
const selectedHistoryTable = ref('all')

const tables = [1, 2, 3, 4]

const cards = [
  { id: 1, name: "Stworzenie profilu organizacji" },
  { id: 2, name: "Przeprowadź szkolenie techniczne" },
  { id: 3, name: "Zarządzanie danymi" }
]

const decisions = ref([])
const blockedCardsMap = ref({})

const formData = reactive({
  Name: 'Plansza podstawowa', 
  LabelsUp: ['Podstawowa kordynacja', 'Standaryzacja procesów', 'Zintegrowane działania', 'Pełna integracja strategiczna'], 
  LabelsRight: ['Nowicjusz', 'Naśladowca', 'Innowator', 'Lider cyfrowy'], 
  DescriptionDown: 'Poziom integracji wew/zew', 
  DescriptionLeft: 'Zawansowanie Cyfrowe', 
  Rows: 8,
  Cols: 8,
  CellColor: '#fefae0', 
  BorderColor: '#595959', 
  BorderColors: ['#008000', '#FFFF00', '#FFA500', '#FF0000']
})

onMounted(() => {
  const saved = localStorage.getItem('decisions')
  decisions.value = saved ? JSON.parse(saved) : []

  const savedBlocked = localStorage.getItem('blockedCards')
  blockedCardsMap.value = savedBlocked ? JSON.parse(savedBlocked) : {}
})

watch(decisions, (newVal) => {
  localStorage.setItem('decisions', JSON.stringify(newVal))
}, { deep: true })

function isCardBlocked(cardId) {
  if (!selectedTableId.value) return false
  const blockedForTable = blockedCardsMap.value[selectedTableId.value] || []
  return blockedForTable.includes(cardId)
}

const filteredDecisions = computed(() => {
  if (selectedHistoryTable.value === 'all') return decisions.value
  return decisions.value.filter(entry => entry.tableId === parseInt(selectedHistoryTable.value))
})

function playCard() {
  const card = cards.find(c => c.id === selectedCardId.value)
  if (!card || !selectedTableId.value || isCardBlocked(card.id)) return

  toast.success(`Zagrano kartę: ${card.name} dla Stołu ${selectedTableId.value}`)

  const alwaysSucceedMap = JSON.parse(localStorage.getItem('alwaysSucceedMap') || '{}')
  const alwaysSucceed = !!alwaysSucceedMap[selectedTableId.value]

  let result = 'Negatywny'
  let feedback = 'Brak informacji.'

  if (alwaysSucceed) {
    result = 'Pozytywny'
    feedback = `Tryb alwaysSucceed: Karta "${card.name}" zakończyła się sukcesem.`
  } else {
    switch (card.id) {
      case 1:
        result = 'Pozytywny'
        feedback = 'Karta wprowadziła dobrą organizację pracy.'
        break
      case 2:
        result = 'Negatywny'
        feedback = 'Szkolenie techniczne było zbyt powierzchowne.'
        break
      case 3:
        result = 'Pozytywny'
        feedback = 'Zarządzanie danymi zwiększyło efektywność.'
        break
    }
  }

  decisions.value.unshift({
    cardId: card.id,
    result,
    feedback,
    tableId: parseInt(selectedTableId.value),
    timestamp: Date.now()
  })

  selectedCardId.value = ''
}

function undoDecision(index) {
  const removed = filteredDecisions.value[index]
  const fullIndex = decisions.value.findIndex(d => d.timestamp === removed.timestamp)
  if (fullIndex !== -1) {
    decisions.value.splice(fullIndex, 1)
    toast.info(`Cofnięto decyzję dla Stołu ${removed.tableId}, Karta ID ${removed.cardId}`)
  }
}

function formatDate(timestamp) {
  const date = new Date(timestamp)
  return date.toLocaleString('pl-PL')
}
</script>