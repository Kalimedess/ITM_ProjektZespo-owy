<template>
    <nav :style="{backgroundColor: navBgColor}"
      class="`w-full py-3 px-6 flex flex-row items-center border-b-2 border-lgray-accent`">
      <div>
         <RouterLink to="/">
              <img :src="logo" class="h-12" alt="ITM logo">
          </RouterLink>
      </div>
  
      <div class="absolute left-1/2 transform -translate-x-1/2">
        <h1 class="text-lg font-semibold text-white">{{ teamName }}</h1>
      </div>
    </nav>
  </template>
  
  <script setup>
  import { computed } from 'vue'
  import { RouterLink } from 'vue-router';
  import logo from '@/assets/logos/ITM_poziom_biale.png'

  const props = defineProps({
  teamName: {
    type: String,
    default: 'Test' // Domyślna wartość, jeśli prop nie zostanie przekazany
  },
  navBgColor: {
    type: String,
    default: '#1E3A8A'
  }
});
  
  // Zmienna koloru tła
  const navBgColor = computed(() => props.navBgColor);
  
  // Zmienna z nazwą drużyny
  const teamName = computed(() => props.teamName);
  </script>