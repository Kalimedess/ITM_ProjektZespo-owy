<template>
    <div class="flex flex-row w-full gap-5 mt-3 md:mt-5">
      <!-- Opis dolny (pod planszą) -->
      <div class="flex-1">
        <label for="description-down" class="block mb-1">Opis dolny</label>
        <input 
          id="description-down"
          type="text" 
          :value="descriptionDown" 
          class="w-full px-3 py-2 border-2 border-lgray-accent rounded-md bg-transparent"
          placeholder="Wprowadź opis dolny"
          @input="$emit('update:descriptionDown', $event.target.value); $emit('update')" />
      </div>
      
      <!-- Opis lewy (po lewej stronie planszy) -->
      <div class="flex-1">
        <label for="description-left" class="block mb-1">Opis z lewej strony</label>
        <input 
          id="description-left"
          type="text" 
          :value="descriptionLeft" 
          class="w-full px-3 py-2 border-2 border-lgray-accent rounded-md bg-transparent"
          placeholder="Wprowadź opis z lewej strony"
          @input="$emit('update:descriptionLeft', $event.target.value); $emit('update')" />
      </div>
    </div>
  </template>
  
  <script setup>
  defineProps({
    descriptionDown: {
      type: String,
      required: true
    },
    descriptionLeft: {
      type: String,
      required: true
    }
  });
  
  defineEmits(['update:descriptionDown', 'update:descriptionLeft', 'update']);
  </script>