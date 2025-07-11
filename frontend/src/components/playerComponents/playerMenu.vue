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
            class="border bg-primary text-white text-left p-2 rounded shadow-sm space-y-1 text-xs leading-tight"
          >
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
    const propsLog = {
      gameId: props.gameId,
      teamId: props.teamId,
    };

    const response = await apiServices.post(apiConfig.player.getLogs, propsLog);

    if (Array.isArray(response.data)) {
    gameLogEntries.value = response.data.map(logEntry => {
        return {
            player: logEntry.teamName || `Drużyna ${logEntry.teamId}`,
            choice: logEntry.cardTitle || `Karta ID: ${logEntry.cardId}`,
            result: logEntry.status ? 'Pozytywny' : 'Negatywny',
            description: logEntry.feedbackDescription || `Koszt: ${logEntry.cost}, Ruch: ${logEntry.moveX},${logEntry.moveY}`,
            id: logEntry.gameLogId, 
        };
    });
};
}

const fetchTeamBud = async () => {
  const response = await apiServices.get(apiConfig.player.getCurrency, {teamId: props.teamId });

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