<template>
  <div class="flex flex-col min-h-screen bg-primary">
    <!-- Górny pasek -->
    <PlayerNavbar />

    <!-- Główna zawartość -->
    <div class="flex flex-1 mt-2">
      
      <!-- Lewa kolumna: wybór kart -->
      <div class="flex-1 ml-4 mr-2 bg-secondary border-2 border-lgray-accent rounded-md shadow-sm text-center p-4">
        <RouterView />
        <QuestionBox />
        <CardCarousel />
      </div>

      <!-- Prawa kolumna: decyzje -->
      <div class="flex-1 ml-4 mr-2 bg-secondary border-2 border-lgray-accent rounded-md shadow-sm text-center p-4 w-1/3 mr-4 flex flex-col space-y-4">
        <!-- Przyciski przełączania paneli -->
        <div class="flex justify-center space-x-2">
          <button
            @click="currentPanel = 'menu'"
            :class="currentPanel === 'menu' ? 'bg-black text-white' : 'bg-white text-black'"
            class="px-4 py-1 rounded-md border"
          >
            Menu
          </button>
          <button
            @click="currentPanel = 'gameboard'"
            :class="currentPanel === 'gameboard' ? 'bg-black text-white' : 'bg-white text-black'"
            class="px-4 py-1 rounded-md border"
          >
            Twoja plansza
          </button>
          <button
            @click="currentPanel = 'panel2'"
            :class="currentPanel === 'panel2' ? 'bg-black text-white' : 'bg-white text-black'"
            class="px-4 py-1 rounded-md border"
          >
            Plansza Rywali
          </button>
        </div>

        <PlayerMenu v-if="currentPanel === 'menu'" />
        <GameBoard v-else-if="currentPanel === 'gameboard'"
        :config="formData"
        :gameMode="true"
        :posX="posX"
        :posY="posY"
        :pawnColor="'#0000ff'" />
      </div>

    </div>

    <!-- Stopka -->
    <div class="mt-2">
      <Footer />
    </div>
  </div>
</template>

<script setup>
import { reactive, ref } from 'vue'
const currentPanel = ref('menu')

    const formData = reactive({
        Name: 'Plansza podstawowa', 
        LabelsUp: ['Podstawowa kordynacja', 'Standaryzacja procesów', 'Zintegrowane działania', 'Pełna integracja strategiczna'], 
        LabelsRight: ['Nowicjusz', 'Naśladowca', 'Innowator', 'Lider cyfrowy'], 
        DescriptionDown: 'Poziom integracji wew/zew', 
        DescriptionLeft: 'Zawansowanie Cyfrowe', 
        Rows: 8,
        Cols: 8,
        CellColor: '#fefae0', 
        BorderColor: '#595959', 
        BorderColors: ['#008000', '#FFFF00', '#FFA500', '#FF0000']
    });

    const posX = ref(0);
    const posY = ref(0);


import PlayerNavbar from '@/components/navbars/playerNavbar.vue'
import CardCarousel from '@/components/playerComponents/cardCarousel.vue'
import PlayerMenu from '@/components/playerComponents/playerMenu.vue'
import QuestionBox from '@/components/playerComponents/questionBox.vue'
import GameBoard from '@/components/game/gameBoard.vue'
import Footer from '@/components/footers/adminFooter.vue'
import { RouterView } from 'vue-router'
</script>
