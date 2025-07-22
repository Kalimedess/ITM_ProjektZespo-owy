<template>
  <div class="w-full border-2 rounded-md p-4 flex flex-col gap-4 text-white transition-all duration-300"
       :style="{ borderColor: props.color }"
       @dblclick="router.push('/admin/game')">

    <!-- Nagłówek -->
    <div class="flex flex-row items-center justify-between">
      <div class="flex items-center min-w-0 flex-1 mr-2 overflow-hidden">
        <font-awesome-icon :icon="faGamepad" class="h-5 mr-2 flex-shrink-0" :style="{ color: props.color }" />
        <span class="text-lg font-semibold break-words" :style="{color : white}">
          {{ table.name }}
        </span>
      </div>
      <span class="text-sm px-2 py-1 rounded-full" :style="{ backgroundColor: props.color }">
        ID: {{ table.id }}
      </span>
    </div>

    <!-- Przycisk QR -->
    <div class="flex flex-row gap-2">
      <button 
        class="flex flex-auto items-center justify-center border-2 border-lgray-accent py-2 px-3 rounded-md hover:border-accent transition-colors duration-300"
        @click="openQrForTeam(table.token)"
        @dblclick.stop>
        <font-awesome-icon :icon="faQrcode" class="h-4 mr-2 text-accent" />
        Pokaż kod QR do gry
      </button>
    </div>

    <!-- Przycisk wejścia do stołu -->
    <RouterLink 
      :to="`/admin/game/${gameId}/${table.id}`"
      @dblclick.stop
      class="w-full flex items-center justify-center border-2 border-lgray-accent py-2 px-3 rounded-md hover:border-accent transition-colors duration-300">
      <font-awesome-icon :icon="faMagnifyingGlass" class="h-4 mr-2 text-accent"/>     
      Wejdź w stół
    </RouterLink>

    <!-- Modal QR -->
    <Transition
      enter-active-class="transition-all duration-300 ease-out"
      enter-from-class="opacity-0"
      enter-to-class="opacity-100"
      leave-active-class="transition-all duration-300 ease-in"
      leave-from-class="opacity-100"
      leave-to-class="opacity-0">
      
      <div v-if="showQr" class="fixed inset-0 flex items-center justify-center z-50 p-4">
        <div class="absolute inset-0 bg-black/70" @click="showQr = false"></div>
        <div class="bg-primary text-white rounded-lg relative z-10 border-2 border-accent p-4 sm:p-6 md:p-8 w-full max-w-xs sm:max-w-sm md:max-w-md lg:max-w-lg">
          <button 
            @click="showQr = false" 
            class="absolute top-2 right-2 sm:top-4 sm:right-4 w-8 h-8 sm:w-10 sm:h-10 flex items-center justify-center rounded-full hover:bg-white/10 transition-colors">
            <font-awesome-icon :icon="faXmark" class="h-4 sm:h-5 text-white hover:text-accent transition-colors" />
          </button>
          <h2 class="text-lg sm:text-xl md:text-2xl font-bold text-center mb-4 sm:mb-6">
            Kod QR do gry
          </h2>
          <div class="bg-white p-3 sm:p-4 md:p-6 rounded-lg flex justify-center">
            <qrcode-vue :value="props.gameUrl" :size="qrSize" />
          </div>
          <h2 class="text-lg sm:text-xl md:text-2xl font-bold text-center mt-4">
            Token: {{ table.token }}
          </h2>
        </div>
      </div>
    </Transition>
  </div>
</template>

<script setup>
    import { ref, computed } from 'vue';
    import { faGamepad, faMagnifyingGlass, faQrcode, faXmark } from '@fortawesome/free-solid-svg-icons';
    import { RouterLink } from 'vue-router';
    import QrcodeVue from 'qrcode.vue';

    const props = defineProps({
        gameId: {
        type: Number,
        required: true
        },
        table: {
        type: Object,
        required: true
        },
        color: {
        type: String,
        required: true
        },
        token: { 
        type: String, 
        required: true 
        },
        gameUrl: { 
        type: String, 
        required: true 
        }
    })
    //logika kodu QR
    const showQr = ref(false);

    const emit = defineEmits(['showQr'])

    const showQrCode = () => {
        emit('showQr', table.id)
    }

    const qrSize = computed(() => {
    const width = window.innerWidth;
    if (width < 640) return 200;
    if (width < 768) return 250;
    if (width < 1024) return 300;
    return 350;
    });

    //Dodać logike żeby pobierało token dla dostępnych drużyn z bazy danych 

    function openQrForTeam(token) {
        showQr.value = true;
    }

</script>
