<template>
  <div class="w-full max-w-xl mx-auto mt-10">
    <div v-if="loading" class="text-center text-white">Ładowanie kart...</div>
    <div v-else-if="fetchError" class="text-center text-red-500">Błąd ładowania kart: {{ fetchError }}</div>
    <div v-else-if="!cards || cards.length === 0" class="text-center text-white">Brak dostępnych kart.</div>
    <div v-else-if="cards[currentIndex]" class="relative"></div>
    <div class="relative">
      
      <div
        @mousedown="startHold"
        @mouseup="cancelHold"
        @mouseleave="cancelHold"
        :class="[
          'w-full h-64 bg-gradient-to-br from-lime-400 via-lime-500 to-lime-600 rounded-2xl shadow-2xl text-white transition-transform duration-200 ease-in-out',
          cardClicked ? 'scale-105' : 'scale-100'
        ]"
      >
        <div class="flex h-full">
          <!-- Lewy przycisk -->
          <button
            @click.stop="prevCard"
            class="w-12 flex items-center justify-center hover:bg-black/20 rounded-l-2xl transition"
            aria-label="Poprzednia karta"
            :disabled="cards.length <= 1"
          >
            <p><</p>
          </button>

          <!-- Środek karty -->
          <div class="flex-1 flex flex-col items-center justify-center px-6 text-center">
            <h2 class="text-2xl font-bold mb-2">
              {{ cards[currentIndex]?.title }}
            </h2>
            <p class="text-base text-white/90 italic">
              {{ cards[currentIndex]?.description }}
            </p>
            <p class="text-xs mt-2">ID: {{ cards[currentIndex]?.id }}</p>
          </div>

          <!-- Prawy przycisk -->
          <button
            @click.stop="nextCard"
            class="w-12 flex items-center justify-center hover:bg-black/20 rounded-r-2xl transition"
            aria-label="Następna karta"
            :disabled="cards.length <= 1"
          >
            <p>></p>
          </button>
        </div>
      </div>

      <!-- Wskaźniki (zawijane) -->
      <div class="flex flex-wrap justify-center gap-2 mt-6">
        <button
          v-for="(card) in cards"
          :key="card.id"
          @click="goToCardByDisplayOrder(card.displayOrder)"
          class="w-8 h-8 text-sm font-medium rounded-full flex items-center justify-center transition-all duration-300 border-2"
          :class="card.displayOrder === cards[currentIndex].displayOrder
            ? 'bg-primary text-white border-white scale-110'
            : 'bg-gray-200 text-gray-700 border-gray-400 hover:bg-gray-300'"
        >
          {{ card.id}}
        </button>
      </div>

      <!-- Przyciski akcji -->
      <div class="flex justify-center mt-6">
        <button
          @click="sendCardSelection"
          class="bg-black text-white px-6 py-2 rounded-full shadow-md hover:bg-gray-800 transition-colors duration-200"
        >
          Wybierz kartę
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { ref, computed, watch} from 'vue'
  import apiClient from '@/assets/plugins/axios';


  const cards = ref([]);
  const currentIndex = ref(0);
  const loading = ref(true);
  const fetchError = ref(null);

  const cardClicked = ref(false)
  let holdTimeout = null

  const props = defineProps({
      deckId: {
          type: Number
      },
      gameId: {
          type: Number
        },
        teamId: {
          type: Number
        },
        boardId: {
            type: Number
        },
        gameProcessId: {
            type: Number
        },
        currentBudget: {
          type: Number,
          default: 0
        }

  });
  
  const emit = defineEmits(['card-action-completed']);

  const deckIdToFetch = computed(() => props.deckId);
  
  async function fetchCards() {
    if (!deckIdToFetch.value) {
      console.log("ID talii nie jest określone.");
      loading.value = false;
      return;
    }
    loading.value = true;
    fetchError.value = null;
    try {
      const response = await apiClient.get(`/api/player/deck/${deckIdToFetch.value}/unified-cards`, {
        params: { gameId: props.gameId, teamId: props.teamId }
      });

      if (response && response.data && Array.isArray(response.data)) {
        const sortedCards = response.data.sort((a, b) => a.displayOrder - b.displayOrder);
        cards.value = sortedCards;

        if (cards.value.length > 0) {
          currentIndex.value = 0;
        } else {
          currentIndex.value = 0; 
        }
      } else {
        cards.value = [];
        currentIndex.value = 0;
      }
      
      console.log("Pobrane dane:", JSON.stringify(cards.value, null, 2));
            
    } catch (error) {
      cards.value = [];
      currentIndex.value = 0;
      console.error("[CardCarousel] Błąd pobierania kart:", error.response?.data || error.message, error);

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
    if (cards.value.length === 0) return;
    currentIndex.value = (currentIndex.value + 1) % cards.value.length;
  };

  const prevCard = () => {
    if (cards.value.length === 0) return;
    currentIndex.value = (currentIndex.value - 1 + cards.value.length) % cards.value.length;
  };

  const goToCardByDisplayOrder = (displayOrder) => {
    const targetIndex = cards.value.findIndex(card => card.displayOrder === displayOrder);
    if (targetIndex !== -1) {
      currentIndex.value = targetIndex;
    }
  };

  const sendCardSelection = async () => {
    if (cards.value.length > 0) {
      const selectedCardData = cards.value[currentIndex.value];
      const selectedCardEnablers = selectedCardData.enablers && Array.isArray(selectedCardData.enablers) && selectedCardData.enablers.length > 0;

      const propsToSend = {
        gameId: props.gameId,
        teamId: props.teamId,
        deckId: props.deckId,
        boardId: props.boardId,
        gameProcessId: props.gameProcessId,
        cost: selectedCardData.cost

    };

      if(!selectedCardEnablers && !(props.currentBudget - propsToSend.cost < 0)){
        const response = await apiClient.post(`/api/player/success/${selectedCardData.id}`, propsToSend)
        console.log(response)
      } else {
        await apiClient.post(`/api/player/failure/${selectedCardData.id}`, propsToSend)
      }

      emit('card-action-completed', { 
        success: true
      },
        fetchCards());
      // Tutaj logika wysyłania wybranej karty (np. emit zdarzenia, wywołanie API)
      // np. emit('card-selected', selectedCardData.id); // Wysyłaj oryginalne ID karty
    }
  };

 watch(() => props.deckId, (newDeckId, oldDeckId) => {
      if (newDeckId && newDeckId !== oldDeckId) {
          fetchCards();
      }
  }, { immediate: true });

</script>