<template>
  <div v-if="boards.length > 0" class="mt-3 md:mt-4 w-full">
    <label for="board-select" class="block mb-0.5 md:mb-1 text-sm md:text-base">{{props.activeView === 'add' ? 'Wybierz szablon' : 'Wybierz planszę do edycji' }}</label>
    
    <div class="flex items-center gap-1 md:gap-2 text-sm md:text-base">
      <select
        id="board-select"
        :value="modelValue"
        class="flex-1 px-2 py-1.5 md:px-3 md:py-2 border-2 border-lgray-accent rounded-md bg-tertiary text-white text-sm md:text-base"
        @change="$emit('update:modelValue', Number($event.target.value))">
        <option :value="null" disabled >Wybierz planszę</option>
        <option v-for="board in boards" :key="board.BoardId" :value="board.BoardId">
          {{ board.Name }}
        </option>
      </select>

      <button
        v-if="modelValue"
        @click="$emit('delete')"
        class="p-1.5 md:p-2 hover:bg-red-900/50 rounded flex-shrink-0"
        title="Usuń wybraną planszę">
        <font-awesome-icon :icon="faTrash" class="h-3.5 md:h-4 text-red-500"/>
      </button>
    </div>
  </div>
</template>

<script setup>

import { faTrash } from '@fortawesome/free-solid-svg-icons';

const props = defineProps({
  modelValue: {
    type: Number,
    default: null
  },
  boards: {
    type: Array,
    required: true
  },
  activeView:{
    type: String,
    required: true
  }
});

defineEmits(['update:modelValue', 'delete']);
</script>