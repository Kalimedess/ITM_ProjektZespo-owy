<template>
  <div class="w-full max-w-xl mx-auto mt-10">
    <div v-if="loading" class="text-center text-white">Ładowanie kart...</div>
    <div v-else-if="fetchError" class="text-center text-red-500">Błąd ładowania kart: {{ fetchError }}</div>
    
    <div v-else-if="!displayCards || displayCards.length === 0" class="text-center text-white">Brak dostępnych kart w tej kategorii.</div>

    <div v-else class="relative">
      
      <!-- Wskaźniki (lista rozwijana) -->
      <div class="flex flex-wrap justify-center gap-2 mt-6">
        <select
          v-model="currentIndex"
          class="w-full py-5 border rounded-t-2xl text-center"
        >
          <option
            v-for="(card, index) in displayCards"
            :key="card.id"
            :value="index"
          >
            <!-- ZMIANA: Wyświetlanie prawdziwego ID karty zamiast indeksu -->
            {{ card.id }}: {{ card.title }}
          </option>
        </select>
      </div>

      <div
        @mousedown="startHold"
        @mouseup="cancelHold"
        @mouseleave="cancelHold"
        :class="[
          'w-full h-full bg-gradient-to-br from-transparent to-transparent rounded-b-2xl shadow-2xl text-white'
        ]"
        style="--tw-gradient-from: #5DBB63; --tw-gradient-to: #607D3B;"
      >
        <div class="flex h-full">
          <!-- Lewy przycisk -->
          <button
            @click.stop="prevCard"
            class="w-12 flex items-center justify-center py-5 hover:bg-black/20 rounded-bl-2xl transition"
            aria-label="Poprzednia karta"
            :disabled="displayCards.length <= 1"
          >
            <p><</p>
          </button>

          <!-- Środek karty -->
          <div class="flex-1 flex flex-col items-center justify-center px-6 text-center py-5">
            <h2 class="text-3xl font-bold mb-2">
              {{ displayCards[currentIndex]?.title }}
            </h2>
            
            <!-- ZMIANA: Opis jest teraz renderowany warunkowo -->
            <p v-if="props.isOnlineGame" class="text-l text-white/90 font-bold">
              {{ displayCards[currentIndex]?.description }}
            </p>
            
            <p class="text-xs mt-2">ID: {{ displayCards[currentIndex]?.id }}</p>
          </div>

          <!-- Prawy przycisk -->
          <button
            @click.stop="nextCard"
            class="w-12 flex items-center justify-center py-5 hover:bg-black/20 rounded-br-2xl transition"
            aria-label="Następna karta"
            :disabled="displayCards.length <= 1"
          >
            <p>></p>
          </button>
        </div>
      </div>

      <!-- Przyciski akcji -->
      <div class="flex justify-center mt-6">
        <button
          @click="sendCardSelection"
          class="bg-black text-white px-6 py-2 rounded-full shadow-md hover:bg-gray-800 transition-colors duration-200"
        >
          {{ buttonLabel }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch, watchEffect } from 'vue';
import apiConfig from '@/services/apiConfig';
import apiServices from '@/services/apiServices';
import { useToast } from 'vue-toastification';

// --- Reaktywne referencje i stałe ---
const toast = useToast();

const decisionCards = ref([]);
const itemCards = ref([]);
const currentIndex = ref(0);
const loading = ref(true);
const fetchError = ref(null);

// Definiowanie propsów i emitów
const props = defineProps({
  deckId: Number,
  gameId: Number,
  teamId: Number,
  boardId: Number,
  gameProcessId: Number,
  currentBudget: {
    type: Number,
    default: 0
  },
  showingDecisionCards: {
    type: Boolean,
    required: true
  },
  isOnlineGame: {
    type: Boolean,
    default: true
  },
  isIndependentTeam: {
    type: Boolean,
    default: true
  }
});

const emit = defineEmits(['card-action-completed']);

// --- Computed Properties (Właściwości Obliczeniowe) ---

const displayCards = computed(() => {
  return props.showingDecisionCards ? decisionCards.value : itemCards.value;
});

// POPRAWKA: Zabezpieczenie przed nieprawidłowym indeksem
const selectedCard = computed(() => {
  // Jeśli `displayCards` jest puste lub indeks jest poza zakresem, zwróć null.
  if (!displayCards.value || currentIndex.value >= displayCards.value.length) {
    return null;
  }
  return displayCards.value[currentIndex.value];
});

const buttonLabel = computed(() => {
  return props.isIndependentTeam ? 'Wybierz kartę' : 'Sugeruj kartę';
});

// --- Metody ---

async function fetchCards() {
  if (!props.deckId) {
    loading.value = false;
    return;
  }
  
  loading.value = true;
  fetchError.value = null;

  try {
    const response = await apiServices.get(apiConfig.player.getCards(props.deckId), {
      params: { gameId: props.gameId, teamId: props.teamId }
    });
    
    // Używamy "optional chaining" (?.) i "nullish coalescing" (??) dla zwięzłości
    decisionCards.value = response.data?.decisionCards ?? [];
    itemCards.value = response.data?.itemCards ?? [];

  } catch (error) {
    decisionCards.value = [];
    itemCards.value = [];
    fetchError.value = "Wystąpił błąd podczas pobierania kart.";
    console.error("[CardCarousel] Błąd pobierania kart:", error);
  } finally {
    loading.value = false;
  }
}

const nextCard = () => {
  if (displayCards.value.length === 0) return;
  currentIndex.value = (currentIndex.value + 1) % displayCards.value.length;
};

const prevCard = () => {
  if (displayCards.value.length === 0) return;
  currentIndex.value = (currentIndex.value - 1 + displayCards.value.length) % displayCards.value.length;
};

// POPRAWKA: Uproszczona i bardziej czytelna metoda
const sendCardSelection = async () => {
  // Używamy właściwości computed, która już jest bezpieczna
  if (!selectedCard.value) {
    toast.warning("Nie wybrano żadnej karty.");
    return;
  }

  const { id: cardId, cost, enablers } = selectedCard.value;

  const hasEnablers = Array.isArray(enablers) && enablers.length > 0;
  const hasSufficientBudget = props.currentBudget >= cost;
  
  // Logika sukcesu jest teraz prostsza
  const isSuccess = !hasEnablers && hasSufficientBudget;

  const cardPlayData = {
    gameId: props.gameId,
    teamId: props.teamId,
    deckId: props.deckId,
    boardId: props.boardId,
    gameProcessId: props.gameProcessId,
    cost: cost
  };
  
  // Budujemy URL dynamicznie
  const apiUrl = isSuccess 
    ? apiConfig.player.playCardSuccess(cardId) 
    : apiConfig.player.playCardFailure(cardId);

  try {
    const response = await apiServices.post(apiUrl, cardPlayData);
    
    toast.success(response.data?.message ?? "Akcja została wykonana.");
    
    emit('card-action-completed', { 
      success: true, 
      newBudget: response.data.newTeamBudget 
    });

    await fetchCards(); // Poczekaj na odświeżenie kart

  } catch (err) {
    toast.error(err.response?.data?.message ?? "Wystąpił błąd podczas komunikacji z serwerem.");
    console.error("Błąd podczas zagrywania karty:", err);
    emit('card-action-completed', { success: false });
  }
};

// --- Watchers (Obserwatorzy) ---

// POPRAWKA: Resetuj indeks, gdy zmienia się wyświetlana lista kart
watch(displayCards, () => {
  currentIndex.value = 0;
});

// Użyj watchEffect do pobierania danych na podstawie propsów - to jest OK
watchEffect(() => {
  const { deckId, gameId, teamId } = props;
  if (deckId != null && gameId != null && teamId != null) {
    fetchCards();
  }
});
</script>