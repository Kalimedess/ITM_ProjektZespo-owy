<template>
    <!--Informacja o potwierdzeniu adresu email-->
  <div class="animate-fade-right">
        <h2 class="text-lg sm:text-xl md:text-2xl font-nasalization mb-2 sm:mb-3 text-center">
            Rejestracja zakończona pomyślnie
        </h2>
        
        <font-awesome-icon :icon="faEnvelopeOpen" class="mb-3 h-20  text-accent"/>

        <div class="w-100 h-0.5 mb-1 sm:mb-2 md:mb-3 lg:mb-4 bg-accent"></div>

        <div  class="text-sm text-gray-300 mb-3">
            <p>Pozostał już tylko jeden krok</p>
            <p>Na twój adres email: <b>{{ email }}</b> </p>
            <p>wysłaliśmy link w celu zweryfikowania adresu e-mail</p>
            <p>Jeśli nie widzisz wiadomości, sprawdź folder spam.</p>
        </div>
        <span class="text-accent hover:text-purple-300 transition-colors cursor-pointer"
        @click="emit('backToLogin')">
            Przejdź do logowania
        </span>
    </div>
</template>

<script setup>

    import { faEnvelopeOpen } from '@fortawesome/free-regular-svg-icons';
    import { defineProps,defineEmits } from 'vue';

    const emit = defineEmits(['backToLogin'])


    const props = defineProps({
        email: {
            type: String,
            required: true
        }
    });

</script>
