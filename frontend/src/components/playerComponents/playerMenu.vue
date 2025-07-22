<template>
  <div class="h-screen w-full max-w-md mx-auto bg-yellow rounded-xl shadow p-4 flex flex-col">
    <!-- Góra: bity + etap -->
    <div class="flex justify-between items-center mb-2">
      <div class="text-xl font-bold text-green-600">Bity: {{ currentBudget  }}</div>
      <div class="text-md font-semibold text-gray-700">Etap:</div>
    </div>

    <!-- Tabela decyzji -->
    <div class="flex flex-col flex-grow border-t pt-3 overflow-hidden">
      <h2 class="text-lg font-semibold mb-2 text-white">Decyzje</h2>

      <!-- Lista -->
      <div class="overflow-y-auto pr-2">
        <ul class="space-y-2 text-sm">
          <li
            v-for="(decision, index) in gameLogEntries"
            :key="index"
          >
            <!-- Widok dla powiadomienia o evencie -->
            <div v-if="decision.isEventNotification" class="border border-blue-500 rounded p-2 bg-blue-900/50 text-center text-xs">
              <h4 class="font-bold text-blue-300">Nowe Wydarzenie</h4>
              <p class="text-white mt-1">{{ decision.description }}</p>
            </div>
          
            <!-- Widok dla normalnego logu zagrania karty -->
            <div v-else
              class="border bg-primary text-white text-left p-2 rounded shadow-sm space-y-1 text-xs leading-tight relative"
              :class="{ 'border-purple-500': decision.eventApplied }"
            >
              <div v-if="decision.eventApplied" class="absolute top-1 right-1 px-2 py-0.5 bg-purple-600 text-white text-xs font-bold rounded-full">
                EVENT
              </div>
            
              <div><strong>{{ decision.player }}</strong> → {{ decision.choice }}</div>
              <div class="border-t border-gray-500 w-full my-1"></div>
              <div>
                Wynik:
                <span
                  class="font-semibold"
                  :class="{
                    'text-green-400': decision.result === 'Pozytywny',
                    'text-red-400': decision.result === 'Negatywny'
                  }"
                >
                  {{ decision.result }}
                </span>
              </div>
              <p class="text-xs text-white">
                {{ decision.description }}
              </p>
            </div>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>
  
  <script setup>
  import apiConfig from '@/services/apiConfig';
import apiServices from '@/services/apiServices';
import { ref, watch, onMounted} from 'vue'
  
  const gameLogEntries = ref([]);
  
 

const props = defineProps({
  gameId: {
    type: Number
  },
  teamId: {
    type: Number
  }
});

const emit = defineEmits(['budget-changed-in-menu']);
const currentBudget = ref(0);
  
// Funkcja do pobierania logu gry
const fetchGameLog = async () => {
    if (!props.gameId || !props.teamId) return;

    const propsLog = {
      gameId: props.gameId,
      teamId: props.teamId,
    };

    try {
        // Używamy nowego, dedykowanego endpointu
        const response = await apiServices.post(apiConfig.player.getPlayerHistory, propsLog);

        if (Array.isArray(response.data)) {
            gameLogEntries.value = response.data.map(log => {
                if (log.isEventNotification) {
                    return {
                        isEventNotification: true,
                        timestamp: log.timestamp,
                        description: log.eventDescription || "Aktywowano nowe wydarzenie."
                    };
                } else {
                    return {
                        isEventNotification: false,
                        player: log.teamName || `Drużyna ${props.teamId}`,
                        choice: log.cardTitle || `Karta ID: ${log.cardId}`,
                        result: log.status ? 'Pozytywny' : 'Negatywny',
                        description: log.feedbackDescription || `Koszt: ${log.cost}`,
                        eventApplied: log.gameEventId != null
                    };
                }
            });
        }
    } catch (error) {
        console.error("Błąd podczas pobierania historii gracza:", error);
    }
}

const fetchTeamBud = async () => {
  const response = await apiServices.get(apiConfig.player.getCurrency, {params: {teamId: props.teamId }});

  currentBudget.value = response.data.teamBud;
  emit('budget-changed-in-menu', response.data.teamBud);
}

 defineExpose({ fetchGameLog, fetchTeamBud});

watch(() => props.budget, (newBudget) => {
  currentBudget.value = newBudget;
  fetchGameLog();
  fetchTeamBud();
}, { immediate: true });

 onMounted(() => {
    fetchGameLog();
  });
</script>