<template>
  <div class="bg-primary min-h-screen">
    <div class="fixed inset-0 flex items-center justify-center z-50">
      <div class="absolute inset-0 bg-black/40 transition-opacity duration-300"></div>
      <div 
        class="bg-primary text-white rounded-lg relative z-10 border-2
         border-accent transition-all duration-300
         p-6 sm:p-8 md:p-10 lg:p-12
         max-h-[90vh]
         w-full max-w-lg 
         "
      >
      <!-- Formularz  zmiany hasła -->
      <div v-if="isTokenValid">
      <h2 class="text-lg sm:text-xl md:text-2xl font-nasalization mb-2 sm:mb-3 text-center">
            Zmień hasło
      </h2>

      <div class="w-100 h-0.5 mb-1 sm:mb-2 md:mb-3 lg:mb-4 bg-accent"></div>
      
      <form @submit.prevent="handleChangePassword">
        <label for="register-password" class="block font-bold text-xs sm:text-sm text-left mb-1">
            Nowe hasło
        </label>
        <!-- Nowe hasło -->
        <div class="
            flex items-center gap-2
            bg-tertiary border border-lgray-accent rounded-md 
            transition-all duration-200
            focus-within:border-accent focus-within:ring-1 focus-within:ring-accent mb-3
          ">
            <input 
              :type="showPassword ? 'text' : 'password'" 
              id="register-confirm-password" 
              v-model="changePasswordData.password" 
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
  
        <div class="mb-4">
          <passwordStrength :password="changePasswordData.password" />
        </div>
        
        <!-- Potwierdź nowe hasło -->
         <label for="register-password" class="block font-bold text-xs sm:text-sm text-left mb-1">
            Potwierdź nowe hasło
        </label>
        <!-- Nowe hasło -->
        <div class="
            flex items-center gap-2
            bg-tertiary border border-lgray-accent rounded-md 
            transition-all duration-200
            focus-within:border-accent focus-within:ring-1 focus-within:ring-accent mb-3
          ">
            <input 
              :type="showConfirmPassword ? 'text' : 'password'" 
              id="register-confirm-password" 
              v-model="changePasswordData.confirmPassword" 
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
        
  
        <div class="bg-tertiary rounded-md px-3 py-2 mb-5">
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
          class="text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm shadow-accent/40 mb-5"
          :class="allPasswordRequirementsMet ? 'bg-accent/50 hover:shadow-lg hover:shadow-accent/60 hover:bg-accent' : 'bg-tertiary' "
          :disabled="!allPasswordRequirementsMet || isLoading"
        >
          {{ isLoading ? 'Zmiana hasła...' : 'Zmień hasło' }}
        </button>
      </form>
    </div>

    <!--Widok wygaśnietego tokenu-->
    <div v-else>
        <h2 class="text-lg sm:text-xl md:text-2xl font-nasalization mb-2 sm:mb-3 text-center">
          Link wygasł
        </h2>

        <font-awesome-icon :icon="faCircleXmark" class="h-20 text-red-500 mb-3 text-center block mx-auto" />

      <div class="w-100 h-0.5 mb-1 sm:mb-2 md:mb-3 lg:mb-4 bg-accent"></div>

      <div class="text-center">
        <p class="mb-3">Wygląda na to, że twój link do resetowania hasła wygasł.</p>
        <p>Powróć do strony logowania i wybierz opcję "Zapomniałem hasła" żeby otrzymać nowy link.</p>
      </div>
      <button
          @click="handleReturnToLogin"
          class="
          bg-tertiary hover:bg-accent text-white w-full rounded-lg font-medium 
          transition-all duration-300 shadow-sm hover:shadow-lg 
          shadow-accent/40 hover:shadow-accent/60
          py-2.5 sm:py-3 
          text-sm sm:text-base md:text-lg
          mt-5
          "
      >
          Powrót do logowania
      </button>
    </div>
  </div>
    </div>
  </div>

</template>

<script setup>
    import { ref, computed, onMounted } from 'vue';
    import { useToast } from 'vue-toastification';
    import passwordStrength from '@/components/auth/passwordStrength.vue';
    import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
    import { faCircleXmark} from '@fortawesome/free-regular-svg-icons';
    import { useRouter } from 'vue-router';
    import apiConfig from '@/services/apiConfig.js';
    import apiService from '@/services/apiServices.js';
    const toast = useToast();
    
    const router = useRouter();

    import { useRoute } from 'vue-router';
    import apiClient from '@/assets/plugins/axios';

    const route = useRoute();
    const token = ref('');

    onMounted(async () => {
      token.value = route.params.token;

      try {
      const res = await apiService.get(apiConfig.auth.validateResetToken(token.value));
        isTokenValid.value = res.data.valid;
      } catch (err) {
        isTokenValid.value = false;
        toast.error('Token wygasł lub jest nieprawidłowy.', {
        position: 'top-center',
        });
      }
    });


    //Ta zmienna przechowuję czy token jest poprawny (nie istnieje albo wygasł) domyślnie false
    const isTokenValid = ref(false);

    const showPassword = ref(false);
    const showConfirmPassword = ref(false);
    
    const isLoading = ref(false);
    
    const changePasswordData = ref({
        password: '',
        confirmPassword: ''
    });
    
    const passwordRequirements = computed(() => {
        return {
        length: changePasswordData.value.password.length >= 8,
        uppercase: /[A-Z]/.test(changePasswordData.value.password),
        lowercase: /[a-z]/.test(changePasswordData.value.password),
        digit: /[0-9]/.test(changePasswordData.value.password),
        special: /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(changePasswordData.value.password)
        };
    });

    const allPasswordRequirementsMet = computed(() => {
      const req = passwordRequirements.value;
      return req.length && req.uppercase && req.lowercase && req.digit && req.special &&
      changePasswordData.value.password.trim() !== '' &&
      changePasswordData.value.confirmPassword.trim() !== '';
    });

    const handleReturnToLogin = () => {
       sessionStorage.setItem('showLoginAfterRedirect', 'true');
       router.push('/')
    }
    
    const handleChangePassword = async () => {

        toast.clear(); // Czyszczenie toastów przed nową zmianą hasła, żeby uniknąć zalania komunikatami

        const errorPasswordsNotMatch = changePasswordData.value.password !== changePasswordData.value.confirmPassword;
    
        if(errorPasswordsNotMatch) {
            toast.error("Hasła się nie zgadzają!", {
                position: "top-center",
              });
            return;
        }
    
        try {
          isLoading.value = true;
          const response = await apiService.post(apiConfig.auth.resetPassword, {
            token: token.value,
            newPassword: changePasswordData.value.password
        });

  if (response.data.success) {
    console.log('✅ Hasło zmienione pomyślnie!');
    toast.success("Pomyślnie zmieniono hasło!", {
      position: 'top-center',
    });
    router.push('/');
  }
} catch (error) {
  console.error('❌ Wystąpił błąd:', error.response?.data || error.message);
  toast.error("Wystąpił błąd! Spróbuj ponownie", {
    position: "top-center",
  });
}

finally {
  isLoading.value = false;
}


    };

</script>
