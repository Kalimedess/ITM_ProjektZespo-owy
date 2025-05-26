<template>
  <div class="
    px-3 py-2 
    sm:px-4 sm:py-3 
    md:px-6 md:py-4
    max-h-[calc(100vh-80px)] 
    overflow-y-auto
    space-y-2 sm:space-y-3 md:space-y-4
  ">
    <h2 class="
      text-lg sm:text-xl md:text-2xl 
      font-nasalization 
      mb-2 sm:mb-3 md:mb-4 
      text-center
    ">
      Zaloguj się do konta
    </h2>
    
    <div v-show="errorLogin" class="
      bg-red-500/20 border border-red-500 rounded-md 
      p-2 sm:p-3 
      text-xs sm:text-sm
    ">
      <p class="text-red-400">Nieprawidłowy e-mail lub hasło. Spróbuj ponownie ❗</p>
    </div>
    
    <form @submit.prevent="handleLogin" class="space-y-2 sm:space-y-3 md:space-y-4">
      <div class="space-y-1 sm:space-y-2">
        <label for="email" class="
          block font-bold 
          text-xs sm:text-sm md:text-base
          text-left
        ">
          E-mail
        </label>
        <input 
          type="email" 
          id="email" 
          v-model="loginData.email" 
          placeholder="E-mail..."
          class="
            w-full px-3 bg-tertiary border border-gray-700 rounded-md text-white 
            focus:outline-none focus:border-accent
            h-[40px] sm:h-[44px] md:h-[48px]
            text-sm sm:text-base
          "
          required
        />
      </div>

      <div class="space-y-1 sm:space-y-2">
        <label for="password" class="
          block font-bold 
          text-xs sm:text-sm md:text-base
          text-left
        ">
          Hasło
        </label>
        <div class="relative">
          <input 
            :type="showPassword ? 'text' : 'password'"
            id="password" 
            v-model="loginData.password" 
            placeholder="Hasło..."
            class="
              w-full px-3 pr-10 bg-tertiary border border-gray-700 rounded-md text-white 
              focus:outline-none focus:border-accent
              h-[40px] sm:h-[44px] md:h-[48px]
              text-sm sm:text-base
            "
            required
          />
          <button 
            @click="showPassword = !showPassword" 
            class="
              absolute right-3 top-1/2 -translate-y-1/2
              h-4 sm:h-5 w-4 sm:w-5
            "
            type="button"
          >
            <font-awesome-icon 
              :icon="showPassword ? faEye : faEyeSlash" 
              class="text-white hover:text-accent transition-all duration-300" 
            />
          </button>
        </div>
      </div>
      
      <div class="text-xs sm:text-sm">
        <a href="#" class="text-accent hover:text-purple-300">Zapomniałem hasła</a>
      </div>
      
      <button 
        type="submit" 
        class="
          bg-tertiary hover:bg-accent text-white w-full rounded-lg font-medium 
          transition-all duration-300 shadow-sm hover:shadow-lg 
          shadow-accent/40 hover:shadow-accent/60
          h-[44px] sm:h-[48px] md:h-[52px]
          text-sm sm:text-base md:text-lg
          mt-3 sm:mt-4 md:mt-5
        "
      >
        Zaloguj się
      </button>
    </form>
  </div>
</template>

<script setup>
import { ref, defineEmits } from 'vue';
import axios from 'axios';
import { useToast } from 'vue-toastification';
import router from '@/router';
import { faEye, faEyeSlash, faEnvelope, faLock } from '@fortawesome/free-solid-svg-icons';
import { useAuthStore } from '@/stores/auth';

const authStore = useAuthStore();

const toast = useToast();
const emit = defineEmits(['login', 'close']);

const showPassword = ref(false);
const errorLogin = ref(false);

const loginData = ref({
  email: '',
  password: ''
});

const handleLogin = async () => {
  try {
    const response = await axios.post('http://localhost:5023/api/auth/login', {
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