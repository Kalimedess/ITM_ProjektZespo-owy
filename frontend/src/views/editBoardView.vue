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
            <div v-if="activeView === 'edit' && boardStore.boards.length > 0" class="mt-4 w-full">
              <label for="board-select" class="block mb-1">Wybierz planszę do edycji</label>
              <div class="flex items-center gap-2">
                <select 
                  id="board-select"
                  v-model="selectedBoardId"
                  class="w-full px-3 py-2 border-2 border-lgray-accent rounded-md bg-tertiary text-white"
                  @change="loadSelectedBoard">
                  <option value="" disabled>Wybierz planszę</option>
                  <option v-for="board in boardStore.boards" :key="board.id" :value="board.id">
                    {{ board.name }}
                  </option>
                </select>
                <!-- Przycisk usuwania - pojawia się tylko gdy plansza jest wybrana -->
                <button 
                  v-if="selectedBoardId"
                  @click="deleteBoard"
                  class="p-2 hover:bg-red-100 rounded" 
                  title="Usuń wybraną planszę">
                  <font-awesome-icon :icon="faTrash" class="h-4 text-red-500"/>
                </button>
              </div>
            </div>
  
            <form>
              <!-- Pole nazwy planszy -->
              <div class="mt-8 mb-5">
                <label for="board-name" class="block mb-1">Nazwa planszy</label>
                <input 
                  id="board-name"
                  v-model="formData.name"
                  type="text" 
                  class="w-full px-3 py-2 border-2 border-lgray-accent rounded-md bg-transparent"
                  placeholder="Wprowadź nazwę planszy"
                  @input="updatePreview" />
              </div>
  
              <!-- Informacje o wymiarach planszy (wyliczane automatycznie) -->
              <div class="flex flex-row w-full items-center justify-center gap-5 mt-8 mb-5">
                <div class="border-2 border-lgray-accent py-2 px-2 rounded-md w-60 text-center">
                  <span>Kolumny: {{ formData.config.labelsUp.length * 2 }}</span>
                </div>
                <div class="border-2 border-lgray-accent py-2 px-2 rounded-md w-60 text-center">
                  <span>Rzędy: {{ formData.config.labelsRight.length * 2 }}</span>
                </div>
              </div>
              
              <!-- Sekcja wyboru kolorów podstawowych -->
              <div class="flex flex-row w-full items-center justify-center gap-5 mt-8 mb-5">
                <!-- Kolor wypełnienia komórki -->
                <div class="border-2 border-lgray-accent py-2 px-2 rounded-md w-60 text-center">
                  <label for="cell-color" class="block mb-1">Kolor komórki</label>
                  <input 
                    id="cell-color"
                    v-model="formData.config.cellColor"
                    type="color" 
                    class="w-12 h-10 border rounded mx-auto" 
                    @input="updatePreview" />
                  <div class="text-sm mt-2">{{ formData.config.cellColor }}</div>
                </div>
  
                <!-- Kolor obramowania komórki -->
                <div class="border-2 border-lgray-accent py-2 px-2 rounded-md w-60 text-center">
                  <label for="border-color" class="block mb-1">Kolor obramowania komórki</label>
                  <input 
                    id="border-color"
                    v-model="formData.config.borderColor"
                    type="color" 
                    class="w-12 h-10 border rounded mx-auto" 
                    @input="updatePreview" />
                  <div class="text-sm mt-2">{{ formData.config.borderColor }}</div>
                </div>
              </div>
              
              <!-- Sekcja zarządzania etykietami (górne i prawe) -->
              <div class="flex flex-row w-full gap-5 mt-8 mb-5">
                <!-- Etykiety górne - wpływają na liczbę kolumn -->
                <div class="flex-1">
                  <label class="block mb-1">Etykiety górne</label>
                  
                  <!-- Lista istniejących etykiet górnych -->
                  <div class="border-2 border-lgray-accent px-2 py-2 rounded-md mb-3">
                    <div class="flex flex-col gap-2">
                      <div 
                        v-for="(label, index) in formData.config.labelsUp" 
                        :key="index" 
                        class="flex items-center border rounded p-2">
                        <input 
                          type="text" 
                          v-model="formData.config.labelsUp[index]" 
                          class="flex-1 bg-transparent border-none focus:outline-none"
                          @input="updatePreview" />
                        <!-- Przycisk usuwania etykiety (niedostępny gdy jest tylko jedna) -->
                        <button 
                          type="button" 
                          @click="removeLabelUp(index)" 
                          class="ml-auto p-1 text-red-500 hover:bg-red-100 rounded"
                          :disabled="formData.config.labelsUp.length <= 1"
                          title="Usuń etykietę">
                          <font-awesome-icon :icon="faTrash" class="h-3" />
                        </button>
                      </div>
                    </div>
                  </div>
                  
                  <!-- Formularz dodawania nowej etykiety górnej -->
                  <div class="flex items-center gap-2">
                    <input 
                      type="text" 
                      v-model="newLabelUp" 
                      class="flex-1 px-3 py-2 border-2 border-lgray-accent rounded-md bg-transparent"
                      placeholder="Nowa etykieta" />
                    <button 
                      type="button" 
                      @click="addLabelUp" 
                      class="px-2 py-2 rounded-md border-2 border-lgray-accent hover:border-accent transition-colors duration-300">
                      <font-awesome-icon :icon="faPlus" class="h-4 text-accent" />
                      Dodaj
                    </button>
                  </div>
                </div>
                
                <!-- Etykiety prawe - wpływają na liczbę rzędów -->
                <div class="flex-1">
                  <label class="block mb-1">Etykiety prawe</label>
                  
                  <!-- Lista istniejących etykiet prawych -->
                  <div class="border-2 border-lgray-accent px-2 py-2 rounded-md mb-3">
                    <div class="flex flex-col gap-2">
                      <div 
                        v-for="(label, index) in formData.config.labelsRight" 
                        :key="index" 
                        class="flex items-center border rounded p-2">
                        <input 
                          type="text" 
                          v-model="formData.config.labelsRight[index]" 
                          class="flex-1 bg-transparent border-none focus:outline-none"
                          @input="updatePreview" />
                        <!-- Przycisk usuwania etykiety (niedostępny gdy jest tylko jedna) -->
                        <button 
                          type="button" 
                          @click="removeLabelRight(index)" 
                          class="ml-auto p-1 text-red-500 hover:bg-red-100 rounded"
                          :disabled="formData.config.labelsRight.length <= 1"
                          title="Usuń etykietę">
                          <font-awesome-icon :icon="faTrash" class="h-3" />
                        </button>
                      </div>
                    </div>
                  </div>
                  
                  <!-- Formularz dodawania nowej etykiety prawej -->
                  <div class="flex items-center gap-2">
                    <input 
                      type="text" 
                      v-model="newLabelRight" 
                      class="flex-1 px-3 py-2 border-2 border-lgray-accent rounded-md bg-transparent"
                      placeholder="Nowa etykieta" />
                    <button 
                      type="button" 
                      @click="addLabelRight" 
                      class="px-2 py-2 rounded-md border-2 border-lgray-accent hover:border-accent transition-colors duration-300">
                      <font-awesome-icon :icon="faPlus" class="h-4 text-accent" />
                      Dodaj
                    </button>
                  </div>
                </div>
              </div>
              
              <!-- Sekcja opisów osi planszy -->
              <div class="flex flex-row w-full gap-5 mb-5">
                <!-- Opis dolny (pod planszą) -->
                <div class="flex-1">
                  <label for="description-down" class="block mb-1">Opis dolny</label>
                  <input 
                    id="description-down"
                    type="text" 
                    v-model="formData.config.descriptionDown" 
                    class="w-full px-3 py-2 border-2 border-lgray-accent rounded-md bg-transparent"
                    placeholder="Wprowadź opis dolny"
                    @input="updatePreview" />
                </div>
                
                <!-- Opis lewy (po lewej stronie planszy) -->
                <div class="flex-1">
                  <label for="description-left" class="block mb-1">Opis z lewej strony</label>
                  <input 
                    id="description-left"
                    type="text" 
                    v-model="formData.config.descriptionLeft" 
                    class="w-full px-3 py-2 border-2 border-lgray-accent rounded-md bg-transparent"
                    placeholder="Wprowadź opis z lewej strony"
                    @input="updatePreview" />
                </div>
              </div>
  
              <!-- Sekcja kolorów stref na planszy -->
              <div class="mb-4 mt-8">
                <label class="block mb-1">Kolory granic planszy</label>
                
                <!-- Lista istniejących kolorów stref -->
                <div class="border-2 border-lgray-accent px-2 py-2 rounded-md mb-3">
                  <div class="grid grid-cols-4 gap-2">
                    <div 
                      v-for="(color, index) in formData.config.borderColors" 
                      :key="index" 
                      class="flex items-center border rounded p-1">
                      <!-- Podgląd koloru -->
                      <div 
                        class="w-8 h-8 rounded-sm mr-1" 
                        :style="{ backgroundColor: color }"></div>
                      <!-- Selektor koloru -->
                      <input 
                        type="color" 
                        v-model="formData.config.borderColors[index]" 
                        class="w-8 h-8"
                        @input="updatePreview" />
                      <!-- Przycisk usuwania koloru (niedostępny gdy jest tylko jeden) -->
                      <button 
                        type="button" 
                        @click="removeColor(index)" 
                        class="ml-auto p-1 text-red-500 hover:bg-red-100 rounded"
                        :disabled="formData.config.borderColors.length <= 1">
                        <font-awesome-icon :icon="faTrash" class="h-3" />
                      </button>
                    </div>
                  </div>
                </div>
                
                <!-- Formularz dodawania nowego koloru -->
                <div class="flex items-center justify-center gap-2">
                  <input 
                    type="color" 
                    v-model="newColor" 
                    class="w-10 h-10" />
                  <button 
                    type="button" 
                    @click="addColor" 
                    class="px-2 py-2 rounded-md border-2 border-lgray-accent hover:border-accent transition-colors duration-300">
                    <font-awesome-icon :icon="faPlus" class="h-4 text-accent" />
                    Dodaj kolor
                  </button>
                </div>
              </div>
  
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
  import { useBoardStore } from '../stores/boardStore';
  import myBoard from '@/components/myBoard.vue';
  
  // Zmienne stanu UI
  const selectedBoardId = ref(''); // ID wybranej planszy w trybie edycji
  const activeView = ref('add');   // Aktualny tryb widoku (add/edit)
  const toast = useToast();        // System powiadomień
  const boardStore = useBoardStore(); // Store z danymi plansz
  
  // Pola dla nowych elementów
  const newColor = ref('#000000');     // Domyślny kolor dla nowego wpisu
  const newLabelUp = ref('');          // Wartość pola dla nowej etykiety górnej
  const newLabelRight = ref('');       // Wartość pola dla nowej etykiety prawej
  
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
  
  // Dodaje nową etykietę górną
  const addLabelUp = () => {
    if (newLabelUp.value.trim()) {
      formData.config.labelsUp.push(newLabelUp.value);
      newLabelUp.value = ''; // Czyszczenie pola po dodaniu
      updatePreview();
    } else {
      toast.warning("Etykieta nie może być pusta!");
    }
  };
  
  // Usuwa etykietę górną o podanym indeksie
  const removeLabelUp = (index) => {
    if (formData.config.labelsUp.length > 1) {
      formData.config.labelsUp.splice(index, 1);
      updatePreview();
    } else {
      toast.warning("Musi istnieć co najmniej jedna etykieta górna!");
    }
  };
  
  // Dodaje nową etykietę prawą
  const addLabelRight = () => {
    if (newLabelRight.value.trim()) {
      formData.config.labelsRight.push(newLabelRight.value);
      newLabelRight.value = ''; // Czyszczenie pola po dodaniu
      updatePreview();
    } else {
      toast.warning("Etykieta nie może być pusta!");
    }
  };
  
  // Usuwa etykietę prawą o podanym indeksie
  const removeLabelRight = (index) => {
    if (formData.config.labelsRight.length > 1) {
      formData.config.labelsRight.splice(index, 1);
      updatePreview();
    } else {
      toast.warning("Musi istnieć co najmniej jedna etykieta prawa!");
    }
  };
  
  // Dodaje nowy kolor do listy
  const addColor = () => {
    formData.config.borderColors.push(newColor.value);
    updatePreview(); 
  };
  
  // Usuwa kolor o podanym indeksie
  const removeColor = (index) => {
    if (formData.config.borderColors.length > 1) {
      formData.config.borderColors.splice(index, 1);
      updatePreview();
    } else {
      toast.warning("Musi istnieć co najmniej jeden kolor granicy!");
    }
  };
  
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