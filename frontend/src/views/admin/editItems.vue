<template>
    <div class="w-full flex">
        <div class="flex flex-col flex-1 justify-center items-center m-4 px-2 py-2 border-2 border-lgray-accent rounded-md bg-tertiary">
            <h1 class="text-3xl font-nasalization text-white mt-5">Edycja Przedmiotów</h1>

            <form class="w-full max-w-lg mt-4 flex flex-col items-center">
                <!-- Dodanie wyboru talii -->
                <div class="w-full mb-4">
                    <label class="block text-white mb-1">Wybierz talię:</label>
                    <dropDown 
                        :items="decksData"
                        v-model="selectedDeck"
                        :item-key="'id'"
                        :display-format="(deck) => `#${deck.id} ${deck.title}`"
                        :item-label="'title'"
                        placeholder="Wybierz talię..."
                    />
                </div>
                
                <!-- Wybór karty - widoczny tylko po wybraniu talii -->
                <div v-if="selectedDeck" class="w-full">
                    <label class="block text-white mb-1">Wybierz kartę:</label>
                    <dropDown 
                        :items="filteredItems"
                        v-model="selectedItem"
                        :item-key="'id'"
                        :display-format="(item) => `#${item.id} ${item.HardwareShortDesc}`"
                        placeholder="Wybierz przedmiot..."
                    />
                </div>
                
                <!-- Formularz edycji karty - widoczny po wybraniu karty -->
                <div v-if="selectedItem && currentItem" class="mt-6 space-y-4 text-white w-full">
                    <div class="flex flex-col">
                        <label for="title" class="mb-1">Tytuł przedmiotu:</label>
                        <input 
                            id="title"
                            v-model="currentItem.HardwareShortDesc"
                            type="text"
                            class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 text-white w-full"
                        />
                    </div>
                    
                    <div class="flex flex-col">
                        <label for="description" class="mb-1">Opis przedmiotu:</label>
                        <textarea
                            id="description"
                            v-model="currentItem.HardwareLongDesc"
                            rows="8"
                            class=" bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 text-white resize-none w-full"
                        ></textarea>
                    </div>
                    <div class="flex justify-center w-full">
                        <button 
                            type="button" 
                            class="bg-accent border-2 border-accent py-3 px-6 rounded-md mt-5">
                            <font-awesome-icon :icon="faSave" class="h-4 mr-2"/>
                            Zapisz 
                        </button>
                    </div>
                </div>
            </form>
        </div>
    </div>
</template>


<script setup>
import { faSave} from '@fortawesome/free-solid-svg-icons';
import dropDown from '@/components/dropDown.vue';
import { reactive, ref, watch, computed } from 'vue';
import apiConfig from '@/services/apiConfig.js';
import apiService from '@/services/apiServices.js';

const selectedDeck = ref(null);
const selectedItem = ref(null);


const decksData = reactive([
    { id: 1, title: 'Talia Strategii Cyfrowej' },
    { id: 2, title: 'Talia Zarządzania Projektami' },
    { id: 3, title: 'Talia Transformacji Biznesowej' }
]);


const itemsData = reactive([
    {
        id: 1,
        deckId: 1,
        HardwareShortDesc: 'Procesor neuronowy XN-5000',
        HardwareLongDesc: 'Zaawansowany układ obliczeniowy dedykowany do przetwarzania algorytmów uczenia maszynowego. Wyposażony w 128 rdzeni tensora i 64GB pamięci HBM2, zapewnia wysoką wydajność przy niskim poborze energii.'
    },
    {
        id: 2,
        deckId: 1,
        HardwareShortDesc: 'Moduł przetwarzania danych QuickThink',
        HardwareLongDesc: 'Specjalistyczny komponent sprzętowy odpowiedzialny za wstępne przetwarzanie danych wejściowych dla systemów AI. Zawiera zintegrowane układy filtrowania sygnałów i kompresji informacji, umożliwiając szybkie przetwarzanie dużych zbiorów danych w czasie rzeczywistym.'
    },
    {
        id: 3,
        deckId: 2,
        HardwareShortDesc: 'Interfejs neuronowy BrainLink-3',
        HardwareLongDesc: 'Zaawansowany interfejs komunikacyjny między systemami AI a infrastrukturą obliczeniową. Zapewnia dwukierunkową transmisję danych z przepustowością do 100 GB/s i opóźnieniem poniżej 0.5ms. Kompatybilny ze standardowymi protokołami komunikacyjnymi oraz dedykowanymi rozwiązaniami dla sieci neuronowych.'
    },
    {
        id: 4,
        deckId: 2,
        HardwareShortDesc: 'Jednostka analizy predykcyjnej FutureVision',
        HardwareLongDesc: 'Wyspecjalizowany komponent systemu AI odpowiedzialny za prognozowanie i analizę predykcyjną. Wyposażony w dedykowane algorytmy statystyczne i probabilistyczne, umożliwia przewidywanie przyszłych trendów i zdarzeń z dokładnością do 94%. Zawiera moduł samodoskonalenia, który poprawia precyzję przewidywań na podstawie wcześniejszych wyników.'
    },
    {
        id: 5,
        deckId: 3,
        HardwareShortDesc: 'Matryca konwolucyjna DeepSight',
        HardwareLongDesc: 'Wysoce zoptymalizowany układ do przetwarzania obrazów i rozpoznawania wzorców wizualnych. Wyposażony w 512 jednostek obliczeniowych dedykowanych do operacji konwolucyjnych, umożliwia identyfikację i klasyfikację obiektów w czasie rzeczywistym, nawet w trudnych warunkach oświetleniowych i przy częściowym przesłonięciu.'
    }
]);

// Filtrowanie kart na podstawie wybranej talii
const filteredItems = computed(() => {
    if (!selectedDeck.value) return [];
    return itemsData.filter(item => item.deckId === selectedDeck.value);
});

const currentItem = ref(null);

// Reset wybranej karty gdy zmienia się talia
watch(selectedDeck, () => {
    selectedItem.value = null;
    currentItem.value = null;
});

// Aktualizacja bieżącej karty gdy zmienia się wybrana karta
watch(selectedItem, (newValue) => {
    if (newValue) {
        const item = itemsData.find(item => item.id === newValue);
        if (item) {
            currentItem.value = { ...item };
        }
    } else {
        currentItem.value = null;
    }
});
</script>