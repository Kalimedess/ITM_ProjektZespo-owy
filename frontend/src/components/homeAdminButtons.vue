<template>
    <div class="flex flex-row gap-3 items-center justify-evenly">
            <button 
                class="border-2 border-lgray-accent py-2 px-4 rounded-md text-center hover:border-accent hover:bg-gray-800 transition-colors duration-300 flex items-center whitespace-nowrap"
                @click="openCreateGame">
                <font-awesome-icon :icon="faPlus" class="h-4 text-accent mr-2"/> 
                Stwórz nową grę
            </button>
            <button 
                class="border-2 border-lgray-accent py-2 px-4 rounded-md text-center hover:border-accent hover:bg-gray-800 transition-colors duration-300 flex items-center whitespace-nowrap">
                <font-awesome-icon :icon="faCircleStop" class="h-4 text-accent mr-2"/> 
                Zatrzymaj wszyskite gry
            </button>
            <button 
                class="border-2 border-lgray-accent py-2 px-4 rounded-md text-center hover:border-accent hover:bg-gray-800 transition-colors duration-300 flex items-center whitespace-nowrap">
                <font-awesome-icon :icon="faForward" class="h-4 text-accent mr-2"/> 
                Przejdź do następnej rundy we wszystkich gier
            </button>
            <button 
                class="border-2 border-lgray-accent py-2 px-4 rounded-md text-center hover:border-accent hover:bg-gray-800 transition-colors duration-300 flex items-center whitespace-nowrap">
                <font-awesome-icon :icon="faPowerOff" class="h-4 text-accent mr-2"/> 
                Zakończ wszystkie gry
            </button>
            </div>
</template>

<script setup>
     import { faForward,faPowerOff,faPlus } from '@fortawesome/free-solid-svg-icons'
     import { faCircleStop } from '@fortawesome/free-regular-svg-icons'

     const emits = defineEmits(['openCreateGame']);

     const openCreateGame = () => {
        emits('openCreateGame');
     };
     
</script>