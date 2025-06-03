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
        <Suspense>
      <template #default>
        <CardCarousel :deckId="aktualneDeckId" />
      </template>
      <template #fallback>
        <div>Ładowanie karuzeli kart... (Fallback ze Suspense)</div>
      </template>
    </Suspense>
      </div>

      <div class="flex-1 ml-4 mr-2 bg-secondary border-2 border-lgray-accent rounded-md shadow-sm text-center p-4 w-1/3 mr-4 flex flex-col space-y-4">
        <div class="flex justify-center space-x-2">
          <button
            @click="currentPanel = 'menu'"
            :class="currentPanel === 'menu' ? 'bg-black text-white' : 'bg-white text-black'"
            class="px-4 py-1 rounded-md border"
          >
            Menu
          </button>
          <button
            @click="currentPanel = 'panel1'"
            :class="currentPanel === 'panel1' ? 'bg-black text-white' : 'bg-white text-black'"
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
      </div>


    </div>
    <!-- Stopka -->
    <div class="mt-2">
      <Footer />
    </div>
  </div>
</template>

    
<script setup>
import { ref } from 'vue'

const currentPanel = ref('menu')

import PlayerNavbar from '@/components/navbars/playerNavbar.vue'
import CardCarousel from '@/components/playerComponents/CardCarousel.vue'
import PlayerMenu from '@/components/playerComponents/playerMenu.vue'
import QuestionBox from '@/components/playerComponents/questionBox.vue'
import Footer from '@/components/footers/adminFooter.vue'
import { RouterView } from 'vue-router'

const aktualneDeckId = ref(1);
</script>
    