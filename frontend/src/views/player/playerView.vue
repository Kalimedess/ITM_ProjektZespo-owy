<template>
  <div class="flex flex-col min-h-screen bg-primary">
    <!-- Górny pasek -->
    <PlayerNavbar />

    <!-- Główna zawartość -->
    <div class="flex flex-1 mt-2">
      
      <!-- Lewa kolumna: wybór kart -->
      <div class="flex-1 ml-4 mr-2 bg-secondary border-2 border-lgray-accent rounded-md shadow-sm text-center p-4">
        <RouterView />
        <CardCarousel />
      </div>

      <!-- Prawa kolumna: decyzje -->
      <div class="flex-1 ml-4 mr-2 bg-secondary border-2 border-lgray-accent rounded-md shadow-sm text-center p-4 w-1/3 mr-4">
        <PlayerMenu />
      </div>
    </div>

    <!-- Stopka -->
    <div class="mt-2">
      <Footer />
    </div>
  </div>
</template>

    
    <script setup>
        import PlayerNavbar from '@/components/navbars/playerNavbar.vue';
        import CardCarousel from '@/components/playerComponents/CardCarousel.vue';
        import PlayerMenu from '@/components/playerComponents/playerMenu.vue';
        import Footer from '@/components/footers/adminFooter.vue';
        import { RouterView } from 'vue-router';
    </script>
    
    