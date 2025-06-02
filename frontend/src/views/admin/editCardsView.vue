<template>
    <div class="w-full flex">
        <div class="flex flex-col flex-1 justify-center items-center m-4 px-2 py-2 border-2 border-lgray-accent rounded-md bg-tertiary">
            <h1 class="text-3xl font-nasalization text-white mt-5">Edycja kart decyzji</h1>


                <!-- Ukryty input -->
                <input
                    type="file"
                    accept=".xls,.xlsx"
                    ref="fileInput"
                    @change="handleFileChange"
                    style="display: none;"
                />

            <!--Przycisk  do wczytania talii z pliku xls-->
            <button
                @click="triggerFileInput"
                class=" bg-green-500 border-2 border-green-700 py-3 px-6 rounded-md mt-5 text-white">
                <font-awesome-icon :icon="faFileExcel" class="h-4 mr-2"/>
                Wczytaj z pliku xls
            </button>

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
                        :items="filteredCards"
                        v-model="selectedCard"
                        :item-key="'id'"
                        :display-format="(card) => `#${card.id} ${card.title}`"
                        placeholder="Wybierz kartę..."
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
        <div v-if="currentCard && selectedCard"
            class="flex flex-col flex-1 items-center m-4 px-2 py-2 border-2 border-lgray-accent rounded-md bg-tertiary">
            <h1 class="text-3xl font-nasalization text-white mt-5">Edycja feedbacku</h1>

            <form class="w-full max-w-lg mt-4 flex flex-col items-center">
                <div class="w-full mb-4">
                    <label class="block text-white mb-1 mt-2">Wybierz feedback:</label>
                    <dropDown
                        :items="feedbackData"
                        v-model="selectedFeedback"
                        :item-key="'id'"
                        :display-format="(feedback) => `${feedback.status === 'P' ? '✅' : '❌'} ${feedback.longDescription.substring(0, 30)}`"
                        placeholder="Wybierz feedback..."
                    />
                </div>

                <div v-if="selectedFeedback" class="flex flex-col w-full">
                    <label for="feedbackDescription" class="block text-white mb-1">Opis feedbacku:</label>
                    <textarea
                        id="feedbackDescription"
                        v-model="currentFeedback.longDescription"
                        rows="8"
                        class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 text-white resize-none w-full"
                    >
                    </textarea>
                    <div class="flex justify-center w-full">
                        <button
                            type="button"
                            class="bg-accent border-2 border-accent py-3 px-6 rounded-md mt-5  text-white">
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
import { faSave,faFileExcel} from '@fortawesome/free-solid-svg-icons';
import dropDown from '@/components/dropDown.vue';
import { reactive, ref, watch, computed } from 'vue';
import axios from 'axios';
import { onMounted } from 'vue';

const selectedDeck = ref(null);
const selectedCard = ref(null);
const selectedFeedback = ref(null);

const feedbackData = reactive([
    {
        id:1,
        longDescription:'Feedback negatywny',
        status:'N'

    },
    {
        id:2,
        longDescription:'Feedback pozytywny',
        status:'P'

    },
]);

const decksData = reactive([

]);

const cardsData = reactive([

]);

// Filtrowanie kart na podstawie wybranej talii
const filteredCards = computed(() => {
    if (!selectedDeck.value) return [];
    return cardsData.filter(card => card.deckId === selectedDeck.value);
});

const currentCard = ref(null);

// Obsługa wczytywania pliku
const fileInput = ref(null);

function triggerFileInput() {
    if (fileInput.value) {
        fileInput.value.click(); // Otwiera okno wyboru pliku
    }
}

async function handleFileChange(event) {
    const file = event.target.files[0];
    console.log('Plik wybrany:', file);

    if (!file) return;

    const formData = new FormData();
    formData.append("file", file);

    try {
        const response = await axios.post("http://localhost:5023/api/deck/upload", formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            },
            withCredentials: true
        });

        console.log("Odpowiedź z backendu:", response.data);
    } catch (error) {
        console.error("Błąd przy wysyłaniu pliku:", error);
    }
}



// Reset wybranej karty gdy zmienia się talia
watch(selectedDeck, () => {
    selectedCard.value = null;
    currentCard.value = null;
});

watch(selectedDeck, async (deckId) => {
    selectedCard.value = null;
    currentCard.value = null;

    if (!deckId) {
        cardsData.splice(0);
        return;
    }

    try {
        const response = await axios.get("http://localhost:5023/api/deck/decisions", {
            params: {
                deckId: selectedDeck.value
            },
            withCredentials: true
        });
        cardsData.splice(0, cardsData.length, ...response.data);
    } catch (error) {
        console.error("Błąd przy pobieraniu kart z talii:", error);
    }
});

// Aktualizacja bieżącej karty gdy zmienia się wybrana karta
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

onMounted(async () => {
    try {
        const response = await axios.get("http://localhost:5023/api/deck/edit", {
            withCredentials: true
        });
        decksData.splice(0, decksData.length, ...response.data);
    } catch (error) {
        console.error("Błąd przy pobieraniu talii:", error);
    }
});




const currentFeedback = ref(null);

watch(selectedFeedback, (newValue) => {
    if (newValue) {
        const feedback = feedbackData.find(feedback => feedback.id === newValue);
        if (feedback) {
            currentFeedback.value = { ...feedback };
        }
    } else {
        currentFeedback.value = null;
    }
});
</script>
