<template>

    <div class="flex flex-col min-h-screen bg-primary">
        <PlayerNavbar />
        
        
        <div class="flex flex-1 mt-2">
          
          <div class="flex-1 ml-4 bg-secondary  border-t-2 border-l-2 border-b-2 border-lgray-accent rounded-md shadow-sm text-center">
              <RouterView />
              <div class="">
                <DecisionHistory />
              </div>
          </div>
        </div>
        <div class="mt-2">
          <Footer />
        </div>
      </div>
    
    </template>
    
    <script setup>
        import PlayerNavbar from '@/components/navbars/playerNavbar.vue';
        import DecisionHistory from '@/components/game/gameDecisionsHistory.vue';
        import Footer from '@/components/footers/adminFooter.vue';
        import { RouterView } from 'vue-router';
    </script>
  