<template>
  <div class="flex flex-col min-h-screen bg-primary">
    <!-- Górny pasek -->
    <PlayerNavbar
      :team-name="gameData?.teamName" 
      :nav-bg-color="gameData?.teamColor"
    />

    <!-- Główna zawartość -->
    <div class="flex flex-1 mt-2">
      
      <!-- Lewa kolumna: wybór kart -->
      <div class="flex-1 ml-4 mr-2 ...">
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

      <!-- Prawa kolumna: decyzje -->
      <div class="flex-1 ml-4 bg-secondary border-2 border-lgray-accent rounded-md shadow-sm text-center p-4 w-1/3 mr-4 flex flex-col space-y-4">
        <!-- Przyciski przełączania paneli -->
        <div class="flex justify-center space-x-2">
          <button
            @click="currentPanel = 'menu'"
            :class="currentPanel === 'menu' ? 'bg-black text-white' : 'bg-white text-black'"
            class="px-4 py-1 rounded-md border"
          >
            Menu
          </button>
          <button
            @click="currentPanel = 'gameboard'"
            :class="currentPanel === 'gameboard' ? 'bg-black text-white' : 'bg-white text-black'"
            class="px-4 py-1 rounded-md border"
          >
            Twoja plansza
          </button>
          <button
            @click="currentPanel = 'panel2'"
            :class="currentPanel === 'panel2' ? 'bg-black text-white' : 'bg-white text-black'"
            class="px-4 py-1 rounded-md border"
          >
            Plansza Rywali
          </button>
        </div>

        <PlayerMenu 
         ref="playerMenuRef"
            v-if="currentPanel === 'menu' && gameData"
            :game-id="gameData?.gameId"
            :team-id="gameData?.teamId"
          @budget-changed-in-menu="handleBudgetChangeFromMenu"
        />
        <GameBoard v-else-if="currentPanel === 'gameboard'"
        :config="formData"
        :gameMode="true"
        :posX="posX"
        :posY="posY"
        :pawnColor="gameData?.teamColor" />
      </div>

    </div>

    <!-- Stopka -->
    <div class="mt-2">
      <Footer/>
    </div>
  </div>
</template>

<script setup>
import { reactive, ref, watch} from 'vue'
import PlayerNavbar from '@/components/navbars/playerNavbar.vue'
import QuestionBox from '@/components/playerComponents/questionBox.vue'
import GameBoard from '@/components/game/gameBoard.vue'
import Footer from '@/components/footers/adminFooter.vue'
import CardCarousel from '@/components/playerComponents/CardCarousel.vue'
import PlayerMenu from '@/components/playerComponents/playerMenu.vue'
import { RouterView } from 'vue-router'
import apiServices from '@/services/apiServices'
import apiConfig from '@/services/apiConfig'

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

    gameData.value = {
        ...response.data,
        boardConfig: response.data.boardConfig, 
        teamPosX: parseInt(response.data.teamPositionX, 10),
        teamPosY: parseInt(response.data.teamPositionY, 10),
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

    posX.value = gameData.value.teamPosX;
    posY.value = gameData.value.teamPosY;


    if (playerMenuRef.value && currentPanel.value === 'menu') {
      playerMenuRef.value.fetchGameLog();
    }

    

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

watch(() => props.teamToken, (newToken) => {
  if (newToken) {
    fetchGameDataByToken(newToken);
  }
}, { immediate: true });

const handleCardActionCompleted = async (eventPayload) => {
    if (eventPayload.success) {
      if (playerMenuRef.value && currentPanel.value === 'menu') {
        playerMenuRef.value.fetchGameLog();
        playerMenuRef.value.fetchTeamBud();
      } else {
        console.warn("PlayerView: Nie można wywołać fetchGameLog - PlayerMenu nie jest dostępne lub aktywne.");
      }
  }
}

const handleBudgetChangeFromMenu = (newBudgetFromMenu) => {
  currentGlobalBudget.value = newBudgetFromMenu;
};

</script>
