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
          <option v-for="board in data.boards" :key="board.BoardId" :value="board.BoardId">{{ board.Name }}</option>
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
  import { defineProps, defineEmits, ref, reactive } from 'vue';
  import { faXmark } from '@fortawesome/free-solid-svg-icons';

  const selectedBoardId = ref(null);
  const selectedDeckId = ref(null);
  const numberOfTeams = ref(1);
  const gameName = ref('');
  const gameDescription = ref('');
  
  // Lokalne dane plansz
  const data = reactive({
    boards: [
      {
        BoardId: 1, 
        Name: 'Plansza podstawowa', 
        LabelsUp: 'Podstawowa kordynacja;Standaryzacja procesów;Zintegrowane działania;Pełna integracja strategiczna', 
        LabelsRight: 'Nowicjusz;Naśladowca;Innowator;Lider cyfrowy', 
        DescriptionDown: 'Poziom integracji wew/zew', 
        DescriptionLeft: 'Zawansowanie Cyfrowe', 
        Rows: 8,
        Cols: 8,
        CellColor: '#fefae0', 
        BorderColor: '#595959', 
        BorderColors: '#008000;#FFFF00;#FFA500;#FF0000'
      },
      {
        BoardId: 2, 
        Name: 'Plansza zaawansowana', 
        LabelsUp: 'Początkowy;Rozwinięty;Zaawansowany;Ekspercki;Mistrzowski', 
        LabelsRight: 'Poziom 1;Poziom 22;Poziom 3;Poziom 4', 
        DescriptionDown: 'Etapy rozwoju kompetencji', 
        DescriptionLeft: 'Poziomy umiejętności', 
        Rows: 8,
        Cols: 10,
        CellColor: '#f5f5f5', 
        BorderColor: '#333333', 
        BorderColors: '#3498db;#2ecc71;#f1c40f;#e74c3c;#9b59b6'
      },
      {
        BoardId: 3, 
        Name: 'Mapa strategiczna', 
        LabelsUp: 'Mapa strategiczna;Planowanie;Implementacja;Kontrola', 
        LabelsRight: 'Strategia;Taktyka;Operacje', 
        DescriptionDown: 'Etapy zarządzania', 
        DescriptionLeft: 'Poziomy zarządzania', 
        Rows: 6,
        Cols: 8,
        CellColor: '#e0f7fa', 
        BorderColor: '#444444', 
        BorderColors: '#1abc9c;#3498db;#f39c12;#e74c3c'
      }
    ]
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
    selectedBoardId.value = null;
    selectedDeckId.value = null;
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
</script>