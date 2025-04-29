<template>
  <div class="w-full h-full flex-1 flex flex-col justify-center items-center text-center text-white">
    <h1 class="m-8 font-nasalization text-7xl">Drzewo decyzji</h1>
    
    <img
      :src="drzewko"
      :style="{ width: imageWidth, maxWidth: '100%' }"
      class="w-100 border-2 border-lgray-accent p-2 overflow-hidden rounded-md transition-all duration-300 cursor-pointer"
      alt="ITM logo"
      @click="toggleImageSize"
    />

    <!-- Przycisk otwierający PDF -->
    <div class="mt-4 mb-4 flex gap-4">
      <button
        @click="openPDF"
        class="px-4 py-2 bg-blue-600 rounded-md hover:bg-blue-700 transition-colors"
      >
        Otwórz instrukcję PDF
      </button>

      <!-- Nowy przycisk otwierający PDF w nowej karcie -->
      <button
        @click="openPDFInNewTab"
        class="px-4 py-2 bg-green-600 rounded-md hover:bg-green-700 transition-colors"
      >
        Otwórz w nowej karcie
      </button>
    </div>

    <!-- Otwarte okna z PDF -->
    <div v-for="(window, index) in pdfWindows" :key="index" class="mt-6 w-[80%] h-[500px] border border-gray-500 rounded-md relative">
      <button
        @click="closePDF(index)"
        class="px-4 py-2 absolute top-2 right-2 text-white bg-red-600 rounded-full w-8 h-8 flex items-center justify-center hover:bg-red-700"
      >
        &times;
      </button>
      <iframe
        :src="pdfSrc"
        class="w-full h-full rounded-md"
      ></iframe>
    </div>
  </div>
  <div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import drzewko from '@/assets/viewPNGs/DrzewoDecyzji.png'
import pdfPath from '@/assets/documents/DIGITAL-WARS_instrukcja_final.pdf'

// Obsługa rozmiaru obrazka
const isZoomed = ref(false)
const imageWidth = ref('300px')
function toggleImageSize() {
  isZoomed.value = !isZoomed.value
  imageWidth.value = isZoomed.value ? '800px' : '300px'
}

// Obsługa otwierania PDF
const pdfWindows = ref([])
const pdfSrc = pdfPath

function openPDF() {
  pdfWindows.value.push({})
}

function closePDF(index) {
  pdfWindows.value.splice(index, 1)
}

function openPDFInNewTab() {
  window.open(pdfSrc, '_blank')
}
</script>

