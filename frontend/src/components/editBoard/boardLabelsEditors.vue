<template>
    <div class="flex flex-row w-full gap-5 mt-3 md:mt-5">
      <!-- Etykiety górne -->
      <div class="flex-1">
        <label class="block mb-1">Etykiety górne</label>
        
        <!-- Lista istniejących etykiet górnych -->
        <div class="border-2 border-lgray-accent px-2 py-2 rounded-md mb-1.5 md:mb-3">
          <div class="flex flex-col gap-2">
            <div 
              v-for="(label, index) in labelsUp" 
              :key="index" 
              class="flex items-center border rounded p-2">
              <input 
                type="text" 
                :value="label" 
                class="flex-1 bg-transparent border-none focus:outline-none"
                @input="updateLabelUp(index, $event.target.value)" />
              <!-- Przycisk usuwania etykiety -->
              <button 
                type="button" 
                @click="removeLabelUp(index)" 
                class="ml-auto p-1 text-red-500 hover:bg-red-100 rounded"
                :disabled="labelsUp.length <= 1"
                title="Usuń etykietę">
                <font-awesome-icon :icon="faTrash" class="h-3" />
              </button>
            </div>
          </div>
        </div>
        
        <!-- Formularz dodawania nowej etykiety górnej -->
        <div class="flex items-center gap-1.5 sm:gap-2">
          <input
            type="text"
            v-model="newLabelUp"
            class="w-full text-sm md:text-base px-2 sm:px-3 py-1.5 sm:py-2 border-2 border-lgray-accent rounded-md bg-transparent"
            placeholder="Nowa etykieta" />
          <button
            type="button"
            @click="addLabelUp"
            class="flex-shrink-0 text-sm md:text-base px-2 py-1.5 sm:py-2 rounded-md border-2 border-lgray-accent hover:border-accent transition-colors duration-300">
            <font-awesome-icon :icon="faPlus" class="h-4 text-accent" />
            Dodaj
          </button>
        </div>
      </div>
      
      <!-- Etykiety prawe -->
      <div class="flex-1">
        <label class="block mb-1">Etykiety prawe</label>
        
        <!-- Lista istniejących etykiet prawych -->
        <div class="border-2 border-lgray-accent px-2 py-2 rounded-md mb-3">
          <div class="flex flex-col gap-2">
            <div 
              v-for="(label, index) in labelsRight" 
              :key="index" 
              class="flex items-center border rounded p-2">
              <input 
                type="text" 
                :value="label" 
                class="flex-1 bg-transparent border-none focus:outline-none"
                @input="updateLabelRight(index, $event.target.value)" />
              <!-- Przycisk usuwania etykiety -->
              <button 
                type="button" 
                @click="removeLabelRight(index)" 
                class="ml-auto p-1 text-red-500 hover:bg-red-100 rounded"
                :disabled="labelsRight.length <= 1"
                title="Usuń etykietę">
                <font-awesome-icon :icon="faTrash" class="h-3" />
              </button>
            </div>
          </div>
        </div>
        
        <!-- Formularz dodawania nowej etykiety prawej -->
        <div class="flex items-center gap-1.5 sm:gap-2">
          <input
            type="text"
            v-model="newLabelRight"
            class="w-full text-sm md:text-base px-2 sm:px-3 py-1.5 sm:py-2 border-2 border-lgray-accent rounded-md bg-transparent"
            placeholder="Nowa etykieta" />
          <button
            type="button"
            @click="addLabelRight"
            class="flex-shrink-0 text-sm md:text-base px-2 py-1.5 sm:py-2 rounded-md border-2 border-lgray-accent hover:border-accent transition-colors duration-300">
            <font-awesome-icon :icon="faPlus" class="h-4 text-accent" />
            Dodaj
          </button>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  import { faPlus, faTrash } from '@fortawesome/free-solid-svg-icons';
  import { useToast } from 'vue-toastification';
  
  const toast = useToast();
  const newLabelUp = ref('');
  const newLabelRight = ref('');
  
  const props = defineProps({
    labelsUp: {
      type: Array,
      required: true
    },
    labelsRight: {
      type: Array,
      required: true
    }
  });
  
  const emit = defineEmits(['update:labelsUp', 'update:labelsRight', 'update']);
  
  // Funkcje dla etykiet górnych
  const addLabelUp = () => {
    if (newLabelUp.value.trim()) {
      const updatedLabels = [...props.labelsUp, newLabelUp.value];
      emit('update:labelsUp', updatedLabels);
      newLabelUp.value = '';
      emit('update');
    } else {
      toast.warning("Etykieta nie może być pusta!");
    }
  };
  
  const removeLabelUp = (index) => {
    if (props.labelsUp.length > 1) {
      const updatedLabels = [...props.labelsUp];
      updatedLabels.splice(index, 1);
      emit('update:labelsUp', updatedLabels);
      emit('update');
    } else {
      toast.warning("Musi istnieć co najmniej jedna etykieta górna!");
    }
  };
  
  const updateLabelUp = (index, value) => {
    const updatedLabels = [...props.labelsUp];
    updatedLabels[index] = value;
    emit('update:labelsUp', updatedLabels);
    emit('update');
  };
  
  // Funkcje dla etykiet prawych
  const addLabelRight = () => {
    if (newLabelRight.value.trim()) {
      const updatedLabels = [...props.labelsRight, newLabelRight.value];
      emit('update:labelsRight', updatedLabels);
      newLabelRight.value = '';
      emit('update');
    } else {
      toast.warning("Etykieta nie może być pusta!");
    }
  };
  
  const removeLabelRight = (index) => {
    if (props.labelsRight.length > 1) {
      const updatedLabels = [...props.labelsRight];
      updatedLabels.splice(index, 1);
      emit('update:labelsRight', updatedLabels);
      emit('update');
    } else {
      toast.warning("Musi istnieć co najmniej jedna etykieta prawa!");
    }
  };
  
  const updateLabelRight = (index, value) => {
    const updatedLabels = [...props.labelsRight];
    updatedLabels[index] = value;
    emit('update:labelsRight', updatedLabels);
    emit('update');
  };
  </script>