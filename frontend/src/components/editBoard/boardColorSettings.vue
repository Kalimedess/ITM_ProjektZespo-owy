<template>
  <div>
    <!-- Sekcja wyboru kolorów podstawowych -->
    <div class="flex flex-row w-full items-center justify-center gap-5 mt-8 mb-5">
      <!-- Kolor wypełnienia komórki -->
      <div class="border-2 border-lgray-accent py-2 px-2 rounded-md w-60 text-center">
        <label for="cell-color" class="block mb-1">Kolor komórki</label>
        <input 
          id="cell-color"
          :value="cellColor"
          type="color" 
          class="w-12 h-10 border rounded mx-auto" 
          @input="$emit('update:cellColor', $event.target.value); $emit('update')" />
        <div class="text-sm mt-2">{{ cellColor }}</div>
      </div>

      <!-- Kolor obramowania komórki -->
      <div class="border-2 border-lgray-accent py-2 px-2 rounded-md w-60 text-center">
        <label for="border-color" class="block mb-1">Kolor obramowania komórki</label>
        <input 
          id="border-color"
          :value="borderColor"
          type="color" 
          class="w-12 h-10 border rounded mx-auto" 
          @input="$emit('update:borderColor', $event.target.value); $emit('update')" />
        <div class="text-sm mt-2">{{ borderColor }}</div>
      </div>
    </div>
    
    <!-- Sekcja kolorów stref na planszy -->
    <div class="mb-4 mt-8">
      <label class="block mb-1">Kolory granic planszy</label>
      
      <!-- Lista istniejących kolorów stref -->
      <div class="border-2 border-lgray-accent px-2 py-2 rounded-md mb-3">
        <div class="grid grid-cols-4 gap-2">
          <div 
            v-for="(color, index) in borderColors" 
            :key="index" 
            class="flex items-center border rounded p-1">
            <!-- Podgląd koloru -->
            <div 
              class="w-8 h-8 rounded-sm mr-1" 
              :style="{ backgroundColor: color }"></div>
            <!-- Selektor koloru -->
            <input 
              type="color" 
              :value="color" 
              class="w-8 h-8"
              @input="updateBorderColor(index, $event.target.value)" />
            <!-- Przycisk usuwania koloru -->
            <button 
              type="button" 
              @click="removeColor(index)" 
              class="ml-auto p-1 text-red-500 hover:bg-red-100 rounded"
              :disabled="borderColors.length <= 1">
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
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { faPlus, faTrash } from '@fortawesome/free-solid-svg-icons';
import { useToast } from 'vue-toastification';

const toast = useToast();
const newColor = ref('#000000');

const props = defineProps({
  cellColor: {
    type: String,
    required: true
  },
  borderColor: {
    type: String,
    required: true
  },
  borderColors: {
    type: Array,
    required: true
  }
});

const emit = defineEmits(['update:cellColor', 'update:borderColor', 'update:borderColors', 'update']);

const addColor = () => {
  const updatedColors = [...props.borderColors, newColor.value];
  emit('update:borderColors', updatedColors);
  emit('update');
};

const removeColor = (index) => {
  if (props.borderColors.length > 1) {
    const updatedColors = [...props.borderColors];
    updatedColors.splice(index, 1);
    emit('update:borderColors', updatedColors);
    emit('update');
  } else {
    toast.warning("Musi istnieć co najmniej jeden kolor granicy!");
  }
};

const updateBorderColor = (index, value) => {
  const updatedColors = [...props.borderColors];
  updatedColors[index] = value;
  emit('update:borderColors', updatedColors);
  emit('update');
};
</script>