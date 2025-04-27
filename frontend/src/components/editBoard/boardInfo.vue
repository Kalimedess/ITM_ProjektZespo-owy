<template>
    <div>
      <!-- Pole nazwy planszy -->
      <div class="mt-8 mb-5">
        <label for="board-name" class="block mb-1">Nazwa planszy</label>
        <input 
          id="board-name"
          :value="name"
          type="text" 
          class="w-full px-3 py-2 border-2 border-lgray-accent rounded-md bg-transparent"
          placeholder="Wprowadź nazwę planszy"
          @input="$emit('update:name', $event.target.value); $emit('update')" />
      </div>
  
      <!-- Informacje o wymiarach planszy -->
      <div class="flex flex-row w-full items-center justify-center gap-5 mt-8 mb-5">
        <div class="border-2 border-lgray-accent py-2 px-2 rounded-md w-60 text-center">
          <span>Kolumny: {{ cols }}</span>
        </div>
        <div class="border-2 border-lgray-accent py-2 px-2 rounded-md w-60 text-center">
          <span>Rzędy: {{ rows }}</span>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  defineProps({
    name: {
      type: String,
      required: true
    },
    cols: {
      type: Number,
      required: true
    },
    rows: {
      type: Number,
      required: true
    }
  });
  
  defineEmits(['update:name', 'update']);
  </script>