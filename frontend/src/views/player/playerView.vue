<template>
  <div class="flex flex-col h-screen bg-primary relative overflow-hidden">
    <PlayerNavbar
      :team-name="gameData?.teamName"
      :nav-bg-color="gameData?.teamColor"
    />

    <div class="flex flex-1 flex-row relative overflow-hidden">
      <!-- Lewy panel -->
      <Transition name="fade-slide" appear>
        <div
          v-if="leftOpen"
          class="absolute left-0 top-0 w-1/2 h-full bg-secondary border-r border-gray-400 shadow-lg z-40 overflow-auto p-4 transition-all duration-500 ease-in-out"
        >
          <RouterView />
          <QuestionBox />

                    <!-- 2. Dodajemy przyciski przełączające -->
          <div class="flex justify-center space-x-2 my-4">
              <button
                  @click="showingDecisionCards = true"
                  :class="showingDecisionCards ? 'bg-blue-600 text-white' : 'bg-white text-black'"
                  class="px-5 py-2 rounded-md border font-semibold"
              >
                  Decyzje
              </button>
              <button
                  @click="showingDecisionCards = false"
                  :class="!showingDecisionCards ? 'bg-blue-600 text-white' : 'bg-white text-black'"
                  class="px-5 py-2 rounded-md border font-semibold"
              >
                  Przedmioty
              </button>
          </div>

          <Suspense>
              <template #default>
                  <CardCarousel
                      ref="cardCarouselRef"
                      v-if="gameData && gameData.deckId" 
                      :deck-id="gameData.deckId" 
                      :team-id="gameData.teamId"
                      :game-id="gameData.gameId"
                      :board-id="gameData.boardConfig?.boardId"
                      :current-budget="currentGlobalBudget"
                    
                      :showing-decision-cards="showingDecisionCards"

                      :is-online-game="gameData?.isOnline"
                      :is-independent-team="gameData?.isIndependent"

                      @card-action-completed="handleCardActionCompleted"
                  />
              </template>
              <template #fallback>
                  <div>Ładowanie karuzeli kart...</div>
              </template>
          </Suspense>
        </div>
      </Transition>

      <!-- Plansza -->
      <div
        class="transition-all duration-300 h-full bg-secondary border-2 border-lgray-accent rounded-md shadow-sm text-center p-4 pb-10 z-30"
        :class="[
          leftOpen ? 'w-1/2 ml-auto' : '',
          rightOpen ? 'w-1/2 mr-auto' : '',
          (!leftOpen && !rightOpen) ? 'w-1/2 mx-auto' : ''
        ]"
      >
        <div class="flex justify-center">
          <button
            @click="currentBoard = 'player'"
            :class="currentBoard === 'player' ? 'bg-black text-white' : 'bg-white text-black'"
            class="px-4 py-1 rounded-md border"
          >
            Twoja plansza
          </button>
          <button
            @click="currentBoard = 'market'"
            :class="currentBoard === 'market' ? 'bg-black text-white' : 'bg-white text-black'"
            class="px-4 py-1 rounded-md border"
          >
            Plansza rynku
          </button>
        </div>

        <div class="flex justify-between items-center">
          <!-- Lewy przycisk -->
          <button
            @click="showLeftPanel"
            class="bg-gray-800 text-white px-4 py-2 rounded-md"
          >
            Panel kart
          </button>

          <!-- Prawy przycisk -->
          <button
            @click="showRightPanel"
            class="bg-gray-800 text-white px-4 py-2 rounded-md"
          >
            Panel decyzji
          </button>
        </div>
        <!-- Zamienić pawnCount na liczbe graczy w danym stole -->
        <!-- Zamienić pawnPositions na pozycje pionkow w grze (w testgameboard jest lokiga dzielenia pionków na grid wiec podajemy 1,1 albo 1,2) -->
        <GameBoard
          v-if="currentBoard === 'player'"
          :config="formData"
          :gameMode="true"
          :pawns="pawns"
        />
        <GameBoard
          v-if="currentBoard === 'market'"
          :config="enemyformData"
          :gameMode="true"
          :pawns="enemypawns"
        />
      </div>

      <!-- Prawy panel -->
      <div
        v-if="rightOpen"
        class="absolute right-0 top-0 w-1/2 h-full bg-secondary border-l border-gray-400 shadow-lg z-40 overflow-auto p-4"
      >
        <PlayerMenu 
          ref="playerMenuRef"
          v-if="currentPanel === 'menu' && gameData"
          :game-id="gameData?.gameId"
          :team-id="gameData?.teamId"
          @budget-changed-in-menu="handleBudgetChangeFromMenu"
        />
      </div>
    </div>

    <!-- Stopka -->
    <div class="mt-auto">
      <Footer />
    </div>
  </div>
</template>



<script setup>


//---------------------------------------------------------------
import { reactive, ref, watch, onMounted, onUnmounted} from 'vue'
import PlayerNavbar from '@/components/navbars/playerNavbar.vue'
import QuestionBox from '@/components/playerComponents/questionBox.vue'
import GameBoard from '@/components/game/gameBoard.vue'
import Footer from '@/components/footers/adminFooter.vue'
import CardCarousel from '@/components/playerComponents/CardCarousel.vue'
import PlayerMenu from '@/components/playerComponents/playerMenu.vue'
import { RouterView } from 'vue-router'
import apiConfig from '@/services/apiConfig'
import apiServices from '@/services/apiServices'
import signalrService from '@/services/signalService';

const showingDecisionCards = ref(true);
const currentPanel = ref('menu')

    const formData = reactive({
        Name: 'Plansza podstawowa', 
        LabelsUp: ['Podstawowa kordynacja', 'Standaryzacja procesów', 'Zintegrowane działania', 'Pełna integracja strategiczna'], 
        LabelsRight: ['Nowicjusz', 'Naśladowca', 'Innowator', 'Lider cyfrowy'], 
        DescriptionDown: 'Poziom integracji wew/zew', 
        DescriptionLeft: 'Zawansowanie Cyfrowe', 
        Rows: 8,
        Cols: 8,
        CellColor: '#fefae0', 
        BorderColor: '#595959', 
        BorderColors: ['#008000', '#FFFF00', '#FFA500', '#FF0000']
    });
    const enemyformData = reactive({
        Name: 'Plansza podstawowa', 
        LabelsUp: ['Podstawowa kordynacja', 'Standaryzacja procesów', 'Zintegrowane działania', 'Pełna integracja strategiczna'], 
        LabelsRight: ['Nowicjusz', 'Naśladowca', 'Innowator', 'Lider cyfrowy'], 
        DescriptionDown: 'Poziom integracji wew/zew', 
        DescriptionLeft: 'Zawansowanie Cyfrowe', 
        Rows: 8,
        Cols: 8,
        CellColor: '#fefae0', 
        BorderColor: '#595959', 
        BorderColors: ['#008000', '#FFFF00', '#FFA500', '#FF0000']
    });
    const posX = ref(7);
    const posY = ref(7);

const props = defineProps({
  teamToken: String // Odbieramy teamToken jako prop dzięki `props: true` w routerze
});
const gameData = ref(null); // Tutaj będą przechowywane wszystkie dane gry
const isLoading = ref(true);
const errorLoading = ref(null);

const currentGlobalBudget = ref(0);

const playerMenuRef = ref(null);
const cardCarouselRef = ref(null);

const fetchGameDataByToken = async (token) => {
  if (!token) {
    errorLoading.value = "Brak tokena drużyny w adresie URL.";
    isLoading.value = false;
    return;
  }
  isLoading.value = true;
  errorLoading.value = null;
  gameData.value = null;
  try {
    const response = await apiServices.get(apiConfig.player.getTeamInfo(token));
    console.log("Dane z backendu (team info):", response.data);


    gameData.value = {
        ...response.data,
        boardConfig: response.data.boardConfig, 
    };

    formData.Name = gameData.value.boardConfig.name;
    formData.LabelsUp = gameData.value.boardConfig.labelsUp;
    formData.LabelsRight = gameData.value.boardConfig.labelsRight;
    formData.BorderColors = gameData.value.boardConfig.borderColors;
    formData.DescriptionDown = gameData.value.boardConfig.descriptionDown;
    formData.DescriptionLeft = gameData.value.boardConfig.descriptionLeft;
    formData.Rows = gameData.value.boardConfig.rows;
    formData.Cols = gameData.value.boardConfig.cols;
    formData.CellColor = gameData.value.boardConfig.cellColor;
    formData.BorderColor = gameData.value.boardConfig.borderColor;

    if (playerMenuRef.value && currentPanel.value === 'menu') {
      playerMenuRef.value.fetchGameLog();
    }


    if (gameData.value.rivalBoardConfig) {
      enemyformData.Name = gameData.value.rivalBoardConfig.name;
      enemyformData.LabelsUp = gameData.value.rivalBoardConfig.labelsUp;
      enemyformData.LabelsRight = gameData.value.rivalBoardConfig.labelsRight;
      enemyformData.BorderColors = gameData.value.rivalBoardConfig.borderColors;
      enemyformData.DescriptionDown = gameData.value.rivalBoardConfig.descriptionDown;
      enemyformData.DescriptionLeft = gameData.value.rivalBoardConfig.descriptionLeft;
      enemyformData.Rows = gameData.value.rivalBoardConfig.rows;
      enemyformData.Cols = gameData.value.rivalBoardConfig.cols;
      enemyformData.CellColor = gameData.value.rivalBoardConfig.cellColor;
      enemyformData.BorderColor = gameData.value.rivalBoardConfig.borderColor;
    } else 
    {
      console.warn("Brak konfiguracji rivalBoardConfig dla planszy rywala/rynku. Prawdopodobnie gra offline lub brak planszy rywala.");
    }

    fetchPawns();
    fetchRivalPawns();
    

  } catch (err) {
    console.error("Błąd ładowania danych gry przez token:", err);
    errorLoading.value = err.response?.data?.message || err.message || "Nieznany błąd serwera.";
    if (err.response?.status === 404) {
        errorLoading.value = "Nie znaleziono gry lub drużyny dla podanego tokena.";
    }
  } finally {
    isLoading.value = false;
  }
};

const handleCardActionCompleted = async (eventPayload) => {
    if (eventPayload.success) {
      if (playerMenuRef.value && currentPanel.value === 'menu') {
        playerMenuRef.value.fetchGameLog();
        playerMenuRef.value.fetchTeamBud();
      } else {
        console.warn("PlayerView: Nie można wywołać fetchGameLog - PlayerMenu nie jest dostępne lub aktywne.");
      }
      fetchPawns();
      fetchRivalPawns();
  }
}

const handleBudgetChangeFromMenu = (newBudgetFromMenu) => {
  currentGlobalBudget.value = newBudgetFromMenu;
};

//funkcja odpowiedzialna za pokazywanie tekstu na kartach
const isOnline = ref(true)

//funckja pokazujaca aktualną planszę
const currentBoard = ref('player')

//logika rozsuwania paneli

const leftOpen = ref(false)
const rightOpen = ref(true)

function showLeftPanel() {
  rightOpen.value = false
  leftOpen.value = true
}

function showRightPanel() {
  leftOpen.value = false
  rightOpen.value = true
}

const pawns = ref([]);
const enemypawns = ref([]);

const fetchPawns= async () => {
  try {
   const response = await apiServices.get(apiConfig.player.getPawns, {
  params: {
    gameId: gameData.value.gameId,
    teamId: gameData.value.teamId,
    boardId: gameData.value.boardConfig.boardId
  }
});

    pawns.value = response.data
      .map(p => ({
        id: p.gpId,
        x: Number(p.posX),
        y: Number(p.posY),
        color: p.color,
        name: p.name
      }));
      console.log(pawns.value)

  } catch (err) {
    console.error("Błąd pobierania pionków:", err);
  }
};

const fetchRivalPawns= async () => {
  try {
   const response = await apiServices.get(apiConfig.player.getRivalPawns, {
  params: {
    gameId: gameData.value.gameId,
    boardId: gameData.value.rivalBoardConfig.boardId
  }
});

    enemypawns.value = response.data
      .map(p => ({
        id: p.teamId,
        x: Number(p.posX),
        y: Number(p.posY),
        color: p.teamColor,
        name: p.teamName

      }));
      console.log(enemypawns.value)

  } catch (err) {
    console.error("Błąd pobierania pionków:", err);
  }
};

let isSignalRConnected = false; // Flaga, aby uniknąć wielokrotnej konfiguracji

watch(() => props.teamToken, (newToken) => {
  if (newToken) {
    fetchGameDataByToken(newToken);
  }
}, { immediate: true });


onMounted(async () => {
    // Czekamy na dane gry, aby mieć gameId
    watch(gameData, async (newGameData) => {
        if (newGameData && newGameData.gameId && !isSignalRConnected) {
            isSignalRConnected = true; 
            try {
                await signalrService.start();
                await signalrService.joinGameRoom(newGameData.gameId);
                console.log(`SignalR: Połączono i dołączono do pokoju gry ${newGameData.gameId}`);

                signalrService.connection.on("BoardUpdated", async () => {
                console.log("SignalR: Otrzymano 'BoardUpdated'. Odświeżam WSZYSTKIE dane dla pewności.");
                
                if (props.teamToken) {
                    await fetchGameDataByToken(props.teamToken); 
                }
            });

                // Nasłuchuj na aktualizacje historii
                signalrService.connection.on("HistoryUpdated", () => {
                    console.log("SignalR: Otrzymano 'HistoryUpdated'. Odświeżam historię.");
                    if (playerMenuRef.value) {
                        playerMenuRef.value.fetchGameLog();
                    }
                });
                
            } catch (err) {
                console.error("Błąd połączenia SignalR w playerView: ", err);
            }
        }
    }, { immediate: true });
});

onUnmounted(() => {
    if (gameData.value && gameData.value.gameId) {
        console.log(`SignalR: Opuszczanie pokoju gry ${gameData.value.gameId}`);
        signalrService.leaveGameRoom(gameData.value.gameId);
        
        // Ważne: usuwamy listenery, aby uniknąć ich duplikacji przy ponownym wejściu na stronę
        signalrService.connection.off("BoardUpdated");
        signalrService.connection.off("HistoryUpdated");
    }
});

</script>
<style scoped>
  .fade-slide-enter-active,
  .fade-slide-leave-active {
    transition: all 0.5s ease;
  }
  .fade-slide-enter-from {
    opacity: 0;
    transform: translateX(-20px);
  }
  .fade-slide-leave-to {
    opacity: 0;
    transform: translateX(-20px);
  }
</style>