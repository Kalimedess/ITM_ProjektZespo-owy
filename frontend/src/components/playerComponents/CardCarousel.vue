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
  import { ref, computed, watch } from 'vue'
  import apiConfig from '@/services/apiConfig';
  import apiServices from '@/services/apiServices';

  const decisionCards = ref([]);
  const itemCards = ref([]);
  
  const currentIndex = ref(0);
  const loading = ref(true);
  const fetchError = ref(null);

  const cardClicked = ref(false)
  let holdTimeout = null

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
        default: true // Domyślnie pokazuj, jeśli prop nie dotrze
      },
      isIndependentTeam: { type: Boolean, default: true }
  });
  
  const emit = defineEmits(['card-action-completed']);

  const buttonLabel = computed(() => {
    return props.isIndependentTeam ? 'Wybierz kartę' : 'Sugeruj kartę';
  });

  const displayCards = computed(() => {
    return props.showingDecisionCards ? decisionCards.value : itemCards.value;
  });

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

      if (response.data && response.data.decisionCards && response.data.itemCards) {
        decisionCards.value = response.data.decisionCards;
        itemCards.value = response.data.itemCards;
        currentIndex.value = 0;
      } else {
        decisionCards.value = [];
        itemCards.value = [];
        fetchError.value = "Otrzymano nieprawidłowe dane z serwera.";
      }
            
    } catch (error) {
      decisionCards.value = [];
      itemCards.value = [];
      fetchError.value = "Wystąpił błąd podczas pobierania kart.";
      console.error("[CardCarousel] Błąd pobierania kart:", error);

    } finally {
      loading.value = false;
    }
  };
  
  const startHold = () => {
    holdTimeout = setTimeout(() => {
      cardClicked.value = true;
    }, 500);
  };

  const cancelHold = () => {
    clearTimeout(holdTimeout);
    cardClicked.value = false;
  };

  const nextCard = () => {
    if (displayCards.value.length === 0) return;
    currentIndex.value = (currentIndex.value + 1) % displayCards.value.length;
  };

  const prevCard = () => {
    if (displayCards.value.length === 0) return;
    currentIndex.value = (currentIndex.value - 1 + displayCards.value.length) % displayCards.value.length;
  };

  const sendCardSelection = async () => {
    if (displayCards.value.length === 0) return;
    
    const selectedCardData = displayCards.value[currentIndex.value];
    if (!selectedCardData) return;
    
    const selectedCardEnablers = selectedCardData.enablers && Array.isArray(selectedCardData.enablers) && selectedCardData.enablers.length > 0;

    const propsToSend = {
      gameId: props.gameId,
      teamId: props.teamId,
      deckId: props.deckId,
      boardId: props.boardId,
      gameProcessId: props.gameProcessId,
      cost: selectedCardData.cost
    };

    try {
        if(!selectedCardEnablers && !(props.currentBudget - propsToSend.cost < 0)){
          await apiServices.post(apiConfig.player.playCardSuccess(selectedCardData.id), propsToSend);
        } else {
          await apiServices.post(apiConfig.player.playCardFailure(selectedCardData.id), propsToSend);
        }

        emit('card-action-completed', { success: true });
        fetchCards();
    } catch (err) {
        console.error("Błąd podczas zagrywania karty:", err);
        emit('card-action-completed', { success: false });
    }
  };
  
  watch(() => props.showingDecisionCards, () => {
    currentIndex.value = 0;
  });

  watch(() => [props.deckId, props.gameId, props.teamId], (newVals) => {
      if (newVals.every(v => v != null)) {
          fetchCards();
      }
  }, { immediate: true });
</script>