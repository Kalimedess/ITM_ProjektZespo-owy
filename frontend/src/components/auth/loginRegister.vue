<template>
  <div v-if="props.isVisible" class="fixed inset-0 flex items-center justify-center z-50">
    <div class="absolute inset-0 bg-black/70" @click="closeModal"></div>
    
    <div class="bg-primary text-white rounded-lg w-96 relative z-10 border-2 border-accent p-12 animate-jump-in">
      <button 
        @click="closeModal" 
        class="absolute top-2 right-2 w-8 h-8 flex items-center justify-center"
      >
        <font-awesome-icon :icon="faXmark" class="h-5 text-white hover:text-accent transition-all duration-100" />
      </button>

      <!-- Przyciski przełączania formularzy -->
      <div class="flex mb-5 gap-2">
        <button 
          class="bg-tertiary hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60"
          :class="{ 'bg-accent': activeView === 'login' }"
          @click="activeView = 'login'"
        >
          Zaloguj
        </button>

        <button 
          class="bg-tertiary hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60"
          :class="{ 'bg-accent': activeView === 'register' }"
          @click="activeView = 'register'"
        >
          Zarejestruj się
        </button>
      </div>

      <div class="w-100 h-0.5 mb-5 bg-accent"></div>

      <!-- Formularze logowania i rejestracji -->
      <LoginForm 
        v-if="activeView === 'login'" 
        @close="closeModal"
        class="animate-fade animate-duration-1000"
      />
      
      <RegisterForm 
        v-if="activeView === 'register'" 
        @close="closeModal"
        @switchToLogin="activeView = 'login'"
        class="animate-fade animate-duration-1000"
      />
    </div>
  </div>
</template>

<script setup>
import { ref, defineProps, defineEmits, watch } from 'vue';
import { faXmark } from '@fortawesome/free-solid-svg-icons';
import LoginForm from '@/components/auth/loginForm.vue';
import RegisterForm from '@/components/auth/registerForm.vue';

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
</script>