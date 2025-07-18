<template>

  <!--Formularz rejestracji użytkownika-->
  <div   class="px-3 py-2 sm:px-4 sm:py-2 md:px-6 md:py-3">
    <h2 class="text-lg sm:text-xl md:text-2xl font-nasalization mb-2 sm:mb-3 text-center">
      Utwórz nowe konto
    </h2>
    
    
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
        class=" text-white w-full rounded-lg font-medium transition-all duration-300 shadow-sm  shadow-accent/40 py-2.5 sm:py-3 text-sm sm:text-base md:text-lg"
        :disabled="!isRegisterFormValid || isLoading"
        :class="isRegisterFormValid ? 'bg-accent/50 hover:shadow-lg hover:shadow-accent/60 hover:bg-accent' : 'bg-tertiary' "
      >
        {{ isLoading ? 'Rejestracja...' : 'Zarejestruj się' }}
      </button>
    </form>
  </div>
</template>

<script setup>
import { ref, computed, defineEmits } from 'vue';
import { useToast } from 'vue-toastification';
import passwordStrength from './passwordStrength.vue';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';



import apiConfig from '@/services/apiConfig.js';
import apiService from '@/services/apiServices.js';


const toast = useToast();
const emit = defineEmits(['register', 'close', 'switchToConfirmEmail']);

const showPassword = ref(false);
const showConfirmPassword = ref(false);
const isLoading = ref(false);

//Funckcja do sprawdzania poprawności adresu e-mail
const validateEmail = (email) => {
  return String(email)
    .toLowerCase()
    .match(
      /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
    );
};

const isRegisterFormValid = computed(() => {
  const req = passwordRequirements.value;

  return registerData.value.username.trim() !== '' &&
         registerData.value.email.trim() !== '' &&
         registerData.value.password.trim() !== '' &&
         registerData.value.confirmPassword.trim() !== '' &&
         validateEmail(registerData.value.email) &&
        req.length && req.uppercase && req.lowercase && req.digit && req.special;
});


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
    special: /[!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]/.test(registerData.value.password)
  };
});

const handleRegister = async () => {

  toast.clear();

  const errorPasswordsNotMatch = registerData.value.password !== registerData.value.confirmPassword;

  if(errorPasswordsNotMatch) {
    toast.error("Podane hasła się nie zgadzają!",{
      position: 'top-center',
    });
    return;
  }

  try {

    isLoading.value = true;
    
    const response = await apiService.post(apiConfig.auth.register, registerData.value);


    if(response.data.success) {
      console.log('✅ Zarejestrowano pomyślnie!');
      toast.success("Pomyślnie zarejestrowano!", {
          position: 'top-center',
      });
          emit('switchToConfirmEmail',registerData.value.email);
      }
  } catch(error) {
      if(error.response?.data) {
        if (error.response.data === 'Email already exist.') {
          toast.error("Ten e-mail jest już zarejestrowany!", {
            position: 'top-center',
          });
        }
        else {
          toast.error("Wystąpił błąd podczas rejestracji. Spróbuj ponownie.", {
            position: 'top-center',
          });
        }
      }
  }
  finally {
    isLoading.value = false;
  }
};
</script>