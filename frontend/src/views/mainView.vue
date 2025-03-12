<template>
<!--Jest to główny widok, który użytkownik widzi po przejsciu na stronę gry-->

    <!--Background strony-->
    <div class="min-h-screen bg-primary flex flex-col">
      <!--navbar-->
      <nav class="w-full bg-secondary py-3 px-6 flex flex-row justify-between items-center shadow-sm shadow-accent/50">
        <div>
          <img :src="logo" class="h-12" alt="ITM logo">
        </div>
        
        <!--Odnośniki do social medi-->
        <div class="flex flex-row justify-around items-center gap-3 mr-5">
          <a href="https://www.linkedin.com/company/itm-ltd/about/" target="_blank"><font-awesome-icon :icon="faLinkedin" class="h-10 text-white hover:text-accent transition-all duration-300"/></a>
          <a href="https://www.youtube.com/channel/UCMA8NVly_eqK114A6-fkjTw" target="_blank"><font-awesome-icon :icon="faYoutube" class="h-10 text-white hover:text-accent transition-all duration-300"/></a>
          <a href="https://www.facebook.com/itm.olsztyn" target="_blank"><font-awesome-icon :icon="faFacebook" class="h-10 text-white  hover:text-accent transition-all duration-300" /></a>
        </div>
      </nav>
      
      <!--Główna sekcja strony-->
      <div class="flex-grow flex flex-col justify-center items-center text-center">
        <!--Tytuł-->
        <h1 class="text-white text-6xl font-bold tracking-wider mb-3 font-nasalization animate-glow ">DIGITAL</h1>
        <h1 class="text-white text-6xl font-bold tracking-wider mb-8 font-nasalization  animate-glow">WARS</h1>
    
        <!--Opis-->
        <p class="text-gray-500 mb-8">Gra o zarządzaniu kapitałem ludzkim</p>
    
        <!--Przycisk do logowania/rejestraci oraz do dołączenia do gry przez kod-->
        <div class="w-full max-w-md space-y-4 px-4">
          <button 
            @click="showAuthModal = true"
            class="bg-tertiary hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60 mb-5"
          >
            <font-awesome-icon :icon="faSignInAlt" class="mr-4" />
            Dołącz do gry jako Game Master
          </button>
        
          <button
          @click="showJoinByCode = true" 
          class="bg-tertiary hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60 mb-5">
            <font-awesome-icon :icon="faQrcode" class="mr-4"/>
            Dołącz poprzez kod
          </button>  
        </div>
  
        <!--Formularz logowania/rejestracji-->
        <LoginRegister
          :is-visible="showAuthModal" 
          :initial-view="'login'"
          @close="showAuthModal = false"
        />

        <joinByCode
        :is-visible="showJoinByCode"
        @close="showJoinByCode = false"
        />


      </div>
    </div>
  </template>
  
  <script setup>

    import { faSignInAlt,faQrcode } from '@fortawesome/free-solid-svg-icons'
    import { faFacebook,faLinkedin,faYoutube } from '@fortawesome/free-brands-svg-icons';
    import logo from '@/assets/logos/ITM_poziom_biale.png'
    import { ref } from 'vue';
    import LoginRegister from '@/components/loginRegister.vue';
    import joinByCode from '@/components/joinByCode.vue';

    //Jest to zmienna od której zależy czy formularz logowania/rejestracji jest wyświetlony
    const showAuthModal = ref(false);
    const showJoinByCode = ref(false);
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
    text-shadow: 0 0 10px #a78bfa, 0 0 20px #a78bfa, 0 0 30px #a78bfa;
  }
}

.animate-glow {
  animation-name: glow;
  animation-duration: 4s; /* długość trwania anmiacji */
  animation-delay: 4s; /* co ile animacja się odpala */
  animation-iteration-count: infinite; 
}
</style>