<template>
  <div class="w-full border-2 rounded-md bg-tertiary p-4 flex flex-col gap-4 text-white hover:transform hover:-translate-y-2 transition-all duration-300"
       :style="{borderColor: props.color}">
    <div class="flex flex-row items-center justify-between">
      <div class="flex items-center">
        <font-awesome-icon :icon="faGamepad" class="h-5 mr-2"
                          :style="{ color: props.color }"/> 
        <span class="text-lg font-semibold"
              :style="{ color: props.color }">
          {{ game.name }}
        </span>
      </div>
      <span class="text-sm px-2 py-1 rounded-full"
            :style="{backgroundColor: props.color}">
        ID: {{ game.id }}
      </span>
    </div>
    
    <div class="flex flex-row gap-2">
      <button 
        v-if="game.status === 'During' || game.status === 'Paused'"
        @click="requestStatusChange(game.id, game.status === 'During' ? 'Paused' : 'During')"
        class="flex flex-auto items-center justify-center border-2 border-lgray-accent py-2 px-3 rounded-md hover:border-accent transition-colors duration-300">
        <font-awesome-icon :icon="game.status === 'During' ? faCircleStop : faCirclePlay" class="h-4 text-accent mr-2"/>
        {{ game.status === 'During' ? 'Wstrzymaj grę' : 'Wznów grę' }}
      </button>
      <RouterLink 
        :to="`/admin/game`"
        class="flex flex-auto items-center justify-center border-2 border-lgray-accent py-2 px-3 rounded-md hover:border-accent transition-colors duration-300">
        <font-awesome-icon :icon="faMagnifyingGlass" class="h-4 mr-2 text-accent"/>     
        Otwórz grę
      </RouterLink>
    </div>
    
    <button 
      class="w-full flex items-center justify-center border-2 border-lgray-accent py-2 px-3 rounded-md hover:border-accent transition-colors duration-300"
      @click="showQr = true">
      <font-awesome-icon :icon="faQrcode" class="h-4 mr-2 text-accent"/>  
      Pokaż kod QR
    </button>
  </div>

  <!-- Modal QR poza głównym divem -->
  <Transition
    enter-active-class="transition-all duration-300 ease-out"
    enter-from-class="opacity-0"
    enter-to-class="opacity-100"
    leave-active-class="transition-all duration-300 ease-in"
    leave-from-class="opacity-100"
    leave-to-class="opacity-0">

    <div v-if="showQr" class="fixed inset-0 flex items-center justify-center z-50 p-4">
        <div class="absolute inset-0 bg-black/70" @click="showQr = false"></div>
        
        <div class="bg-primary text-white rounded-lg relative z-10 border-2 border-accent 
                    p-4 sm:p-6 md:p-8 
                    w-full max-w-xs sm:max-w-sm md:max-w-md lg:max-w-lg">
            <button 
            @click="showQr = false" 
            class="absolute top-2 right-2 sm:top-4 sm:right-4 
                    w-8 h-8 sm:w-10 sm:h-10 
                    flex items-center justify-center rounded-full hover:bg-white/10 transition-colors">
            <font-awesome-icon :icon="faXmark" class="h-4 sm:h-5 text-white hover:text-accent transition-colors" />
            </button>
            
            <h2 class="text-lg sm:text-xl md:text-2xl font-bold text-center mb-4 sm:mb-6">
            Kod QR do gry
            </h2>
            
            <div class="bg-white p-3 sm:p-4 md:p-6 rounded-lg flex justify-center">
            <qrcode-vue 
                :value="gameUrl" 
                :size="qrSize" 
            />
            </div>
        </div>
        </div>
  </Transition>
</template>

<script setup>
import { ref, computed } from 'vue';
import { faGamepad, faCircleStop, faMagnifyingGlass, faQrcode, faXmark, faCirclePlay } from '@fortawesome/free-solid-svg-icons';
import { RouterLink } from 'vue-router';
import QrcodeVue from 'qrcode.vue';

const qrSize = computed(() => {
  const width = window.innerWidth;
  if (width < 640) return 200;  
  if (width < 768) return 250;  
  if (width < 1024) return 300; 
  return 350; 
});

const props = defineProps({
  game: {
    type: Object,
    required: true
  },
  color: {
    type: String,
    required: true
  }
});

const emit = defineEmits(['update-status']);

const showQr = ref(false);

const gameUrl = computed(() => {
  return `${window.location.origin}/player`;
});

const requestStatusChange = (gameId, newStatus) => {

    emit('update-status', { gameId, newStatus });
};
</script>