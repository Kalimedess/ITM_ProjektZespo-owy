<template>
   <div class="w-full border-2 rounded-md bg-tertiary p-4 flex flex-col gap-4 text-white hover:transform hover:-translate-y-2 transition-all duration-300"
        :style="{borderColor: props.color}">
    <div class="flex flex-row items-center justify-between">
        <div class="flex items-center">
            <font-awesome-icon :icon="faGamepad" class="h-5  mr-2"
                :style="{ color: props.color }"/> 
            <span class="text-lg font-semibold"
                :style="{ color: props.color }" 
            >
            {{ game.name}}
            </span>
        </div>
        <span class="text-sm px-2 py-1 rounded-full"
            :style="{backgroundColor: props.color}">
            ID: {{ game.id }}
        </span>
    </div>
    
    <div class="flex flex-row gap-2">
        <button 
            class="flex-1 flex items-center justify-center border-2 border-lgray-accent py-2 px-3 rounded-md  hover:border-accent hover:bg-gray-800 transition-colors duration-300">
            <font-awesome-icon :icon="faCircleStop" class="h-4 text-accent mr-2"/> 
            Wstrzymaj grę
        </button>
        <button 
            class="flex-1 flex items-center justify-center border-2 border-lgray-accent py-2 px-3 rounded-md  hover:border-accent hover:bg-gray-800 transition-colors duration-300">
            <font-awesome-icon :icon="faForward" class="h-4 text-accent mr-2"/> 
            Następna runda
        </button>
    </div>
    
    <div class=" bg-lgray-accent p-3 rounded-md">
        <div class="mb-1 font-bold">
            <font-awesome-icon :icon="faHourglass" class="h-4 text-accent mr-2"/>
            <span>Runda:</span>
            <span>{{ game.round }}</span>
        </div>
        <div class="mb-1 font-bold">
            <font-awesome-icon :icon="faCoins" class="h-4 text-accent mr-2"/>
            <span>Liczba bitów: </span>
            <span>{{ game.bits }}</span>
        </div>
    </div>
    
    <button 
    class="flex items-center justify-center border-2 border-lgray-accent py-2 px-3 rounded-md  hover:border-accent hover:bg-gray-800 transition-colors duration-300">
    <font-awesome-icon :icon="faMagnifyingGlass" class="h-4 mr-2 text-accent"/>     
    <span>Otwórz grę</span>
    </button>
</div>


</template>
<script setup>
    import { defineProps } from 'vue';
    import { faForward, faGamepad,faCircleStop,faMagnifyingGlass,faCoins,faHourglass } from '@fortawesome/free-solid-svg-icons'

    const props = defineProps({
        game: {
            type: Object,
            required: true
        },
        color:{
            type:String,
            required:true
        }
    })

</script>