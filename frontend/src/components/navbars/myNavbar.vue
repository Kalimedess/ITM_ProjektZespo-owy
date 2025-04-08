<template>
    <nav class="w-full bg-secondary py-3 px-6 flex flex-row justify-between items-center shadow-[0_2px_4px_theme(colors.accent/0.5)]">
            <div>
              <img :src="logo" class="h-12" alt="ITM logo">
            </div>
            
            <!--Odnośniki do social medi-->
            <div class="flex flex-row justify-around items-center gap-3 mr-5">
              <a href="https://www.linkedin.com/company/itm-ltd/about/" target="_blank"><font-awesome-icon :icon="faLinkedin" class="h-8 text-white hover:text-accent transition-all duration-300"/></a>
              <a href="https://www.youtube.com/channel/UCMA8NVly_eqK114A6-fkjTw" target="_blank"><font-awesome-icon :icon="faYoutube" class="h-8 text-white hover:text-accent transition-all duration-300"/></a>
              <a href="https://www.facebook.com/itm.olsztyn" target="_blank"><font-awesome-icon :icon="faFacebook" class="h-8 text-white  hover:text-accent transition-all duration-300" /></a>
            </div>
          </nav>
    </template>
    
    
    <script setup>
        import { faFacebook,faLinkedin,faYoutube } from '@fortawesome/free-brands-svg-icons';
        import logo from '@/assets/logos/ITM_poziom_biale.png'
    </script>