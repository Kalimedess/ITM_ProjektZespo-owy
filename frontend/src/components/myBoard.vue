<template>
    <div class="flex justify-center items-center h-screen w-full ">
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
  
    // Referencja do elementu SVG
    const board = ref(null);
  
    // Inicjalizacja sklepu
    const boardStore = useBoardStore();
  
    // Inicjalizacja komunikatów
    const toast = useToast();
  
    // Aktualna szerokość i wysokość obrazu
    const windowWidth = ref(window.innerWidth);
    const windowHeight = ref(window.innerHeight);
    
    // Funkcja obsługująca zmianę rozmiaru okna
    const handleResize = () => {
      windowWidth.value = window.innerWidth;
      windowHeight.value = window.innerHeight;
    };
  
    // Pobierz konfigurację z currentBoard w sklepie
    const BoardData = computed(() => {
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
        borderColors: ["green", "yellow", "orange", "red"],
        labelsUp: ['Podstawowa kordynacja', 'Standaryzacja procesów', 'Zintegrowane działania', 'Pełna integracja strategiczna'],
        labelsRight: ['Nowicjusz','Naśladowca','Innowator','Lider cyfrowy'],
        descriptionDown: "Poziom integracji wew/zew",
        descriptionLeft: "Zawansowanie Cyfrowe",
      };
    });
    
    // Dynamicznie obliczany rozmiar komórki w zależności od rozmiaru okna
    const cellSize = computed(() => {
      // Używamy mniejszego z wymiarów, uwzględniając marginesy i dodatkową przestrzeń na etykiety
      const availableWidth = windowWidth.value * 0.7 - BoardData.value.margin * 3;
      const availableHeight = windowHeight.value * 0.7 - BoardData.value.margin * 3;
      
      const cellWidthBased = availableWidth / BoardData.value.cols;
      const cellHeightBased = availableHeight / BoardData.value.rows;
      
      // Wybieramy mniejszą wartość, aby cała plansza zmieściła się na ekranie
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
          .attr("font-size", getFontSize(14))
          .text(label);
      });
    
      // Etykiety na osi X (górne)
      labelsUp.forEach((label, i) => {
        svg.append("text")
          .attr("x", (2 * i + 1) * cellSize.value + margin)
          .attr("y", margin - 20)
          .attr("text-anchor", "middle")
          .attr("font-size", getFontSize(12))
          .text(label);
      });
  
      // Etykiety na osi Y (lewa strona)
      labelsY.value.forEach((label, i) => {
        svg.append("text")
          .attr("x", margin - 20)
          .attr("y", boardSizeY.value - i * cellSize.value + margin - cellSize.value / 2)
          .attr("text-anchor", "middle")
          .attr("dominant-baseline", "middle")
          .attr("font-size", getFontSize(14))
          .text(label);
      });
  
      // Etykieta na osi Y (prawa strona)
      labelsRight.forEach((label, i) => {
        svg.append("text")
          .attr("transform", `translate(${margin + boardSizeX.value + 20}, ${margin + (rows - i * 2 - 1) * cellSize.value}) rotate(-90)`)
          .attr("text-anchor", "middle")
          .attr("dominant-baseline", "middle")
          .attr("font-size", getFontSize(12))
          .text(label);
      });
      
      // Opis z lewej strony
      svg.append("text")
        .attr("transform", `translate(${margin - 30}, ${margin + boardSizeY.value / 2}) rotate(-90)`)
        .attr("text-anchor", "middle")
        .attr("font-size", getFontSize(12))
        .attr("font-weight", "bold")
        .text(descriptionLeft);
    
      // Opis na dole
      svg.append("text")
        .attr("x", margin + boardSizeX.value / 2)
        .attr("y", boardSizeY.value + margin + 60)
        .attr("text-anchor", "middle")
        .attr("font-size", getFontSize(14))
        .attr("font-weight", "bold")
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
      const scale = Math.max(0.8, Math.min(1.2, cellSize.value / 80));
      return `${baseSize * scale}px`;
    };
    
    // Obserwowanie zmian w danych planszy i rozmiarze okna
    watch([BoardData, windowWidth, windowHeight], () => {
      drawBoard();
    });
    
    // Przygotowanie i inicjalizacja przy montowaniu komponentu
    onMounted(async () => {
      // Dodanie nasłuchiwania na zmianę rozmiaru okna
      window.addEventListener('resize', handleResize);
      
      try {
        // Wyświetl toast informujący o ładowaniu
        const loadingToast = toast.info("Ładowanie planszy...", {
          timeout: false, // Bez automatycznego zamknięcia
          closeOnClick: false,
        });
        
        // Załaduj planszę z serwera lub użyj domyślnej, jeśli jeszcze nie załadowano
        if (boardStore.boards.length === 0) {
          await boardStore.loadDefaultBoard();
        }
        
        // Rysuj planszę po załadowaniu danych
        drawBoard();
        
        // Zamknij toast informujący o ładowaniu
        toast.dismiss(loadingToast);
        
        // Wyświetl toast o sukcesie
        toast.success("Plansza załadowana pomyślnie!");
      } catch (error) {
        console.error('Błąd podczas ładowania planszy:', error);
        
        // Wyświetl toast z błędem
        toast.error(`Błąd: ${error.message}`);
      }
    });
  
    // Monitoruj zmiany błędów w sklepie i wyświetlaj toasty, gdy się pojawią
    watch(() => boardStore.error, (newError) => {
      if (newError) {
        toast.error(`Błąd: ${newError}`);
      }
    });
    
    // Sprzątanie przy odmontowywaniu komponentu
    onUnmounted(() => {
      // Usunięcie nasłuchiwania po odmontowaniu komponentu
      window.removeEventListener('resize', handleResize);
    });
  </script>