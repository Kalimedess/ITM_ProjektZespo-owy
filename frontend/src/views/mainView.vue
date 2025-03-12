<template>
    <div class="min-h-screen bg-primary flex flex-col">
      <nav class="w-full bg-secondary py-3 px-6 flex flex-row justify-between items-center shadow-sm shadow-accent/50">
        <div>
          <img :src="logo" class="h-12" alt="Digital Wars logo">
        </div>
  
        <div class="flex flex-row justify-around items-center gap-3 mr-5">
          <a href="https://www.linkedin.com/company/itm-ltd/about/" target="_blank"><font-awesome-icon :icon="faLinkedin" class="h-10 text-white"/></a>
          <a href="https://www.youtube.com/channel/UCMA8NVly_eqK114A6-fkjTw" target="_blank"><font-awesome-icon :icon="faYoutube" class="h-10 text-white"/></a>
          <a href="https://www.facebook.com/itm.olsztyn" target="_blank"><font-awesome-icon :icon="faFacebook" class="h-10 text-white" /></a>
        </div>
      </nav>
      
      <div class="flex-grow flex flex-col justify-center items-center text-center">
        <h1 class="text-white text-6xl font-bold tracking-wider mb-3 font-nasalization">DIGITAL</h1>
        <h1 class="text-white text-6xl font-bold tracking-wider mb-8 font-nasalization">WARS</h1>
    
        <p class="text-gray-500 mb-8">Gra o zarządzaniu kapitałem ludzkim</p>
    
        <div class="w-full max-w-md space-y-4 px-4">
          <button 
            @click="showAuthModal = true"
            class="bg-tertiary hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60 mb-5"
          >
            <font-awesome-icon :icon="['fas', 'sign-in-alt']" class="mr-4" />
            Zaloguj się jako GameMaster
          </button>
        
          <button class="bg-tertiary hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60 mb-5">
            <font-awesome-icon :icon="['fas', 'qrcode']" class="mr-4"/>
            Dołącz poprzez kod
          </button>  
        </div>
  
        <LoginRegister
          :is-visible="showAuthModal" 
          :initial-view="'login'"
          @close="showAuthModal = false"
        />
      </div>
    </div>
  </template>
  
  <script setup>
    import { faFacebook,faLinkedin,faYoutube } from '@fortawesome/free-brands-svg-icons';
    import logo from '@/assets/logos/ITM_poziom_biale.png'
    import { ref } from 'vue';
    import LoginRegister from '@/components/loginRegister.vue';
    const showAuthModal = ref(false);
  </script>