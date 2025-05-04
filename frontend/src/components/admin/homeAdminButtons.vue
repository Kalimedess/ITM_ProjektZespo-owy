<template>
    <div class="flex flex-row justify-center gap-3 items-center mt-5 mb-5">
        <div class="flex gap-2">
            <button 
                class="border-2 border-lgray-accent py-2 px-4 rounded-md text-center hover:border-accent transition-colors duration-300 flex items-center"
                @click="openCreateGame">
                <font-awesome-icon :icon="faPlus" class="h-4 text-accent mr-2"/> 
                Stwórz nową grę
            </button>
        </div>
        <div class="flex gap-2">
            <button 
                class="border-2 border-lgray-accent py-2 px-4 rounded-md text-center hover:border-accent transition-colors duration-300 flex items-center">
                <font-awesome-icon :icon="faCircleStop" class="h-4 text-accent mr-2"/> 
                Zatrzymaj wszystkie gry
            </button>
            <button 
                class="border-2 border-lgray-accent py-2 px-4 rounded-md text-center hover:border-accent transition-colors duration-300 flex items-center">
                <font-awesome-icon :icon="faPowerOff" class="h-4 text-accent mr-2"/> 
                Zakończ wszystkie gry
            </button>
        </div>
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