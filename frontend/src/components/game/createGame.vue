<template>
    <div v-if="props.isVisible" class="fixed inset-0 flex items-center justify-center z-50">
      <div class="absolute inset-0 bg-black/70" @click="closeModal"></div>
      
      <div class="bg-primary text-white rounded-lg relative z-10 border-2 border-accent p-12 animate-jump-in">
        <button 
          @click="closeModal" 
          class="absolute top-2 right-2 w-8 h-8 flex items-center justify-center"
        >
          <font-awesome-icon :icon="faXmark" class="h-5 text-white hover:text-accent transition-all duration-100" />
        </button>

        <h1 class="text-center text-white font-nasalization text-3xl mt-2 mb-4">Utwórz nową grę</h1>

        <hr class ="my-4 border-lgray-accent"/>

        <form class="mt-4">
            <input type="text" v-model="gameName" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2  w-full mb-4" placeholder="Wprowadź nazwę gry..."/>
            <textarea v-model="gameDescription" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2  w-full mb-4" placeholder="Wprowadź opis gry..." rows="4"></textarea>
            <select v-model="selectedBoardId" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2  w-full mb-4">
                <option value="" disabled selected>Wybierz planszę</option>
                <option v-for="board in boardStore.boards" :key="board.id" :value="board.id">{{ board.name }}</option>
            </select>
            <select v-model="selectedDeckId" class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 text-white w-full mb-4">
                <option value="" disabled selected>Wybierz talię kart:</option>
                <option v-for="deck in decks" :key="deck.id" :value="deck.id">{{ deck.name }}</option>
            </select>

            <label for="numberOfTeams">Wybierz liczbę drużyn:</label>
            <input
                    id="numberOfTeams"
                    class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 text-white w-full mb-4 mt-2" 
                    type="number"
                    v-model="numberOfTeams"
                    min="1"
                    max="15"
                    step="1"
                />

            <button
             
                type="button" 
                class="bg-accent border-2 border-accent py-3 px-6 rounded-md mt-5">
                <font-awesome-icon :icon="faSave" class="h-4 mr-2" />
                Stwórz grę
              </button>
        </form>

      </div>
    </div>
  </template>

<script setup>
    import { faSave } from '@fortawesome/free-solid-svg-icons';
    import { defineProps, defineEmits,onMounted,ref} from 'vue';
    import { faXmark } from '@fortawesome/free-solid-svg-icons';
    import { useBoardStore } from '@/stores/boardStore.js';

    const boardStore = useBoardStore();
    const selectedBoardId = ref('');
    const selectedDeckId = ref('');
    const numberOfTeams = ref(1);
    const gameName = ref('');
    const gameDescription = ref('');

    onMounted(async () => {
        try {
        // Pobieranie plansz przy pierwszym uruchomieniu
        if (boardStore.boards.length === 0) {
            await boardStore.fetchBoards();
        }
        } catch (error) {
        console.error('Błąd podczas inicjalizacji formularza:', error);
        }
    });
    
    const props = defineProps({
        isVisible: {
            type: Boolean,
            default: false
        }
    });

    const emits = defineEmits(['close']);

    const closeModal = () => {
        gameName.value = '';
        gameDescription.value = '';
        selectedBoardId.value = '';
        selectedDeckId.value = '';
        numberOfTeams.value = 1;
        emits('close');
    };

   const decks = [{
        id: 1,
        name: 'Talia 1',
    },
    {
        id: 2,
        name: 'Talia 2',
    },
    {
        id: 3,
        name: 'Talia 3',
    },
    {
        id: 4,
        name: 'Talia 4',
    }];

    const teams = [{
        id: 1,
        name: 'Talia A',
    },
    {
        id: 2,
        name: 'Talia B',
    },
    {
        id: 3,
        name: 'Talia C',
    },
    {
        id: 4,
        name: 'Talia D',
    }];
</script>