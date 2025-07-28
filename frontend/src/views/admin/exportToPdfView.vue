<template>
    <div class="w-full flex">
        <div class="flex relative flex-col flex-1 justify-center items-center m-4 px-2 py-2 border-2 border-lgray-accent rounded-md bg-tertiary">
            <h1 class="text-3xl font-nasalization text-white mt-5">Eksport gry do PDF</h1>

            <form class="w-full max-w-lg mt-4 flex flex-col items-center">
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
                <div class="flex justify-center w-full text-white">
                 <button 
                    type="button"
                    @click="exportDeckToPDF"
                    :disabled="!isFormDeckValid"
                    :class="isFormDeckValid ? 'bg-accent/70 hover:bg-accent' : 'bg-gray-500 cursor-not-allowed'" 
                    class=" py-3 px-6 rounded-md mt-5 mb-3  transition-all">
                    <font-awesome-icon :icon="faFileExport" class="h-4 mr-2"/>
                    Generuj PDF z Kartami
                </button>
                  </div>          
                <!--Wybór plansz-->
                <label class="block text-white mb-1">Wybierz plansze:</label>
                <div class="space-y-3 max-h-60 overflow-y-auto custom-scrollbar pr-2 border-2 border-lgray-accent rounded-md bg-primary p-4 w-full">
         
                <!--Wybór Planszy Stołu-->
                <div class="mb-1 sm:mb-2">
                  <label for="selectBoard" class="block font-bold text-left text-xs text-white mb-1">Wybierz planszę stołu</label>
                  <select v-model="selectedBoard" id="selectBoard" required class="text-white bg-tertiary border-2  border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
                    <option value="" disabled>Wybierz planszę stołu</option>
                    <option v-for="board in boardsData" :key="board.boardId" :value="board.boardId">{{ board.name }}</option>
                  </select>
                </div>
            
                <!-- Wybór Plansza konkurencji nw czy to będzie pobieranie z osobnej tabelii czy wybory jak przy wyborze planszy-->
                 <div class="mb-1 sm:mb-2">
                  <label for="selectOpponentBoard" class="block font-bold text-left text-xs text-white mb-1">Wybierz planszę konkurencji</label>
                  <select v-model="selectedOponentBoard" id="selectOpponentBoard" required class="text-white bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
                    <option value="" disabled>Wybierz planszę konkurencji</option>
                    <option v-for="board in boardsData" :key="board.boardId" :value="board.boardId">{{ board.name }}</option>
                  </select>
                    </div> 
                </div>

                 <div class="flex justify-center w-full text-white">
                        <button 
                            type="button"
                            @click="exportBoardsToPDF"
                            :disabled="!isFormBoardsValid"
                            :class="isFormBoardsValid ? 'bg-accent/70 hover:bg-accent' : 'bg-gray-500 cursor-not-allowed'" 
                            class=" py-3 px-6 rounded-md mt-5 mb-3  transition-all">
                            <font-awesome-icon :icon="faFileExport" class="h-4 mr-2"/>
                            Generuj PDF z planszami
                        </button>
                </div>
            <loadingSpinner
                :show="isLoading"
                message="Generuję PDF..."
            />
            </form>
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
  const selectedBoard = ref(null);
  const selectedOponentBoard = ref(null);

    //Ładowanie plików
    const isLoading = ref(false);
    const isGeneratingCards = ref(false);
    const isGeneratingBoards = ref(false);

    //Weryfikacja czy użytkownik może wygenerować PDF
    const isFormBoardsValid = computed(() => {
        return selectedBoard.value && selectedOponentBoard.value;
    })

    const isFormDeckValid = computed(() => {

        return selectedDeck.value != null;
    })

    // Funkcja pomocnicza do pobierania plików
    const downloadFileFromResponse = (response) => {
    const header = response.headers['content-disposition'];
    let fileName = 'pobrany_plik.pdf'; // Domyślna nazwa

    if (header) {
        // 1. Spróbuj znaleźć nowoczesny format (filename*) - najbardziej niezawodny
        let match = header.match(/filename\*=UTF-8''([^;]+)/);
        if (match && match[1]) {
            // Jeśli znaleziono, zdekoduj nazwę pliku (np. ze znaków %20 na spacje)
            fileName = decodeURIComponent(match[1]);
        } else {
            // 2. Jeśli nie ma formatu nowoczesnego, szukaj starego (filename=)
            match = header.match(/filename="?([^"]+)"?/);
            if (match && match[1]) {
                fileName = match[1];
            }
        }
    }

    const blob = new Blob([response.data], { type: 'application/pdf' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.style.display = 'none';
    a.href = url;
    a.download = fileName; // Użyj czystej, wyparsowanej nazwy
    document.body.appendChild(a);
    a.click();
    window.URL.revokeObjectURL(url);
    document.body.removeChild(a);
};
    //Eksport talii kart do PDF
   const exportDeckToPDF = async () => {
  if (!selectedDeck.value) return;

  isGeneratingCards.value = true;
  try {
    // Używamy getFile, przekazując ID talii jako parametr zapytania (query param)
    const response = await apiService.getFile(
      apiConfig.admin.export.cards, 
      { deckId: selectedDeck.value }
    );
    downloadFileFromResponse(response);
  } catch (error) {
    console.error("Błąd podczas generowania PDF z kartami:", error);
  } finally {
    isGeneratingCards.value = false;
  }
};

    //Eksport plansz do PDF
    const exportBoardsToPDF = async () => {
      // Poprawiona walidacja
      if (!isFormBoardsValid.value) return;

      isGeneratingBoards.value = true;
      try {
        // Stwórz obiekt JSON pasujący do DTO na backendzie
        const requestData = {
          teamBoardId: selectedBoard.value,
          rivalBoardId: selectedOponentBoard.value
        };

        // Wyślij ten obiekt jako dane w żądaniu POST
        const response = await apiService.postForFile(
          apiConfig.admin.export.boards,
          requestData // Przekaż obiekt jako JEDYNY argument danych
        );

        downloadFileFromResponse(response);
      } catch (error) {
        console.error("Błąd podczas generowania PDF z planszami:", error);
        toast.error("Wystąpił błąd podczas generowania PDF.");
      } finally {
        isGeneratingBoards.value = false;
      }
    };

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