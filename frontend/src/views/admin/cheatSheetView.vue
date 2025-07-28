<template>
  <div class="w-full h-full flex-1 flex flex-col justify-center items-center text-center text-white">
    <h1 class="m-8 font-nasalization text-7xl">Drzewo decyzji</h1>

    <div class="w-full flex justify-center items-start gap-8 px-4">
      <!-- Wrapper dla drzewa decyzji z obsługą zoomu -->
      <div
        @click="toggleZoom"
        :class="[
          'border-2 border-lgray-accent p-2 rounded-md bg-gray-800 transition-all duration-500 cursor-pointer',
          isZoomed ? 'w-[80%]' : 'w-[50%]'
        ]"
      >
        <DecisionTree
          v-if="nodes.length > 0"
          :nodes="nodes"
          :edges="edges"
          :edge-colors="edgeColorsConfig"
        />
        <div v-else class="flex items-center justify-center h-[60vh]">
          <p>Ładowanie danych drzewa...</p>
        </div>
      </div>

      <!-- Legenda -->
      <div class="w-[25%] flex-shrink-0">
        <DecisionTreeLegend />
      </div>
    </div>

    <!-- Panel z wyborem PDF i przyciskami (pozostaje bez zmian) -->
    <div class="mt-4 mb-4 flex gap-4 items-center">
      <select
        v-model="selectedPDF"
        class="px-4 py-2 bg-white text-black rounded-md border border-gray-300"
      >
        <option v-for="(pdf, index) in availablePDFs" :key="index" :value="pdf.path">
          {{ pdf.name }}
        </option>
      </select>
      <button
        @click="openPDF"
        class="px-4 py-2 bg-blue-600 rounded-md hover:bg-blue-700 transition-colors"
      >
        Otwórz PDF
      </button>
      <button
        @click="openPDFInNewTab"
        class="px-4 py-2 bg-green-600 rounded-md hover:bg-green-700 transition-colors"
      >
        Otwórz w nowej karcie
      </button>
    </div>

    <!-- Otwarte PDF-y (pozostaje bez zmian) -->
    <div
      v-for="(window, index) in pdfWindows"
      :key="index"
      class="mt-6 w-[80%] h-[500px] border border-gray-500 rounded-md relative"
    >
      <button
        @click="closePDF(index)"
        class="px-4 py-2 absolute top-2 right-2 text-white bg-red-600 rounded-full w-8 h-8 flex items-center justify-center hover:bg-red-700"
      >
        ×
      </button>
      <iframe :src="selectedPDF" class="w-full h-full rounded-md"></iframe>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import DecisionTree from '@/components/decisionTree.vue' // Import nowego komponentu
import DecisionTreeLegend from '@/components/decisionTreeLegend.vue'; // Import legendy
import apiService from '@/services/apiServices'
import apiConfig from '@/services/apiConfig'
import * as dagre from 'dagre' 
import { edgeColors as edgeColorsConfig } from '@/assets/treeLayout.js'; 


const isZoomed = ref(false);
function toggleZoom() {
  isZoomed.value = !isZoomed.value;
}

const saturation = ref(65);
const lightness = ref(55);
const tresholdData = ref(2);

// --- Dane dla drzewa ---
const nodes = ref([])
const edges = ref([])

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

// FUNKCJA GENERUJĄCA UNIKALNY KOLOR HSL NA PODSTAWIE ID
function generateDistinctColorById(id, saturation, lightness) {
  if (id === 0) return '#A1A1AA'; // Domyślny szary

  // Prawdziwa wartość "złotego podziału" dla najlepszego rozproszenia
  const goldenRatioConjugate = 0.61803398875;
  
  let seed = id * 137; // Mnożenie przez liczbę pierwszą
  seed = (seed + goldenRatioConjugate) % 1;

  // Używamy pełnego koła barw (360 stopni)
  const hue = Math.floor(seed * 360);

  // Zwracamy kolor w HSL. Domyślne wartości są teraz żywe i widoczne.
  return `hsl(${hue}, ${saturation}%, ${lightness}%)`;
}

// NOWA FUNKCJA - heurystyka do określania typu węzła
function determineNodeType(nodeId, inDegree, outDegree, threshold = tresholdData.value) {
    // Jeśli węzeł jest punktem startowym (brak wejść, ale są wyjścia) - jest kluczowy
    if (inDegree === 0 && outDegree > 0) {
        return 'key';
    }
    // Jeśli węzeł jest zaangażowany w więcej niż `threshold` połączeń - jest kluczowy
    if ((inDegree + outDegree) > threshold) {
        return 'key';
    }
    // W przeciwnym razie jest normalny
    return 'normal';
}

const dynamicEdgeColors = ref({});


const selectedPDF = ref(availablePDFs.length > 0 ? availablePDFs[0].path : '')
const pdfWindows = ref([])

const props = defineProps({
  gameId: {
    type: Number,
    default: 1
  },
  teamId: {
    type: Number,
  },
})

function processTreeDataWithDagre(enablersMap) {
  const g = new dagre.graphlib.Graph();
  g.setGraph({ rankdir: 'TB', nodesep: 50, ranksep: 100 });
  g.setDefaultEdgeLabel(() => ({}));

  const allNodeIds = new Set();
  const processedEdges = [];

  for (const cardId in enablersMap) {
    const enablers = enablersMap[cardId];
    const cardIdInt = parseInt(cardId);
    allNodeIds.add(cardIdInt);
    enablers.forEach(enablerId => {
      allNodeIds.add(enablerId);
      processedEdges.push({ from: enablerId, to: cardIdInt });
    });
  }

  const inDegrees = new Map();
  const outDegrees = new Map();
  allNodeIds.forEach(id => {
    inDegrees.set(id, 0);
    outDegrees.set(id, 0);
  });

  processedEdges.forEach(edge => {
    outDegrees.set(edge.from, (outDegrees.get(edge.from) || 0) + 1);
    inDegrees.set(edge.to, (inDegrees.get(edge.to) || 0) + 1);
  });

  const edgeColorsResult = {};
  
  allNodeIds.forEach(id => {
    // KROK 2: Użyj heurystyki do dynamicznego określenia typu
    const nodeType = determineNodeType(
      id,
      inDegrees.get(id),
      outDegrees.get(id)
    );

    g.setNode(id.toString(), {
      label: id,
      width: 80,
      height: 50,
      type: nodeType // <-- Używamy dynamicznie określonego typu
    });
    // GENERUJ KOLOR DYNAMICZNIE
    edgeColorsResult[id] = generateDistinctColorById(
        id, 
        saturation.value, 
        lightness.value
    );
  });
  
  dynamicEdgeColors.value = edgeColorsResult;

  processedEdges.forEach(edge => g.setEdge(edge.from.toString(), edge.to.toString()));

  dagre.layout(g);

  const finalNodes = g.nodes().map(nodeId => {
    const node = g.node(nodeId);
    return {
      id: parseInt(nodeId),
      x: node.x,
      y: node.y,
      type: node.type // <-- Przekazujemy typ określony przez dagre
    };
  });
  
  nodes.value = finalNodes;
  edges.value = processedEdges;
}

// Pobieranie danych
const fetchEnablersMap = async () => {
  try {
    const response = await apiService.get(apiConfig.games.getEnablersMap);
    console.log("Dane enablerów:", response.data);

    return response.data;
  } catch (error) {
    console.error("Błąd podczas pobierania mapy enablerów:", error);
  }
} 

const fetchTeamEntry = async () => {
  try {
    const result = await apiService.get(apiConfig.games.getTeamEntry, {params: { gameId: props.gameId, teamId: props.teamId}});
    console.log("Dane dla drużyny:", result);
    // Tutaj w przyszłości możesz użyć tych danych do podświetlenia zagranych kart
  } catch (error) {
    console.error("Błąd podczas pobierania danych drużyny:", error);
  }
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
  fetchTeamEntry();
  // Nie potrzebujemy już `fetchAllCards`, bo typ jest generowany dynamicznie
  const enablersMap = await fetchEnablersMap();

  if (enablersMap) {
    // `allCardsData` może być pustą tablicą
    processTreeDataWithDagre(enablersMap); 
  } else {
    console.error("Nie udało się pobrać danych do narysowania drzewa.");
  }
});
</script>
