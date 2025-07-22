<template>
    <div class="w-full flex">
        
        <div class="flex flex-col flex-1 items-center m-4 px-2 py-2 border-2 border-lgray-accent rounded-md bg-tertiary">
            <h1 class="text-3xl font-nasalization text-white mt-5">Edycja Procesów</h1>

            <form @submit.prevent="saveProcessChanges" class="w-full max-w-lg mt-4 flex flex-col items-center">

                <!-- Wybór talii kart -->
                <div class="w-full">
                    <label class="block text-white mb-2 mt-2">Wybierz talię:</label>
                    <dropDown
                        :items="decksData"
                        v-model="selectedDeck"
                        :item-key="'id'"
                        :display-format="(deck) => `#${deck.id} ${deck.title}`"
                        :item-label="'title'"
                        placeholder="Wybierz talię..."
                    />
                </div>
                
                <!--Wybór procesu-->
                <div v-show="selectedDeck" class="w-full">
                    <label class="block text-white mb-2 mt-2">Wybierz proces:</label>
                    <div class="flex items-end gap-2">
                        <div class="flex-1">
                            <dropDown
                                :items="processesData"
                                v-model="selectedProcess"
                                :item-key="'processId'"
                                placeholder="Wybierz proces..."
                            >
                                <template #item-display="{ item }">
                                    <div class="flex items-center gap-2">
                                        <div 
                                            class="w-4 h-4 rounded-full flex-shrink-0" 
                                            :style="`background-color: ${item.processColor || '#6B7280'}`"
                                        ></div>
                                        <span>{{ item.processDesc }}</span>
                                    </div>
                                </template>
                        
                                <template #selected-display="{ selected }">
                                    <div v-if="selected" class="flex items-center gap-2">
                                        <div 
                                            class="w-4 h-4 rounded-full flex-shrink-0" 
                                            :style="`background-color: ${selected.processColor || '#6B7280'}`"
                                        ></div>
                                        <span>{{ selected.processDesc }}</span>
                                    </div>
                                    <span v-else class="text-gray-400">Wybierz proces...</span>
                                </template>

                            </dropDown>
                        </div>
                        
                        <!-- Przyciski do dodania i usunięcia -->
                        <div class="flex gap-2">
                            <button
                                type="button" 
                                @click="addNewProcess" 
                                class="px-3 py-2 bg-green-600 hover:bg-green-700 text-white rounded-md transition-colors"
                                title="Dodaj nowy proces"
                            >
                                <font-awesome-icon :icon="faPlus" />
                            </button>
                            <button
                                type="button" 
                                @click="deleteSelectedProcess" 
                                :disabled="!selectedProcess"
                                class="px-3 py-2 bg-red-600 hover:bg-red-700 disabled:bg-gray-500 disabled:cursor-not-allowed text-white rounded-md transition-colors"
                                title="Usuń wybrany proces"
                            >
                                <font-awesome-icon :icon="faTrash" />
                            </button>
                        </div>
                    </div>
                </div>


                <!--Edycja procesu-->
                <div v-show="selectedProcess || isAddingNewProcess" class="w-full mb-2 mt-2">

                    <!--Skrót procesu-->
                    <label class="block text-white mb-1 mt-2">Skrót procesu:</label>
                    <input
                        v-model="editedProcess.processDesc"
                        type="text"
                        minlength="2"
                        maxlength="25"
                        class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 text-white w-full focus:outline-none focus:ring-2 focus:ring-accent transition-colors" 
                        placeholder="Wprowadź skrót procesu"
                    />

                    <!--Opis procesu-->
                    <label for="processDescription" class="block text-white mb-1 mt-4">Opis procesu:</label>
                    <textarea
                        id="processDescription"
                        v-model="editedProcess.processLongDesc"
                        rows="2"
                        maxlength="75"
                        class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 text-white w-full focus:outline-none focus:ring-2 focus:ring-accent transition-colors resize-none"
                        placeholder="Szczegółowy opis procesu"
                    ></textarea>
                    

                    <!--Kolor procesu-->
                    <label class="block text-white mb-1 mt-4">Kolor procesu:</label>
                    <p class="mb-1 text-accent">(Kliknij na pionek aby zmienić)</p>

                        <div class="relative">
                        <input
                            ref="colorPicker"
                            v-model="editedProcess.processColor"
                            type="color"
                            class="absolute -top-16 -left-1/4 transform opacity-0 pointer-events-none z-50"
                        />
                        
                        <div class="bg-white rounded-lg inline-block border-2">
                            <pawnPreview 
                                :pawnColor="editedProcess.processColor" 
                                class="w-[8rem] h-[8rem]"
                                @click="openColorPicker"
                            />
                        </div>
                    </div>

                </div>

                <div v-show="selectedProcess || isAddingNewProcess" class="flex justify-center w-full text-white">
                        <button 
                            type="submit"
                            :disabled="!isProcessValid"
                            :class="isProcessValid ? 'bg-accent/70 hover:bg-accent' : 'bg-gray-500 cursor-not-allowed'" 
                            class=" py-3 px-6 rounded-md mt-5 mb-3  transition-all">
                            <font-awesome-icon :icon="faSave" class="h-4 mr-2"/>
                            {{ isAddingNewProcess ? 'Dodaj Nowy Proces' : 'Zapisz Zmiany' }} 
                        </button>
                </div>


            </form>
        </div>
    </div>
</template>

<script setup>
    import {ref, onMounted, watch, computed} from 'vue';
    import { useToast } from 'vue-toastification';
    import { faTrash, faPlus, faSave } from '@fortawesome/free-solid-svg-icons';
    import dropDown from '@/components/dropDown.vue';
    import apiConfig from '@/services/apiConfig.js';
    import apiService from '@/services/apiServices.js';
    import pawnPreview from '@/components/game/pawnPreview.vue';

    const toast = useToast();
    

    //Talie kart
    const selectedDeck = ref(null);
    const decksData = ref([]);

    //Procesy
    const processesData = ref([]);
    const selectedProcess = ref(null);
    const isAddingNewProcess = ref(false);
    const editedProcess = ref({
        processDesc: '',
        processLongDesc: '',
        processColor: '',

    })


    const colorPicker = ref(null);

    const openColorPicker = () => {
        colorPicker.value?.click();
    };

    //Dodanie nowego procesu
    const addNewProcess = () => {
        if (!selectedDeck.value) {
            toast.clear();
            toast.error("Najpierw wybierz talię kart", {
                position: "top-right",
            });
            return;
        }

        editedProcess.value = {
            processDesc: '',
            processLongDesc: '',
            processColor: '#6B7280', 
        };
        selectedProcess.value = null;
        isAddingNewProcess.value = true;
    };

    //Usunięcie procesu
    const deleteSelectedProcess = async () => {
        if (!selectedProcess.value) return;

        const processId = selectedProcess.value;
        const processToDelete = processesData.value.find(process => process.processId === processId);

        if (!processToDelete) {
            toast.error("Nie znaleziono procesu do usunięcia", {
                position: "top-right",
            });
            return;
        }

        const confirmDelete = confirm(`Czy na pewno chcesz usunąć proces "${processToDelete.processDesc}"?`);
        if (!confirmDelete) return;

        try {
     
            
            // Tu wywołanie API do usunięcia procesu
            
            processesData.value = processesData.value.filter(process => process.processId !== processId);
            selectedProcess.value = null;
            editedProcess.value = { 
                processDesc: '',
                processLongDesc: '',
                processColor: '',
            };
            
            toast.success("Proces został pomyślnie usunięty", {
                position: "top-right",
            });

        } catch (error) {
            console.error("Błąd przy usuwaniu procesu:", error);
            toast.error("Błąd podczas usuwania procesu", {
                position: "top-right",
            });
        }
        };

    //Zapisanie zmian procesu
    const saveProcessChanges = async () => {
        if (!isProcessValid.value) {
            toast.error("Proszę wprowadzić poprawne dane procesu", {
                position: "top-right",
            });
            return;
        }

        try {
            if (isAddingNewProcess.value) {
               
                toast.clear();
                // Tu wywołanie API do dodania procesu
              
                const newProcess = {
                    ...editedProcess.value,
                    processId: Date.now(), 
                    deckId: selectedDeck.value
                };
                processesData.value.push(newProcess);
                
                toast.success("Nowy proces został dodany", {
                    position: "top-right",
                });
                
            } else {
                toast.clear();
                
                // Tu wywołanie API do aktualizacji procesu
                
                const index = processesData.value.findIndex(process => process.processId === selectedProcess.value);
                if (index !== -1) {
                    processesData.value[index] = { 
                        ...editedProcess.value, 
                        processId: selectedProcess.value 
                    };
                }
                
                toast.success("Proces został zaktualizowany", {
                    position: "top-right",
                });
            }
            
            isAddingNewProcess.value = false;
            selectedProcess.value = null;
            editedProcess.value = {
                processDesc: '',
                processLongDesc: '',
                processColor: '',
            };
            
        } catch (error) {
            console.error("Błąd przy zapisie procesu:", error);
            toast.error("Błąd podczas zapisywania procesu", {
                position: "top-right",
            });
        }
    };

    //Sprawdzenie poprawności procesu
    const isProcessValid = computed(() => {
        return editedProcess.value.processDesc.length >= 2 && 
            editedProcess.value.processDesc.length <= 25 &&
            editedProcess.value.processLongDesc.length >= 8 && 
            editedProcess.value.processLongDesc.length <= 75 &&
            editedProcess.value.processColor.trim() !== '';
    });
    


    //Pobieranie procesów dla wybranej talii kart
    watch(selectedDeck, async (newDeck) => {
        if (newDeck) {
            toast.clear();
            processesData.value = [];
            selectedProcess.value = null;
            isAddingNewProcess.value = false;
            toast.info(`Ładowanie procesów dla talii #${newDeck}...`, {
               position: "top-right",
            }); 
            try {
                toast.clear();
                const response = await apiService.get(apiConfig.processes.getByDeck(newDeck));
                processesData.value = response.data;
                toast.success(`Pomyślnie pobrano procesy`, {
                    position: "top-right",
                });
            } catch (error) {
                toast.clear();
                console.error("Błąd przy pobieraniu procesów:",error);
                toast.error(`Błąd podczas pobierania procesów`, {
                    position: "top-right",
                });
            }
        }
    });


    //Możliwość edycji wybranego procesu
    watch(selectedProcess, (newProcess) => {
        if (newProcess && !isAddingNewProcess.value) {
            const process = processesData.value.find(process => process.processId === newProcess);
            if (process) {
                editedProcess.value = {...process};
            }
        } else if (newProcess && isAddingNewProcess.value) {
            isAddingNewProcess.value = false;
            const process = processesData.value.find(process => process.processId === newProcess);
            if (process) {
                editedProcess.value = {...process};
            }
        } else if (!newProcess && !isAddingNewProcess.value) {
            editedProcess.value = {
                processDesc: '',
                processLongDesc: '',
                processColor: '',
            };
        }
    });


    // Pobieranie talii kart
    onMounted(async () => {
        toast.clear();
        toast.info("Ładowanie dostępnych talii kart...", {
            position: "top-right",
        });
        try {
            toast.clear();
            const response = await apiService.get(apiConfig.admin.deck.getAll);
            decksData.value = response.data;
            toast.success("Pomyślnie pobrano talie kart", {
                position: "top-right",
            });
        } catch (error) {
            console.error("Błąd przy pobieraniu talii:", error);
            toast.error("Błąd podczas pobierania talii kart", {
                position: "top-right",
            });
        }
    });

</script>