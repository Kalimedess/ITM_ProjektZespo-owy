<template>
    <div v-if="boardStore.boards.length > 0" class="mt-4 w-full">
      <label for="board-select" class="block mb-1">Wybierz planszę do edycji</label>
      <div class="flex items-center gap-2">
        <select 
          id="board-select"
          :value="modelValue"
          class="w-full px-3 py-2 border-2 border-lgray-accent rounded-md bg-tertiary text-white"
          @change="$emit('update:modelValue', $event.target.value); $emit('load')">
          <option value="" disabled>Wybierz planszę</option>
          <option v-for="board in boardStore.boards" :key="board.id" :value="board.id">
            {{ board.name }}
          </option>
        </select>
        <!-- Przycisk usuwania -->
        <button 
          v-if="modelValue"
          @click="$emit('delete')"
          class="p-2 hover:bg-red-100 rounded" 
          title="Usuń wybraną planszę">
          <font-awesome-icon :icon="faTrash" class="h-4 text-red-500"/>
        </button>
      </div>
    </div>
  </template>
  
  <script setup>
  import { faTrash } from '@fortawesome/free-solid-svg-icons';
  import { useBoardStore } from '@/stores/boardStore';
  
  const props = defineProps({
    modelValue: {
      type: String,
      default: ''
    }
  });
  
  defineEmits(['update:modelValue', 'load', 'delete']);
  
  const boardStore = useBoardStore();
  </script>