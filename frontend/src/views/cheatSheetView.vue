<template>
  <div class="w-full h-full flex-1 flex flex-col justify-center items-center text-center text-white">
    <h1 class="m-8 font-nasalization text-7xl">Drzewo decyzji</h1>
    <img
      :src="drzewko"
      :style="{ width: imageWidth, maxWidth: '100%' }"
      class="w-100 border-2 border-lgray-accent p-2 overflow-hidden rounded-md transition-all duration-300 cursor-pointer"
      alt="ITM logo"
      @click="toggleImageSize"
    >
  </div>
</template>

<script setup>
import { ref } from 'vue'
import drzewko from '@/assets/viewPNGs/DrzewoDecyzji.png'

const isZoomed = ref(false)
const imageWidth = ref('300px')

function toggleImageSize() {
  isZoomed.value = !isZoomed.value
  imageWidth.value = isZoomed.value ? '800px' : '300px'
}
</script>