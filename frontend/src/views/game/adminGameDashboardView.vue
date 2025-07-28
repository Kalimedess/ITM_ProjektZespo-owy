<template>

    <div class="flex flex-col min-h-screen bg-primary">
        <AdminNavbar />
        
        
        <div class="flex flex-1 mt-2">
          
          <mySidebar class="flex-shrink-0" />
          
          <!--Content-->
          <div class="flex-1 ml-4 bg-secondary  border-t-2 border-l-2 border-b-2 border-lgray-accent rounded-md shadow-sm text-center">
              <RouterView />
          </div>
        </div>
        <div class="mt-2">
          <Footer />
        </div>
      </div>

       <!--Ikona chatu-->
        <div class=" bg-tertiary flex justify-center items-center px-2 py-2 fixed bottom-12 right-8 h-12 w-12 z-10  
        rounded-full border cursor-pointer  border-lgray-accent text-white transition-all duration-300 ease-in-out
        hover:shadow-lg hover:shadow-accent/60 hover:border-accent hover:text-accent hover:scale-110 hover:-translate-y-2"
        @click="showChat = true">
            <font-awesome-icon :icon="faCommentDots" class="h-8"/>
        </div>

        <!--Chat-->
        <div v-if="showChat" class="fixed inset-0 z-50 bg-black bg-opacity-50 lg:flex lg:items-center lg:justify-start lg:pl-4">
            <chatAdmin
                class="
                    h-full w-full
                    lg:h-3/4 lg:w-1/5 lg:max-w-2xl
                "
                @close-chat="showChat = false"
            />
        </div>
    
    </template>
    
<script setup>
    import AdminNavbar from '@/components/navbars/adminNavbar.vue';
    import mySidebar from '@/components/sidebars/adminGameSidebar.vue';
    import Footer from '@/components/footers/adminFooter.vue';
    import { RouterView } from 'vue-router';
    import { faCommentDots } from '@fortawesome/free-regular-svg-icons';
    import { ref } from 'vue';
    import chatAdmin from '@/components/game/chatAdmin.vue';

  //Zmienna czy wyświetlać chat czy nie
  const showChat = ref(false);
  
</script>
    
    