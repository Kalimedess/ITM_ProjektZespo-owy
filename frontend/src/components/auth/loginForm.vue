<template>
  <div class="px-3 py-2 sm:px-4 sm:py-2 md:px-6 md:py-3">
    <h2 class="text-lg sm:text-xl md:text-2xl font-nasalization mb-2 sm:mb-3 text-center">
      Zaloguj się do konta
    </h2>
    
    <form @submit.prevent="handleLogin" class="space-y-3 sm:space-y-4">
      
      <div class="space-y-1">
        <label for="email" class="block font-bold text-xs sm:text-sm text-left">
          E-mail
        </label>
        <input 
          type="email" 
          id="email" 
          v-model="loginData.username" 
          class="w-full px-3 py-2 bg-tertiary border border-gray-600 rounded-md text-white focus:outline-none focus:border-accent text-sm sm:text-base"
          required
        />
      </div>

      <div class="space-y-1">
        <label for="password" class="block font-bold text-xs sm:text-sm text-left">
          Hasło
        </label>
        <div class="
          flex items-center gap-2
          bg-tertiary border border-gray-600 rounded-md 
          transition-all duration-200
          focus-within:border-accent focus-within:ring-1 focus-within:ring-accent
        ">
          <input 
            :type="showPassword ? 'text' : 'password'"
            id="password" 
            v-model="loginData.password" 
            class="w-full px-3 py-2 bg-transparent focus:outline-none focus:ring-0 text-white placeholder-gray-400 flex-grow"
            required
          />
          <button 
            @click="showPassword = !showPassword" 
            class="h-8 w-8 flex-shrink-0 mr-1 flex items-center justify-center rounded-full text-gray-400 hover:bg-white/10 hover:text-white transition-all duration-200"
            type="button"
          >
            <font-awesome-icon :icon="showPassword ? faEye : faEyeSlash" class="h-5 w-5" />
          </button>
        </div>
      </div>
      
      <div class="text-xs sm:text-sm text-right">
        <span class="text-accent hover:text-purple-300 transition-colors cursor-pointer"
        @click="emit('forgotPassword')">
        Zapomniałem hasła
      </span>
      </div>
      
      <button 
        type="submit" 
        class="
          text-white w-full rounded-lg font-medium 
          transition-all duration-300 shadow-sm 
          shadow-accent/40 
          py-2.5 sm:py-3 
          text-sm sm:text-base md:text-lg
        "
        :class="isLoginFormValid ? 'bg-accent/50 hover:shadow-lg hover:shadow-accent/60 hover:bg-accent' : 'bg-tertiary' "
        :disabled="!isLoginFormValid  || isLoading"
      >
        {{ isLoading ? 'Logowanie...' : 'Zaloguj się' }}
      </button>
    </form>
  </div>
</template>

<script setup>
<<<<<<< HEAD
import { ref, defineEmits, computed, nextTick } from 'vue';
import apiClient from '@/assets/plugins/axios';
import { useToast } from 'vue-toastification';
import router from '@/router';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
=======
import { ref,nextTick} from 'vue';
import { useToast } from 'vue-toastification';
import router from '@/router';
import { faEye, faEyeSlash} from '@fortawesome/free-solid-svg-icons';
>>>>>>> 7c8006530c79909b4a73b185519bc8461dac2749
import { useAuthStore } from '@/stores/auth';

import apiConfig from '@/services/apiConfig.js';
import apiService from '@/services/apiServices.js';

const authStore = useAuthStore();

const toast = useToast();
<<<<<<< HEAD
const emit = defineEmits(['login', 'close','forgotPassword']);
=======
>>>>>>> 7c8006530c79909b4a73b185519bc8461dac2749

const showPassword = ref(false);

//Zmienna zapobiegająca wielokrotnemu wysyłaniu żądania logowania
const isLoading = ref(false);

const loginData = ref({
  username: '',
  password: ''
});

//Funckcja do sprawdzania poprawności adresu e-mail
const validateEmail = (email) => {
  return String(email)
    .toLowerCase()
    .match(
      /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
    );
};


//Sprawdzenie poprawności formularza logowania, jeżeli pola są puste lub e-mail jest niepoprawny, przycisk będzie zablokowany
const isLoginFormValid = computed(() => {
  return loginData.value.email !== '' && 
  loginData.value.password !== '' && 
  validateEmail(loginData.value.email);
});




const handleLogin = async () => {

  toast.clear(); // Czyszczenie toastów przed nowym logowaniem, żeby uniknąć zalania komunikatami

  if (!isLoginFormValid.value) {
    toast.error('Proszę wprowadzić poprawny e-mail i hasło.', {
      position: 'top-center',
    });
    return;
  }

  isLoading.value = true;

  try {
    const response = await apiService.post(apiConfig.auth.login, loginData.value);

    if (response.data.success) {
      console.log('✅ Zalogowano pomyślnie');
      authStore.setAuthenticated(true);
<<<<<<< HEAD

=======
>>>>>>> 7c8006530c79909b4a73b185519bc8461dac2749
      await nextTick();
      router.push('/admin');
    }
  } catch (error) {
    
    // Sprawdzamy czy mamy response z serwera
    if (error.response?.data) {
      
      // Niepoprawne dane logowania
      if (error.response.data === 'Invalid credentials') {
        toast.error('Nieprawidłowy e-mail lub hasło. Spróbuj ponownie!', {
          position: 'top-center',
        });
      }
      
      // Niezweryfikowany e-mail
      else if (error.response.data === 'E-mail nie został potwierdzony. Wysłano ponownie link aktywacyjny.') {
        toast.warning('E-mail nie został potwierdzony. Wysłano ponownie link aktywacyjny. Sprawdź skrzynkę pocztową!', {
          position: 'top-center',
        });
      }
      
      // Inne błędy z serwera
      else {
        toast.error('Wystąpił błąd podczas logowania. Spróbuj ponownie!', {
          position: 'top-center',
        });
      }
      
    } else {
      // Błędy sieci 
      toast.error('Brak połączenia z serwerem. Sprawdź połączenie internetowe.', {
        position: 'top-center',
      });
    }
    console.error('❌ Wystąpił błąd:', error.response?.data || error.message);
  }
  finally {
    isLoading.value = false;
  }
};

</script>