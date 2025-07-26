<template>
  <div class="w-full h-full flex-1 flex flex-col justify-center items-center text-center text-white">
    <h1 class="m-8 font-nasalization text-7xl">Drzewo decyzji</h1>

    <!--Wrapper img i overlay-->
    <div
    ref="imageWrapper"
    class="relative inline-block"
    @click="toggleImageSize"
    >
      <!-- Obrazek drzewa decyzji -->
      <img
        :src="drzewko"
        :style="{ width: imageWidth, maxWidth: '100%' }"
        class="block w-100 border-2 border-lgray-accent p-2 overflow-hidden rounded-md transition-all duration-300 cursor-pointer"
        alt="Drzewo decyzji"
      />
      <!--Overlay z pozycjami-->
      <div
      v-for="point in points"
      :key="point.id"
      class="absolute flex items-center justify-center rounded-full transition-all duration-300 cursor-pointer"
      :style="{
        width: `${pointSize * 1.8}em`,
        height: `${pointSize * 1.8}em`,
        background: 'white',
        border: `0.15em solid ${point.team === 1 ? 'red' : 'blue'}`,
        left: `${point.x * 100}%`,
        top: `${point.y * 100}%`,
        transform: 'translate(-50%, -50%)'
      }"
    >
      <div
        class="rounded-full"
        :style="{
          width: `${pointSize}em`,
          height: `${pointSize}em`,
          background: point.team === 1 ? 'red' : 'blue'
        }"
      ></div>
    </div>

    </div>

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
import { ref, computed, onMounted } from 'vue'
import drzewko from '@/assets/viewPNGs/DrzewoDecyzji.png'
import apiService from '@/services/apiServices'
import apiConfig from '@/services/apiConfig'

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

const props = defineProps({
  gameId: {
    Number,
    default: 1
  },
  teamId: {
    Number,
  },

})


// Rozmiar obrazka
const isZoomed = ref(false)
const imageWidth = ref('300px')
const imageWidthInt = ref(300)
const imageHeightInt = ref(621)
const pointSize = ref(0.5)
const points = ref([
  { id: 1, team: 1, x: 2, y: 1 },
  { id: 2, team: 1, x: 0, y: 0 },
  { id: 3, team: 2, x: 0.5, y: 0.5 },
  { id: 4, team: 2, x: 0.8, y: 0.8 },
  ])
function toggleImageSize() {
  isZoomed.value = !isZoomed.value
  imageWidth.value = isZoomed.value ? '800px' : '300px'
  imageWidthInt.value = isZoomed.value ? 800 : 300
  imageHeightInt.value = isZoomed.value ? 1656 : 621
  pointSize.value = isZoomed.value ? 2 : 0.5
}

  
function separateOverlaidPoints(){

}

const enablersMap = async () => {
  const result = await apiService.get(apiConfig.games.getEnablersMap);

  console.log("Dane enablerów:", result);
} 

const teamEntry = async () => {

  const result = await apiService.get(apiConfig.games.getTeamEntry, {params: { gameId: props.gameId, teamId: props.teamId}});

  console.log("Dane dla drużyny:", result);
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

onMounted(async () => {
  await enablersMap();
  await teamEntry();

});
</script>
