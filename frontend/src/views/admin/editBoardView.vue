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
import { ref, reactive, computed, onMounted, watch } from 'vue';
import { useToast } from 'vue-toastification';
import myBoard from '@/components/game/gameBoard.vue'; // Załóżmy, że te są potrzebne
import boardSelector from '@/components/editBoard/boardSelector.vue';
import boardInfo from '@/components/editBoard/boardInfo.vue';
import boardColorSettings from '@/components/editBoard/boardColorSettings.vue';
import boardLabelsEditors from '@/components/editBoard/boardLabelsEditors.vue';
import boardDescriptions from '@/components/editBoard/boardDescriptions.vue';
import apiClient from '@/assets/plugins/axios';

const selectedBoardId = ref(null);
const activeView = ref('add');
const toast = useToast();

const data = reactive({
  boards: []
});

const getDefaultFormData = () => ({
  BoardId: 0,
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

watch(() => formData.LabelsUp, (newLabels) => {
  if (Array.isArray(newLabels)) {
    formData.Cols = newLabels.length * 2;
  }
}, { deep: true }); 

watch(() => formData.LabelsRight, (newLabels) => {
  if (Array.isArray(newLabels)) {
    formData.Rows = newLabels.length * 2;
  }
}, { deep: true });


const stringToArray = (str) => {
  if (typeof str !== 'string' || !str) return [];
  return str.split(';').map(item => item.trim()).filter(item => item);
};

const arrayToString = (arr) => {
  if (!arr || !Array.isArray(arr)) return '';
  return arr.join(';');
};

const fetchBoardsFromAPI = async () => {
  try {
    const response = await apiClient.get(`/api/Board/get`, { withCredentials: true });
    data.boards = response.data;
    console.log('Pobrane plansze:', JSON.stringify(data.boards, null, 2)); // DEBUG
    if (data.boards.length === 0 && activeView.value === 'edit') {
        toast.info("Brak plansz do edycji, przełączam na dodawanie.");
        activeView.value = 'add';
    }
  } catch (error) {
    console.error('Błąd pobierania plansz:', error.response?.data || error.message);
    toast.error(`Nie udało się pobrać plansz: ${error.response?.data?.title || error.message}`);
  }
};

const resetForm = () => {
  Object.assign(formData, getDefaultFormData());
  selectedBoardId.value = null;
};

const loadSelectedBoard = async () => {
  if (!selectedBoardId.value) {
    resetForm();
    return;
  }
  try {
    // Tutaj jest OK, bo selectedBoardId.value to liczba
    const selectedBoard = data.boards.find(board => board.boardId === selectedBoardId.value); 

    if (!selectedBoard) {
      toast.error('Nie znaleziono wybranej planszy lokalnie. Spróbuj odświeżyć listę.');
      return;
    }

    // Poniżej używasz camelCase, co jest ZGODNE z tym, co zwraca API!
    formData.BoardId = selectedBoard.boardId; 
    formData.Name = selectedBoard.name;
    formData.LabelsUp = stringToArray(selectedBoard.labelsUp);
    formData.LabelsRight = stringToArray(selectedBoard.labelsRight);
    formData.DescriptionDown = selectedBoard.descriptionDown;
    formData.DescriptionLeft = selectedBoard.descriptionLeft;
    formData.Rows = selectedBoard.rows;
    formData.Cols = selectedBoard.cols;
    formData.CellColor = selectedBoard.cellColor;
    formData.BorderColor = selectedBoard.borderColor;
    formData.BorderColors = stringToArray(selectedBoard.borderColors);

    toast.success(`Załadowano planszę: ${formData.Name}`); // Jeśli formData.Name jest teraz poprawnie ustawione, to zadziała
  } catch (error) {
    console.error('Błąd podczas ładowania planszy:', error);
    toast.error(`Wystąpił błąd podczas ładowania: ${error.message}`);
  }
};

watch(selectedBoardId, loadSelectedBoard);

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

    const payload = {
      Name: formData.Name,
      LabelsUp: arrayToString(formData.LabelsUp),
      LabelsRight: arrayToString(formData.LabelsRight),
      DescriptionDown: formData.DescriptionDown,
      DescriptionLeft: formData.DescriptionLeft,
      Rows: formData.Rows,
      Cols: formData.Cols,
      CellColor: formData.CellColor,
      BorderColor: formData.BorderColor,
      BorderColors: arrayToString(formData.BorderColors)
    };
    
    let response;
    
    if (activeView.value === 'add') {
      response = await apiClient.post(`/api/Board/add`, payload, { withCredentials: true });
      toast.success(`Plansza "${response.data.name}" dodana pomyślnie!`);
      await fetchBoardsFromAPI();
      resetForm();

    } else {
      if (!selectedBoardId.value) {
        toast.warning('Wybierz planszę do edycji!');
        return;
      }
      response = await apiClient.put(`/api/Board/edit/${selectedBoardId.value}`, payload, { withCredentials: true });
      
      toast.success(`Plansza "${response.data.name}" zaktualizowana pomyślnie!`);
      await fetchBoardsFromAPI();
    }
  } catch (error) {
    console.error('Błąd podczas zapisywania planszy:', error.response?.data || error.message);
    const errorMessage = error.response?.data?.title || error.response?.data || error.message;
    toast.error(`Błąd zapisu: ${errorMessage}`);
  }
};

// W pliku frontendu, zastąp tę funkcję:
const deleteBoard = async () => {
  if (!selectedBoardId.value) {
    toast.warning('Nie wybrano planszy do usunięcia!');
    return;
  }
  
  const boardToDelete = data.boards.find(b => b.boardId === selectedBoardId.value);
  const boardName = boardToDelete ? boardToDelete.name : "wybrana plansza";

  if (confirm(`Czy na pewno chcesz usunąć planszę "${boardName}"? Tej operacji nie można cofnąć.`)) {
    try {
      // --- TUTAJ JEST ZMIANA ---
      // Zmieniamy URL, aby pasował do nowego endpointu w backendzie: /api/Board/delete/{id}
      await apiClient.delete(`/api/Board/delete/${selectedBoardId.value}`, { withCredentials: true });
      
      toast.success('Plansza została usunięta pomyślnie!');
      
      // Reszta logiki jest już dobra - odświeżenie danych i zresetowanie formularza
      await fetchBoardsFromAPI();
      resetForm();
      if (data.boards.length === 0) {
        activeView.value = 'add';
      }
    } catch (error) {
      console.error('Błąd podczas usuwania planszy:', error.response?.data || error.message);
      const errorMessage = error.response?.data?.title || error.response?.data || error.message;
      toast.error(`Błąd usuwania: ${errorMessage}`);
    }
  }
};

onMounted(async () => {
  await fetchBoardsFromAPI();
});

watch(activeView, (newView) => {
  resetForm();
  if (newView === 'edit') {
    if (data.boards.length === 0) {
      toast.info('Brak plansz do edycji. Dodaj nową planszę.');
      activeView.value = 'add';
    }
  }
});

const previewConfig = computed(() => {
  const labelsUpArray = Array.isArray(formData.LabelsUp) ? formData.LabelsUp : [];
  const labelsRightArray = Array.isArray(formData.LabelsRight) ? formData.LabelsRight : [];

  const cols = (labelsUpArray.length || 0) * 2;
  const rows = (labelsRightArray.length || 0) * 2;
  
  return {
    Name: formData.Name,
    LabelsUp: labelsUpArray,
    LabelsRight: labelsRightArray,
    DescriptionDown: formData.DescriptionDown,
    DescriptionLeft: formData.DescriptionLeft,
    Rows: rows,
    Cols: cols,
    CellColor: formData.CellColor,
    BorderColor: formData.BorderColor,
    BorderColors: Array.isArray(formData.BorderColors) ? formData.BorderColors : []
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
</script>