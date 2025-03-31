<template>
    <div class="relative w-full">
      <div  
        @click="isOpen = !isOpen"
        class="text-white flex flex-row justify-between items-center border-2 border-lgray-accent rounded-md px-2 py-2 mt-5 cursor-pointer"
      >
        <div>
          {{ selectedCard ? `#${selectedCard.id} ${selectedCard.title}` : 'Wybierz kartę do edycji' }}
        </div>
        <div class="text-center">
          <font-awesome-icon :icon="isOpen ? faArrowUp : faArrowDown" class="h-3" />
        </div>
      </div>
  
      <div v-if="isOpen"
        class="absolute z-10 w-full mt-0.5 border-2 border-lgray-accent bg-primary rounded-md">
        <div class="max-h-32 overflow-y-auto py-1 text-left">
          <div 
            v-for="card in cards"
            :key="card.id"
            @click="selectCard(card.id)"
            class="px-3 py-2 cursor-pointer hover:bg-accent text-white"
            :class="{'bg-accent': modelValue === card.id}" 
          >
            <span>#{{ card.id }} {{ card.title }}</span>
          </div>
        </div>
      </div>
    </div>
  </template>
    
  <script setup>
  import { ref, defineProps, defineEmits, computed } from 'vue';
  import { faArrowUp, faArrowDown } from '@fortawesome/free-solid-svg-icons';
  
  const props = defineProps({
    cards: {
      type: Array,
      required: true
    },
    modelValue: {
      type: Number,
      default: null
    }
  });
  
  const emit = defineEmits(['update:modelValue']);
  
  const isOpen = ref(false);
  

  const selectedCard = computed(() => {
    if (props.modelValue) {
      return props.cards.find(card => card.id === props.modelValue);
    }
    return null;
  });
  
  const selectCard = (id) => {
    emit('update:modelValue', id);
    isOpen.value = false;
  };
  </script>