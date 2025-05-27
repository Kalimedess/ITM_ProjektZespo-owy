<template>
  <div class="px-3 py-2 sm:px-4 sm:py-2 md:px-6 md:py-3">
    <h2 class="text-lg sm:text-xl md:text-2xl font-nasalization mb-2 sm:mb-3 text-center">
      Utwórz nowe konto
    </h2>
    
    <div v-show="errorPasswordsNotMatch || !passwordValid" class="bg-red-500/20 border border-red-500 rounded-md p-2 sm:p-3 text-xs sm:text-sm">
      <p v-if="errorPasswordsNotMatch" class="text-red-400">Podane hasła się nie zgadzają ❗</p>
      <p v-if="!passwordValid" class="text-red-400">Hasło nie spełnia wymagań ❗</p>
    </div>
    
    <form @submit.prevent="handleRegister" class="space-y-3 sm:space-y-4">
      <div class="space-y-1">
        <label for="register-username" class="block font-bold text-xs sm:text-sm text-left">
          Nazwa użytkownika
        </label>
        <input 
          type="text" 
          id="register-username" 
          v-model="registerData.username" 
          class="w-full px-3 py-2 bg-tertiary border border-lgray-accent rounded-md text-white focus:outline-none focus:border-accent text-sm sm:text-base"
          required
        />
      </div>

      <div class="space-y-1">
        <label for="register-email" class="block font-bold text-xs sm:text-sm text-left">
          E-mail
        </label>
        <input 
          type="email" 
          id="register-email" 
          v-model="registerData.email" 
          class="w-full px-3 py-2 bg-tertiary border border-lgray-accent rounded-md text-white focus:outline-none focus:border-accent text-sm sm:text-base"
          required
        />
      </div>

      <div class="grid grid-cols-1 md:grid-cols-2 gap-3 sm:gap-4">
        <div class="space-y-1">
          <label for="register-password" class="block font-bold text-xs sm:text-sm text-left">
            Hasło
          </label>
          <div class="
            flex items-center gap-2
            bg-tertiary border border-lgray-accent rounded-md 
            transition-all duration-200
            focus-within:border-accent focus-within:ring-1 focus-within:ring-accent
          ">
            <input 
              :type="showPassword ? 'text' : 'password'" 
              id="register-password" 
              v-model="registerData.password" 
              class="w-full px-3 py-2 bg-transparent focus:outline-none focus:ring-0 text-white flex-grow"
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

        <div class="space-y-1">
          <label for="register-confirm-password" class="block font-bold text-xs sm:text-sm text-left">
            Potwierdź hasło
          </label>
          <div class="
            flex items-center gap-2
            bg-tertiary border border-lgray-accent rounded-md 
            transition-all duration-200
            focus-within:border-accent focus-within:ring-1 focus-within:ring-accent
          ">
            <input 
              :type="showConfirmPassword ? 'text' : 'password'" 
              id="register-confirm-password" 
              v-model="registerData.confirmPassword" 
              class="w-full px-3 py-2 bg-transparent focus:outline-none focus:ring-0 text-white flex-grow"
              required
            />
            <button 
              @click="showConfirmPassword = !showConfirmPassword" 
              class="h-8 w-8 flex-shrink-0 mr-1 flex items-center justify-center rounded-full text-gray-400 hover:bg-white/10 hover:text-white transition-all duration-200"
              type="button"
            >
              <font-awesome-icon :icon="showConfirmPassword ? faEye : faEyeSlash" class="h-5 w-5" />
            </button>
          </div>
        </div>
      </div>

      <div class="text-xs sm:text-sm">
        <passwordStrength :password="registerData.password" />
      </div>

      <div class="bg-tertiary rounded-md px-3 py-2">
        <ul class="list-disc text-left text-white pl-4">
          <li :class="{ 'text-green-500': passwordRequirements.length, 'text-gray-500': !passwordRequirements.length }" class="text-xs transition-colors duration-300">
            Co najmniej 8 znaków
          </li>
          <li :class="{ 'text-green-500': passwordRequirements.uppercase, 'text-gray-500': !passwordRequirements.uppercase }" class="text-xs transition-colors duration-300">
            Co najmniej jedna duża litera
          </li>
          <li :class="{ 'text-green-500': passwordRequirements.lowercase, 'text-gray-500': !passwordRequirements.lowercase }" class="text-xs transition-colors duration-300">
            Co najmniej jedna mała litera
          </li>
          <li :class="{ 'text-green-500': passwordRequirements.special, 'text-gray-500': !passwordRequirements.special }" class="text-xs transition-colors duration-300">
            Co najmniej jeden znak specjalny
          </li>
          <li :class="{ 'text-green-500': passwordRequirements.digit, 'text-gray-500': !passwordRequirements.digit }" class="text-xs transition-colors duration-300">
            Co najmniej jedna cyfra
          </li>
        </ul>
      </div>
      
      <button 
        type="submit" 
        class="bg-tertiary hover:bg-accent text-white w-full rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60 py-2.5 sm:py-3 text-sm sm:text-base md:text-lg !mt-5"
      >
        Zarejestruj się
      </button>
    </form>
  </div>
</template>

<script setup>
import { ref, computed, defineEmits } from 'vue';
import axios from 'axios';
import { useToast } from 'vue-toastification';
import passwordStrength from './passwordStrength.vue';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';

const toast = useToast();
const emit = defineEmits(['register', 'close', 'switchToLogin']);

const showPassword = ref(false);
const showConfirmPassword = ref(false);
const errorPasswordsNotMatch = ref(false);
const passwordValid = ref(true);

const registerData = ref({
  username: '',
  email: '',
  password: '',
  confirmPassword: ''
});

const passwordRequirements = computed(() => {
  return {
    length: registerData.value.password.length >= 8,
    uppercase: /[A-Z]/.test(registerData.value.password),
    lowercase: /[a-z]/.test(registerData.value.password),
    digit: /[0-9]/.test(registerData.value.password),
    special: /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(registerData.value.password)
  };
});

const handleRegister = async () => {
  passwordValid.value = true;
  errorPasswordsNotMatch.value = registerData.value.password !== registerData.value.confirmPassword;

  if(errorPasswordsNotMatch.value) {
    passwordValid.value = true;
    toast.error("Hasła się nie zgadzają!");
    return;
  }

  const req = passwordRequirements.value;
  passwordValid.value = req.length && req.uppercase && req.lowercase && req.digit && req.special;

  if(!passwordValid.value) {   
    toast.error("Podane hasło nie spełnia wymagań!", {
      position: 'top-center'
    });
    return;
  }
  
  try {
    const response = await axios.post('http://localhost:5023/api/auth/register', {
      username: registerData.value.username,
      email: registerData.value.email,
      password: registerData.value.password
    });

    if(response.data.success) {
      console.log('✅ Zarejestrowano pomyślnie!');
      toast.success("Pomyślnie zarejestrowano!", {
          position: 'top-center',
      });
              emit('switchToLogin');
      }
  } catch(error) {
    console.error('❌ Wystąpił błąd:', error.response?.data || error.message);
    toast.error("Wystąpił błąd! Spróbuj ponownie", {
      position: "top-center",
    });
  }
};
</script>