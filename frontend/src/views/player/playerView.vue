<template>

    <div class="flex flex-col min-h-screen bg-primary">
        <PlayerNavbar />
        
        
        <div class="flex flex-1 mt-2">
          
          <!--Content-->
          <div class="flex-1 ml-4 bg-secondary  border-t-2 border-l-2 border-b-2 border-lgray-accent rounded-md shadow-sm text-center">
              <RouterView />
                <!--menu gracza-->
              <div class="">
                <PlayerMenu />
              </div>
              <!--system wybierania kart-->
              <div class="">
                <CardCarousel />
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
        import CardCarousel from '@/components/playerComponents/CardCarousel.vue';
        import PlayerMenu from '@/components/playerComponents/playerMenu.vue';
        import Footer from '@/components/footers/adminFooter.vue';
        import { RouterView } from 'vue-router';
    </script>
    
    