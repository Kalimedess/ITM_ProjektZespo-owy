<template>
  <div v-if="props.isVisible" class="fixed inset-0 flex items-center justify-center z-50">
    <div class="absolute inset-0 bg-black/70" @click="closeModal"></div>
    
    <div class="bg-primary text-white rounded-lg relative z-10 border-2 border-accent p-6 sm:p-8 md:p-10 lg:p-12 max-h-[90vh] w-full max-w-lg animate-jump-in">
      <button 
        @click="closeModal" 
        class="absolute top-2 right-2 w-8 h-8"
      >
        <font-awesome-icon :icon="faXmark" class="h-5 text-white hover:text-accent transition-all duration-100" />
      </button>

      <h1 class="text-center text-white font-nasalization text-lg sm:text-xl md:text-2xl mt-1 mb-3">Utwórz nową grę</h1>

      <hr class="my-3 border-lgray-accent"/>

      <div class="flex flex-row justify-center space-x-2">
        <div class="rounded-full bg-accent h-3 w-3"></div>
        <div class="rounded-full h-3 w-3" :class="step === 2 ? 'bg-accent' : 'bg-tertiary'"></div>
      </div>

      <!--Krok 1-->
      <form class="mt-3" @submit.prevent="handleSubmit">
        <div v-if="step === 1" :class="direction === 'backwards' ? 'animate-fade-left' : ''">
          <div class="space-y-1 mb-1 sm:mb-2">
            <label for="gameName" class="block font-bold text-left text-xs sm:text-sm">Nazwa Gry</label>
            <input 
              type="text" 
              v-model="gameName" 
              id="gameName" 
              class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4" 
              placeholder="Wprowadź nazwę gry..."
              required
            />
          </div>
          
          <div class="space-y-1 mb-1 sm:mb-2">
            <label for="gameDescription" class="block font-bold text-left text-xs sm:text-sm">Opis Gry</label>
            <textarea 
              v-model="gameDescription" 
              id="gameDescription" 
              class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4" 
              placeholder="Wprowadź opis gry..." 
              rows="4"
              required
            ></textarea>
          </div>
        
          <div class="mb-1 sm:mb-2">
            <label for="selectBoard" class="block font-bold text-left text-xs mb-1">Wybierz planszę</label>
            <select v-model="selectedBoardId" id="selectBoard" required class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
              <option value="" disabled>Wybierz planszę</option>
              <option v-for="board in data.boards" :key="board.boardId" :value="board.boardId">{{ board.name }}</option>
            </select>
          </div>

          <div class="mb-1 sm:mb-2">
            <label for="selectDeck" class="block font-bold text-left text-xs mb-1">Wybierz talię kart</label>
            <select v-model="selectedDeckId" required id="selectDeck" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 w-full mb-4">
              <option value="" disabled>Wybierz talię kart</option>
              <option v-for="deck in data.decks" :key="deck.id" :value="deck.id">{{ deck.title }}</option>
            </select>
          </div>

          <button 
            @click="handleNextStep"
            type="button" 
            class="bg-tertiary hover:bg-accent text-white w-full py-2 xl:py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60 mb-5"
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
                <div class="flex items-center gap-4">
                  <input 
                    :id="'editTeamColor-' + selectedTeam.id"
                    type="color" 
                    v-model="selectedTeam.colour"
                    class="w-12 h-12 p-0 bg-transparent border-none rounded-md cursor-pointer"
                  />
                  <div class="w-full text-left px-3 py-2 rounded-md bg-primary border border-lgray-accent">{{ selectedTeam.colour }}</div>
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
  import { faArrowRight,faArrowLeft } from '@fortawesome/free-solid-svg-icons';
  import { defineProps, defineEmits, ref, reactive, watch, computed, onMounted } from 'vue';
  import { faXmark } from '@fortawesome/free-solid-svg-icons';
  import { useToast } from 'vue-toastification';
  import DropDown from '../dropDown.vue';
  import apiClient from '@/assets/plugins/axios';

  const toast = useToast();

  const selectedBoardId = ref(null);
  const selectedDeckId = ref(null);
  const numberOfTeams = ref(1);
  const gameName = ref('');
  const gameDescription = ref('');
  const numberOfBits = ref(1);
  const step = ref(1);
  const direction = ref('');
  const currentlyEditingTeamId = ref(0);

  const teams = ref([]);

  const defaultColors = [
    '#E63946', '#F1FAEE', '#A8DADC', '#457B9D', '#1D3557', '#F4A261',
    '#2A9D8F', '#E9C46A', '#264653', '#E76F51', '#8ECAE6', '#FB8500'
  ];

  const selectedTeam = computed(() => {
    if (currentlyEditingTeamId.value === null) return null;
    return teams.value.find(team => team.id === currentlyEditingTeamId.value);
  });

    const updateTeamsArray = (count) => {
    const newTeams = [];
    for (let i = 0; i < count; i++) {
      const existingTeam = teams.value.find(t => t.id === i);
      newTeams.push({
        id: i, 
        name: existingTeam?.name || `Drużyna ${i + 1}`,
        colour: existingTeam?.colour || defaultColors[i % defaultColors.length]});}teams.value = newTeams;
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
    if (!selectedDeckId.value) {
      errors.push('Wybierz talię kart');
    }
    if (!gameName.value?.trim()) {
      errors.push('Wprowadź nazwę gry');
    }
    if (!gameDescription.value?.trim()) {
      errors.push('Wprowadź opis gry');
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
      const response = await apiClient.get(`/api/Board/get`, { withCredentials: true });
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
      const response = await apiClient.get(`api/deck/edit`, { withCredentials: true });
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
    gameDescription.value = '';
    selectedBoardId.value = null;
    selectedDeckId.value = null;
    numberOfTeams.value = 1;
    step.value = 1;
    numberOfBits.value = 1;
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

    const gamePayload = {
      GameName: gameName.value,
      GameDescription: gameDescription.value,
      BoardId: selectedBoardId.value,
      DeckId: selectedDeckId.value,
      NumberOfTeams: numberOfTeams.value,
      StartBits: Number(numberOfBits.value),
      Teams: teams.value.map(team => ({
        Name: team.name,
        Colour: team.colour 
      }))
    };

    try {
      const response = await apiClient.post(`/api/games/create`, gamePayload, { withCredentials: true });

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