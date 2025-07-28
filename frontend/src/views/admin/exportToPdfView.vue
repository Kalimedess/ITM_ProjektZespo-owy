<template>
    <div class="w-full flex">
        <div class="flex relative flex-col flex-1 justify-center items-center m-4 px-2 py-2 border-2 border-lgray-accent rounded-md bg-tertiary">
            <h1 class="text-3xl font-nasalization text-white mt-5">Eksport gry do PDF</h1>

            <form @submit.prevent="exportToPDF" class="w-full max-w-lg mt-4 flex flex-col items-center">

                <!--Wybór talii kart-->
                <div class="w-full mb-4">
                    <label class="block text-white mb-1">Wybierz talię:</label>
                    <dropDown 
                        :items="decksData"
                        v-model="selectedDeck"
                        :item-key="'id'"
                        :display-format="(deck) => `#${deck.id} ${deck.title}`"
                        :item-label="'title'"
                        placeholder="Wybierz talię..."
                    />
                </div>

                <!--Wybór plansz-->
                <label class="block text-white mb-1">Wybierz planszę:</label>
                <div class="space-y-3 max-h-60 overflow-y-auto custom-scrollbar pr-2 border-2 border-lgray-accent rounded-md bg-primary p-4 w-full">
                    <label v-for="board in boardsData" :key="board.boardId" 
                        class="flex items-center p-3 bg-tertiary rounded-md cursor-pointer hover:bg-accent/30 transition-colors duration-200">
                        
                        <input 
                            type="checkbox"
                            :value="board.boardId"
                            v-model="selectedBoards"
                            class="w-5 h-5 rounded bg-primary border-lgray-accent text-accent focus:ring-accent"
                        />
                        
                        <span class="ml-3 font-bold text-white">{{ board.name }}</span>
                    </label>
                </div>

                 <div class="flex justify-center w-full text-white">
                        <button 
                            type="submit"
                            :disabled="!isFormValid"
                            :class="isFormValid ? 'bg-accent/70 hover:bg-accent' : 'bg-gray-500 cursor-not-allowed'" 
                            class=" py-3 px-6 rounded-md mt-5 mb-3  transition-all">
                            <font-awesome-icon :icon="faFileExport" class="h-4 mr-2"/>
                            Generuj PDF
                        </button>
                </div>
            </form>
            <loadingSpinner
                :show="isLoading"
                message="Generuję PDF..."
            />
        </div>
    </div>
</template>

<script setup>

    import {ref, onMounted, computed } from 'vue';
    import { useToast } from 'vue-toastification';
    import { faFileExport } from '@fortawesome/free-solid-svg-icons';
    import apiConfig from '@/services/apiConfig.js';
    import apiService from '@/services/apiServices.js';
    import dropDown from '@/components/dropDown.vue';
    import loadingSpinner from '@/components/loadingSpinner.vue';

    const toast = useToast();
    

    // Talie kart
    const decksData = ref([]);
    const selectedDeck = ref(null);

    // Plansze
    const boardsData = ref([]);
    const selectedBoards = ref([]);

    //Ładowanie plików
    const isLoading = ref(false);

    //Weryfikacja czy użytkownik może wygenerować PDF
    const isFormValid = computed(() => {
        return selectedDeck.value !== null && selectedBoards.value.length > 0;
    })

    //Eksport talii kart oraz planszy do PDF
    const exportToPDF = async () => {
        if (!isFormValid.value) return
        
        isLoading.value = true
        try {
            //Logika tutaj
            toast.success('PDF został wygenerowany!')
            selectedBoards.value = [];
            selectedDeck.value = null;
        } catch (error) {
            toast.error('Błąd podczas generowania PDF')
        } finally {
            isLoading.value = false
        }
    }

    // Pobieranie plansz z bazy danych
    const fetchBoardsFromAPI = async () => {
        try {
        const response = await apiService.get(apiConfig.boards.getAll);
        boardsData.value = response.data;
        if (boardsData.value.length === 0) {
            toast.info("Brak plansz do wyboru!");
        }
        } catch (error) {
        console.error('Błąd pobierania plansz:', error.response?.data || error.message);
        toast.error(`Nie udało się pobrać plansz: ${error.response?.data?.title || error.message}`);
        }
    };

    // Pobieranie talii kart z bazy danych
    const fetchDecksFromAPI = async () => {
        try {
        const response = await apiService.get(apiConfig.admin.deck.getAll);
        decksData.value = response.data;
        if (decksData.value.length === 0) {
            toast.info("Brak talii kart do wyboru!");
        }
        } catch (error) {
        console.error('Błąd pobierania talii kart:', error.response?.data || error.message);
        toast.error(`Nie udało się pobrać talii kart: ${error.response?.data?.title || error.message}`);
        }
    };


    onMounted(async () => {
       await Promise.all([
            fetchDecksFromAPI(),
            fetchBoardsFromAPI()
        ]);
    });


</script>


<style scoped>
    .custom-scrollbar::-webkit-scrollbar {
    width: 0.6rem; 
    }

    .custom-scrollbar::-webkit-scrollbar-track {
    background: transparent; 
    margin: 0.5rem 0.3rem; 
    }

    .custom-scrollbar::-webkit-scrollbar-thumb {
    background: #a78bfa; 
    border-radius: 0.25rem; 
    border: 0.1rem solid transparent; 
    background-clip: content-box; 
    }
</style>