<template>
  <div ref="container" class="flex justify-center items-center h-full w-full">
    <svg :width="svgWidth" :height="svgHeight" class="max-w-full">
      <g ref="board"></g>
    </svg>
  </div>
</template>
  
<script setup>
  import { ref, computed, onMounted, onUnmounted, watch } from "vue";
  import * as d3 from "d3";
  import { useBoardStore } from '../stores/boardStore';
  import { useToast } from 'vue-toastification';

  // Props dla trybu podglądu
  const props = defineProps({
    previewMode: {
      type: Boolean,
      default: false
    },
    previewConfig: {
      type: Object,
      default: null
    }
  });

  // Referencje do elementów
  const board = ref(null);
  const container = ref(null);

  // Inicjalizacja sklepu
  const boardStore = useBoardStore();

  // Inicjalizacja komunikatów
  const toast = useToast();

  // Wymiary kontenera
  const containerWidth = ref(0);
  const containerHeight = ref(0);
  
  // Funkcja aktualizująca wymiary kontenera
  const updateContainerSize = () => {
    if (container.value) {
      containerWidth.value = container.value.clientWidth;
      containerHeight.value = container.value.clientHeight;
    }
  };

  // Pobierz konfigurację z propsów (w trybie podglądu) lub z currentBoard w sklepie
  const BoardData = computed(() => {
    // Użyj konfiguracji z propsów jeśli jesteśmy w trybie podglądu
    if (props.previewMode && props.previewConfig) {
      return props.previewConfig;
    }
    
    // W przeciwnym razie użyj konfiguracji z currentBoard
    if (boardStore.currentBoard) {
      return boardStore.currentBoard.config;
    }
    
    // Fallback na domyślne wartości, jeśli nie ma aktualnej planszy
    return {
      rows: 8,
      cols: 8,
      borderColor: "#595959",
      cellColor: "#fefae0",
      margin: 40,
      borderColors: ['#008000', '#FFFF00', '#FFA500', '#FF0000'],
      labelsUp: ['Podstawowa kordynacja', 'Standaryzacja procesów', 'Zintegrowane działania', 'Pełna integracja strategiczna'],
      labelsRight: ['Nowicjusz','Naśladowca','Innowator','Lider cyfrowy'],
      descriptionDown: "Poziom integracji wew/zew",
      descriptionLeft: "Zawansowanie Cyfrowe",
    };
  });
  
  // Dynamicznie obliczany rozmiar komórki w zależności od rozmiaru kontenera
  const cellSize = computed(() => {
    if (containerWidth.value === 0 || containerHeight.value === 0) return 30; 
    
    
    const availableWidth = containerWidth.value - BoardData.value.margin * 3;
    const availableHeight = containerHeight.value - BoardData.value.margin * 3;
    
    const cellWidthBased = availableWidth / BoardData.value.cols;
    const cellHeightBased = availableHeight / BoardData.value.rows;
    
    
    return Math.min(cellWidthBased, cellHeightBased);
  });
  
  // Obliczenie całkowitego rozmiaru planszy
  const boardSizeX = computed(() => BoardData.value.cols * cellSize.value);
  const boardSizeY = computed(() => BoardData.value.rows * cellSize.value);
  
  // Całkowite wymiary SVG
  const svgWidth = computed(() => boardSizeX.value + BoardData.value.margin * 3);
  const svgHeight = computed(() => boardSizeY.value + BoardData.value.margin * 3);
  
  // Etykiety dla osi X na dole - Generowane litery zależnie od ilości kolumn
  const labelsX = computed(() =>
    Array.from({ length: BoardData.value.cols }, (_, i) => String.fromCharCode(65 + i))
  );
  
  // Etykiety dla osi Y z lewej strony - Generowanie cyfr zależnie od ilości rzędów
  const labelsY = computed(() =>
    Array.from({ length: BoardData.value.rows }, (_, i) => (i + 1).toString())
  );
  
  // Funkcja rysująca planszę
  const drawBoard = () => {
    if (!board.value) return;
  
    const svg = d3.select(board.value);

    // Czyści poprzednie elementy DOM należące do SVG przed narysowaniem zmian na planszy
    svg.selectAll("*").remove();

    // Pobrane dane komponentu
    const {
      rows, cols, margin, cellColor, borderColor,
      labelsUp, labelsRight, descriptionDown, descriptionLeft, borderColors
    } = BoardData.value;
  
    // Tworzenie siatki
    for (let row = 0; row < rows; row++) {
      for (let col = 0; col < cols; col++) {
        svg.append("rect")
          .attr("x", col * cellSize.value + margin)
          .attr("y", row * cellSize.value + margin)
          .attr("width", cellSize.value)
          .attr("height", cellSize.value)
          .attr("fill", cellColor)
          .attr("stroke", borderColor)
          .attr("stroke-width", 1);
      }
    }
  
    // Etykiety na osi X (dolne)
    labelsX.value.forEach((label, i) => {
      svg.append("text")
        .attr("x", i * cellSize.value + margin + cellSize.value / 2)
        .attr("y", boardSizeY.value + margin + 20)
        .attr("text-anchor", "middle")
        .attr("font-size", getFontSize(16))
        .attr("fill","white")
        .text(label);
    });
  
    // Etykiety na osi X (górne)
    labelsUp.forEach((label, i) => {
      svg.append("text")
        .attr("x", (2 * i + 1) * cellSize.value + margin)
        .attr("y", margin - 20)
        .attr("text-anchor", "middle")
        .attr("font-size", getFontSize(12))
        .attr("fill","white")
        .text(label);
    });

    // Etykiety na osi Y (lewa strona)
    labelsY.value.forEach((label, i) => {
      svg.append("text")
        .attr("x", margin - 20)
        .attr("y", boardSizeY.value - i * cellSize.value + margin - cellSize.value / 2)
        .attr("text-anchor", "middle")
        .attr("dominant-baseline", "middle")
        .attr("font-size", getFontSize(16))
        .attr("fill","white")
        .text(label);
    });

    // Etykieta na osi Y (prawa strona)
    labelsRight.forEach((label, i) => {
      svg.append("text")
        .attr("transform", `translate(${margin + boardSizeX.value + 20}, ${margin + (rows - i * 2 - 1) * cellSize.value}) rotate(-90)`)
        .attr("text-anchor", "middle")
        .attr("dominant-baseline", "middle")
        .attr("font-size", getFontSize(16))
        .attr("fill","white")
        .text(label);
    });
    
    // Opis z lewej strony
    svg.append("text")
      .attr("transform", `translate(${margin - 30}, ${margin + boardSizeY.value / 2}) rotate(-90)`)
      .attr("text-anchor", "middle")
      .attr("font-size", getFontSize(14))
      .attr("font-weight", "bold")
      .attr("fill","white")
      .text(descriptionLeft);
  
    // Opis na dole
    svg.append("text")
      .attr("x", margin + boardSizeX.value / 2)
      .attr("y", boardSizeY.value + margin + 60)
      .attr("text-anchor", "middle")
      .attr("font-size", getFontSize(16))
      .attr("font-weight", "bold")
      .attr("fill","white")
      .text(descriptionDown);
  
    // Tworzenie linii na górnej krawędzi - Kolor zmienia się co 2 komórki
    for (let i = 0; i < cols; i += 2) {
      svg.append("line")
        .attr("x1", margin + i * cellSize.value)
        .attr("y1", margin)
        .attr("x2", margin + (i + 2) * cellSize.value)
        .attr("y2", margin)
        .attr("stroke", borderColors[(i / 2) % borderColors.length])
        .attr("stroke-width", 3);
    }
  
    // Tworzenie linii na dolnej krawędzi - Kolor zmienia się co 2 komórki
    for (let i = 0; i < cols; i += 2) {
      svg.append("line")
        .attr("x1", margin + i * cellSize.value)
        .attr("y1", margin + boardSizeY.value)
        .attr("x2", margin + (i + 2) * cellSize.value)
        .attr("y2", margin + boardSizeY.value)
        .attr("stroke", borderColors[(i / 2) % borderColors.length])
        .attr("stroke-width", 3);
    }
  
    // Tworzenie linii z lewej strony - Kolor zmienia się co 2 komórki
    for (let i = 0; i < rows; i += 2) {
      svg.append("line")
        .attr("x1", margin)
        .attr("y1", margin + boardSizeY.value - i * cellSize.value)
        .attr("x2", margin)
        .attr("y2", margin + boardSizeY.value - (i + 2) * cellSize.value)
        .attr("stroke", borderColors[(i / 2) % borderColors.length])
        .attr("stroke-width", 3);
    }
  
    // Tworzenie linii z prawej strony - Kolor zmienia się co 2 komórki
    for (let i = 0; i < rows; i += 2) {
      svg.append("line")
        .attr("x1", margin + boardSizeX.value)
        .attr("y1", margin + boardSizeY.value - i * cellSize.value)
        .attr("x2", margin + boardSizeX.value)
        .attr("y2", margin + boardSizeY.value - (i + 2) * cellSize.value)
        .attr("stroke", borderColors[(i / 2) % borderColors.length])
        .attr("stroke-width", 3);
    }
  };
  
  // Funkcja zwracająca responsywny rozmiar czcionki
  const getFontSize = (baseSize) => {
    // Skalowanie czcionki w zależności od rozmiaru komórki
    const scale = Math.max(0.6, Math.min(1.2, cellSize.value / 80));
    return `${baseSize * scale}px`;
  };
  
  // Obserwowanie zmian w danych planszy i rozmiarze kontenera
  watch([BoardData, containerWidth, containerHeight], () => {
    drawBoard();
  });
  
  // Przygotowanie i inicjalizacja przy montowaniu komponentu
  onMounted(async () => {
    // Inicjalizacja ResizeObserver do monitorowania rozmiaru kontenera
    const resizeObserver = new ResizeObserver(() => {
      updateContainerSize();
    });
    
    if (container.value) {
      resizeObserver.observe(container.value);
      updateContainerSize(); // Inicjalne pobranie rozmiaru
    }
    
    try {
      // Jeśli nie jesteśmy w trybie podglądu, ładujemy dane z serwera
      if (!props.previewMode) {
        // Wyświetl toast informujący o ładowaniu
        const loadingToast = toast.info("Ładowanie planszy...", {
          timeout: false, // Bez automatycznego zamknięcia
          closeOnClick: false,
        });
        
        // Załaduj planszę z serwera lub użyj domyślnej, jeśli jeszcze nie załadowano
        if (boardStore.boards.length === 0) {
          await boardStore.loadDefaultBoard();
        }
        
        // Zamknij toast informujący o ładowaniu
        toast.dismiss(loadingToast);
        
        // Wyświetl toast o sukcesie
        toast.success("Plansza załadowana pomyślnie!");
      }
      
      // Rysuj planszę po załadowaniu danych
      drawBoard();
    } catch (error) {
      console.error('Błąd podczas ładowania planszy:', error);
      
      // Wyświetl toast z błędem tylko jeśli nie jesteśmy w trybie podglądu
      if (!props.previewMode) {
        toast.error(`Błąd: ${error.message}`);
      }
    }
    
    // Sprzątanie przy odmontowywaniu komponentu
    onUnmounted(() => {
      if (container.value) {
        resizeObserver.unobserve(container.value);
      }
      resizeObserver.disconnect();
    });
  });

  // Monitoruj zmiany błędów w sklepie i wyświetlaj toasty, gdy się pojawią
  // Tylko jeśli nie jesteśmy w trybie podglądu
  watch(() => boardStore.error, (newError) => {
    if (newError && !props.previewMode) {
      toast.error(`Błąd: ${newError}`);
    }
  });
</script>