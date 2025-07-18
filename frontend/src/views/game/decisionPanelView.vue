<template>
  <div class="flex flex-col md:flex-row bg-secondary p-4 rounded-md text-white w-full max-w-7xl mx-auto space-y-6 md:space-y-0 md:space-x-6">
    <!-- Lewa kolumna -->
    <div class="flex-1 p-4 bg-secondary rounded-md min-h-[500px]">
      <h2 class="text-xl font-bold mb-4 text-center">Wybierz stół i kartę</h2>

      <select v-model="selectedTableId" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-2">
        <option disabled value="">-- Wybierz stół --</option>
        <option v-for="table in tables" :key="table" :value="table">Stół {{ table }}</option>
      </select>

      <select v-model="selectedCardId" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-2">
        <option disabled value="">-- Wybierz kartę --</option>
        <option v-for="card in cards" :key="card.id" :value="card.id">{{ card.name }}</option>
      </select>

      <p v-if="selectedCard" class="text-sm text-center text-gray-300 mt-2 italic">
        {{ selectedCard.description }}
      </p>

      <p v-if="selectedCardId" class="text-center text-sm mt-2">
        Koszt karty: <span class="font-semibold">{{ getCardCost(selectedCardId) }} bitów</span>
      </p>

      <p v-if="selectedTableId" class="text-center text-sm mt-1">
        Bity stołu {{ selectedTableId }}: <span class="font-semibold">{{ currentBits }}</span>
      </p>

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

      <!-- Wybór i zatwierdzenie zdarzenia -->
      <div class="mt-6">
        <label class="block text-lg font-bold mb-2">Wybierz zdarzenie losowe:</label>

        <select
          v-model="selectedPendingEventIndex"
          class="bg-tertiary text-base border border-gray-500 rounded px-3 py-2 w-full mb-2"
        >
          <option v-for="(event, index) in availableEvents" :key="index" :value="index">
            {{ event.name }}
          </option>
        </select>

        <p v-if="selectedEvent" class="text-sm text-center text-gray-300 mt-2 italic mb-4">
          {{ selectedEvent.description }}
        </p>

        <div class="flex justify-center">
          <button
            @click="applySelectedEvent"
            class="px-4 py-1 bg-blue-500 text-sm font-semibold text-white rounded hover:bg-blue-600"
          >
            Zastosuj
          </button>
        </div>
      </div>

      <!-- Trzy przełączniki -->
      <div class="mt-6 mb-4 flex flex-wrap gap-6 justify-center items-center">
        <div class="flex items-center gap-2">
          <label class="relative w-16 h-8 rounded-full cursor-pointer block transition-colors duration-300"
                :class="showMenu ? 'bg-accent' : 'bg-primary'">
            <input type="checkbox" class="sr-only" v-model="showMenu" />
            <span class="w-6 h-6 bg-white absolute left-1 top-1 rounded-full transition-transform duration-300"
                  :class="{ 'translate-x-8': showMenu }"></span>
          </label>
          <span class="text-sm">Menu</span>
        </div>

        <div class="flex items-center gap-2">
          <label class="relative w-16 h-8 rounded-full cursor-pointer block transition-colors duration-300"
                :class="showOwnBoard ? 'bg-accent' : 'bg-primary'">
            <input type="checkbox" class="sr-only" v-model="showOwnBoard" />
            <span class="w-6 h-6 bg-white absolute left-1 top-1 rounded-full transition-transform duration-300"
                  :class="{ 'translate-x-8': showOwnBoard }"></span>
          </label>
          <span class="text-sm">Twoja plansza</span>
        </div>

        <div class="flex items-center gap-2">
          <label class="relative w-16 h-8 rounded-full cursor-pointer block transition-colors duration-300"
                :class="showOpponentsBoard ? 'bg-accent' : 'bg-primary'">
            <input type="checkbox" class="sr-only" v-model="showOpponentsBoard" />
            <span class="w-6 h-6 bg-white absolute left-1 top-1 rounded-full transition-transform duration-300"
                  :class="{ 'translate-x-8': showOpponentsBoard }"></span>
          </label>
          <span class="text-sm">Plansza rywali</span>
        </div>
      </div>

      <!-- GameBoard -->
      <div class="w-full flex justify-center">
        <GameBoard
          :config="formData"
          :game-mode="true"
          :pos-x="7"
          :pos-y="7"
          :pawn-color="'#000000'"
        />
      </div>
    </div>

    <!-- Prawa kolumna: decyzje do zatwierdzenia -->
    <!-- Prawa kolumna z przełącznikiem widoku -->
    <div class="flex-1 p-4 bg-secondary rounded-md min-h-[500px]">
      <div class="flex items-center justify-between mb-4">
        <h2 class="text-xl font-bold text-center">🕒 Panel decyzji</h2>
        <div class="flex justify-center gap-2 mb-4">
          <button
            @click="decisionMode = 'pending'"
            :class="[
              'px-4 py-2 rounded font-semibold transition',
              decisionMode === 'pending' ? 'bg-accent text-black shadow' : 'bg-gray-700 text-gray-300 hover:bg-gray-600'
            ]"
          >
            Do zatwierdzenia
          </button>

          <button
            @click="decisionMode = 'history'"
            :class="[
              'px-4 py-2 rounded font-semibold transition',
              decisionMode === 'history' ? 'bg-accent text-black shadow' : 'bg-gray-700 text-gray-300 hover:bg-gray-600'
            ]"
          >
            Historia decyzji
          </button>
        </div>
      </div>

      <!-- Decyzje do zatwierdzenia -->
      <div v-if="decisionMode === 'pending'" class="overflow-y-auto scroll-smooth max-h-[650px] pr-2 space-y-3 border border-lgray-accent rounded-md shadow-inner bg-secondary-dark">
        <div v-if="pendingDecisions.length === 0" class="text-center text-gray-400">
          Brak decyzji do zatwierdzenia.
        </div>

        <div
          v-for="(entry, index) in pendingDecisions"
          :key="entry.timestamp"
          class="p-2 rounded border border-yellow-500 bg-secondary relative"
        >
          <p><strong>Stół {{ entry.tableId }}</strong> – Karta ID: {{ entry.cardId }}</p>
          <p class="text-gray-300">{{ entry.feedback }}</p>
          <p class="text-xs text-gray-400 mt-1">Zagrano: {{ formatDate(entry.timestamp) }}</p>
          <div class="flex justify-end space-x-2 mt-2">
            <button @click="approveDecision(entry)" class="px-2 py-1 bg-green-500 text-sm text-black rounded hover:bg-green-600">Zatwierdź</button>
            <button @click="rejectDecision(index)" class="px-2 py-1 bg-red-500 text-sm text-white rounded hover:bg-red-600">Odrzuć</button>
          </div>
        </div>
      </div>

      <!-- Historia decyzji -->
      <div v-else class="space-y-4 max-h-[650px] overflow-y-auto scroll-smooth">
        

        <div v-if="filteredDecisions.length === 0" class="text-center text-gray-400">Brak decyzji w historii.</div>

        <div
          v-for="(entry, index) in filteredDecisions"
          :key="entry.cardId + '-' + entry.tableId + '-' + entry.timestamp"
          class="border border-gray-600 rounded p-3 bg-secondary relative"
        >
          <p><strong>Stół {{ entry.tableId }}</strong> – Karta ID: {{ entry.cardId }}</p>
          <p :class="entry.result === 'Pozytywny' ? 'text-green-400' : 'text-red-400'">
            <strong>Wynik:</strong> {{ entry.result }}
          </p>
          <p class="text-sm mt-1">{{ entry.feedback }}</p>
          <p class="text-xs text-gray-400 mt-1">Zagrano: {{ formatDate(entry.timestamp) }}</p>

          <!-- Zdarzenie losowe, jeśli było -->
          <p v-if="entry.eventUsed" class="text-xs italic text-blue-300 mt-1">
            Zastosowano zdarzenie: {{ entry.eventUsed }}
          </p>

          
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, computed } from 'vue'
import { useToast } from 'vue-toastification'
import GameBoard from '@/components/game/gameBoard.vue'

const toast = useToast()

const showMenu = ref(true)
const showOwnBoard = ref(true)
const showOpponentsBoard = ref(true)

const selectedCardId = ref('')
const selectedTableId = ref('')
const selectedEventIndex = ref(0)
const selectedPendingEventIndex = ref(0)
const eventReadyToUse = ref(false)

const tables = [1, 2, 3, 4]

const cards = [
  {
    id: 1,
    name: "Stworzenie profilu organizacji",
    cost: 40,
    description: "Ta karta umożliwia zespołowi utworzenie podstawowego profilu organizacyjnego w systemie, co pozwala na rejestrowanie aktywności i zarządzanie danymi zespołu w dalszych etapach gry.",
    result: "Pozytywny",
    feedback: "Karta wprowadziła dobrą organizację pracy."
  },
  {
    id: 2,
    name: "Przeprowadź szkolenie techniczne",
    cost: 50,
    description: "Szkolenie techniczne podnosi kwalifikacje członków zespołu w zakresie używania narzędzi cyfrowych, co przekłada się na większą efektywność podejmowanych działań oraz skraca czas realizacji kolejnych kroków.",
    result: "Negatywny",
    feedback: "Szkolenie techniczne było zbyt powierzchowne."
  },
  {
    id: 3,
    name: "Zarządzanie danymi",
    cost: 60,
    description: "Umożliwia uporządkowanie danych operacyjnych oraz wdrożenie struktury przechowywania informacji, co pozytywnie wpływa na jakość analityki i podejmowanie decyzji strategicznych.",
    result : 'Pozytywny',
    feedback : 'Zarządzanie danymi zwiększyło efektywność.'
  }
]

const selectedCard = computed(() =>
  cards.find(c => c.id === Number(selectedCardId.value))
)

// Bity per stół
const bitsPerTable = reactive({
  1: 1000,
  2: 800,
  3: 500,
  4: 1200
})

// Pokazuje bity aktualnie wybranego stołu
const currentBits = computed(() => {
  return selectedTableId.value ? bitsPerTable[selectedTableId.value] : 0
})

const availableEvents = [
  {
    name: "Brak zdarzenia",
    modifier: 1,
    description: ""
  },
  {
    name: "Zniżka 10% na następną kartę",
    modifier: 0.9,
    description: "W tej turze koszt zagrania jednej dowolnej karty zostaje obniżony o 10%. Bonus ten dotyczy wyłącznie jednej akcji i nie sumuje się z innymi zniżkami lub premiami."
  },
  {
    name: "Karta gratis (0 bitów)",
    modifier: 0,
    description: "Wybrana karta nie generuje kosztów bitów. To specjalne zdarzenie może znacznie przyspieszyć strategię zespołu i powinno być wykorzystane z rozwagą do zagrania najdroższej karty."
  }
]

const selectedEvent = computed(() =>
  availableEvents[selectedPendingEventIndex.value]
)

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

const pendingDecisions = ref([
  {
    cardId: 2,
    tableId: 1,
    timestamp: Date.now() - 200000,
    feedback: 'Zagrano kartę Przeprowadź szkolenie techniczne'
  },
  {
    cardId: 1,
    tableId: 2,
    timestamp: Date.now() - 100000,
    feedback: 'Zagrano kartę Stworzenie profilu organizacji'
  },
  {
    cardId: 3,
    tableId: 3,
    timestamp: Date.now() - 50000,
    feedback: 'Zagrano kartę Zarządzanie danymi'
  },
  {
    cardId: 4,
    tableId: 2,
    timestamp: Date.now() - 200000,
    feedback: 'Zagrano kartę Przeprowadź szkolenie techniczne'
  },
  {
    cardId: 5,
    tableId: 3,
    timestamp: Date.now() - 100000,
    feedback: 'Zagrano kartę Stworzenie profilu organizacji'
  },
  {
    cardId: 6,
    tableId: 1,
    timestamp: Date.now() - 50000,
    feedback: 'Zagrano kartę Zarządzanie danymi'
  }
])

const decisions = ref([
  {
    cardId: 1,
    tableId: 1,
    timestamp: Date.now() - 100000,
    feedback: "Zagrano kartę Stworzenie profilu organizacji",
    result: "Pozytywny",
    eventUsed: "Zniżka 10% na następną kartę"
  },
  {
    cardId: 2,
    tableId: 3,
    timestamp: Date.now() - 90000,
    feedback: "Szkolenie techniczne było zbyt powierzchowne",
    result: "Negatywny",
    eventUsed: null
  }
])

function isCardBlocked(cardId) {
  const blockedForTable = blockedCardsMap.value[selectedTableId.value] || []
  return blockedForTable.includes(cardId)
}

function getCardCost(cardId) {
  const card = cards.find(c => c.id === Number(cardId))
  return card ? card.cost : 0
}

function applySelectedEvent() {
  selectedEventIndex.value = selectedPendingEventIndex.value
  eventReadyToUse.value = true
  toast.success(`Zdarzenie „${availableEvents[selectedEventIndex.value].name}” zostało zastosowane.`)
}

function playCard() {
  const card = cards.find(c => c.id === Number(selectedCardId.value))
  const tableId = selectedTableId.value
  if (!card || !tableId || isCardBlocked(card.id)) return

  let modifier = eventReadyToUse.value ? availableEvents[selectedEventIndex.value].modifier : 1
  const finalCost = Math.ceil(card.cost * modifier)

  if (bitsPerTable[tableId] < finalCost) {
    toast.error(`Stół ${tableId} ma za mało bitów (wymagane ${finalCost})`)
    return
  }

  bitsPerTable[tableId] -= finalCost

  // Dodanie do historii decyzji:
  const result = card.result
  const feedback = card.feedback

  decisions.value.unshift({
    cardId: card.id,
    tableId,
    timestamp: Date.now(),
    feedback,
    result,
    eventUsed: eventReadyToUse.value ? selectedEvent.value.name : null
  })

  toast.success(`Zagrano kartę: ${card.name} za ${finalCost} bitów (Stół ${tableId})`)

  // Reset zdarzenia i wyboru
  eventReadyToUse.value = false
  selectedCardId.value = ''
}

const decisionMode = ref('pending') // 'pending' lub 'history'
const selectedHistoryTable = ref('all')

const filteredDecisions = computed(() => {
  if (selectedHistoryTable.value === 'all') return decisions.value
  return decisions.value.filter(entry => entry.tableId === parseInt(selectedHistoryTable.value))
})

function approveDecision(entry, index) {
  // Przenieś decyzję do historii
  const card = cards.find(c => c.id === entry.cardId)
  decisions.value.unshift({
    cardId: entry.cardId,
    tableId: entry.tableId,
    timestamp: entry.timestamp,
    feedback: entry.feedback,
    result: card?.result || 'Pozytywny',
    eventUsed: entry.eventUsed || null
  })

  // Usuń tylko tę jedną konkretną decyzję
  pendingDecisions.value.splice(index, 1)

  toast.success(`Zatwierdzono decyzję dla Stołu ${entry.tableId}, Karta ${entry.cardId}`)
}

function rejectDecision(index) {
  const removed = pendingDecisions.value[index]
  pendingDecisions.value.splice(index, 1)
  toast.info(`Odrzucono decyzję dla Stołu ${removed.tableId}, Karta ${removed.cardId}`)
}

function formatDate(timestamp) {
  const date = new Date(timestamp)
  return date.toLocaleString('pl-PL')
}
</script>