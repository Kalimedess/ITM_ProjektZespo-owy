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
      
      <!--Widok potwierdzonego email użytkownika-->
      <div v-if="isTokenValid">
          <h2 class="text-lg sm:text-xl md:text-2xl font-nasalization mb-2 sm:mb-3 text-center">
            Konto aktywne
          </h2>

          <font-awesome-icon :icon="faUserCheck" class="h-20 text-accent mb-3 text-center block mx-auto" />

        <div class="w-100 h-0.5 mb-1 sm:mb-2 md:mb-3 lg:mb-4 bg-accent"></div>

        <div class="text-center text-md text-gray-300">
          <p class="mb-3">Twoje konto jest już aktywne i możesz z niego korzystać.</p>
          <p>Wróć do strony logowania i zaloguj się na swoje konto.</p>
          <p class="mt-3">Zostaniesz automatycznie przekierowany do strony logowania za <span class="text-accent font-bold">{{ time }}s</span>.</p>
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
            Zaloguj się
        </button>
      </div>

      <!--Widok wygasłego linku-->
       <div v-else>
          <h2 class="text-lg sm:text-xl md:text-2xl font-nasalization mb-2 sm:mb-3 text-center">
            Link wygasł
          </h2>

          <font-awesome-icon :icon="faCircleXmark" class="h-20 text-red-500 mb-3 text-center block mx-auto" />

        <div class="w-100 h-0.5 mb-1 sm:mb-2 md:mb-3 lg:mb-4 bg-accent"></div>

        <div class="text-center text-gray-300">
          <p class="mb-3">Wygląda na to, że twój link aktywacyjny wygasł lub jest nieprawidłowy.</p>
          <p class="mb-4">Aby otrzymać nowy link aktywacyjny zaloguj się na nowo.</p>
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
import { faUserCheck } from '@fortawesome/free-solid-svg-icons';
import { faCircleXmark } from '@fortawesome/free-regular-svg-icons';
import { onMounted, ref, onUnmounted } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();

const time = ref(20);

//Ta zmienna przechowuje czy token jest poprawny (nie istnieje albo wygasł) domyślnie false
const isTokenValid = ref(false);

const handleReturnToLogin = () => {
   sessionStorage.setItem('showLoginAfterRedirect', 'true');
   router.push('/');
};

onMounted(async () => {
  
  const route = useRoute();
  const token = route.query.token;    
  if (token) {
    try {
      const response = await axios.get(apiConfig.auth.confirmEmail(token));
      if (response.data.success) {
        isTokenValid.value = true;
      }
    } catch (error) {
      console.error('Błąd potwierdzania tokenu:', error);
      isTokenValid.value = false;
    }
  }
    //Odliczanie czasu do przekierowania z powrotem do logowania tylko jeżeli token jest poprawny
    if(isTokenValid.value) {
        const interval = setInterval(() => {
            if (time.value > 0) {
                time.value--;
            } else {
                clearInterval(interval);
                handleReturnToLogin();
            }
        }, 1000);
        
        onUnmounted(() => {
            clearInterval(interval);
        });
    }
});
</script>