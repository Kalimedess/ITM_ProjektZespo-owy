<template>
  <div class="p-6 text-white">
    <h2 class="text-2xl font-bold mb-4 text-center text-lime-400">Zarządzanie budżetem drużyn</h2>

    <!-- Wybór drużyny (zastępuje wybór stołu) -->
    <div v-if="!loading" class="mb-6 text-center max-w-md mx-auto">
      <label class="mr-2 font-semibold">Wybierz drużynę:</label>
      <select v-model="selectedTeamId" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
        <option :value="null">-- Wybierz drużynę --</option>
        <option v-for="team in teams" :key="team.teamId" :value="team.teamId">
          {{ team.teamName }}
        </option>
      </select>
    </div>

    <!-- Edycja budżetu (zastępuje edycję bitów) -->
    <div v-if="selectedTeam" class="max-w-md mx-auto">
      <div class="mb-4 flex items-center justify-center gap-4 bg-secondary p-3 rounded">
        <input
          v-model.number="selectedTeam.teamBud"
          type="number"
          class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-32 text-center"
        />
        <button
          class="px-4 py-2 bg-lime-500 hover:bg-lime-600 text-black font-bold rounded"
          @click="saveBudget"
        >
          Zapisz
        </button>
      </div>
    </div>

    <!-- Komunikaty pomocnicze -->
    <div v-if="loading" class="text-center text-white">Ładowanie drużyn...</div>
    <div v-else-if="!teams.length" class="text-center text-white">Nie znaleziono drużyn dla tej gry.</div>
    <div v-else-if="!selectedTeam" class="text-center text-white">Wybierz drużynę, aby edytować jej budżet.</div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useToast } from 'vue-toastification'
import apiClient from '@/assets/plugins/axios'; // Używamy Twojego globalnego klienta axios

const toast = useToast()
const route = useRoute()

// 1. Pobieramy ID gry dynamicznie z adresu URL
const gameId = route.params.gameId;

// 2. Zmieniamy nazwy zmiennych na bardziej odpowiednie
const teams = ref([])
const selectedTeamId = ref(null)
const loading = ref(true)

// 3. Właściwość computed, która znajduje obiekt wybranej drużyny
// Używamy PascalCase (team.TeamId), bo tak najprawdopodobniej zwraca dane backend .NET
const selectedTeam = computed(() => {
  if (!selectedTeamId.value) return null;
  return teams.value.find(t => t.teamId === selectedTeamId.value)
})

// 4. Funkcja do pobierania drużyn z backendu
const fetchTeams = async () => {
  if (!gameId) {
    toast.error("Błąd: Brak ID gry w adresie URL!");
    loading.value = false;
    return;
  }

  loading.value = true;
  try {
    // Używamy apiClient i poprawnego endpointu
    const response = await apiClient.get(`/api/player/game/${gameId}/teams-management`);
    teams.value = response.data;
  } catch (error) {
    toast.error('Nie udało się wczytać listy drużyn z serwera.');
    console.error("Błąd podczas pobierania drużyn:", error);
  } finally {
    loading.value = false;
  }
}

// 5. Funkcja do zapisywania nowego budżetu
const saveBudget = async () => {
  if (!selectedTeam.value) {
    toast.warning('Najpierw wybierz drużynę.');
    return;
  }

  const teamToUpdate = selectedTeam.value;
  const teamId = teamToUpdate.teamId;
  const newBudgetValue = teamToUpdate.teamBud;

  try {
    // Używamy apiClient i poprawnego endpointu
    await apiClient.put(`/api/player/team/${teamId}/budget`, { newBudget: newBudgetValue });
    toast.success(`Zapisano budżet dla drużyny ${teamToUpdate.teamName}`);
    // Opcjonalnie: odśwież dane po zapisie
    // fetchTeams(); 
  } catch (error) {
    toast.error(`Błąd podczas zapisywania budżetu dla ${teamToUpdate.teamName}`);
    console.error("Błąd podczas aktualizacji budżetu:", error);
  }
}

// 6. Wywołujemy funkcję pobierającą dane, gdy komponent jest gotowy
onMounted(() => {
  fetchTeams();
})
</script>