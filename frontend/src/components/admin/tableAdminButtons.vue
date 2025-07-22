<template>
    <!-- Use v-if to prevent rendering before game data is loaded -->
    <div v-if="game.status" class="flex flex-row justify-center gap-3 items-center mt-5 mb-5">
        <div class="flex gap-2">
            <div class="flex flex-row gap-2">
                <!-- This button only shows for games that are 'During' or 'Paused' -->
                <button 
                    v-if="game.status === 'During' || game.status === 'Paused'"
                    @click="updateGameStatus(gameId, game.status === 'During' ? 'Paused' : 'During')"
                    class="flex flex-auto items-center justify-center border-2 border-lgray-accent py-2 px-3 rounded-md hover:border-accent transition-colors duration-300" 
                    @dblclick.stop>
                    <font-awesome-icon :icon="game.status === 'During' ? faCircleStop : faCirclePlay" class="h-4 text-accent mr-2"/>
                    {{ game.status === 'During' ? 'Wstrzymaj grę' : 'Wznów grę' }}
                </button>

                <!-- This button is always available for active/paused games to end them -->
                 <button 
                    v-if="game.status !== 'End'"
                    @click="updateGameStatus(gameId, 'End')"
                    class="flex flex-auto items-center justify-center border-2 border-lgray-accent py-2 px-3 rounded-md hover:border-accent transition-colors duration-300" 
                    @dblclick.stop>
                    <font-awesome-icon :icon="faPowerOff" class="h-4 mr-2 text-accent"/>  
                    Zakończ grę
                </button>
            </div>
        </div>
    </div>
</template>

<script setup>
import { faCircleStop, faCirclePlay, faPowerOff } from '@fortawesome/free-solid-svg-icons';
import { onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import apiService from '@/services/apiServices';
import apiConfig from '@/services/apiConfig';

const route = useRoute();
const router = useRouter();
const toast = useToast();
const emit = defineEmits(['update-status']);

const gameId = Number(route.params.gameId);
const game = ref({});

const updateGameStatus = async (id, newStatus) => {
    try {
        const response = await apiService.put(apiConfig.games.updateStatus(id), newStatus);
        
        game.value.status = newStatus;
        toast.success(response.data?.message || `Status gry pomyślnie zaktualizowany.`);
        emit('update-status', { gameId: id, newStatus: newStatus });

        if (newStatus === 'End') {
            setTimeout(() => {
                router.push('/admin');
            }, 750);
        }

    } catch (error) {
        console.error(`Błąd aktualizacji statusu gry ${id}:`, error);
        toast.error(`Nie udało się zaktualizować statusu gry: ${error.response?.data?.message || 'Wystąpił nieznany błąd.'}`);
    }
};

const getGameDetails = async () => {
    try {
        const response = await apiService.get(apiConfig.admin.games.getGames(gameId));
        game.value = response.data;
    } catch (error) {
        console.error('Błąd przy pobieraniu danych gry:', error);
        toast.error('Nie udało się załadować szczegółów gry.');
    }
};

onMounted(() => {
    getGameDetails();
});
</script>