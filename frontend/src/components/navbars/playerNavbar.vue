<template>
    <nav :class="navClasses">
      <div>
        <img :src="logo" class="h-12" alt="ITM logo">
      </div>
  
      <div class="absolute left-1/2 transform -translate-x-1/2">
        <h1 class="text-lg font-semibold text-white">{{ teamName }}</h1>
      </div>
    </nav>
  </template>
  
  <script setup>
  import { ref, computed } from 'vue'
  import logo from '@/assets/logos/ITM_poziom_biale.png'
  
  // Zmienna koloru tła
  const navBgColor = ref('bg-blue-700')
  
  // Zmienna z nazwą drużyny
  const teamName = ref('Drużyna 15')
  
  // Klasa CSS dla nav
  const navClasses = computed(() =>
    `w-full ${navBgColor.value} py-3 px-6 flex flex-row justify-between items-center border-b-2 border-lgray-accent`
  )
  </script>