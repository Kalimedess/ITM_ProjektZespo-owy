<template>
    <div class="w-full text-white">
        <div class="m-4 px-2 py-2 border-2 border-lgray-accent rounded-md bg-tertiary">
            <h1 class="text-center text-white font-nasalization text-3xl mt-2 mb-4">Gry w sesji</h1>
            <homeAdminButtons 
                @open-create-game="showCreateGame = true"/>
            <hr class ="my-4 border-lgray-accent"/>

            <div v-if="loadingGames" class="text-center py-4">Ładowanie gier...</div>
            <div v-else class="grid grid-cols-1 xl:grid-cols-3 gap-4 mt-6">
                <gameCard
                    v-for="game in activeGames"
                    :key="game.id"
                    :game="game"
                    :color="getGameColor(game.id)"
                    @update-status="handleUpdateGameStatus"
                />
            </div>
        </div>
        <createGame
            :isVisible="showCreateGame"
            @close="showCreateGame = false"
            @gameCreated="handleGameCreated" 
            />
    </div>

</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import { useToast } from 'vue-toastification';
    import gameCard from '@/components/game/gameCard.vue';
    import homeAdminButtons from '@/components/admin/homeAdminButtons.vue';
    import CreateGame from '@/components/game/createGame.vue';

    import apiConfig from '@/services/apiConfig.js';
    import apiService from '@/services/apiServices.js';

    const toast = useToast();
    const showCreateGame = ref(false);

    const activeGames = ref([]);
    const loadingGames = ref(false);
    const fetchError = ref(null);

    const getGameColor = (gameId) => {
        const colors = [
            '#E53E3E', '#3182CE', '#38A169', '#D69E2E',
            '#805AD5', '#D53F8C', '#DD6B20', '#319795', '#5A67D8',
        ];
        return colors[ (gameId -1) % colors.length ]; 
    }

    const fetchActiveGames = async () => {
        loadingGames.value = true;
        fetchError.value = null;
        try {
            const response = await apiService.get(apiConfig.games.getAll);
            activeGames.value = response.data;
            if (activeGames.value.length === 0) {
                // 
            }
        } catch (error) {
            console.error("Błąd pobierania aktywnych gier:", error.response?.data || error.message, error);
            fetchError.value = `Nie udało się pobrać gier: ${error.response?.data?.title || error.response?.statusText || error.message}`;
        } finally {
            loadingGames.value = false;
        }
    };

    const handleGameCreated = (creationResponse) => {
        fetchActiveGames();
        if (creationResponse) {
            toast.success(creationResponse.message || "Nowa gra została dodana!");
        }
    };

    const handleUpdateGameStatus = async ({ gameId, newStatus }) => {
  const gameToUpdate = activeGames.value.find(g => g.id === gameId);
  if (!gameToUpdate) return;

  try {
    const response = await apiService.put(apiConfig.games.updateStatus(gameId), newStatus);
    
    if (response.data && response.data.newStatus) {
        gameToUpdate.status = response.data.newStatus;
        toast.success(response.data.message || `Status gry ${gameToUpdate.name} zaktualizowany.`);
        await fetchActiveGames();
    } else {
        await fetchActiveGames();
        toast.success(`Status gry ${gameToUpdate.name} zaktualizowany (lista odświeżona).`);
    }

  } catch (error) {
    console.error(`Błąd aktualizacji statusu gry ${gameId}:`, error.response?.data || error.message, error);
    toast.error(`Nie udało się zaktualizować statusu gry: ${error.response?.data?.message || error.response?.data || error.message}`);
    await fetchActiveGames();
  }
};

    onMounted(() => {
        fetchActiveGames();
    });

</script>