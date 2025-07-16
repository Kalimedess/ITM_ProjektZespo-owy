<template>
  <div v-if="props.isVisible" class="fixed inset-0 flex items-center justify-center z-50">
    <div class="absolute inset-0 bg-black/70" @click="closeModal"></div>
    
    <div class="bg-primary text-white relative z-10 border-2 border-accent animate-jump-in
                w-full h-full p-4 overflow-y-auto custom-scrollbar
                sm:w-full sm:max-w-lg sm:h-auto sm:max-h-[90vh] sm:rounded-lg sm:p-6 md:p-8 lg:p-10">
      <button 
        @click="closeModal" 
        class="absolute top-3 right-3 sm:top-2 sm:right-2 w-8 h-8 z-10"
      >
        <font-awesome-icon :icon="faXmark" class="h-5 text-white hover:text-accent transition-all duration-100" />
      </button>

      <h1 class="text-center text-white font-nasalization text-lg sm:text-xl md:text-2xl mt-1 mb-3">Utwórz nową grę</h1>

      <hr class="my-3 border-lgray-accent"/>

      <div class="flex flex-row justify-center space-x-2">
        <div class="rounded-full bg-accent h-3 w-3"></div>
        <div class="rounded-full h-3 w-3" :class="step >= 2 ? 'bg-accent' : 'bg-tertiary'"></div>
        <div class="rounded-full h-3 w-3" :class="step === 3 ? 'bg-accent' : 'bg-tertiary'"></div>
      </div>

      <!--Krok 1-->
      <form class="mt-3" @submit.prevent="handleSubmit">
        <div v-if="step === 1" :class="direction === 'backwards' ? 'animate-fade-left' : ''">

          <!--Nazwa Gry-->
          <div class="space-y-1 mb-1 sm:mb-2">
            <label for="gameName" class="block font-bold text-left text-xs sm:text-sm">Nazwa Gry</label>
            <input 
              type="text"
              maxlength="25" 
              v-model="gameName" 
              id="gameName" 
              class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4" 
              placeholder="Wprowadź nazwę gry..."
              required
            />
          </div>
          
          <!--Wybór Planszy Stołu-->
          <div class="mb-1 sm:mb-2">
            <label for="selectBoard" class="block font-bold text-left text-xs mb-1">Wybierz planszę</label>
            <select v-model="selectedBoardId" id="selectBoard" required class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
              <option value="" disabled>Wybierz planszę</option>
              <option v-for="board in data.boards" :key="board.boardId" :value="board.boardId">{{ board.name }}</option>
            </select>
          </div>

          <!-- Wybór Plansza konkurencji nw czy to będzie pobieranie z osobnej tabelii czy wybory jak przy wyborze planszy-->
           <div class="mb-1 sm:mb-2">
            <label for="selectOpponentBoard" class="block font-bold text-left text-xs mb-1">Wybierz planszę konkurencji</label>
            <select v-model="selectedOponentBoardId" id="selectOpponentBoard" required class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
              <option value="" disabled>Wybierz planszę konkurencji</option>
              <option v-for="board in data.boards" :key="board.boardId" :value="board.boardId">{{ board.name }}</option>
            </select>
          </div>
          

          <!--Wybór talii kart-->
          <div class="mb-1 sm:mb-2">
            <label for="selectDeck" class="block font-bold text-left text-xs mb-1">Wybierz talię kart</label>
            <select v-model="selectedDeckId" required id="selectDeck" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
              <option value="" disabled>Wybierz talię kart</option>
              <option v-for="deck in data.decks" :key="deck.id" :value="deck.id">{{ deck.title }}</option>
            </select>
          </div>

          <!--Wybór trybu rozgrywki-->
          <p class="block font-bold text-left text-xs mb-2">Wybierz rodzaj rozgrywki</p>
          <div class="flex gap-2 w-full mb-5">
              <div class="flex flex-col items-center justify-center rounded-md w-full h-20 gap-2 cursor-pointer"
              :class="selectedGameMode === 'remote' ? 'bg-accent shadow-md shadow-accent/60' : 'bg-tertiary transition delay-150 duration-300 ease-in-out hover:-translate-y-1 hover:scale-105 hover:bg-accent/70 border-2 border-lgray-accent'"
              @click="selectedGameMode = 'remote'">
                <font-awesome-icon :icon="faGlobe" class="h-6" :class="selectedGameMode === 'remote' ?  'text-tertiary' : 'text-accent' "/>
                <h2 class=" block font-nasalization font-semibold">Gra zdalna</h2>
              </div>
              <div class=" flex flex-col items-center justify-center rounded-md w-full h-20 gap-2 cursor-pointer"
              :class="selectedGameMode === 'stationary' ? 'bg-accent shadow-md shadow-accent/60' : 'bg-tertiary  transition delay-150 duration-300 ease-in-out hover:-translate-y-1 hover:scale-105 hover:bg-accent/70 border-2 border-lgray-accent'"
              @click="selectedGameMode = 'stationary'">
                <font-awesome-icon :icon="faBuilding" class="h-6" :class="selectedGameMode === 'stationary' ?  'text-primary' : 'text-accent' "/>
                <h2 class=" block font-nasalization font-semibold">Gra stacjonarna</h2>
              </div>
          </div>

          <button 
            @click="handleNextStep"
            type="button" 
            class="bg-tertiary hover:bg-accent text-white w-full py-2 xl:py-4 rounded-lg font-medium 
            transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60 mb-5"
          >
            <div class="flex items-center justify-center gap-2">
              <p class="mr-2">Dalej</p>
              <font-awesome-icon :icon="faArrowRight" class="h-4 text-center" />
            </div>
          </button>
        </div>
        
        <!--Krok 2-->
        <div v-if="step === 2" :class="direction === 'forwards' ? 'animate-fade-right' : 'animate-fade-left'"> 
          <div class="flex flex-row gap-2">
            <!--Wybór liczby drużyn -->
            <div class="flex-1">
              <label for="numberOfTeams" class="block font-bold text-left text-xs sm:text-sm mb-1">Wybierz liczbę drużyn:</label>
              <input
                id="numberOfTeams"
                class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 text-white w-full mb-4" 
                type="number"
                v-model="numberOfTeams"
                min="1"
                max="15"
                step="1">
            </div>

             <!--Wybór liczby bitów na start -->
             <div>
                <label for="numberOfBits" class="block font-bold text-left text-xs sm:text-sm mb-1" >Wybierz liczbę bitów na start</label>
                <input 
                  type="number" 
                  id="numberOfBits"
                  class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 text-white w-full mb-4 [appearance:textfield] [&::-webkit-outer-spin-button]:appearance-none [&::-webkit-inner-spin-button]:appearance-none" 
                  v-model="numberOfBits"
                  min="1"
                  max="100000"
                >
             </div>


          </div>
          <div class="mb-6">
            <label class="block text-left text-xs sm:text-sm font-bold text-white mb-2">Wybierz drużynę do edycji:</label>
            <DropDown
              :items="teams"
              v-model="currentlyEditingTeamId"
              item-key="id"
              item-label="name"
              placeholder="Wybierz drużynę"
            >
              <template #selected-display="{ selected }">
                <div v-if="selected" class="flex items-center gap-3">
                  <div class="w-4 h-4 rounded-full" :style="{ backgroundColor: selected.colour }"></div>
                  <span>{{ selected.name }}</span>
                </div>
                <span v-else>Wybierz drużynę</span>
              </template>
              
              <template #item-display="{ item }">
                <div class="flex items-center gap-3">
                  <div class="w-4 h-4 rounded-full" :style="{ backgroundColor: item.colour }"></div>
                  <span>{{ item.name }}</span>
                </div>
              </template>
            </DropDown>
          </div>

          <!--Drużyny-->
          <div v-if="selectedTeam" class="p-4 rounded-lg bg-tertiary border border-lgray-accent mb-4">
            <h3 class="font-bold text-lg mb-4 text-white">Edytujesz: <span class="text-accent">{{ selectedTeam.name }}</span></h3>
            
            <div class="space-y-4">
              <div>
                <label :for="'editTeamName-' + selectedTeam.id" class="block text-sm font-medium text-gray-300 mb-1">Nazwa drużyny</label>
                <input
                  :id="'editTeamName-' + selectedTeam.id"
                  type="text"
                  v-model="selectedTeam.name"
                  class="w-full bg-primary border-2 border-lgray-accent rounded-md px-3 py-2 text-white"
                />
              </div>
              
              <div>
                <label :for="'editTeamColor-' + selectedTeam.id" class="block text-sm font-medium text-gray-300 mb-1">Kolor drużyny</label>
                <div class="flex items-center gap-2">
                  <input 
                    :id="'editTeamColor-' + selectedTeam.id"
                    type="color" 
                    v-model="selectedTeam.colour"
                    class="w-20 h-12 p-0 bg-transparent  rounded-md cursor-pointer"
                  />
                  <div class="w-full text-left px-3 py-2 rounded-md bg-primary border border-lgray-accent">{{ selectedTeam.colour }}</div>
                </div>
              </div>
            </div>
            
              <label 
              :for="'decision-' + selectedTeam.id" 
              class="block text-sm font-medium text-gray-300 mb-2 mt-5 cursor-pointer"
            >
              Czy drużyna może podejmować samodzielne decyzje?
            </label>

            <!--Switch do wyboru czy drużyna może podejmować samodzielnie decyzje-->
            <div class="flex items-center gap-2">
              <label :for="'decision-' + selectedTeam.id" :class="[
                'relative w-16 h-8 rounded-full cursor-pointer block transition-colors duration-300',
                selectedTeam.isAbleToMakeDecisions ? 'bg-accent' : 'bg-primary'
              ]">
                <input 
                  type="checkbox" 
                  :id="'decision-' + selectedTeam.id"
                  class="sr-only"
                  v-model="selectedTeam.isAbleToMakeDecisions"
                >
                <span class="w-6 h-6 bg-white absolute left-1 top-1 rounded-full transition-transform duration-300"
                      :class="{ 'translate-x-8': selectedTeam.isAbleToMakeDecisions }"></span>
              </label>
              <span class="text-sm">{{ selectedTeam.isAbleToMakeDecisions ? 'Samodzielne decyzje' : 'Kontrola Game Mastera' }}</span>
              <div class="relative">
                <font-awesome-icon 
                :icon="faCircleQuestion" class=" text-accent h-4 cursor-pointer"
                @mouseover="showTip = true "
                @mouseleave="showTip = false "
                />
                <div class="absolute border border-accent rounded-md bottom-full left-1/2 -translate-x-1/2 mb-2 
                          bg-primary p-3 text-white text-xs z-50 w-72
                            flex items-center"
                            v-show="showTip"
                            >
                  <div>
                      <div>
                        <h2 class=" font-nasalization mb-1 font-semibold text-orange-500">Kontrola GM'a</h2>
                        <span>Drużyna ma możliwość zasugerowania decyzji ale Game Master musi ją zakceptować </span>
                      </div>
                      <hr class="mt-2  border-accent">
                      <div>
                        <h2 class=" font-nasalization mb-1 mt-2 font-semibold text-green-500">Samodziene dezyje</h2>
                        <span>Drużyna podejmuje decyzje bezpośrednio z urządzenie i nie potrzebuję akcpetacji decyzji przez Game Mastera</span>
                      </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="flex gap-2">
            <button 
              @click="handlePreviousStep"
              type="button" 
              class="bg-tertiary hover:bg-accent text-white w-full py-2 xl:py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60"
            >
            <div class="flex items-center justify-center gap-2">
                <font-awesome-icon :icon="faArrowLeft" class="h-4 text-center" />
                <p class="ml-2">Wstecz</p>
            </div>
            </button>
            <button 
              @click="handleNextStep"
              type="button" 
              class="bg-tertiary hover:bg-accent text-white w-full py-2 xl:py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60"
            >
              <div class="flex items-center justify-center gap-2">
                <font-awesome-icon :icon="faArrowRight" class="h-4 text-center" />
                <p class="ml-2">Dalej</p>
              </div>
            </button>
          </div>
        </div>

      <!--Krok 3-->
      <div v-if="step === 3" :class="direction === 'forwards' ? 'animate-fade-right' : 'animate-fade-left'">
      <div class="mb-6">
        <label for="selectProcess" class="block text-left text-xs sm:text-sm font-bold text-white mb-2">
          Wybierz proces do edycji:
        </label>
        <select 
          v-model="selectedProcessId" 
          id="selectProcess" 
          class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4"
        >
          <option value="" disabled>Wybierz proces</option>
          <option v-for="process in processes" :key="process.id" :value="process.id">
            {{ process.shortName }}
          </option>
        </select>
      </div>

      <!-- Edycja wybranego procesu -->
        <div v-if="selectedProcess" class="p-4 rounded-lg bg-tertiary border border-lgray-accent mb-4">
          <h3 class="font-bold text-lg mb-4 text-white">
            Edytujesz: <span class="text-accent">{{ selectedProcess.shortName }}</span>
          </h3>
          
          <div class="space-y-4">
            <div>
              <label :for="'editProcessShort-' + selectedProcess.id" class="block text-sm font-medium text-gray-300 mb-1">
                Skrót procesu
              </label>
              <input
                :id="'editProcessShort-' + selectedProcess.id"
                type="text"
                maxlength="7"
                v-model="selectedProcess.shortName"
                class="w-full bg-primary border-2 border-lgray-accent rounded-md px-3 py-2 text-white"
              />
            </div>
            
            <div>
              <label :for="'editProcessFull-' + selectedProcess.id" class="block text-sm font-medium text-gray-300 mb-1">
                Pełna nazwa procesu
              </label>
              <input
                :id="'editProcessFull-' + selectedProcess.id"
                type="text"
                v-model="selectedProcess.fullName"
                maxlength="20"
                class="w-full bg-primary border-2 border-lgray-accent rounded-md px-3 py-2 text-white"
              />
            </div>
          </div>
        </div>

        <div class="flex gap-2">
          <button 
            @click="handlePreviousStep"
            type="button" 
            class="bg-tertiary hover:bg-accent text-white w-full py-2 xl:py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60"
          >
            <div class="flex items-center justify-center gap-2">
              <font-awesome-icon :icon="faArrowLeft" class="h-4 text-center" />
              <p class="ml-2">Wstecz</p>
            </div>
          </button>

          <button 
            type="submit" 
            class="bg-tertiary hover:bg-accent text-white w-full py-2 xl:py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60"
          >
            <div class="flex items-center justify-center gap-2">
              <p>Utwórz nową grę</p>
            </div>
          </button>
        </div>
      </div>
      </form>
    </div>
  </div>
</template>


<script setup>
  import { faArrowRight,faArrowLeft,faXmark, faGlobe, faBuilding } from '@fortawesome/free-solid-svg-icons';
  import { defineProps, defineEmits, ref, reactive, watch, computed, onMounted } from 'vue';
  import { faCircleQuestion } from '@fortawesome/free-regular-svg-icons';
  import { useToast } from 'vue-toastification';
  import DropDown from '../dropDown.vue';

  import apiConfig from '@/services/apiConfig.js';
  import apiService from '@/services/apiServices.js';


  const toast = useToast();

  const selectedBoardId = ref(null);
  const selectedOponentBoardId = ref(null);
  const selectedDeckId = ref(null);
  const selectedGameMode = ref('stationary');
  const numberOfTeams = ref(1);
  const gameName = ref('');
  const numberOfBits = ref(1);
  const step = ref(1);
  const direction = ref('');
  const currentlyEditingTeamId = ref(0);
  const showTip = ref(false);
  const selectedProcessId = ref(null);

  const teams = ref([]);

  const processes = ref([
    { id: 1, shortName: 'O2C', fullName: 'Order to Cash' },
    { id: 2, shortName: 'P2V', fullName: 'Purchase to Validate' },
    { id: 3, shortName: 'M2S', fullName: 'Manufacturing to Supply' },
    { id: 4, shortName: 'OPP', fullName: 'Operational Production Planning' },
    { id: 5, shortName: 'MM', fullName: 'Material Management' }
  ]);

  //Domyślne kolory drużyn
  const defaultColors = [
      '#8B0000',
      '#2D1B69', 
      '#1B4332',
      '#A4133C',
      '#7209B7',
      '#6A994E',
      '#2B2D42',
      '#CC6600',
      '#B8860B',
      '#008B8B',
      '#8B008B',
      '#556B2F',
      '#722F37',
      '#4A4A4A',
      '#36454F'
  ];

  const selectedTeam = computed(() => {
    if (currentlyEditingTeamId.value === null) return null;
    return teams.value.find(team => team.id === currentlyEditingTeamId.value);
  });

  const selectedProcess = computed(() => {
    if (selectedProcessId.value === null) return null;
    return processes.value.find(process => process.id === selectedProcessId.value);
  });

  const updateTeamsArray = (count) => {
    const newTeams = [];
    for (let i = 0; i < count; i++) {
      const existingTeam = teams.value.find(t => t.id === i);
      newTeams.push({
        id: i, 
        name: existingTeam?.name || `Drużyna ${i + 1}`,
        colour: existingTeam?.colour || defaultColors[i % defaultColors.length],
        isAbleToMakeDecisions: existingTeam?.isAbleToMakeDecisions ?? false
      });
    }
    teams.value = newTeams;
    if (!teams.value.some(team => team.id === currentlyEditingTeamId.value)) {
      currentlyEditingTeamId.value = 0;
    }
  };

  watch(numberOfTeams, (newCount) => {
    const count = Math.max(1, Math.min(15, newCount || 1));
    updateTeamsArray(count);
  }, { immediate: true });


  const handleNextStep = () => {
    let result = true;

    if (step.value === 1) {
       result = validateFirstStep();
    }

    if(result){
      step.value += 1;
      direction.value = 'forwards';
    }

  }

  const handlePreviousStep = () => {
    step.value -= 1;
    direction.value = 'backwards';
  }

  const validateFirstStep = () => {
    const errors = [];
    
    if (!selectedBoardId.value) {
      errors.push('Wybierz planszę');
    }
    if (!selectedOponentBoardId.value) {
      errors.push('Wybierz planszę konkurencji');
    }
    if (!selectedDeckId.value) {
      errors.push('Wybierz talię kart');
    }
    if (!gameName.value?.trim()) {
      errors.push('Wprowadź nazwę gry');
    }
 

    if (errors.length > 0) {
      toast.error(errors.join('\n'), {
        position: 'top-center',
      });
      return false;
    }

    return true;
  }
  const data = reactive({
    boards: [],
    decks:[]
  });

  const fetchBoardsFromAPI = async () => {
    try {
      const response = await apiService.get(apiConfig.boards.getAll);
      data.boards = response.data;
      if (data.boards.length === 0) {
          toast.info("Brak plansz do wyboru!");
      }
    } catch (error) {
      console.error('Błąd pobierania plansz:', error.response?.data || error.message);
      toast.error(`Nie udało się pobrać plansz: ${error.response?.data?.title || error.message}`);
    }
  };

  const fetchDecksFromAPI = async () => {
    try {
      const response = await apiService.get(apiConfig.admin.deck.getAll);
      data.decks = response.data;
      if (data.decks.length === 0) {
          toast.info("Brak talii kart do wyboru!");
      }
    } catch (error) {
      console.error('Błąd pobierania talii kart:', error.response?.data || error.message);
      toast.error(`Nie udało się pobrać talii kart: ${error.response?.data?.title || error.message}`);
    }
  };
  
  const props = defineProps({
    isVisible: {
      type: Boolean,
      default: false
    }
  });

  const emits = defineEmits(['close', 'gameCreated']);

  const closeModal = () => {
    gameName.value = '';
    selectedBoardId.value = null;
    selectedOponentBoardId.value =  null;
    selectedDeckId.value = null;
    numberOfTeams.value = 1;
    step.value = 1;
    numberOfBits.value = 1;
    selectedProcessId.value = null;

    processes.value = [
      { id: 1, shortName: 'O2C', fullName: 'Order to Cash' },
      { id: 2, shortName: 'P2V', fullName: 'Purchase to Validate' },
      { id: 3, shortName: 'M2S', fullName: 'Manufacturing to Supply' },
      { id: 4, shortName: 'OPP', fullName: 'Operational Production Planning' },
      { id: 5, shortName: 'MM', fullName: 'Material Management' }
    ];

    emits('close');
    emits('gameCreated');
  };

  const handleSubmit = async () => {
    if (step.value === 1) {
        handleNextStep();
        return;
    }

    const teamNameErrors = teams.value.filter(team => !team.name.trim());
    if (teamNameErrors.length > 0) {
        toast.error(`Nazwy drużyn nie mogą być puste (Drużyna ${teamNameErrors.map(t => t.id + 1).join(', ')})`);
        return;
    }

    if (numberOfBits.value < 1 || numberOfBits.value > 100000) {
        toast.error("Liczba bitów na start musi być pomiędzy 1 a 100000.");
        return;
    }



    //Sprawdź czy tu payload jest okej wszystkie zmienne itp 
    const gamePayload = {
      GameName: gameName.value,
      BoardId: selectedBoardId.value,
      RivalBoardId: selectedOponentBoardId.value,
      DeckId: selectedDeckId.value,
      GameMode: selectedGameMode.value === 'stationary' ? false : true, // Rodzaj rozgrywki zdalna / stacjonarna
      NumberOfTeams: numberOfTeams.value,
      StartBits: Number(numberOfBits.value),
      Teams: teams.value.map(team => ({
        Name: team.name,
        Colour: team.colour,
        IsAbleToMakeDecisions: team.isAbleToMakeDecisions // To jest zmienna: podejmowania decyzji z tableta lub sugestii kolejnego działania
      })),
      //Procesy
      Process: processes.value.map(process => ({
          Name: process.fullName,
          ShortName: process.shortName,
      }))

    };

    try {
      const response = await apiService.post(apiConfig.games.create, gamePayload);

      emits('gameCreated')

      toast.success(response.data.message || `Gra "${gamePayload.GameName}" utworzona pomyślnie!`);

      closeModal();
    } catch (error) {
      console.error('Błąd tworzenia gry:', error.response?.data || error.message, error);
      let errorMessage = "Nie udało się utworzyć gry.";
      if (error.response && error.response.data) {
          if (typeof error.response.data === 'string') {
              errorMessage = error.response.data;
          } else if (error.response.data.title) {
              errorMessage = error.response.data.title;
          } else if (error.response.data.errors) {
              const errors = Object.values(error.response.data.errors).flat();
              errorMessage = errors.join('\n');
          } else if (error.response.data.message) {
              errorMessage = error.response.data.message;
          }
      } else if (error.message) {
          errorMessage = error.message;
      }
      toast.error(errorMessage);
    }
  };

  onMounted(async () => {
  await fetchBoardsFromAPI();
  await fetchDecksFromAPI();
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