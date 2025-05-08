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
  
            <!-- Dropdown do wyboru planszy - widoczny tylko w trybie edycji -->
            <boardSelector 
              v-model="selectedBoardId"
              @load="loadSelectedBoard"
              @delete="deleteBoard"
            />
            
            <!--Informacja o nazwie planszy i ilości kolumn oraz rzędów-->
            <form>
              <boardInfo 
                v-model:name="formData.name"
                :cols="formData.config.labelsUp.length * 2"
                :rows="formData.config.labelsRight.length * 2"
                @update="updatePreview"
              />
              
              <!-- Sekcja wyboru kolorów podstawowych -->
              <boardColorSettings 
                v-model:cellColor="formData.config.cellColor"
                v-model:borderColor="formData.config.borderColor"
                v-model:borderColors="formData.config.borderColors"
                @update="updatePreview"
              />
              
              <!-- Sekcja zarządzania etykietami (górne i prawe) -->
                <boardLabelsEditors 
                  v-model:labelsUp="formData.config.labelsUp"
                  v-model:labelsRight="formData.config.labelsRight"
                  @update="updatePreview"
                />
              
              <!-- Sekcja opisów osi planszy -->
              <boardDescriptions 
                v-model:descriptionDown="formData.config.descriptionDown"
                v-model:descriptionLeft="formData.config.descriptionLeft"
                @update="updatePreview"
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
              :key="previewKey" 
              :preview-mode="true" 
              :preview-config="previewConfig" />
          </div>
        </div>
      </div>
    </div>
  </template>
  
  
  <script setup>
  // Importy ikon i zależności
  import { faPlus, faPenToSquare, faTrash, faSave } from '@fortawesome/free-solid-svg-icons';
  import { ref, reactive, computed, onMounted, nextTick, watch } from 'vue';
  import { useToast } from 'vue-toastification';
  import { useBoardStore } from '@/stores/boardStore';
  import myBoard from '@/components/game/gameBoard.vue';
  import boardSelector from '@/components/editBoard/boardSelector.vue';
  import boardInfo from '@/components/editBoard/boardInfo.vue';
  import boardColorSettings from '@/components/editBoard/boardColorSettings.vue';
  import boardLabelsEditors from '@/components/editBoard/boardLabelsEditors.vue';
  import boardDescriptions from '@/components/editBoard/boardDescriptions.vue';
  
  // Zmienne stanu UI
  const selectedBoardId = ref(''); // ID wybranej planszy w trybie edycji
  const activeView = ref('add');   // Aktualny tryb widoku (add/edit)
  const toast = useToast();        // System powiadomień
  const boardStore = useBoardStore(); // Store z danymi plansz
  
  // Licznik wymuszający przeładowanie komponentu podglądu po zmianach
  // Zwiększanie tej wartości powoduje odświeżenie widoku planszy
  const previewKey = ref(0);
  
  // Główny formularz - dane planszy
  const formData = reactive({
    name: 'Nowa plansza',
    config: {
      // Etykiety wpływają na wymiary planszy (2 komórki na etykietę)
      labelsUp: ['Podstawowa kordynacja', 'Standaryzacja procesów', 'Zintegrowane działania', 'Pełna integracja strategiczna'],
      labelsRight: ['Nowicjusz', 'Naśladowca', 'Innowator', 'Lider cyfrowy'],
      descriptionDown: 'Poziom integracji wew/zew',
      descriptionLeft: 'Zawansowanie Cyfrowe',
      cellColor: '#fefae0',        // Kolor tła komórek
      borderColor: '#595959',      // Kolor obramowania komórek
      borderColors: ['#008000', '#FFFF00', '#FFA500', '#FF0000']  // Kolory stref (zielony, żółty, pomarańczowy, czerwony)
    }
  });
  
  // Resetuje formularz do wartości domyślnych
  const resetForm = () => {
    formData.name = 'Nowa plansza';
    formData.config = {
      labelsUp: ['Podstawowa kordynacja', 'Standaryzacja procesów', 'Zintegrowane działania', 'Pełna integracja strategiczna'],
      labelsRight: ['Nowicjusz', 'Naśladowca', 'Innowator', 'Lider cyfrowy'],
      descriptionDown: 'Poziom integracji wew/zew',
      descriptionLeft: 'Zawansowanie Cyfrowe',
      cellColor: '#fefae0',
      borderColor: '#595959',
      borderColors: ['#008000', '#FFFF00', '#FFA500', '#FF0000']
    };
    
    // Czyszczenie ID wybranej planszy
    selectedBoardId.value = '';
    
    // Odświeżenie podglądu
    updatePreview();
  };
  
  // Ładuje dane wybranej planszy do formularza
  const loadSelectedBoard = async () => {
    if (!selectedBoardId.value) return;
    
    try {
      // Pobieranie planszy z magazynu
      const selectedBoard = boardStore.getBoardById(selectedBoardId.value);
      
      if (!selectedBoard) {
        toast.error('Nie znaleziono wybranej planszy');
        return;
      }
      
      // Ustawienie jako aktualnej w magazynie
      boardStore.setCurrentBoard(selectedBoardId.value);
      
      // Wypełnienie formularza danymi planszy
      formData.name = selectedBoard.name;
      formData.config = { ...selectedBoard.config };
      
      // Aktualizacja podglądu
      await updatePreview();
      
      toast.success(`Załadowano planszę: ${selectedBoard.name}`);
    } catch (error) {
      console.error('Błąd podczas ładowania planszy:', error);
      toast.error(`Wystąpił błąd: ${error.message}`);
    }
  };
  
  // Wyliczenie konfiguracji dla podglądu planszy na podstawie aktualnego stanu formularza
  const previewConfig = computed(() => {
    // Wymiary planszy zależą od liczby etykiet (2 komórki na etykietę)
    const cols = formData.config.labelsUp.length * 2;
    const rows = formData.config.labelsRight.length * 2;
    
    return {
      ...formData.config,
      rows: rows,
      cols: cols,
      margin: 40,  // Stały margines wokół planszy
      borderColor: formData.config.borderColor
    };
  });

  
  // Sprawdza i ewentualnie wypełnia domyślne wartości opisów
  const validateDescriptions = () => {
    // Uzupełnianie pustych opisów wartościami domyślnymi
    if (!formData.config.descriptionDown.trim()) {
      formData.config.descriptionDown = 'Poziom integracji wew/zew';
      toast.warning("Opis dolny nie może być pusty. Ustawiono wartość domyślną.");
    }
    
    if (!formData.config.descriptionLeft.trim()) {
      formData.config.descriptionLeft = 'Zawansowanie Cyfrowe';
      toast.warning("Opis lewy nie może być pusty. Ustawiono wartość domyślną.");
    }
  };
  
  // Aktualizuje podgląd planszy
  const updatePreview = async () => {
    // Upewniamy się, że opisy nie są puste
    validateDescriptions();
    
    // Zwiększamy licznik, by wymusić przeładowanie komponentu podglądu
    previewKey.value++;
    
    // Czekamy na zakończenie cyklu renderowania Vue
    await nextTick();
  };
  
  // Zapisuje planszę (dodaje nową lub aktualizuje istniejącą)
  const saveBoard = async () => {
    try {
      // Sprawdzamy czy nazwa planszy została podana
      if (!formData.name.trim()) {
        toast.error('Nazwa planszy jest wymagana!');
        return;
      }
      
      // Sprawdzamy czy wszystkie etykiety są wypełnione
      if (formData.config.labelsUp.some(label => !label.trim()) || 
          formData.config.labelsRight.some(label => !label.trim())) {
        toast.error('Wszystkie etykiety muszą być wypełnione!');
        return;
      }
      
      // Upewniamy się, że opisy nie są puste
      validateDescriptions();
      
      // Zależnie od trybu: dodajemy nową lub aktualizujemy istniejącą planszę
      if (activeView.value === 'add') {
        // Dodawanie nowej planszy
        const boardData = {
          name: formData.name,
          config: {
            ...formData.config
          }
        };
        
        await boardStore.createBoard(boardData);
        toast.success('Plansza została dodana pomyślnie!');
        resetForm();
      } else {
        // Aktualizacja istniejącej planszy
        if (!selectedBoardId.value) {
          toast.warning('Wybierz planszę do edycji!');
          return;
        }
        
        const updatedBoard = {
          name: formData.name,
          config: formData.config
        };
        
        await boardStore.updateBoard(selectedBoardId.value, updatedBoard);
        toast.success('Plansza została zaktualizowana pomyślnie!');
      }
    } catch (error) {
      toast.error(`Wystąpił błąd: ${error.message}`);
      console.error('Błąd podczas zapisywania planszy:', error);
    }
  };
  
  // Usuwa wybraną planszę po potwierdzeniu
  const deleteBoard = async () => {
    if (!selectedBoardId.value) {
      toast.warning('Nie wybrano planszy do usunięcia!');
      return;
    }
    
    // Potwierdzenie przed usunięciem
    if (confirm(`Czy na pewno chcesz usunąć planszę "${formData.name}"? Tej operacji nie można cofnąć.`)) {
      try {
        await boardStore.deleteBoard(selectedBoardId.value);
        toast.success('Plansza została usunięta pomyślnie!');
        
        // Sprawdzenie czy są jeszcze jakieś plansze
        if (boardStore.boards.length === 0) {
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
      // Pobieranie plansz przy pierwszym uruchomieniu
      if (boardStore.boards.length === 0) {
        await boardStore.fetchBoards();
      }
      // Inicjalizacja podglądu
      await updatePreview();
    } catch (error) {
      console.error('Błąd podczas inicjalizacji formularza:', error);
      toast.error(`Wystąpił błąd: ${error.message}`);
    }
  });
  
  // Obserwowanie zmian w trybie widoku
  watch(activeView, (newView) => {
    if (newView === 'add') {
      // Czyszczenie formularza przy przejściu do trybu dodawania
      resetForm();
    } else if (newView === 'edit') {
      // Czyszczenie wyboru planszy przy przejściu do trybu edycji
      selectedBoardId.value = '';
      
      // Sprawdzenie czy są dostępne plansze do edycji
      if (boardStore.boards.length === 0) {
        toast.warning('Brak dostępnych plansz do edycji');
        // Automatyczny powrót do trybu dodawania
        activeView.value = 'add';
      } else {
        toast.info('Wybierz planszę do edycji');
      }
    }
  });
  </script>