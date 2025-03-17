<template>
    <aside class="w-64 h-container bg-secondary text-white flex flex-col  rounded-r-md border-t-2 border-r-2 border-solid border-lgray-accent">
       
        <div class="py-6 text-center">
            <h1 class="text-xl font-bold text-white font-nasalization">DIGITAL WARS</h1>
        </div>
        
        <nav class="flex-1 px-2 py-2">
            <ul class="space-y-2 ">
                
                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer"
                    @click="isDropdownOpen = !isDropdownOpen">
                    <div class="flex justify-between items-center px-4 py-3 rounded-md"  >
                    <div>
                        <font-awesome-icon :icon="faGamepad" class="h-4 mr-2 text-accent"/>
                        Gry w Sesji
                    </div>
                    <div v-if="isDropdownOpen">
                        <font-awesome-icon :icon="faArrowUp" class="h-4 "/> 
                    </div>
                    <div v-else>
                        <font-awesome-icon :icon="faArrowDown" class="h-4 "/>
                    </div> 
                    </div>


                    <div v-show="isDropdownOpen" class="flex flex-col py-1 space-y-1">
                        <router-link 
                        to="/" 
                        class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
                        @click.stop
                        >
                        Gra 1
                        </router-link>
                        <router-link 
                        to="/" 
                        class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
                        @click.stop
                        >
                        Gra 2
                        </router-link>
                    </div>
                </li>

                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer">
                    <RouterLink 
                        to="/admin"
                        class="flex items-center gap-4 px-4 py-3 rounded-md"
                    >
                        <font-awesome-icon :icon="faHouse" class="h-4 text-accent"/>
                        <p>Home</p>
                    </RouterLink>
                </li>
                
               
                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer">
                    <RouterLink 
                        to="/admin/statistics"
                        class="flex items-center gap-4 px-4 py-3 rounded-md"
                    >
                        <font-awesome-icon :icon="faChartLine" class="h-4 text-accent"/>
                        <p>Statystyki Gier</p>
                    </RouterLink>
                </li>
                
                
                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer">
                    <RouterLink
                        class="flex items-center gap-4 px-4 py-3 rounded-md"
                    >
                        <font-awesome-icon :icon="faPenToSquare" class="h-4 text-accent"/>
                        <p>Edycja kart decyzji</p>
                    </RouterLink>
                </li>

                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer">
                    <RouterLink 
                        class="flex items-center gap-4 px-4 py-3 rounded-md"
                    >
                        <font-awesome-icon :icon="faPenToSquare" class="h-4 text-accent"/>
                        <p>Edycja planszy</p>
                    </RouterLink>
                </li>


                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer">
                    <RouterLink 
                        class="flex items-center gap-4 px-4 py-3 rounded-md"
                    >
                        <font-awesome-icon :icon="faFile" class="h-4 text-accent"/>
                         <p>Ściąga mistrza gry</p>
                    </RouterLink>
                </li>

                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer">
                    <RouterLink 
                        class="flex items-center gap-4 px-4 py-3 rounded-md"
                    >
                        <font-awesome-icon :icon="faFileSignature" class="h-4 text-accent"/>
                        <p>Licencje</p>
                    </RouterLink>
                </li>
            </ul>
        </nav>
    </aside>
</template>

<script setup>
    import {faArrowDown,faArrowUp,faGamepad,faChartLine,faPenToSquare,faFile,faFileSignature,faHouse} from '@fortawesome/free-solid-svg-icons'
    import { ref } from 'vue';
    import { RouterLink } from 'vue-router';

    const isDropdownOpen = ref(false)
</script>