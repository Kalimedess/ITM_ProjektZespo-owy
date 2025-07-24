<template>
  <div class="flex flex-col md:flex-row bg-secondary p-4 rounded-md text-white w-full max-w-7xl mx-auto space-y-6 md:space-y-0 md:space-x-6">
    <!-- Lewa kolumna: Akcje Stołu -->
    <div class="flex-1 p-4 bg-secondary rounded-md min-h-[500px]">
      <h2 class="text-xl font-bold mb-4 text-center">
        Wybierz {{ actionMode === 'cards' ? 'decyzję' : 'przedmiot' }}
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

      <!-- Selectory kart i przedmiotów -->
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

      <!-- Opisy i koszty -->
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
      <p v-if="teamData" class="text-center text-sm mt-1">
        Twoje bity: <span class="font-semibold">{{ teamData.teamBud }}</span>
      </p>

      <!-- Przyciski akcji -->
      <div class="flex justify-center mt-4">
        <button
          v-if="actionMode === 'cards'"
          :disabled="!selectedCardId" 
          @click="playCard"
          class="px-4 py-2 bg-lime-500 text-black font-bold rounded hover:bg-lime-600 disabled:opacity-50"
        >
          Zagraj kartę
        </button>
        <button
          v-else
          :disabled="!selectedItemId"
          @click="giveItem"
          class="px-4 py-2 bg-cyan-500 text-black font-bold rounded hover:bg-cyan-600 disabled:opacity-50"
        >
          Użyj przedmiot
        </button>
      </div>
      
       <div v-if="showOwnBoard" class="w-full flex justify-center">
        <GameBoard
          :config="formData"
          :game-mode="true"
          :pawns="pawns"
        />
      </div>
    </div>

    <!-- Prawa kolumna: Panel Decyzji -->
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

     <div v-if="decisionMode === 'pending'" class="overflow-y-auto scroll-smooth max-h-[650px] pr-2 space-y-3 border border-lgray-accent rounded-md shadow-inner bg-secondary-dark p-2">
        <div v-if="loading.pending" class="text-center text-gray-400">Ładowanie sugestii...</div>
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
      <div v-else class="space-y-4 max-h-[650px] overflow-y-auto scroll-smooth">
        <div v-if="loading.history" class="text-center text-gray-400">Ładowanie historii...</div>
        <div v-else-if="decisions.length === 0" class="text-center text-gray-400">Brak decyzji w historii dla tej gry.</div>
        <div
          v-for="(entry, index) in decisions"
          :key="index"
          class="mb-2" 
        >
          <div v-if="entry.isEventNotification" class="border border-blue-500 rounded p-3 bg-blue-900/50 text-center">
            <h3 class="font-bold text-lg text-blue-300">Nowe Wydarzenie</h3>
            <p class="text-white mt-1">{{ entry.feedbackDescription }}</p>
            <p class="text-xs text-gray-400 mt-2">Aktywowano: {{ formatDate(entry.timestamp) }}</p>
          </div>
          <div v-else class="border border-gray-600 rounded p-3 bg-secondary relative">
            <div v-if="entry.eventAppliedId" class="absolute top-1 right-2 px-2 py-0.5 bg-purple-600 text-white text-xs font-bold rounded-full">
              EVENT
            </div>
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
  </div>
</template>

<script setup>
import { ref, reactive, computed, onMounted, onUnmounted, watch } from 'vue';
import { useToast } from 'vue-toastification';
import GameBoard from '@/components/game/gameBoard.vue';
import apiConfig from '@/services/apiConfig';
import apiServices from '@/services/apiServices';
import signalService from '@/services/signalService';

const props = defineProps({
  gameId: { type: [Number, String], required: true },
  teamId: { type: [Number, String], required: true }
});

const toast = useToast();

// Stany ładowania
const loading = reactive({
  teamData: true,
  cards: true,
  items: true,
  history: true,
  pending: true,
});

// Dane komponentu
const teamData = ref(null); 
const cards = ref([]);
const items = ref([]);
const decisions = ref([]);
const pendingDecisions = ref([]);
const pawns = ref([]);

// Stany UI
const actionMode = ref('cards'); // 'cards' vs 'items'
const decisionMode = ref('history'); // 'pending' vs 'history'
const selectedCardId = ref(null);
const selectedItemId = ref(null);
const showMenu = ref(true);
const showOwnBoard = ref(true);


// Wersje do sprawdzania aktualizacji
const historyVersion = ref(null);
const pendingVersion = ref(null);

// Konfiguracje plansz
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
    });


const selectedCard = computed(() => cards.value.find(c => c.id === selectedCardId.value));
const selectedItem = computed(() => items.value.find(i => i.id === selectedItemId.value));

// --- GŁÓWNA LOGIKA POBIERANIA DANYCH ---

const fetchAllDataForTeam = async () => {
  // Sprawdź, czy mamy potrzebne ID
  if (!props.gameId || !props.teamId) {
    console.warn("Brak gameId lub teamId. Przerywam pobieranie danych.");
    return;
  }

  // Ustaw stany ładowania na true
  Object.keys(loading).forEach(k => loading[k] = true);

  try {
    const teamsResponse = await apiServices.get(apiConfig.games.getTeamsManagement(props.gameId));
    const targetTeamData = teamsResponse.data.find(team => team.teamId === Number(props.teamId));

    if (!targetTeamData || !targetTeamData.teamToken) {
      toast.error(`Nie udało się znaleźć drużyny lub tokena dla teamId: ${props.teamId}`);
      // Zresetuj stany ładowania
      Object.keys(loading).forEach(k => loading[k] = false);
      return;
    }

    const token = targetTeamData.teamToken;
    console.log(`Pomyślnie uzyskano token: ${token} dla drużyny ID: ${props.teamId}`);

    const sessionResponse = await apiServices.get(apiConfig.player.getTeamInfo(token));
    const sessionData = sessionResponse.data;

    console.log("Otrzymano pełne dane sesji:", sessionData);

    
    // a) Wypełnij konfigurację planszy (`formData`)
    if (sessionData.boardConfig) {
      const config = sessionData.boardConfig;
      formData.Name = config.name;
      formData.LabelsUp = config.labelsUp;
      formData.LabelsRight = config.labelsRight;
      formData.DescriptionDown = config.descriptionDown;
      formData.DescriptionLeft = config.descriptionLeft;
      formData.Rows = config.rows;
      formData.Cols = config.cols;
      formData.CellColor = config.cellColor;
      formData.BorderColor = config.borderColor;
      formData.BorderColors = config.borderColors
    } else {
      toast.error("Błąd krytyczny: Brak konfiguracji planszy w danych sesji!");
    }

    teamData.value = {
      teamId: sessionData.teamId,
      teamName: sessionData.teamName,
      teamBud: sessionData.teamBud,
      deckId: sessionData.deckId,
      boardId: sessionData.boardConfig?.boardId,
      teamToken: token 
    };
    
    await Promise.all([
      fetchAvailableCardsAndItems(),
      fetchPawns(),
      fetchDecisionHistory(),
      fetchPendingDecisions()
    ]);

  } catch (error) {
    toast.error("Wystąpił błąd podczas ładowania danych drużyny.");
    console.error("Błąd w fetchAllDataForTeam:", error);
  } finally {
    // Na koniec ustaw wszystkie stany ładowania na false
    Object.keys(loading).forEach(k => loading[k] = false);
  }
};

const fetchAvailableCardsAndItems = async () => {
  if (!teamData.value?.deckId) return;
  loading.cards = true;
  loading.items = true;
  try {
    const response = await apiServices.get(apiConfig.player.getCards(teamData.value.deckId), {
      params: { gameId: Number(props.gameId), teamId: Number(props.teamId) }
    });
    cards.value = response.data.decisionCards || [];
    items.value = response.data.itemCards || [];
  } catch (error) {
    toast.error("Wystąpił błąd podczas pobierania kart i przedmiotów.");
    console.error(error);
  } finally {
    loading.cards = false;
    loading.items = false;
  }
};

const fetchDecisionHistory = async (isUpdate = false) => {
  if (!isUpdate) loading.history = true;
  try {
    const response = await apiServices.post(apiConfig.player.getLogs, { gameId: Number(props.gameId) });
    
    const mappedLogs = response.data
      .filter(log => log.teamId === Number(props.teamId) || (!log.teamId && log.gameEventId))
      .map(log => {
        const isEvent = log.gameEventId && !log.teamId;
        if (isEvent) {
          return {};
        }
        return {
          isEventNotification: false,
          cardId: log.cardId,
          cardTitle: log.cardTitle,
          tableId: log.teamId,
          tableName: log.teamName,
          timestamp: log.timestamp,
          feedbackDescription: log.feedbackDescription,
          result: log.status ? 'Pozytywny' : 'Negatywny',
          eventAppliedId: log.gameEventId
        };
      });

    decisions.value = mappedLogs;
    historyVersion.value = (await apiServices.get(apiConfig.games.getHistoryVersion(props.gameId))).data.version;
  } catch (error) {
    toast.error("Błąd ładowania historii decyzji.");
  } finally {
    if (!isUpdate) loading.history = false;
  }
};

const fetchPendingDecisions = async (isUpdate = false) => {
  if (!isUpdate) loading.pending = true;
  try {
    const response = await apiServices.get(apiConfig.games.getPendingLogs(props.gameId));
    
    // Uzupełnienie brakującej logiki mapowania
    pendingDecisions.value = response.data
      .filter(log => log.teamId === Number(props.teamId))
      .map(log => ({
        logId: log.logId,
        cardId: log.cardId,
        cardTitle: log.cardTitle,
        tableId: log.teamId,
        tableName: log.teamName,
        timestamp: log.timestamp,
    }));

    pendingVersion.value = (await apiServices.get(apiConfig.games.getPendingVersion(props.gameId))).data.version;
  } catch (error) {
    toast.error("Błąd pobierania sugestii.");
  } finally {
    if (!isUpdate) loading.pending = false;
  }
};


// --- AKCJE UŻYTKOWNIKA ---

async function playCard() {
  const card = selectedCard.value;
  const team = teamData.value; 

  if (!card || !team) return;

  if (team.teamBud < (card.cost || 0)) { 
    toast.error(`Drużyna ${team.teamName} ma za mało bitów!`);
    return;
  }
  
  const hasEnablers = card.enablers && Array.isArray(card.enablers) && card.enablers.length > 0;
  const wasSuccess = !hasEnablers;
  const endpoint = wasSuccess 
    ? apiConfig.player.playCardSuccess(card.id) 
    : apiConfig.player.playCardFailure(card.id);

  const payload = {
    gameId: Number(props.gameId),   
    teamId: team.teamId,            
    deckId: team.deckId,            
    boardId: team.boardId,          
    cost: card.cost || 0,
    ForceExecution: true
  };

  try {
    const response = await apiServices.post(endpoint, payload);
    const resultText = wasSuccess ? "Sukces" : "Porażka";
    toast.success(`${response.data.message || 'Akcja przetworzona.'} Wynik: ${resultText}`);
    
    await fetchAllDataForTeam();

  } catch (error) {
    toast.error("Wystąpił błąd podczas zagrywania karty.");
    console.error(error);
  }
}

async function giveItem() {
  const item = selectedItem.value;
  const team = teamData.value; 

  if (!item || !team) return;

  if (team.teamBud < (item.cost || 0)) {
    toast.error(`Drużyna ${team.teamName} ma za mało bitów!`);
    return;
  }
  
  const payload = {
    gameId: Number(props.gameId),
    teamId: team.teamId,
    deckId: team.deckId,
    boardId: team.boardId,
    cost: item.cost || 0,
    ForceExecution: true
  };
  
  try {
    const response = await apiServices.post(apiConfig.player.playCardSuccess(item.id), payload);
    toast.success(response.data.message || `Użyto przedmiot dla ${team.teamName}.`);

    // Po akcji, odświeżamy wszystkie dane, wywołując główną funkcję
    await fetchAllDataForTeam();
    
  } catch (error) {
    toast.error("Wystąpił błąd podczas używania przedmiotu.");
    console.error(error);
  }
}

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


// --- FUNKCJE POMOCNICZE I HOOKI ---



const fetchPawns = async () => {
  // Zabezpieczenie przed wywołaniem bez potrzebnych danych
  if (!teamData.value?.boardId) {
    console.warn("Pominięto pobieranie pionków - brak boardId.");
    return;
  }
  try {
    const response = await apiServices.get(apiConfig.player.getPawns, {
      params: {
        gameId: Number(props.gameId),
        teamId: Number(props.teamId),
        boardId: Number(teamData.value.boardId)
      }
    });
    pawns.value = response.data.map(p => ({
      id: p.gpId,
      x: Number(p.posX),
      y: Number(p.posY),
      color: p.color,
      name: p.name
    }));
  } catch (err) {
    console.error("Błąd pobierania pionków:", err);
  }
};

function formatDate(timestamp) {
  const date = new Date(timestamp)
  return date.toLocaleString('pl-PL')
}


watch(() => props.teamId, (newId) => {
  if (newId) {
    fetchAllDataForTeam(); 

    selectedCardId.value = null;
    selectedItemId.value = null;
  }
}, { immediate: true }); //


// Finalna, poprawna wersja onMounted
onMounted(async () => {
  // 1. Zdefiniuj nasłuchiwacze zdarzeń SignalR
  signalService.connection.on("HistoryUpdated", () => {
    console.log("SignalR: Historia się zmieniła. Odświeżam...");
    fetchAllDataForTeam(); 
  });
  signalService.connection.on("PendingUpdated", () => {
    console.log("SignalR: Sugestie się zmieniły. Odświeżam sugestie...");
    fetchPendingDecisions(true); 
  });
  signalService.connection.on("BoardUpdated", () => {
    console.log("SignalR: Plansza się zmieniła. Odświeżam pionki...");
    fetchPawns();
  });

  // Sprawdź, czy mamy kluczowe ID, zanim cokolwiek zrobisz
  if (!props.gameId || !props.teamId) {
    toast.error("Błąd krytyczny: Brak ID gry lub drużyny!");
    return;
  }

  // 2. Uruchom połączenie SignalR i DOŁĄCZ DO POKOJU
  try {
    await signalService.start();
    console.log("Połączenie SignalR pomyślnie nawiązane.");
    
    await signalService.joinGameRoom(props.gameId);
    console.log(`Pomyślnie dołączono do pokoju SignalR dla gry: ${props.gameId}`);
    
  } catch (err) {
    console.error("Błąd połączenia SignalR lub dołączania do pokoju: ", err);
    toast.error("Nie udało się połączyć z serwerem czasu rzeczywistego.");
  }
  
  // 3. Teraz, gdy jesteśmy w pokoju, pobierz wszystkie dane
  await fetchAllDataForTeam();


});

onUnmounted(() => {
  if (props.gameId) {
      signalService.leaveGameRoom(props.gameId);
  }
});


</script>
