<template>
    <div>
      <h2 class="text-2xl font-nasalization mb-6 text-center">Zmień hasło</h2>
      
      <div class="text-red-500 mt-2 mb-2" v-show="errorPasswordsNotMatch">
        <p>Podane hasła się nie zgadzają ❗</p>
      </div>
  
      <div class="text-red-500 mt-2 mb-2" v-show="!passwordValid">
        <p>Hasło nie spełnia wymagań ❗</p>
      </div>
      
      <form @submit.prevent="handleChangePassword">

        <!-- Stare hasło -->
        <div class="mb-4 relative">
          <input 
            :type="showOldPassword ? 'text' : 'password'" 
            id="register-password" 
            v-model="changePasswordData.oldPassword" 
            placeholder="Stare hasło..."
            class="w-full px-3 py-3 bg-tertiary border border-gray-700 rounded-md text-white focus:outline-none focus:border-accent"
            required
          />
  
          <button 
            @click="showOldPassword = !showOldPassword" 
            class="absolute right-3 -translate-y-1/2 top-1/2"
            type="button"
          >
            <font-awesome-icon :icon="showOldPassword ? faEye : faEyeSlash" class="h-4 text-white hover:text-accent transition-all duration-300" />
          </button>
        </div>
        
        <!-- Nowe hasło -->
        <div class="mb-4 relative">
          <input 
            :type="showPassword ? 'text' : 'password'" 
            id="register-password" 
            v-model="changePasswordData.password" 
            placeholder="Nowe hasło..."
            class="w-full px-3 py-3 bg-tertiary border border-gray-700 rounded-md text-white focus:outline-none focus:border-accent"
            required
          />
  
          <button 
            @click="showPassword = !showPassword" 
            class="absolute right-3 -translate-y-1/2 top-1/2"
            type="button"
          >
            <font-awesome-icon :icon="showPassword ? faEye : faEyeSlash" class="h-4 text-white hover:text-accent transition-all duration-300" />
          </button>
        </div>
  
        <div class="mb-4">
          <passwordStrength :password="changePasswordData.password" />
        </div>
        
        <!-- Potwierdź nowe hasło -->
        <div class="mb-5 relative">
          <input 
            :type="showConfirmPassword ? 'text' : 'password'" 
            id="register-confirm-password" 
            v-model="changePasswordData.confirmPassword" 
            placeholder="Potwierdź hasło..."
            class="w-full px-3 py-3 bg-tertiary border border-gray-700 rounded-md text-white focus:outline-none focus:border-accent"
            required
          />
  
          <button 
            @click="showConfirmPassword = !showConfirmPassword" 
            class="absolute right-3 -translate-y-1/2 top-1/2"
            type="button"
          >
            <font-awesome-icon :icon="showConfirmPassword ? faEyeSlash : faEye" class="h-4 text-white hover:text-accent transition-all duration-300" />
          </button>
        </div>
  
        <div class="mb-5">
          <ul class="list-disc text-left text-white pl-5">
            <li :class="{ 'text-green-500': passwordRequirements.length, 'text-gray-500': !passwordRequirements.length }">
              Co najmniej 8 znaków
            </li>
            <li :class="{ 'text-green-500': passwordRequirements.uppercase, 'text-gray-500': !passwordRequirements.uppercase }">
              Co najmniej jedna duża litera
            </li>
            <li :class="{ 'text-green-500': passwordRequirements.lowercase, 'text-gray-500': !passwordRequirements.lowercase }">
              Co najmniej jedna mała litera
            </li>
            <li :class="{ 'text-green-500': passwordRequirements.special, 'text-gray-500': !passwordRequirements.special }">
              Co najmniej jeden znak specjalny
            </li>
            <li :class="{ 'text-green-500': passwordRequirements.digit, 'text-gray-500': !passwordRequirements.digit }">
              Co najmniej jedna cyfra
            </li>
          </ul>
        </div>
        
        <button 
          type="submit" 
          class="bg-tertiary hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60 mb-5"
        >
          Zmień hasło
        </button>
      </form>
    </div>
  </template>
  
  <script setup>
    import { ref, computed} from 'vue';
    import { useToast } from 'vue-toastification';
    import passwordStrength from './passwordStrength.vue';
    import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
    
    const toast = useToast();

    const showOldPassword = ref(false);
    const showPassword = ref(false);
    const showConfirmPassword = ref(false);
    const errorPasswordsNotMatch = ref(false);
    const passwordValid = ref(true);
    
    const changePasswordData = ref({
        oldPassword: '',
        password: '',
        confirmPassword: ''
    });
    
    const passwordRequirements = computed(() => {
        return {
        length: changePasswordData.value.password.length >= 8,
        uppercase: /[A-Z]/.test(changePasswordData.value.password),
        lowercase: /[a-z]/.test(changePasswordData.value.password),
        digit: /[0-9]/.test(changePasswordData.value.password),
        special: /[!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]/.test(changePasswordData.value.password)
        };
    });
    
    const handleChangePassword = async () => {
        passwordValid.value = true;
        errorPasswordsNotMatch.value = changePasswordData.value.password !== changePasswordData.value.confirmPassword;
    
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
          const response = null;
            //Tu będzie możliwość zmiany hasła
    
        if(response.data.success) {
            console.log('✅ Zarejestrowano pomyślnie!');
            toast.success("Pomyślnie zmieniono hasło!", {
                position: 'top-center',
            });
            }
        } catch(error) {
        console.error('❌ Wystąpił błąd:', error.response?.data || error.message);
        toast.error("Wystąpił błąd! Spróbuj ponownie", {
            position: "top-center",
        });
        }
    };
  </script>