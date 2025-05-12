<template>
  <div class="w-full h-full flex-1 flex flex-col justify-center items-center text-center text-white">
    <h1 class="m-8 font-nasalization text-7xl">Drzewo decyzji</h1>

    <!-- Obrazek drzewa decyzji -->
    <img
      :src="drzewko"
      :style="{ width: imageWidth, maxWidth: '100%' }"
      class="w-100 border-2 border-lgray-accent p-2 overflow-hidden rounded-md transition-all duration-300 cursor-pointer"
      alt="Drzewo decyzji"
      @click="toggleImageSize"
    />

    <!-- Panel z wyborem PDF i przyciskami -->
    <div class="mt-4 mb-4 flex gap-4 items-center">
      <!-- Select z wyborem PDF -->
      <select
        v-model="selectedPDF"
        class="px-4 py-2 bg-white text-black rounded-md border border-gray-300"
      >
        <option
          v-for="(pdf, index) in availablePDFs"
          :key="index"
          :value="pdf.path"
        >
          {{ pdf.name }}
        </option>
      </select>

      <!-- Przycisk otwarcia PDF -->
      <button
        @click="openPDF"
        class="px-4 py-2 bg-blue-600 rounded-md hover:bg-blue-700 transition-colors"
      >
        Otwórz PDF
      </button>

      <!-- Przycisk otwarcia PDF w nowej karcie -->
      <button
        @click="openPDFInNewTab"
        class="px-4 py-2 bg-green-600 rounded-md hover:bg-green-700 transition-colors"
      >
        Otwórz w nowej karcie
      </button>
    </div>

    <!-- Otwarte PDF-y w iframe -->
    <div
      v-for="(window, index) in pdfWindows"
      :key="index"
      class="mt-6 w-[80%] h-[500px] border border-gray-500 rounded-md relative"
    >
      <button
        @click="closePDF(index)"
        class="px-4 py-2 absolute top-2 right-2 text-white bg-red-600 rounded-full w-8 h-8 flex items-center justify-center hover:bg-red-700"
      >
        &times;
      </button>
      <iframe
        :src="selectedPDF"
        class="w-full h-full rounded-md"
      ></iframe>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import drzewko from '@/assets/viewPNGs/DrzewoDecyzji.png'

// Lista dostępnych PDF-ów
const availablePDFs = [
  {
    name: 'Instrukcja główna',
    path: new URL('@/assets/documents/DIGITAL-WARS_instrukcja_final.pdf', import.meta.url).href,
  },
  {
    name: 'Dodatek 1',
    path: new URL('@/assets/documents/dodatek1.pdf', import.meta.url).href,
  },
  {
    name: 'Dodatek 2',
    path: new URL('@/assets/documents/dodatek2.pdf', import.meta.url).href,
  }
]

const selectedPDF = ref(availablePDFs[0].path)
const pdfWindows = ref([])

// Rozmiar obrazka
const isZoomed = ref(false)
const imageWidth = ref('300px')
function toggleImageSize() {
  isZoomed.value = !isZoomed.value
  imageWidth.value = isZoomed.value ? '800px' : '300px'
}

// Obsługa PDF
function openPDF() {
  pdfWindows.value.push({})
}

function closePDF(index) {
  pdfWindows.value.splice(index, 1)
}

function openPDFInNewTab() {
  window.open(selectedPDF.value, '_blank')
}
</script>
