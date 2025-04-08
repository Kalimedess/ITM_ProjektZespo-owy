<template>
    <div>
      <h2 class="text-2xl font-bold mb-8 text-center">Zaloguj się</h2>
      
      <div v-show="errorLogin">
        <p class="text-red-500 mt-2 mb-2">Nieprawidłowy e-mail lub hasło. Spróbuj ponownie ❗</p>
      </div>
      
      <form @submit.prevent="handleLogin">
        <div class="mb-4">
          <input 
            type="email" 
            id="login" 
            v-model="loginData.email" 
            placeholder="E-mail..."
            class="w-full px-3 py-3 bg-tertiary border border-gray-700 rounded-md text-white focus:outline-none focus:border-accent"
            required
          />
        </div>
        
        <div class="mb-2 relative">
          <input 
            :type="showPassword ? 'text' : 'password'"
            id="password" 
            v-model="loginData.password" 
            placeholder="Hasło..."
            class="w-full px-3 py-3 bg-tertiary border border-gray-700 rounded-md text-white focus:outline-none focus:border-accent"
            required
          />
  
          <button 
            @click="showPassword = !showPassword" 
            class="absolute right-3 -translate-y-1/2 top-1/2"
            type="button"
          >
            <font-awesome-icon :icon="showPassword ? faEyeSlash : faEye" class="h-4 text-white hover:text-accent transition-all duration-300" />
          </button>
        </div>
        
        <div class="mb-5 text-sm">
          <a href="#" class="text-accent hover:text-purple-300">Zapomniałem hasła</a>
        </div>
        
        <button 
          type="submit" 
          class="bg-tertiary hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60 mb-5"
        >
          Zaloguj
        </button>
      </form>
    </div>
  </template>
  
  <script setup>
  import { ref, defineEmits } from 'vue';
  import axios from 'axios';
  import { useToast } from 'vue-toastification';
  import router from '@/router';
  import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
  
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
      });
  
      if (response.data.success) {
        console.log('✅ Zalogowano pomyślnie');
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