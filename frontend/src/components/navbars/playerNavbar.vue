<template>
  <nav :style="{ backgroundColor: navBgColor }"
       class="w-full py-3 px-6 flex items-center relative border-b-2 border-lgray-accent">

    <!-- Logo -->
    <div>
      <RouterLink to="/">
        <img :src="logo" class="h-12" alt="ITM logo" />
      </RouterLink>
    </div>

    <!-- Nazwa drużyny na środku -->
    <div class="absolute left-1/2 transform -translate-x-1/2">
      <h1 class="text-lg font-semibold text-white">{{ teamName }}</h1>
    </div>

    <!-- Przycisk AI w prawym górnym rogu -->
    <div class="absolute top-3 right-6">
      <button class="w-10 h-10 rounded-full bg-white text-blue-900 font-bold shadow-md hover:bg-gray-200 transition">
        AI
      </button>
    </div>
  </nav>
</template>

<script setup>
import { computed } from 'vue'
import { RouterLink } from 'vue-router'
import logo from '@/assets/logos/ITM_poziom_biale.png'

const props = defineProps({
  teamName: {
    type: String,
    default: 'Test'
  },
  navBgColor: {
    type: String,
    default: '#1E3A8A'
  }
})

const navBgColor = computed(() => props.navBgColor)
const teamName = computed(() => props.teamName)
</script>
