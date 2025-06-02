<template>
  <div v-if="props.isVisible" class="fixed inset-0 flex items-center justify-center z-50">
    <div class="absolute inset-0 bg-black/70" @click="closeModal"></div>

    <div class="flex flex-col justify-start items-center bg-primary text-white rounded-lg w-96 relative z-10 border-2 border-accent p-6 sm:p-8 md:p-10 lg:p-12 animate-jump-in">
      <button 
        @click="closeModal" 
        class="absolute top-2 right-2 w-8 h-8 flex items-center justify-center"
      >
        <font-awesome-icon :icon="faXmark" class="h-5 text-white hover:text-accent transition-all duration-100" />
      </button>
      
      <!-- Widok formularza -->
      <div v-if="!isScanning" class="animate-fade">
        <h2 class="text-lg sm:text-xl md:text-2xl font-nasalization mb-2 sm:mb-3 text-center">Dołącz do gry</h2>

        <div class="w-full h-0.5 mb-1 sm:mb-2 md:mb-3 lg:mb-4 bg-accent"></div>

        <form @submit.prevent="joinGame">
          <div class="mb-4">
            <input 
              type="text" 
              v-model="code" 
              placeholder="Wprowadź kod gry..."
              class="w-full px-3 py-3 bg-tertiary border border-lgray-accent rounded-md text-white focus:outline-none focus:border-accent"
              required
            />
          </div>

          <button 
            type="submit" 
            class="bg-tertiary hover:bg-accent/80 text-white w-full py-4 rounded-lg font-medium transition-all duration-300"
          >
            Dołącz do gry
          </button>
        </form>

        <div class="flex items-center gap-2 my-6">
          <div class="flex-1 h-px bg-lgray-accent"></div>
          <span class="text-gray-500 px-2">lub</span>
          <div class="flex-1 h-px bg-lgray-accent"></div>
        </div>

        <button 
          type="button"
          @click="startScanning" 
          class="bg-tertiary hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300"
        >
          <font-awesome-icon :icon="faQrcode" class="mr-2" />
          Zeskanuj kod QR
        </button>
      </div>

      <!-- Widok skanera -->
      <div v-else class="w-full">
        <h2 class="text-lg sm:text-xl md:text-2xl font-nasalization mb-4 text-center">Skanuj kod QR</h2>
        
        <div class="mb-4 relative">
          <qrcode-stream 
            :formats="['qr_code']"
            @detect="onDetect"
            @init="onInit"
            class="rounded-lg overflow-hidden"
          >
            <div v-if="scanError" class="text-center p-4 bg-red-500/20">
              <p class="text-red-400">{{ scanError }}</p>
            </div>
            
            <!-- Nakładka skanowania -->
            <div class="absolute inset-0 pointer-events-none">
              <div class="w-full h-full flex items-center justify-center">
                <div class="w-48 h-48 border-2 border-white/50 rounded-lg">
                  <div class="w-full h-full relative">
                    <div class="absolute top-0 left-0 w-8 h-8 border-t-4 border-l-4 border-accent rounded-tl-lg"></div>
                    <div class="absolute top-0 right-0 w-8 h-8 border-t-4 border-r-4 border-accent rounded-tr-lg"></div>
                    <div class="absolute bottom-0 left-0 w-8 h-8 border-b-4 border-l-4 border-accent rounded-bl-lg"></div>
                    <div class="absolute bottom-0 right-0 w-8 h-8 border-b-4 border-r-4 border-accent rounded-br-lg"></div>
                  </div>
                </div>
              </div>
            </div>
          </qrcode-stream>
        </div>

        <button 
          @click="isScanning = false" 
          class="bg-tertiary hover:bg-accent/80 text-white w-full py-3 rounded-lg font-medium transition-all duration-300"
        >
          Anuluj
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { defineProps, ref, defineEmits } from 'vue';
import { faXmark, faQrcode } from '@fortawesome/free-solid-svg-icons';
import { QrcodeStream } from 'vue-qrcode-reader';
import { useRouter } from 'vue-router';

const router = useRouter();

const isScanning = ref(false);
const scanError = ref('');
const code = ref('');
const isProcessing = ref(false);
const lastScannedCode = ref('');

const props = defineProps({
  isVisible: {
    type: Boolean,
    default: false,
  }
});

const emit = defineEmits(['close']);

const closeModal = () => {
  emit('close');
  code.value = '';
  isScanning.value = false;
  scanError.value = '';
  isProcessing.value = false;
  lastScannedCode.value = '';
};

const joinGame = () => {
  router.push('/player');
  closeModal();
};

const startScanning = () => {
  isScanning.value = true;
  scanError.value = '';
  lastScannedCode.value = '';
};

// Handler dla skanera QR
const onDetect = async (detectedCodes) => {
  if (!detectedCodes || detectedCodes.length === 0 || isProcessing.value) {
    return;
  }
  
  const codes = await Promise.resolve(detectedCodes);
  const firstCode = codes[0];
  
  let decodedText = '';
  
  if (typeof firstCode === 'string') {
    decodedText = firstCode;
  } else if (firstCode && typeof firstCode === 'object') {
    decodedText = firstCode.rawValue || '';
  }
  
  if (decodedText && decodedText !== lastScannedCode.value) {
    processScanResult(decodedText);
  }
};

// Przetwarzanie wyniku skanowania
const processScanResult = (decodedText) => {
  isProcessing.value = true;
  lastScannedCode.value = decodedText;
  
  try {
    const url = new URL(decodedText);
    
    if (url.pathname === '/player' || url.pathname.includes('/player')) {
      router.push('/player');
      closeModal();
    } else {
      scanError.value = 'Kod QR nie prowadzi do gry';
      resetProcessing();
    }
  } catch (urlError) {
    // Jeśli to nie URL, sprawdź czy to 4-cyfrowy kod
    if (/^\d{4}$/.test(decodedText)) {
      code.value = decodedText;
      joinGame();
    } else {
      scanError.value = 'Nieprawidłowy kod QR';
      resetProcessing();
    }
  }
};

const resetProcessing = () => {
  setTimeout(() => {
    isProcessing.value = false;
    lastScannedCode.value = '';
  }, 1500);
  
  setTimeout(() => {
    scanError.value = '';
  }, 3000);
};

// Inicjalizacja kamery
const onInit = async (promise) => {
  try {
    await promise;
  } catch (error) {
    let errorMessage = 'Błąd kamery';
    
    if (error.name === 'NotAllowedError') {
      errorMessage = 'Brak dostępu do kamery. Sprawdź uprawnienia przeglądarki.';
    } else if (error.name === 'NotFoundError') {
      errorMessage = 'Nie znaleziono kamery w urządzeniu.';
    } else if (error.name === 'NotReadableError') {
      errorMessage = 'Kamera jest obecnie używana przez inną aplikację.';
    }
    
    scanError.value = errorMessage;
  }
};

</script>