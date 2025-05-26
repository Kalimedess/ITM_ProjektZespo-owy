<template>
  <!--Jest to główny widok, który użytkownik widzi po przejsciu na stronę gry-->
  
  <div class="
    min-h-screen bg-primary flex flex-col
    h-screen
  ">
    <!--navbar-->
    <Navbar />
    
    <div class="
      flex-1 flex flex-col justify-center items-center text-center
      px-4 sm:px-6 md:px-8
      py-4 sm:py-6 md:py-8
      overflow-y-auto
    ">
      <div class="mb-3 sm:mb-4 md:mb-6">
        <h1 class="
          text-white font-bold tracking-wider font-nasalization animate-glow
          text-3xl sm:text-4xl md:text-5xl lg:text-6xl xl:text-7xl
          mb-1 sm:mb-2 md:mb-3
        ">
          DIGITAL
        </h1>
        <h1 class="
          text-white font-bold tracking-wider font-nasalization animate-glow
          text-3xl sm:text-4xl md:text-5xl lg:text-6xl xl:text-7xl
        ">
          WARS
        </h1>
      </div>

      <!--Opis - responsive text-->
      <p class="
        text-gray-500 
        text-sm sm:text-base md:text-lg
        mb-4 sm:mb-6 md:mb-8
        max-w-xs sm:max-w-sm md:max-w-md lg:max-w-lg
        px-2 sm:px-0
      ">
        Gra o zarządzaniu kapitałem ludzkim
      </p>

      <!--Przycisk do logowania/rejestraci oraz do dołączenia do gry przez kod-->
      <div class="
        w-full space-y-3 sm:space-y-4
        max-w-xs sm:max-w-sm md:max-w-md lg:max-w-lg
        px-2 sm:px-4 md:px-0
      ">
        <button 
          @click="handleLoginClick"
          class="
            bg-tertiary hover:bg-accent text-white w-full rounded-lg font-medium 
            transition-all duration-300 shadow-sm hover:shadow-lg 
            shadow-accent/40 hover:shadow-accent/60
            py-2.5 sm:py-3 md:py-4
            text-sm sm:text-base md:text-lg
            h-[44px] sm:h-[48px] md:h-[52px]
          "
        > 
          Stwórz grę jako Game Master
        </button>
      
        <button
          @click="showJoinByCode = true" 
          class="
            bg-tertiary hover:bg-accent text-white w-full rounded-lg font-medium 
            transition-all duration-300 shadow-sm hover:shadow-lg 
            shadow-accent/40 hover:shadow-accent/60
            py-2.5 sm:py-3 md:py-4
            text-sm sm:text-base md:text-lg
            h-[44px] sm:h-[48px] md:h-[52px]
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

const authStore = useAuthStore();

//Jest to zmienna od której zależy czy formularz logowania/rejestracji jest wyświetlony
const showAuthModal = ref(false);
const showJoinByCode = ref(false);

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

/* Enhanced responsive glow effect */
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