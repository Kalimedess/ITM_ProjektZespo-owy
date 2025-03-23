<template>
<!--Komponent służący jako formularz logowania/rejestracji-->

    <div v-if="props.isVisible" class="fixed inset-0 flex items-center justify-center z-50">
        <div class="absolute inset-0  bg-black/70" @click="closeModal"></div>
  
        <div class="bg-primary text-white rounded-lg w-96 relative z-10 border-2 border-accent p-12 animate-jump-in">


          <button 
            @click="closeModal" 
            class="absolute top-2 right-2 w-8 h-8 flex items-center justify-center"
          >
            <font-awesome-icon :icon="faXmark" class="h-5 text-white  hover:text-accent transition-all duration-100" />
          </button>

          <!--Formularz logowania-->
            <div v-if="activeView === 'login'" class=" animate-fade animate-duration-1000">
                <h2 class="text-2xl font-bold mb-8 text-center">Zaloguj się</h2>
                
                <div class="flex mb-5 gap-2">
                    <button 
                      class="bg-accent hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60" 
                      @click="activeView = 'login'"
                    >
                      Zaloguj
                    </button>
  
                    <button 
                      class="bg-tertiary hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60"
                      :class="{ 'bg-accent': activeView === 'register'}"
                      @click="activeView = 'register'"
                    >
                      Zarejestruj się
                    </button>
                </div>

                <div v-show="errorLogin">
                  <p class=" text-red-500 mt-2 mb-2">Nieprawidłowy e-mail lub hasło. Spróbuj ponownie ❗</p>
                </div>

                <div class="w-100 h-0.5 mb-5 bg-accent"></div>
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
                          :type="showLoginPassword ? 'text' : 'password'"
                          id="password" 
                          v-model="loginData.password" 
                          placeholder="Hasło..."
                          class="w-full px-3 py-3 bg-tertiary border border-gray-700 rounded-md text-white focus:outline-none focus:border-accent"
                          required
                        />

                        <button @click="showLoginPassword = !showLoginPassword" 
                        class="absolute right-3 -translate-y-1/2 top-1/2"
                        type="button">
                          <font-awesome-icon :icon="showLoginPassword ? faEyeSlash : faEye" class="h-4 text-white  hover:text-accent transition-all duration-300" />
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
  
           
            <!--Formularz rejestracji-->
            <div v-if="activeView === 'register'" class=" animate-fade animate-duration-1000">
                <h2 class="text-2xl font-bold mb-8 text-center">Zarejestruj się</h2>
  
                
                <div class="flex mb-5 gap-2">
                    <button 
                      class=" bg-tertiary hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60"
                      @click="activeView = 'login'"
                    >
                      Zaloguj
                    </button>
  
                    <button 
                      class="bg-accent hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60"
                      @click="activeView = 'register'"
                    >
                      Zarejestruj się
                    </button>
                </div>
            
                <!--Komunikat jeżeli hasła się nie zgadzają-->
                <div class="text-red-500 mt-2 mb-2" v-show="errorPasswordsNotMatch">
                  <p>Podane hasła się nie zgadzają ❗</p>
                </div>

                <div class="w-100 h-0.5 mb-5 bg-accent"></div>
                <form @submit.prevent = "handleRegister">
                    
                    <div class="mb-4">
                        <input 
                          type="text" 
                          id="register-username" 
                          v-model="registerData.username" 
                          placeholder="Nazwa użytkownika..."
                          class="w-full px-3 py-3 bg-tertiary border border-gray-700 rounded-md text-white focus:outline-none focus:border-accent"
                          required
                        />
                    </div>
                  
                    
                    <div class="mb-4">
                        <input 
                          type="email" 
                          id="register-email" 
                          v-model="registerData.email" 
                          placeholder="Email..."
                          class="w-full px-3 py-3 bg-tertiary border border-gray-700 rounded-md text-white focus:outline-none focus:border-accent"
                          required
                        />
                    </div>
                  
                    
                    <div class="mb-4 relative">
                        <input 
                          :type="showRegisterPassword ? 'text' : 'password' " 
                          id="register-password" 
                          v-model="registerData.password" 
                          placeholder="Hasło..."
                          class="w-full px-3 py-3 bg-tertiary border border-gray-700 rounded-md text-white focus:outline-none focus:border-accent"
                          required
                        />

                        <button @click="showRegisterPassword = !showRegisterPassword" 
                        class="absolute right-3 -translate-y-1/2 top-1/2"
                        type="button">
                          <font-awesome-icon :icon="showRegisterPassword ? faEyeSlash : faEye" class="h-4 text-white  hover:text-accent transition-all duration-300" />
                        </button>
                    </div>


                    <div class="mb-4">
                      <passwordStrength
                      :password="registerData.password"
                      @validation-change="handlePasswordValidation"
                      />
                  </div>
                    
                  
                    
                    <div class="mb-5 relative">
                        <input 
                          :type="showRegisterConfirmPassword ? 'text' : 'password' " 
                          id="register-confirm-password" 
                          v-model="registerData.confirmPassword" 
                          placeholder="Potwierdź hasło..."
                          class="w-full px-3 py-3 bg-tertiary border border-gray-700 rounded-md text-white focus:outline-none focus:border-accent"
                          required
                        />

                        <button @click="showRegisterConfirmPassword = !showRegisterConfirmPassword" 
                        class="absolute right-3 -translate-y-1/2 top-1/2"
                        type="button">
                          <font-awesome-icon :icon="showRegisterConfirmPassword ? faEyeSlash : faEye" class="h-4 text-white  hover:text-accent transition-all duration-300" />
                        </button>
                    </div>

                    <div v-show="!passwordValid" class="mb-5">
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
                        Zarejestruj
                    </button>
                </form>
            </div>
        </div>
    </div>
  </template>
  
  <script setup>
  import { ref, defineProps, defineEmits, watch, computed } from 'vue';
  import axios from 'axios';
  import { useToast } from 'vue-toastification';
  import router from '@/router';
  import passwordStrength from './passwordStrength.vue';
  import {faXmark,faEye,faEyeSlash} from '@fortawesome/free-solid-svg-icons'



  const toast = useToast();
  
  const props = defineProps({
  //Prop odpowiedzialny za to czy komponent jest wyświetlany
  isVisible: {
    type: Boolean,
    default: false
  },

  //Przy pomocy tego można zmieniać, który formularz jest wyświetlany jako pierwszy, domyślnie jest to logowanie
  initialView: {
    type: String,
    default: 'login',
    validator: (value) => ['login', 'register'].includes(value)
  }
  });
  
  //Aktualnie działa tylko close
  const emit = defineEmits(['close', 'login', 'register']);


  //Zmienna wskazująca na to, który formularz jest wyświetlany
  const activeView = ref(props.initialView);


  const errorLogin = ref(false);
  const errorPasswordsNotMatch = ref(false);
  const passwordValid = ref(false);


  //Zmienne obsługujące widoczność haseł
  const showLoginPassword = ref(false);
  const showRegisterPassword = ref(false);
  const showRegisterConfirmPassword = ref(false);

  const handlePasswordValidation = (isValid) => {
    passwordValid.value = isValid;
  };
  
  //Dane Logowania
  const loginData = ref({
  email: '',
  password: ''
  });

  //Dane Rejestracji
  const registerData = ref({
  username: '',
  email: '',
  password: '',
  confirmPassword: ''
  });
  
  
  watch(() => props.isVisible, (newValue) => {
  if (newValue) {
    activeView.value = props.initialView;
  }
  });
  
  
  watch(() => props.initialView, (newValue) => {
  activeView.value = newValue;
  });
  

  //Funkcja do zamykania komponentu
  const closeModal = () => {
    emit('close');
  
  //Dane są z powrotem ustawiane na puste
  loginData.value = {
    email: '',
    password: ''
  };
  
  registerData.value = {
    username: '',
    email: '',
    password: '',
    confirmPassword: ''
  };

  showLoginPassword.value = false;
  showRegisterPassword.value = false;
  showRegisterConfirmPassword.value = false;
  errorLogin.value = false;
  errorPasswordsNotMatch.value = false;
  };

  //Funkcja sprawdzająca czy hasło spełnia wymagania
  const passwordRequirements = computed(() => {
  return {
    length: registerData.value.password.length >= 8,
    uppercase: /[A-Z]/.test(registerData.value.password),
    lowercase: /[a-z]/.test(registerData.value.password),
    digit: /[0-9]/.test(registerData.value.password),
    special: /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(registerData.value.password)
    };
  });

  
  // Funkcja do obsługi logowania
  const handleLogin = async () => {
  try {
    const response = await axios.post('http://localhost:5023/api/auth/login', {
      username: loginData.value.email, 
      password: loginData.value.password
    });

    if (response.data.success) {
      console.log('✅ Zalogowano pomyślnie');
      emit('login', response.data);
      router.push('/admin')
    }
  } catch (error) {
    errorLogin.value = true;
    toast.error('Nieprawidłowy e-mail lub hasło. Spróbuj ponownie !',{
      position: 'top-center',
    });
    console.error('❌ Wystąpił błąd:', error.response?.data || error.message);
  }
};

  //Funkcja do obsługi rejesracji
  const handleRegister = async () => {

    if(registerData.value.password !== registerData.value.confirmPassword) {
      toast.error("Hasła się nie zgadzają !");
      errorPasswordsNotMatch.value = true;
      return;
    }


    if(!passwordValid.value) {   
      toast.error("Podane hasło nie spełnia wymagań !", {
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
        console.log('!✅ Zarejestrowano pomyślnie!');
        toast.success("Pomyślnie zarejestrowano !",{
          position: 'top-center',
        })
        emit('register', response.data);
        closeModal();
      }
    } catch(error) {
      console.error('❌ Wystąpił błąd:', error.response?.data || error.message);
      toast.error("Wystąpił błąd !, spróbuj ponownie",{
        position: "top-center",
      });
    }
  };
</script>


