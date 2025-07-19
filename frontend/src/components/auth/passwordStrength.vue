<template>
    <div>
      <!-- Pasek siły hasła -->
      <div class="w-full h-2 bg-gray-700 rounded overflow-hidden">
        <div 
          class="h-full transition-all duration-300"
          :class="passwordStrengthColor"
          :style="{ width: passwordStrength + '%' }"
        ></div>
      </div>
      
      <!-- Tekst opisujący siłę hasła -->
      <p class="text-sm mt-1" :class="passwordStrengthTextColor">
        {{ passwordStrengthText }}
      </p>
    </div>
  </template>
  
  <script setup>
  import { computed } from 'vue';
  
  const props = defineProps({
    password: {
      type: String,
      required: true
    }
  });
  
  const requirements = computed(() => {
    return {
      length: props.password.length >= 8,
      uppercase: /[A-Z]/.test(props.password),
      lowercase: /[a-z]/.test(props.password),
      digit: /[0-9]/.test(props.password),
      special: /[!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]/.test(props.password)
    };
  });
  
  const passwordStrength = computed(() => {
    let score = 0;
    
    // Puste hasło
    if (props.password.length === 0) {
      return 0;
    }
    
    // Długość hasła 
    if(props.password.length >= 8) {
        score += 20;
    }
    
    // Duże litery 
    if (requirements.value.uppercase) {
      score += 20;
    }
    
    // Małe litery 
    if (requirements.value.lowercase) {
      score += 20;
    }
    
    // Cyfry 
    if (requirements.value.digit) {
      score += 20;
    }
    
    // Znaki specjalne 
    if (requirements.value.special) {
      score += 20;
    }
    
    return score;
  });
  
  const passwordStrengthColor = computed(() => {
    if (passwordStrength.value < 40) {
      return 'bg-red-500'; // Słabe
    } else if (passwordStrength.value < 60) {
      return 'bg-yellow-500'; // Średnie
    } else if (passwordStrength.value < 80) {
      return 'bg-green-200'; // Dobre
    } else {
      return 'bg-green-700'; // Silne
    }
  });
  
  const passwordStrengthText = computed(() => {

    if (props.password.length === 0) {
      return '';
    } else if (passwordStrength.value < 40) {
      return 'Słabe hasło';
    } else if (passwordStrength.value < 60) {
      return 'Średnie hasło';
    } else if (passwordStrength.value < 80) {
      return 'Dobre hasło';
    } else {
      return 'Silne hasło';
    }
  });
  
  const passwordStrengthTextColor = computed(() => {
    if (passwordStrength.value < 30) {
      return 'text-red-500';
    } else if (passwordStrength.value < 60) {
      return 'text-yellow-500';
    } else if (passwordStrength.value < 80) {
      return 'text-green-200';
    } else {
      return 'text-green-500 ';
    }
  });
  
  </script>