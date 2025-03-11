<template>
    <div v-if="props.isVisible" class="fixed inset-0 flex items-center justify-center z-50">
        <div class="absolute inset-0 bg-black opacity-70 transition delay-500 duration-1000" @click="closeModal"></div>
  
        <div class="bg-primary text-white rounded-lg w-96 relative z-10 border-2 border-accent p-12">
  
            <div v-if="activeView === 'login'">
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
                <div class="w-100 h-0.5 mb-5 bg-accent"></div>
                <form>
                    
                    <div class="mb-4">
                        <input 
                          type="text" 
                          id="login" 
                          v-model="loginData.username" 
                          placeholder="Login..."
                          class="w-full px-3 py-3 bg-tertiary border border-gray-700 rounded-md text-white focus:outline-none focus:border-accent"
                          required
                        />
                    </div>
                  
                    
                    <div class="mb-2">
                        <input 
                          type="password" 
                          id="password" 
                          v-model="loginData.password" 
                          placeholder="Hasło..."
                          class="w-full px-3 py-3 bg-tertiary border border-gray-700 rounded-md text-white focus:outline-none focus:border-accent"
                          required
                        />
                    </div>
                  
                    
                    <div class="mb-5 text-sm">
                        <a href="#" class="text-accent hover:text-purple-300">Zapomniałem hasła!</a>
                    </div>
                  
                    
                    <button 
                        type="submit" 
                        class="bg-tertiary hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60 mb-5"
                    >
                        Zaloguj
                    </button>
                </form>
            </div>
  
           
            <div v-if="activeView === 'register'">
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
                <div class="w-100 h-0.5 mb-5 bg-accent"></div>
                <form>
                    
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
                  
                    
                    <div class="mb-4">
                        <input 
                          type="password" 
                          id="register-password" 
                          v-model="registerData.password" 
                          placeholder="Hasło..."
                          class="w-full px-3 py-3 bg-tertiary border border-gray-700 rounded-md text-white focus:outline-none focus:border-accent"
                          required
                        />
                    </div>
                  
                    
                    <div class="mb-5">
                        <input 
                          type="password" 
                          id="register-confirm-password" 
                          v-model="registerData.confirmPassword" 
                          placeholder="Potwierdź hasło..."
                          class="w-full px-3 py-3 bg-tertiary border border-gray-700 rounded-md text-white focus:outline-none focus:border-accent"
                          required
                        />
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
  import { ref, defineProps, defineEmits, watch } from 'vue';
  
  const props = defineProps({
  isVisible: {
    type: Boolean,
    default: false
  },
  initialView: {
    type: String,
    default: 'login',
    validator: (value) => ['login', 'register'].includes(value)
  }
  });
  
  const emit = defineEmits(['close', 'login', 'register']);
  
  const activeView = ref(props.initialView);
  
  const loginData = ref({
  username: '',
  password: ''
  });
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
  
  const closeModal = () => {
  emit('close');
  
  
  loginData.value = {
    username: '',
    password: ''
  };
  
  registerData.value = {
    username: '',
    email: '',
    password: '',
    confirmPassword: ''
  };
  };
  </script>