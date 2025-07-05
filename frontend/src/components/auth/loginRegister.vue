<template>
    <div v-if="props.isVisible" class="fixed inset-0 flex items-center justify-center z-50">
      <div class="absolute inset-0 bg-black/70 transition-opacity duration-300" @click="closeModal"></div>
      
      <div 
        class="bg-primary text-white rounded-lg relative z-10 border-2
         border-accent transition-all duration-300
         p-6 sm:p-8 md:p-10 lg:p-12
         max-h-[90vh]
         w-full max-w-lg 
         "
        :class="props.isVisible ? 'scale-100 translate-y-0 opacity-100' : 'scale-95 translate-y-4 opacity-0'"
      >
        <button 
          @click="closeModal" 
          class="absolute top-2 right-2 w-8 h-8 flex items-center justify-center"
        >
          <font-awesome-icon :icon="faXmark" class="h-5 text-white hover:text-accent transition-all duration-100" />
        </button>
        <div class="flex mb-1 sm:mb-2 md:mb-3 lg:mb-4 gap-2" v-if="activeView !== 'forgotPassword' ">
          <button 
            class="text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60"
            :class="activeView === 'login' ? 'bg-accent' : 'bg-tertiary'"
            @click="activeView = 'login'"
          >
            Logowanie
          </button> 

          <button 
            class="text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60"
            :class="activeView === 'register' ? 'bg-accent' : 'bg-tertiary'"
            @click="activeView = 'register'"
          >
            Rejestracja
          </button>
        </div>

        <div class="w-100 h-0.5 mb-1 sm:mb-2 md:mb-3 lg:mb-4 bg-accent" v-if="activeView !== 'forgotPassword' "></div>

        <LoginForm 
          v-if="activeView === 'login'"
          @close="closeModal"
          @forgotPassword="handleForgotPassword"
          class="animate-fade-left"
        />
        
        <RegisterForm 
          v-if="activeView === 'register'" 
          @close="closeModal"
          @switchToLogin="activeView = 'login'"
          class="animate-fade-right"
        />

        <ForgotPassword 
          v-if="activeView === 'forgotPassword'"
          @back-to-login="handleBackToLogin"
        />

      </div>
    </div>
</template>

<script setup>
import { ref, defineProps, defineEmits, watch} from 'vue';
import { faXmark } from '@fortawesome/free-solid-svg-icons';
import LoginForm from '@/components/auth/loginForm.vue';
import RegisterForm from '@/components/auth/registerForm.vue';
import ForgotPassword from './forgotPassword.vue';

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

const emit = defineEmits(['register', 'close', 'switchToLogin']);

const activeView = ref(props.initialView);



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
};

const handleForgotPassword = () => {
  activeView.value = 'forgotPassword';
}

const handleBackToLogin = () => {
  activeView.value = 'login';
}

</script>