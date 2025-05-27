<template>
  <div class="w-full">
    <div class="grid grid-cols-1 md:grid-cols-[55fr_45fr] gap-4">

      <div class="order-2 md:order-1 flex flex-col justify-start border-2 border-lgray-accent py-6 px-4 m-4 rounded-md text-white bg-tertiary">
        <div class="flex flex-row w-full items-center justify-center gap-5 flex-shrink-0">
          <button
            class="border-2 border-lgray-accent py-2 px-2 rounded-md w-60 text-center hover:border-accent transition-colors duration-300"
            :class="{'border-accent': activeView === 'add'}"
            @click="activeView = 'add'">
            <font-awesome-icon :icon="faPlus" class="h-4 text-accent"/>
            Dodaj nową planszę
          </button>
          <button
            class="border-2 border-lgray-accent py-2 px-2 rounded-md w-60 text-center hover:border-accent transition-colors duration-300"
            :class="{'border-accent': activeView === 'edit'}"
            @click="activeView = 'edit'">
            <font-awesome-icon :icon="faPenToSquare" class="h-4 text-accent"/>
            Edytuj planszę
          </button>
        </div>

        <div class="w-full">
          <h1 class="mt-8 font-nasalization text-lg md:text-xl lg-text-2xl xl:test-3xl ">
            {{ activeView === 'add' ? 'Dodaj nową planszę' : 'Edytuj planszę' }}
          </h1>

          <boardSelector
            :boards="data.boards"
            v-model="selectedBoardId"
            :activeView="activeView"
            @delete="deleteBoard"
          />

          <form class="mt-4">
            <boardInfo
              v-model:name="formData.Name"
              :cols="formData.LabelsUp.length * 2"
              :rows="formData.LabelsRight.length * 2"
              @update="validateDescriptions"
            />

            <boardColorSettings
              v-model:cellColor="formData.CellColor"
              v-model:borderColor="formData.BorderColor"
              v-model:borderColors="formData.BorderColors"
              @update="validateDescriptions"
            />

            <boardLabelsEditors
              v-model:labelsUp="formData.LabelsUp"
              v-model:labelsRight="formData.LabelsRight"
              @update="validateDescriptions"
            />

            <boardDescriptions
              v-model:descriptionDown="formData.DescriptionDown"
              v-model:descriptionLeft="formData.DescriptionLeft"
              @update="validateDescriptions"
            />

            <button
              type="button"
              class="bg-accent border-2 border-accent py-3 px-6 rounded-md mt-5 hover:bg-opacity-80 transition-all"
              @click="saveBoard">
              <font-awesome-icon :icon="faSave" class="h-4 mr-2" />
              {{ activeView === 'add' ? 'Dodaj planszę' : 'Zapisz zmiany' }}
            </button>
          </form>
        </div>
      </div>

      <div class="order-1 md:order-2 border-2 border-lgray-accent py-6 px-8 m-4 rounded-md text-white bg-tertiary flex flex-col
                  md:sticky md:top-4 self-start md:max-h-[calc(100vh-2rem)]">
        
        <h2 class="text-xl mb-4 text-center flex-shrink-0">Podgląd planszy</h2>

        <div class="relative flex-grow min-h-0">
          <myBoard
            :config="previewConfig"
          />
        </div>
      </div>

    </div>
  </div>
</template>


<script setup>
import { faPlus, faPenToSquare, faSave } from '@fortawesome/free-solid-svg-icons';
import { ref, reactive, computed, onMounted,watch } from 'vue';
import { useToast } from 'vue-toastification';
import myBoard from '@/components/game/gameBoard.vue';
import boardSelector from '@/components/editBoard/boardSelector.vue';
import boardInfo from '@/components/editBoard/boardInfo.vue';
import boardColorSettings from '@/components/editBoard/boardColorSettings.vue';
import boardLabelsEditors from '@/components/editBoard/boardLabelsEditors.vue';
import boardDescriptions from '@/components/editBoard/boardDescriptions.vue';

const selectedBoardId = ref(null);
const activeView = ref('add');
const toast = useToast();

watch(selectedBoardId, async (id) => {
  if (id) await loadSelectedBoard();
});

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

const getDefaultFormData = () => ({
  Name: 'Nowa plansza', 
  LabelsUp: ['Etykieta 1', 'Etykieta 2', 'Etykieta 3', 'Etykieta 4'], 
  LabelsRight: ['Etykieta A', 'Etykieta B', 'Etykieta C', 'Etykieta D'], 
  DescriptionDown: 'Opis dolny', 
  DescriptionLeft: 'Opis lewy', 
  Rows: 8,
  Cols: 8,
  CellColor: '#ffffff', 
  BorderColor: '#000000', 
  BorderColors: ['#008000', '#FFFF00', '#FFA500', '#FF0000']
});

const formData = reactive(getDefaultFormData());

const stringToArray = (str) => {
  if (!str) return [];
  return str.split(';').map(item => item.trim());
};

const arrayToString = (arr) => {
  if (!arr || !Array.isArray(arr)) return '';
  return arr.join(';');
};

const resetForm = () => {
  Object.assign(formData, getDefaultFormData());
  selectedBoardId.value = null;
};

const loadSelectedBoard = async () => {
  if (!selectedBoardId.value) return;
  try {
    const selectedBoard = data.boards.find(board => board.BoardId === selectedBoardId.value);
    if (!selectedBoard) {
      toast.error('Nie znaleziono wybranej planszy');
      return;
    }
    
    formData.Name = selectedBoard.Name;
    formData.LabelsUp = stringToArray(selectedBoard.LabelsUp);
    formData.LabelsRight = stringToArray(selectedBoard.LabelsRight);
    formData.DescriptionDown = selectedBoard.DescriptionDown;
    formData.DescriptionLeft = selectedBoard.DescriptionLeft;
    formData.Rows = selectedBoard.Rows;
    formData.Cols = selectedBoard.Cols;
    formData.CellColor = selectedBoard.CellColor;
    formData.BorderColor = selectedBoard.BorderColor;
    formData.BorderColors = stringToArray(selectedBoard.BorderColors);
    
    toast.success(`Załadowano planszę: ${selectedBoard.Name}`);
  } catch (error) {
    console.error('Błąd podczas ładowania planszy:', error);
    toast.error(`Wystąpił błąd: ${error.message}`);
  }
};

const previewConfig = computed(() => {
  const cols = (formData.LabelsUp?.length || 0) * 2;
  const rows = (formData.LabelsRight?.length || 0) * 2;
  
  return {
    ...formData,
    Rows: rows,
    Cols: cols,
  };
});

const validateDescriptions = () => {
  if (!formData.DescriptionDown?.trim()) {
    formData.DescriptionDown = 'Opis dolny';
    toast.warning("Opis dolny nie może być pusty. Ustawiono wartość domyślną.");
  }
  if (!formData.DescriptionLeft?.trim()) {
    formData.DescriptionLeft = 'Opis lewy';
    toast.warning("Opis lewy nie może być pusty. Ustawiono wartość domyślną.");
  }
};

const saveBoard = async () => {
  try {
    if (!formData.Name.trim()) {
      toast.error('Nazwa planszy jest wymagana!');
      return;
    }
    if (formData.LabelsUp.some(label => !label.trim()) || 
        formData.LabelsRight.some(label => !label.trim())) {
      toast.error('Wszystkie etykiety muszą być wypełnione!');
      return;
    }
    validateDescriptions();
    
    const boardData = {
      ...formData,
      LabelsUp: arrayToString(formData.LabelsUp),
      LabelsRight: arrayToString(formData.LabelsRight),
      BorderColors: arrayToString(formData.BorderColors)
    };
    
    if (activeView.value === 'add') {
      const maxId = data.boards.reduce((max, board) => Math.max(max, board.BoardId), 0);
      const newBoard = { BoardId: maxId + 1, ...boardData };
      data.boards.push(newBoard);
      toast.success('Plansza została dodana pomyślnie!');
      resetForm();
    } else {
      if (!selectedBoardId.value) {
        toast.warning('Wybierz planszę do edycji!');
        return;
      }
      const boardIndex = data.boards.findIndex(board => board.BoardId === selectedBoardId.value);
      if (boardIndex === -1) {
        toast.error('Nie znaleziono planszy do aktualizacji!');
        return;
      }
      data.boards[boardIndex] = { ...data.boards[boardIndex], ...boardData };
      toast.success('Plansza została zaktualizowana pomyślnie!');
    }
  } catch (error) {
    toast.error(`Wystąpił błąd: ${error.message}`);
    console.error('Błąd podczas zapisywania planszy:', error);
  }
};

const deleteBoard = async () => {
  if (!selectedBoardId.value) {
    toast.warning('Nie wybrano planszy do usunięcia!');
    return;
  }
  
  if (confirm(`Czy na pewno chcesz usunąć planszę "${formData.Name}"? Tej operacji nie można cofnąć.`)) {
    try {
      const boardIndex = data.boards.findIndex(board => board.BoardId === selectedBoardId.value);
      if (boardIndex === -1) {
        toast.error('Nie znaleziono planszy do usunięcia!');
        return;
      }
      data.boards.splice(boardIndex, 1);
      toast.success('Plansza została usunięta pomyślnie!');
      resetForm();
      if (data.boards.length === 0) {
        activeView.value = 'add';
        toast.info('Wszystkie plansze zostały usunięte. Dodaj nową planszę.');
      }
    } catch (error) {
      toast.error(`Błąd podczas usuwania planszy: ${error.message}`);
    }
  }
};

onMounted(() => {
  resetForm();
});

watch(activeView, (newView) => {
  if (newView === 'add') {
    resetForm();
  } else if (newView === 'edit') {
    selectedBoardId.value = null; 
    if (data.boards.length === 0) {
      toast.warning('Brak dostępnych plansz do edycji');
      activeView.value = 'add';
    } else {
      Object.assign(formData, getDefaultFormData());
      formData.Name = '';
    }
  }
});
</script>