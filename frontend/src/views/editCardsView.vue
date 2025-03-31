<template>
    <div class="w-full">
        <div class="flex flex-col justify-center items-center m-4 px-2 py-2 border-2 border-lgray-accent rounded-md bg-tertiary">
            <h1 class="text-3xl font-nasalization text-white mt-5">Edycja kart decyzji</h1>

            <form class="w-full max-w-lg mt-4 flex flex-col items-center">
                <div class="w-full">
                    <dropDown 
                        :cards="cardsData"
                        v-model="selectedCard"
                    />
                </div>
                
                <!-- Formularz edycji karty - widoczny po wybraniu karty -->
                <div v-if="selectedCard && currentCard" class="mt-6 space-y-4 text-white w-full">
                    <div class="flex flex-col">
                        <label for="title" class="mb-1">Tytuł karty:</label>
                        <input 
                            id="title"
                            v-model="currentCard.title"
                            type="text"
                            class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 text-white w-full"
                        />
                    </div>
                    
                    <div class="flex flex-col">
                        <label for="description" class="mb-1">Opis karty:</label>
                        <textarea
                            id="description"
                            v-model="currentCard.description"
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
import { faSave } from '@fortawesome/free-solid-svg-icons';
import dropDown from '@/components/dropDown.vue';
import { reactive, ref,  watch } from 'vue';

const selectedCard = ref(null);

const cardsData = reactive([
    {
        id: 1,
        title: 'Stworzenie szczegółowego planu wdrożenia',
        description: 'Kluczowy krok w realizacji projektu, który zapewnia płynne przejście od etapu planowania do realizacji'
    },
    {
        id: 2,
        title: 'Zarządzanie danymi',
        description: 'W miarę skalowania technologii rośnie znaczenie zarządzania danymi. Warto zainwestować w narzędzia do analizy danych, które pozwolą na lepsze wykorzystanie zasobów informacyjnych oraz podejmowanie decyzji opartych na danych.'
    },
    {
        id: 3,
        title: 'Określenie kluczowych procesów digitalizacji',
        description: 'Proces polegający na analizie działalności organizacji w celu zidentyfikowania obszarów, które mogą zostać ulepszone lub zautomatyzowane za pomocą technologii cyfrowych. Ustal priorytet koncentracji na procesie  na podstawie obecnego stanu digitalizacji. Wybierz maksymalnie 4 procesy.'
    },
    {
        id:4,
        title:'Ocena efektywności szkoleń',
        description:'Po zakończeniu szkoleń warto przeprowadzać testy wiedzy lub certyfikacje, aby upewnić się, że pracownicy opanowali niezbędne umiejętności. Dodatkowo regularne zbieranie opinii od uczestników szkoleń pozwala na bieżąco dostosowywać programy szkoleniowe i reagować na ewentualne braki w edukacji.Należy też pamiętać, że wdrażanie technologii i szkoleń powinno być monitorowane poprzez ocenę postępów pracowników w codziennej pracy oraz ich adaptacji do nowych narzędzi.'
    },
    {
        id:5,
        title:'Stwórz strategię transformacji modelu biznesowego',
        description:'Oceń, czy istnieją możliwości wsparcia modelu biznesowego w technologie cyfrowe, np. sprzedaż online, e-gwarancja, e-instrukcja itp.'
    }
]);


const currentCard = ref(null);


watch(selectedCard, (newValue) => {
    if (newValue) {
       
        const card = cardsData.find(card => card.id === newValue);
        if (card) {
            
            currentCard.value = { ...card };
        }
    } else {
        currentCard.value = null;
    }
});
</script>