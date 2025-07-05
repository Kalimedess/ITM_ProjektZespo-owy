<template>
  <div class="px-3 py-2 sm:px-4 sm:py-2 md:px-6 md:py-3">
    <h2 class="text-lg sm:text-xl md:text-2xl font-nasalization mb-2 sm:mb-3 text-center">
      Zaloguj się do konta
    </h2>
    
    <div v-show="errorLogin" class="bg-red-500/20 border border-red-500 rounded-md p-2 sm:p-3 text-xs sm:text-sm">
      <p class="text-red-400">Nieprawidłowy e-mail lub hasło. Spróbuj ponownie ❗</p>
    </div>
    
    <form @submit.prevent="handleLogin" class="space-y-3 sm:space-y-4">
      
      <div class="space-y-1">
        <label for="email" class="block font-bold text-xs sm:text-sm text-left">
          E-mail
        </label>
        <input 
          type="email" 
          id="email" 
          v-model="loginData.email" 
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
          bg-tertiary hover:bg-accent text-white w-full rounded-lg font-medium 
          transition-all duration-300 shadow-sm hover:shadow-lg 
          shadow-accent/40 hover:shadow-accent/60
          py-2.5 sm:py-3 
          text-sm sm:text-base md:text-lg
        "
      >
        Zaloguj się
      </button>
    </form>
  </div>
</template>

<script setup>
import { ref, defineEmits } from 'vue';
import apiClient from '@/assets/plugins/axios';
import { useToast } from 'vue-toastification';
import router from '@/router';
import { faEye, faEyeSlash, faEnvelope, faLock } from '@fortawesome/free-solid-svg-icons';
import { useAuthStore } from '@/stores/auth';

const authStore = useAuthStore();

const toast = useToast();
const emit = defineEmits(['login', 'close','forgotPassword']);

const showPassword = ref(false);
const errorLogin = ref(false);

const loginData = ref({
  email: '',
  password: ''
});

const handleLogin = async () => {
  try {
    const response = await apiClient.post('/api/auth/login', {
      username: loginData.value.email, 
      password: loginData.value.password
    }, {
      withCredentials: true
    });

    if (response.data.success) {
      console.log('✅ Zalogowano pomyślnie');
      authStore.setAuthenticated(true);
      router.push('/admin');
    }
  } catch (error) {
    errorLogin.value = true;
    toast.error('Nieprawidłowy e-mail lub hasło. Spróbuj ponownie!', {
      position: 'top-center',
    });
    console.error('❌ Wystąpił błąd:', error.response?.data || error.message);
  }
};
</script>