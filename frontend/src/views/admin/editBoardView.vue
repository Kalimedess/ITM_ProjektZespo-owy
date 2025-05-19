<template>
    <div class="w-full h-full">
      <div class="grid grid-cols-1 lg:grid-cols-[55%_45%] gap-4">
        <!-- Główny formularz po lewej stronie -->
        <div class="flex flex-col justify-start items-center border-2 border-lgray-accent py-6 px-4 m-4 rounded-md text-white bg-tertiary">
          <!-- Przyciski przełączające widok dodawania/edycji -->
          <div class="flex flex-row w-full items-center justify-center gap-5">
            <button 
              class="border-2 border-lgray-accent py-2 px-2 rounded-md w-60 text-center hover:border-accent transition-colors duration-300"
              @click="activeView = 'add'">
              <font-awesome-icon :icon="faPlus" class="h-4 text-accent"/> 
              Dodaj nową planszę
            </button>
            <button 
              class="border-2 border-lgray-accent py-2 px-2 rounded-md w-60 text-center hover:border-accent transition-colors duration-300"
              @click="activeView = 'edit'">
              <font-awesome-icon :icon="faPenToSquare" class="h-4 text-accent"/>
              Edytuj planszę
            </button> 
          </div>
          <div>
            <!-- Dynamiczny nagłówek zależny od aktywnego widoku -->
            <h1 class="mt-8 font-nasalization text-3xl">
              {{ activeView === 'add' ? 'Dodaj nową planszę' : 'Edytuj planszę' }}
            </h1>
  
            <!-- Dropdown do wyboru planszy -->
            <boardSelector 
              :boards="data.boards"
              v-model="selectedBoardId"
              @delete="deleteBoard"
            />
            
            <!--Informacja o nazwie planszy i ilości kolumn oraz rzędów-->
            <form>
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
  
              
              <!-- Przycisk zapisywania planszy - zmienia tekst zależnie od trybu -->
              <button 
                type="button" 
                class="bg-accent border-2 border-accent py-3 px-6 rounded-md mt-5"
                @click="saveBoard">
                <font-awesome-icon :icon="faSave" class="h-4 mr-2" />
                {{ activeView === 'add' ? 'Dodaj planszę' : 'Zapisz zmiany' }}
              </button>
            </form>
          </div>
        </div>
  
        <!-- Panel podglądu planszy po prawej stronie -->
        <div class="border-2 border-lgray-accent py-6 px-8 m-4 rounded-md text-white bg-tertiary flex flex-col">
          <h2 class="text-xl mb-4 text-center">Podgląd planszy</h2>
          
          <div class="relative flex-grow overflow-hidden pl-12">
            <!-- Komponent planszy z trybem podglądu -->
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
  
  const selectedBoardId = ref(null); // ID wybranej planszy 
  const activeView = ref('add');   // Aktualny tryb widoku (add/edit)
  const toast = useToast();        // System powiadomień

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

  // Główny formularz - dane planszy
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

  const stringToArray = (str) => {
    if (!str) return [];
    return str.split(';').map(item => item.trim());
  };

  const arrayToString = (arr) => {
    if (!arr || !Array.isArray(arr)) return '';
    return arr.join(';');
  };
  
  // Resetuje formularz do wartości domyślnych
    const resetForm = () => {

      formData.Name = 'Plansza podstawowa';  
      formData.LabelsUp = ['Podstawowa kordynacja', 'Standaryzacja procesów', 'Zintegrowane działania', 'Pełna integracja strategiczna'];
      formData.LabelsRight = ['Nowicjusz', 'Naśladowca', 'Innowator', 'Lider cyfrowy'];
      formData.DescriptionDown = 'Poziom integracji wew/zew';
      formData.DescriptionLeft = 'Zawansowanie Cyfrowe';
      formData.Rows = 8;
      formData.Cols = 8;
      formData.CellColor = '#fefae0';
      formData.BorderColor = '#595959';
      formData.BorderColors = ['#008000', '#FFFF00', '#FFA500', '#FF0000'];
      
      // Czyszczenie ID wybranej planszy
      selectedBoardId.value = null;
      
    };
  

  // Ładuje dane wybranej planszy do formularza
  const loadSelectedBoard = async () => {
    if (!selectedBoardId.value) return;
    
    try {
      // Pobieranie planszy z tablicy boards
      const selectedBoard = data.boards.find(board => board.BoardId === selectedBoardId.value);
      
      if (!selectedBoard) {
        toast.error('Nie znaleziono wybranej planszy');
        return;
      }
      
      // Konwersja tekstów oddzielonych średnikami na tablice
      const labelsUp = stringToArray(selectedBoard.LabelsUp);
      const labelsRight = stringToArray(selectedBoard.LabelsRight);
      const borderColors = stringToArray(selectedBoard.BorderColors);
      

      formData.Name = selectedBoard.Name;
      formData.LabelsUp = labelsUp;
      formData.LabelsRight = labelsRight;
      formData.DescriptionDown = selectedBoard.DescriptionDown;
      formData.DescriptionLeft = selectedBoard.DescriptionLeft;
      formData.Rows = selectedBoard.Rows;
      formData.Cols = selectedBoard.Cols;
      formData.CellColor = selectedBoard.CellColor;
      formData.BorderColor = selectedBoard.BorderColor;
      formData.BorderColors = borderColors;
      
      toast.success(`Załadowano planszę: ${selectedBoard.Name}`);
    } catch (error) {
      console.error('Błąd podczas ładowania planszy:', error);
      toast.error(`Wystąpił błąd: ${error.message}`);
    }
  };
  
  
  const previewConfig = computed(() => {
    // Wymiary planszy zależą od liczby etykiet (2 komórki na etykietę)
    const cols = formData.LabelsUp.length * 2;
    const rows = formData.LabelsRight.length * 2;
    
    return {
      Name: formData.Name,
      LabelsUp: formData.LabelsUp,
      LabelsRight: formData.LabelsRight,
      DescriptionDown: formData.DescriptionDown,
      DescriptionLeft: formData.DescriptionLeft,
      Rows: rows,
      Cols: cols,
      CellColor: formData.CellColor,
      BorderColor: formData.BorderColor,
      BorderColors: formData.BorderColors
    };
  });

  
  // Sprawdza i ewentualnie wypełnia domyślne wartości opisów
  const validateDescriptions = () => {
    
    if (!formData.DescriptionDown.trim()) {
      formData.DescriptionDown = 'Poziom integracji wew/zew';
      toast.warning("Opis dolny nie może być pusty. Ustawiono wartość domyślną.");
    }
    
    if (!formData.DescriptionLeft.trim()) {
      formData.DescriptionLeft = 'Zawansowanie Cyfrowe';
      toast.warning("Opis lewy nie może być pusty. Ustawiono wartość domyślną.");
    }
  };
  

  // Zapisuje planszę (dodaje nową lub aktualizuje istniejącą)
const saveBoard = async () => {
  try {
    // Sprawdzamy czy nazwa planszy została podana
    if (!formData.Name.trim()) {
      toast.error('Nazwa planszy jest wymagana!');
      return;
    }
    
    // Sprawdzamy czy wszystkie etykiety są wypełnione
    if (formData.LabelsUp.some(label => !label.trim()) || 
        formData.LabelsRight.some(label => !label.trim())) {
      toast.error('Wszystkie etykiety muszą być wypełnione!');
      return;
    }
    
    // Upewniamy się, że opisy nie są puste
    validateDescriptions();
    
    // Przygotowanie danych do zapisu w formacie bazy danych
    const boardData = {
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
    
    // Zależnie od trybu: dodajemy nową lub aktualizujemy istniejącą planszę
    if (activeView.value === 'add') {
      // Dodawanie nowej planszy (symulacja generowania ID)
      const maxId = data.boards.reduce((max, board) => Math.max(max, board.BoardId), 0);
      const newBoard = {
        BoardId: maxId + 1,
        ...boardData
      };
      
      // Dodanie nowej planszy do tablicy
      data.boards.push(newBoard);
      toast.success('Plansza została dodana pomyślnie!');
      
      // Resetowanie formularza
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
      
      
      data.boards[boardIndex] = {
        ...data.boards[boardIndex], // zachowaj BoardId i inne pola, które nie są w formularzu
        ...boardData
      };
      
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
        toast.info('Wszystkie plansze zostały usunięte. Dodaj nową planszę.');
      }
    } catch (error) {
      toast.error(`Błąd podczas usuwania planszy: ${error.message}`);
    }
  }
};
  
  // Kod wykonywany po zamontowaniu komponentu
  onMounted(async () => {
  try {
    // Inicjalizacja formularza domyślnymi wartościami
    resetForm();
    
  } catch (error) {
    console.error('Błąd podczas inicjalizacji formularza:', error);
    toast.error(`Wystąpił błąd: ${error.message}`);
  }
});
  
watch(activeView, (newView) => {
  if (newView === 'add') {
    // Czyszczenie formularza przy przejściu do trybu dodawania
    resetForm();
  } else if (newView === 'edit') {
    // Czyszczenie wyboru planszy przy przejściu do trybu edycji
    selectedBoardId.value = null; 
    
    
    if (data.boards.length === 0) {
      toast.warning('Brak dostępnych plansz do edycji');
      
      activeView.value = 'add';
    } else {
      toast.info('Wybierz planszę do edycji');
    }
  }
});
  </script>