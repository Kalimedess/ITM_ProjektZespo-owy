<template>
  <div class="
    px-3 py-2 
    sm:px-4 sm:py-3 
    md:px-6 md:py-4
    max-h-[90vh]
    overflow-y-auto
    overflow-x-hidden
    space-y-2 sm:space-y-3 md:space-y-4
  ">
    <h2 class="
      text-lg sm:text-xl md:text-2xl 
      font-nasalization 
      mb-2 sm:mb-3 md:mb-4 
      text-center
    ">
      Utwórz nowe konto
    </h2>
    
    <div v-show="errorPasswordsNotMatch || !passwordValid" class="
      bg-red-500/20 border border-red-500 rounded-md 
      p-2 sm:p-3 
      text-xs sm:text-sm
    ">
      <p v-if="errorPasswordsNotMatch" class="text-red-400">Podane hasła się nie zgadzają ❗</p>
      <p v-if="!passwordValid" class="text-red-400">Hasło nie spełnia wymagań ❗</p>
    </div>
    
    <form @submit.prevent="handleRegister" class="space-y-2 sm:space-y-3 md:space-y-4">
      <div class="space-y-1 sm:space-y-2">
        <label for="register-username" class="
          block font-bold 
          text-xs sm:text-sm md:text-base
        ">
          Nazwa użytkownika
        </label>
        <input 
          type="text" 
          id="register-username" 
          v-model="registerData.username" 
          placeholder="Nazwa użytkownika..."
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
        <label for="register-email" class="
          block font-bold 
          text-xs sm:text-sm md:text-base
        ">
          E-mail
        </label>
        <input 
          type="email" 
          id="register-email" 
          v-model="registerData.email" 
          placeholder="Email..."
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
        <label for="register-password" class="
          block font-bold 
          text-xs sm:text-sm md:text-base
        ">
          Hasło
        </label>
        <div class="relative">
          <input 
            :type="showPassword ? 'text' : 'password'" 
            id="register-password" 
            v-model="registerData.password" 
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
        <passwordStrength :password="registerData.password" />
      </div>

      <div class="space-y-1 sm:space-y-2">
        <label for="register-confirm-password" class="
          block font-bold 
          text-xs sm:text-sm md:text-base
        ">
          Potwierdź hasło
        </label>
        <div class="relative">
          <input 
            :type="showConfirmPassword ? 'text' : 'password'" 
            id="register-confirm-password" 
            v-model="registerData.confirmPassword" 
            placeholder="Potwierdź hasło..."
            class="
              w-full px-3 pr-10 bg-tertiary border border-gray-700 rounded-md text-white 
              focus:outline-none focus:border-accent
              h-[40px] sm:h-[44px] md:h-[48px]
              text-sm sm:text-base
            "
            required
          />
          <button 
            @click="showConfirmPassword = !showConfirmPassword" 
            class="
              absolute right-3 top-1/2 -translate-y-1/2
              h-4 sm:h-5 w-4 sm:w-5
            "
            type="button"
          >
            <font-awesome-icon 
              :icon="showConfirmPassword ? faEye : faEyeSlash" 
              class="text-white hover:text-accent transition-all duration-300" 
            />
          </button>
        </div>
      </div>

      <div class="
        bg-gray-800/30 rounded-md p-2 sm:p-3
      ">
        <ul class="list-disc text-left text-white pl-5 space-y-1">
          <li :class="{ 'text-green-500': passwordRequirements.length, 'text-gray-500': !passwordRequirements.length }" class="text-xs sm:text-sm">
            Co najmniej 8 znaków
          </li>
          <li :class="{ 'text-green-500': passwordRequirements.uppercase, 'text-gray-500': !passwordRequirements.uppercase }" class="text-xs sm:text-sm">
            Co najmniej jedna duża litera
          </li>
          <li :class="{ 'text-green-500': passwordRequirements.lowercase, 'text-gray-500': !passwordRequirements.lowercase }" class="text-xs sm:text-sm">
            Co najmniej jedna mała litera
          </li>
          <li :class="{ 'text-green-500': passwordRequirements.special, 'text-gray-500': !passwordRequirements.special }" class="text-xs sm:text-sm">
            Co najmniej jeden znak specjalny
          </li>
          <li :class="{ 'text-green-500': passwordRequirements.digit, 'text-gray-500': !passwordRequirements.digit }" class="text-xs sm:text-sm">
            Co najmniej jedna cyfra
          </li>
        </ul>
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