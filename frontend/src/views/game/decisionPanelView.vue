<template>
  <div class="flex flex-col md:flex-row bg-secondary p-4 rounded-md text-white w-full max-w-7xl mx-auto space-y-6 md:space-y-0 md:space-x-6">
    <!-- Lewa kolumna -->
    <div class="flex-1 p-4 bg-secondary rounded-md min-h-[500px]">
      <h2 class="text-xl font-bold mb-4 text-center">
        Wybierz stół i {{ actionMode === 'cards' ? 'kartę' : 'przedmiot' }}
      </h2>

      <!-- Przełącznik kart/przedmiotów -->
      <div class="flex justify-center gap-4 mb-4">
        <button
          @click="actionMode = 'cards'"
          :class="actionMode === 'cards' ? 'bg-accent text-black shadow' : 'bg-gray-700 text-gray-300 hover:bg-gray-600'"
          class="px-4 py-1 rounded font-semibold transition"
        >
          Decyzje
        </button>
        <button
          @click="actionMode = 'items'"
          :class="actionMode === 'items' ? 'bg-accent text-black shadow' : 'bg-gray-700 text-gray-300 hover:bg-gray-600'"
          class="px-4 py-1 rounded font-semibold transition"
        >
          Przedmioty
        </button>
      </div>

      <select v-model="selectedTableId" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-2">
        <option disabled value="">-- Wybierz stół --</option>
        <option v-for="team in tables" :key="team.teamId" :value="team.teamId">{{ team.teamName }}</option>
      </select>

      <select
        v-if="actionMode === 'cards'"
        v-model="selectedCardId"
        class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-2"
      >
        <option disabled value="">-- Wybierz kartę --</option>
        <option v-for="card in cards" :key="card.id" :value="card.id">{{ card.id }} - {{ card.title }}</option>
      </select>

      <select
        v-else
        v-model="selectedItemId"
        class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-2"
      >
        <option disabled value="">-- Wybierz przedmiot --</option>
        <option v-for="item in items" :key="item.id" :value="item.id">{{ item.id }} - {{ item.title }}</option>
      </select>

      <p v-if="actionMode === 'cards' && selectedCard" class="text-sm text-center text-gray-300 mt-2 italic">
        {{ selectedCard.description }}
      </p>
      <p v-if="actionMode === 'items' && selectedItem" class="text-sm text-center text-gray-300 mt-2 italic">
        {{ selectedItem.description }}
      </p>

      <p v-if="actionMode === 'cards' && selectedCard" class="text-center text-sm mt-2">
        Koszt karty: <span class="font-semibold">{{ selectedCard?.cost || 0 }} bitów</span>
      </p>
      <p v-if="actionMode === 'items' && selectedItem" class="text-center text-sm mt-2">
        Koszt przedmiotu: <span class="font-semibold">{{ selectedItem?.cost || 0 }} bitów</span>
      </p>

      <p v-if="selectedTableId" class="text-center text-sm mt-1">
        Bity {{ selectedTeam.teamName }}: <span class="font-semibold">{{ currentBits }}</span>
      </p>

      <div class="flex justify-center mt-4">
        <button
          v-if="actionMode === 'cards'"
          :disabled="!selectedCardId || !selectedTableId" 
          @click="playCard"
          class="px-4 py-2 bg-lime-500 text-black font-bold rounded hover:bg-lime-600 disabled:opacity-50"
        >
          Zagraj kartę
        </button>

        <button
          v-else
          :disabled="!selectedItemId || !selectedTableId"
          @click="giveItem"
          class="px-4 py-2 bg-cyan-500 text-black font-bold rounded hover:bg-cyan-600 disabled:opacity-50"
        >
          Użyj przedmiot
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
     <div v-if="decisionMode === 'pending'" class="overflow-y-auto scroll-smooth max-h-[650px] pr-2 space-y-3 border border-lgray-accent rounded-md shadow-inner bg-secondary-dark p-2">
        <div v-if="loadingPending" class="text-center text-gray-400">Ładowanie sugestii...</div>
        <div v-else-if="pendingDecisions.length === 0" class="text-center text-gray-400 p-4">
          Brak decyzji do zatwierdzenia.
        </div>

        <div
          v-for="entry in pendingDecisions"
          :key="entry.logId"
          class="p-2 rounded border border-yellow-500 bg-secondary relative"
        >
          <p><strong>{{ entry.tableName }}</strong> sugeruje:</p>
          <p class="font-semibold text-lg">{{ entry.cardTitle }}</p>
          <p class="text-xs text-gray-400 mt-1">Zasugerowano: {{ formatDate(entry.timestamp) }}</p>
          <div class="flex justify-end space-x-2 mt-2">
            <button @click="approveDecision(entry.logId)" class="px-2 py-1 bg-green-500 text-sm text-black rounded hover:bg-green-600">Zatwierdź</button>
            <button @click="rejectDecision(entry.logId)" class="px-2 py-1 bg-red-500 text-sm text-white rounded hover:bg-red-600">Odrzuć</button>
          </div>
        </div>
     </div>

      <!-- Historia decyzji -->
      <div v-else class="space-y-4 max-h-[650px] overflow-y-auto scroll-smooth">
        <div v-if="loadingHistory" class="text-center text-gray-400">Ładowanie historii...</div>
        <div v-else-if="decisions.length === 0" class="text-center text-gray-400">Brak decyzji w historii dla tej gry.</div>

        <div
          v-for="(entry, index) in decisions"
          :key="index"
          class="border border-gray-600 rounded p-3 bg-secondary relative"
        >
          <p><strong>{{ entry.tableName }}</strong> – Karta ID: {{ entry.cardId }}</p>
          <p :class="entry.result === 'Pozytywny' ? 'text-green-400' : 'text-red-400'">
            <strong>Wynik:</strong> {{ entry.result }}
          </p>
          <p class="text-sm mt-1">Zagrano kartę: <span class="font-semibold">{{ entry.cardTitle }}</span></p>
          <p class="text-sm mt-1">{{ entry.feedbackDescription || 'Brak opisu feedbacku.' }}</p>
          <p class="text-xs text-gray-400 mt-1">Zagrano: {{ formatDate(entry.timestamp) }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, computed, onMounted, onUnmounted } from 'vue'
import { useToast } from 'vue-toastification'
import { useRoute } from 'vue-router'
import { watch } from 'vue';
import GameBoard from '@/components/game/gameBoard.vue'
import apiConfig from '@/services/apiConfig';
import apiServices from '@/services/apiServices';

const toast = useToast()
const route = useRoute();
const gameId = route.params.gameId;

const loading = reactive({
  teams: true,
  cards: true,
  items: true,
  history: true,
  pending: true,
});

// --- DANE DYNAMICZNE Z API ---
const decisions = ref([]) // Historia decyzji
const loadingHistory = ref(true);

// --- DANE STATYCZNE (DO PODPIĘCIA W PRZYSZŁOŚCI) ---
const showMenu = ref(true)
const showOwnBoard = ref(true)
const showOpponentsBoard = ref(true)
const selectedCardId = ref(null)
const selectedTableId = ref(null)
const selectedEventIndex = ref(0)
const selectedPendingEventIndex = ref(0)
const eventReadyToUse = ref(false)
const tables = ref([])
const cards = ref([])
const availableEvents = [
  { name: "Brak zdarzenia", modifier: 1, description: "" },
  { name: "Zniżka 10% na następną kartę", modifier: 0.9, description: "..." },
  { name: "Karta gratis (0 bitów)", modifier: 0, description: "..." }
]
const formData = reactive({
  Name: 'Plansza podstawowa', LabelsUp: ['...'], LabelsRight: ['...'],
  DescriptionDown: '...', DescriptionLeft: '...', Rows: 8, Cols: 8,
  CellColor: '#fefae0', BorderColor: '#595959', BorderColors: ['...']
})
const pendingDecisions = ref([]); 
const loadingPending = ref(true);

const selectedCard = computed(() => cards.value.find(c => c.id === selectedCardId.value));
const selectedTeam = computed(() => tables.value.find(t => t.teamId === selectedTableId.value));
const currentBits = computed(() => selectedTeam.value ? selectedTeam.value.teamBud : 0);
const selectedEvent = computed(() => availableEvents[selectedPendingEventIndex.value])

// --- FUNKCJE ---
let hasLoadedOnce = false;

watch(selectedTableId, (newTeamId) => {
  // Resetuj wybór karty/przedmiotu przy zmianie drużyny
  selectedCardId.value = null;
  selectedItemId.value = null;
  
  // Jeśli wybrano nową drużynę, pobierz dla niej dostępne karty
  if (newTeamId) {
    fetchAvailableCardsForTeam();
  } else {
    // Jeśli odznaczono drużynę, wyczyść listy
    cards.value = [];
    items.value = [];
  }
});

const fetchDecisionHistory = async () => {
  if (!gameId) {
    toast.error("Brak ID gry w adresie URL.");
    return;
  }

  if (!hasLoadedOnce) loadingHistory.value = true;

  try {
    const payload = {
      gameId: parseInt(gameId, 10),
      teamId: 0
    };

    const response = await apiServices.post(apiConfig.player.getLogs, payload);

    const newMapped = response.data.map(log => ({
      cardId: log.cardId,
      cardTitle: log.cardTitle,
      tableId: log.teamId,
      tableName: log.teamName,
      timestamp: log.timestamp,
      feedbackDescription: log.feedbackDescription,
      result: log.status ? 'Pozytywny' : 'Negatywny',
    }));

    const oldSerialized = JSON.stringify(decisions.value);
    const newSerialized = JSON.stringify(newMapped);

    if (oldSerialized !== newSerialized) {
      decisions.value = newMapped;
    }

  } catch (error) {
    toast.error("Wystąpił błąd podczas ładowania historii decyzji.");
    console.error("Błąd ładowania historii:", error);
  } finally {
    if (!hasLoadedOnce) loadingHistory.value = false;
    hasLoadedOnce = true;
  }
};

const decisionMode = ref('history')

function getCardCost(cardId) {
  const card = cards.value.find(c => c.cardId === Number(cardId))
  return card ? card.cost : 0
}

const actionMode = ref('cards') // 'cards' albo 'items'

const items = ref([])

const selectedItemId = ref(null)
const selectedItem = computed(() => items.value.find(i => i.id === selectedItemId.value));

function applySelectedEvent() {
  // Logika do zaimplementowania w przyszłości
  toast.info("Funkcjonalność zdarzeń losowych zostanie podpięta wkrótce.");
}

async function playCard() {
  const card = selectedCard.value;
  const team = selectedTeam.value;
  if (!card || !team) return;

  // ... (reszta kodu walidacji budżetu i tworzenia payloadu bez zmian) ...
  if (team.teamBud < (card.cost || 0)) {
    toast.error(`Drużyna ${team.teamName} ma za mało bitów!`);
    return;
  }
  const payload = {
    gameId: parseInt(gameId, 10),
    teamId: team.teamId,
    deckId: team.deckId,
    boardId: team.boardId,
    cost: card.cost || 0,
    ForceExecution: true
  };

  try {
    const response = await apiServices.post(apiConfig.player.playCardSuccess(card.id), payload);
    toast.success(response.data.message || `Zagrano kartę dla ${team.teamName}.`);
    
    // --- KLUCZOWA ZMIANA: ZAPEWNIENIE KOLEJNOŚCI ---
    // 1. Najpierw odśwież dane drużyn i poczekaj na zakończenie.
    await fetchTeams();
    // 2. Dopiero teraz, gdy dane drużyn są aktualne, odśwież dostępne karty.
    await fetchAvailableCardsForTeam();
    // --- KONIEC ZMIANY ---

  } catch (error) {
    toast.error("Wystąpił błąd podczas zagrywania karty.");
    console.error(error);
  }
}

async function giveItem() {
  const item = selectedItem.value;
  const team = selectedTeam.value;
  if (!item || !team) return;

  // ... (reszta kodu walidacji budżetu i tworzenia payloadu bez zmian) ...
  if (team.teamBud < (item.cost || 0)) {
    toast.error(`Drużyna ${team.teamName} ma za mało bitów!`);
    return;
  }
  const payload = {
    gameId: parseInt(gameId, 10),
    teamId: team.teamId,
    deckId: team.deckId,
    boardId: team.boardId,
    cost: item.cost || 0,
    ForceExecution: true
  };
  
  try {
    const response = await apiServices.post(apiConfig.player.playCardSuccess(item.id), payload);
    toast.success(response.data.message || `Użyto przedmiot dla ${team.teamName}.`);

    // --- KLUCZOWA ZMIANA: ZAPEWNIENIE KOLEJNOŚCI ---
    // 1. Najpierw odśwież dane drużyn i poczekaj na zakończenie.
    await fetchTeams();
    // 2. Dopiero teraz, gdy dane drużyn są aktualne, odśwież dostępne karty.
    await fetchAvailableCardsForTeam();
    // --- KONIEC ZMIANY ---
    
  } catch (error) {
    toast.error("Wystąpił błąd podczas używania przedmiotu.");
    console.error(error);
  }
}


function formatDate(timestamp) {
  const date = new Date(timestamp)
  return date.toLocaleString('pl-PL')
}

let hasLoadedPendingOnce = false;

const fetchPendingDecisions = async () => {
  if (!gameId) return;

  if (!hasLoadedPendingOnce) loadingPending.value = true;

  try {
    const response = await apiServices.get(apiConfig.games.getPendingLogs(gameId));

    const newMapped = response.data.map(log => ({
      logId: log.logId,
      cardId: log.cardId,
      cardTitle: log.cardTitle,
      tableId: log.teamId,
      tableName: log.teamName,
      timestamp: log.timestamp,
    }));

    const oldSerialized = JSON.stringify(pendingDecisions.value);
    const newSerialized = JSON.stringify(newMapped);

    if (oldSerialized !== newSerialized) {
      pendingDecisions.value = newMapped;
    }

  } catch (error) {
    toast.error("Błąd podczas pobierania sugestii do zatwierdzenia.");
    console.error(error);
  } finally {
    if (!hasLoadedPendingOnce) loadingPending.value = false;
    hasLoadedPendingOnce = true;
  }
};

const approveDecision = async (logId) => {
  try {
    await apiServices.post(apiConfig.games.approveLog(logId));
    toast.success("Sugestia została zatwierdzona!");
    // Odśwież obie listy po akcji
    fetchPendingDecisions();
    fetchDecisionHistory();
  } catch (error) {
    toast.error("Wystąpił błąd podczas zatwierdzania sugestii.");
    console.error(error);
  }
};

const rejectDecision = async (logId) => {
  try {
    await apiServices.delete(apiConfig.games.rejectLog(logId));
    toast.info("Sugestia została odrzucona.");
    // Odśwież listę oczekujących
    fetchPendingDecisions();
  } catch (error) {
    toast.error("Wystąpił błąd podczas odrzucania sugestii.");
    console.error(error);
  }
};

const fetchTeams = async () => {
  if (!gameId) return;
  try {
    const response = await apiServices.get(apiConfig.games.getTeamsManagement(gameId));
    tables.value = response.data; // Zapisujemy drużyny do 'tables'
  } catch (error) { toast.error("Błąd pobierania drużyn."); }
};

const fetchAvailableCardsForTeam = async () => {
  if (!selectedTableId.value) {
    cards.value = [];
    items.value = [];
    return;
  }
  
  const team = selectedTeam.value;
  if (!team || !team.deckId) {
    toast.error("Wybrana drużyna nie ma przypisanej talii.");
    return;
  }

  // Używamy poprawnych właściwości obiektu 'loading'
  loading.cards = true;
  loading.items = true;
  
  try {
    const response = await apiServices.get(apiConfig.player.getCards(team.deckId), {
      params: { 
        gameId: parseInt(gameId, 10), 
        teamId: team.teamId 
      }
    });

    if (response.data && response.data.decisionCards && response.data.itemCards) {
      cards.value = response.data.decisionCards;
      items.value = response.data.itemCards;
    } else {
      toast.error("Otrzymano nieprawidłowe dane kart z serwera.");
    }
  } catch (error) {
    toast.error("Wystąpił błąd podczas pobierania dostępnych kart.");
    console.error(error);
  } finally {
    // Używamy poprawnych właściwości obiektu 'loading'
    loading.cards = false;
    loading.items = false;
  }
};


let intervalId = null;

onMounted(() => {
  fetchTeams();  
  fetchDecisionHistory();
  fetchPendingDecisions();

  // Co 5 sekund aktualizuj historię
  intervalId = setInterval(() => {
    fetchDecisionHistory();
    fetchPendingDecisions();
  }, 5000);
});

onUnmounted(() => {
  // Czyścimy timer przy przełączaniu komponentu
  clearInterval(intervalId);
});


</script>