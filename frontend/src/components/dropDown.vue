<template>
  <div class="relative w-full">
    <div  
      @click="isOpen = !isOpen"
      class="text-white flex flex-row justify-between items-center border-2 border-lgray-accent rounded-md px-2 py-2 mt-5 cursor-pointer"
    >
      <div>
        <slot name="selected-display" :selected="selectedItem">
          {{ selectedItem ? displayValue(selectedItem) : placeholder }}
        </slot>
      </div>
      <div class="text-center">
        <font-awesome-icon :icon="isOpen ? faArrowUp : faArrowDown" class="h-3" />
      </div>
    </div>

    <div v-if="isOpen"
      class="absolute z-10 w-full mt-0.5 border-2 border-lgray-accent bg-primary rounded-md">
      <div class="max-h-32 overflow-y-auto py-1 text-left">
        <div 
          v-for="item in items"
          :key="getItemKey(item)"
          @click="selectItem(getItemKey(item))"
          class="px-3 py-2 cursor-pointer hover:bg-accent text-white"
          :class="{'bg-accent': modelValue === getItemKey(item)}" 
        >
          <slot name="item-display" :item="item">
            {{ displayValue(item) }}
          </slot>
        </div>
      </div>
    </div>
  </div>
</template>
  
<script setup>
import { ref, defineProps, defineEmits, computed } from 'vue';
import { faArrowUp, faArrowDown } from '@fortawesome/free-solid-svg-icons';

const props = defineProps({
  items: {
    type: Array,
    required: true
  },
  modelValue: {
    type: [Number, String, Object],
    default: null
  },
  placeholder: {
    type: String,
    default: "Wybierz element"
  },
  itemKey: {
    type: String,
    default: "id"
  },
  itemLabel: {
    type: String,
    default: "title"
  },
  displayFormat: {
    type: Function,
    default: null
  }
});

const emit = defineEmits(['update:modelValue']);

const isOpen = ref(false);

const getItemKey = (item) => {
  return typeof item === 'object' ? item[props.itemKey] : item;
};

const displayValue = (item) => {
  if (props.displayFormat) {
    return props.displayFormat(item);
  }
  
  return typeof item === 'object' ? item[props.itemLabel] : item;
};

const selectedItem = computed(() => {
  if (props.modelValue === null) return null;
  return props.items.find(item => getItemKey(item) === props.modelValue);
});

const selectItem = (value) => {
  emit('update:modelValue', value);
  isOpen.value = false;
};
</script>