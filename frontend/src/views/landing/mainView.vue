<template>
  <!--Jest to główny widok, który użytkownik widzi po przejsciu na stronę gry-->
  
  <div class="
     bg-primary flex flex-col min-h-screen
  ">
    <!--navbar-->
    <Navbar />
    
    <div class="
      flex-1 flex flex-col justify-center items-center text-center">
      <div class="mb-3  md:mb-4 lg:mb-6 xl:mb-8">
        <h1 class="
          text-white font-bold tracking-wider font-nasalization animate-glow
          text-3xl sm:text-4xl md:text-5xl lg:text-6xl xl:text-7xl
          mb-1 sm:mb-2 md:mb-3 lg:mb-5 xl:mb-7
        ">
          DIGITAL
        </h1>
        <h1 class="
          text-white font-bold tracking-wider font-nasalization animate-glow
          text-3xl sm:text-4xl md:text-5xl lg:text-6xl xl:text-7xl
          mb-1 sm:mb-2 md:mb-3 lg:mb-5 xl:mb-7
        ">
          WARS
        </h1>
      </div>

      <div class="flex text-white bg-secondary border border-accent/50  shadow-xl px-4 py-3  rounded-full  mb-11 gap-4">
        <div @click="currentView =  'game master' "
          class= "px-4 py-4 rounded-full cursor-pointer"
          :class="currentView === 'game master' ? 'bg-accent font-semibold shadow-lg shadow-accent/70' : 'bg-transparent transition delay-150 duration-300 ease-in-out hover:-translate-y-1 hover:scale-110 hover:bg-accent/70' ">
          <span class="mr-2">Game Master</span>
          <font-awesome-icon :icon="faUserGear" class="h-4"/>
        </div>
        <div @click="currentView = 'player' "
          class="px-4 py-4 rounded-full cursor-pointer"
          :class="currentView === 'player' ? 'bg-accent font-semibold shadow-lg shadow-accent/70' : 'bg-transparent transition delay-150 duration-300 ease-in-out hover:-translate-y-1 hover:scale-110 hover:bg-accent/70' ">
          <span class="mr-2">Gracz</span>
          <font-awesome-icon :icon="faUser" class="h-4" />
        </div>
      </div>

      <!--Przycisk do logowania/rejestraci oraz do dołączenia do gry przez kod-->
      <div class="
        w-full space-y-3 sm:space-y-4
        max-w-xs sm:max-w-sm md:max-w-md lg:max-w-lg
        px-2 sm:px-4 md:px-0
      ">
        <button 
          @click="handleLoginClick"
          v-if="currentView === 'game master' "
          class="
            bg-tertiary border border-accent/50 hover:bg-accent text-white w-full rounded-3xl font-medium 
            transition-all duration-300 shadow-sm hover:shadow-lg 
            shadow-accent/60 hover:shadow-accent/70
            py-2.5 sm:py-3 md:py-4
            text-sm sm:text-base md:text-md lg:text-lg xl:text-xl
          "
        > 
          Stwórz grę jako Game Master
        </button>
      
        <button
          @click="showJoinByCode = true" 
          v-if="currentView === 'player' "
          class="
            bg-tertiary border border-accent/50 hover:bg-accent text-white w-full rounded-3xl font-medium 
            transition-all duration-300 shadow-sm shadow-accent/60 hover:shadow-lg  hover:shadow-accent/70
            py-2.5 sm:py-3 md:py-4
            text-sm sm:text-base md:text-md lg:text-lg xl:text-xl
          "
        >
          Dołącz do gry jako gracz
        </button>  
      </div>

      <!--Formularz logowania/rejestracji-->
      <LoginRegister
        :is-visible="showAuthModal" 
        @close="showAuthModal = false"
      />

      <joinByCode
        :is-visible="showJoinByCode"
        @close="showJoinByCode = false"
      />

    </div>
    <Footer />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import LoginRegister from '@/components/auth/loginRegister.vue';
import joinByCode from '@/components/game/joinGameByCode.vue';
import Footer from '@/components/footers/myFooter.vue';
import Navbar from '@/components/navbars/myNavbar.vue';
import { useAuthStore } from '@/stores/auth';
import router from '@/router';

 import {faUser,faUserGear} from '@fortawesome/free-solid-svg-icons'

const authStore = useAuthStore();

//Jest to zmienna od której zależy czy formularz logowania/rejestracji jest wyświetlony
const showAuthModal = ref(false);
const showJoinByCode = ref(false);

const currentView = ref('game master');

onMounted(() => {
  if (sessionStorage.getItem('showLoginAfterRedirect') === 'true') {
    sessionStorage.removeItem('showLoginAfterRedirect');
    showAuthModal.value = true;
  }
});

const handleLoginClick = () => {
  if (authStore.isAuthenticated === true) {
    router.push('/admin');
  }
  else {
    showAuthModal.value = true;
  }
};
</script>

<style scoped>
/*Animacja świecenia się napisu DIGITAL WARS, #a78bfa jest kolor akcentu z configu tailwind*/
@keyframes glow {
  0%, 100% {
    color: white;
    text-shadow: none;
  }
  50% {
    color: #a78bfa;
    text-shadow: 
      0 0 5px #a78bfa, 
      0 0 10px #a78bfa, 
      0 0 15px #a78bfa,
      0 0 20px #a78bfa;
  }
}

.animate-glow {
  animation-name: glow;
  animation-duration: 4s;
  animation-delay: 4s;
  animation-iteration-count: infinite;
}

@media (max-width: 640px) {
  @keyframes glow {
    0%, 100% {
      color: white;
      text-shadow: none;
    }
    50% {
      color: #a78bfa;
      text-shadow: 
        0 0 3px #a78bfa, 
        0 0 6px #a78bfa, 
        0 0 9px #a78bfa;
    }
  }
}
</style>