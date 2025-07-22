<template>
    <aside 
    class="h-container bg-secondary text-white flex flex-col  rounded-r-md border-t-2 border-r-2 border-b-2 border-solid border-lgray-accent"
            :class="isSideBarOpen ? 'w-64' : 'w-16' ">
       
        <div class="py-6  flex flex-row justify-between items-center px-4">
            <div></div>
            <h1 v-if="isSideBarOpen" class="text-xl font-bold text-white font-nasalization">DIGITAL WARS</h1>
            <div>
                <button @click="handleSidebar">
                    <font-awesome-icon :icon="isSideBarOpen ? faArrowLeft : faArrowRight" class="h-4" />
                </button>
            </div>
        </div>
        
        <nav class="flex-1 px-2 py-2">
            <ul class="space-y-2 ">

                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer">
                    <RouterLink 
                        to="/admin"
                        class="flex items-center gap-4 px-4 py-3 rounded-md"
                        :class="isSideBarOpen ? '': 'justify-center' "
                    >
                        <font-awesome-icon :icon="faGamepad" class="h-4 text-accent"/>
                        <span v-if="isSideBarOpen">Aktywne gry</span>
                    </RouterLink>
                </li>
                
                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer"
                @click="handleStats">
                <div class="flex  items-center px-4 py-3 rounded-md" :class="isSideBarOpen ? 'justify-between' : 'justify-center' ">
                    <div>
                    <font-awesome-icon :icon="faChartLine" class="h-4 text-accent"  :class="isSideBarOpen ? 'mr-4' : 'mr-0'"/>
                        <span v-if="isSideBarOpen">Statystyki gier</span>
                    </div>

                    <div v-if="isSideBarOpen">
                        <font-awesome-icon :icon="isStatsDropdownOpen ? faArrowUp : faArrowDown" class="h-4" />
                    </div>
                </div>

                <div v-show="isStatsDropdownOpen" class="flex flex-col py-1 space-y-1">          
                     
            <RouterLink 
              :to="{ path: '/admin/statistics', query: { stat: 'results' } }"
              class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
              @click.stop>
              Skuteczność decyzji
            </RouterLink>
            <RouterLink 
              :to="{ path: '/admin/statistics', query: { stat: 'bits' } }"
              class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
              @click.stop>
              Średnie zużycie bitów na rundę
            </RouterLink>   
            <RouterLink 
              :to="{ path: '/admin/statistics', query: { stat: 'deviation' } }"
              class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
              @click.stop>
              Odchylenie standardowe
            </RouterLink>
          </div>
          </li>
                
                
                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer">
                    <RouterLink
                        class="flex items-center gap-4 px-4 py-3 rounded-md"
                        :class="isSideBarOpen ? '': 'justify-center' "
                        to="/admin/editCards"
                    >
                        <font-awesome-icon :icon="faPenToSquare" class="h-4 text-accent"/>
                        <span v-if="isSideBarOpen">Edycja kart decyzji</span>
                    </RouterLink>
                </li>

                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer">
                    <RouterLink
                        class="flex items-center gap-4 px-4 py-3 rounded-md"
                        :class="isSideBarOpen ? '': 'justify-center' "
                        to="/admin/editItems"
                    >
                        <font-awesome-icon :icon="faMicrochip" class="h-4 text-accent"/>
                        <span v-if="isSideBarOpen">Edycja przedmiotów</span>
                    </RouterLink>
                </li>

                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer">
                    <RouterLink 
                        class="flex items-center gap-4 px-4 py-3 rounded-md"
                        :class="isSideBarOpen ? '': 'justify-center' "
                        to="/admin/editBoard"
                    >
                        <font-awesome-icon :icon="faChessBoard" class="h-4 text-accent"/>
                        <span v-if="isSideBarOpen">Edycja planszy</span>
                    </RouterLink>
                </li>


                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer">
                    <RouterLink 
                        class="flex items-center gap-4 px-4 py-3 rounded-md"
                        :class="isSideBarOpen ? '': 'justify-center' "
                        to="/admin/editProcesses"
                    >
                        <font-awesome-icon :icon="faChessPawn" class="h-4 text-accent"/>
                        <span v-if="isSideBarOpen">Edycja procesów</span>
                    </RouterLink>
                </li>


                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer">
                    <RouterLink 
                        to="/admin/cheatSheet"
                        class="flex items-center gap-4 px-4 py-3 rounded-md"
                        :class="isSideBarOpen ? '': 'justify-center' "
                    >
                        <font-awesome-icon :icon="faFile" class="h-4 text-accent"/>
                         <span v-if="isSideBarOpen">Ściąga mistrza gry</span>
                    </RouterLink>
                </li>
            </ul>
        </nav>
    </aside>
</template>

<script setup>
    import {faArrowDown,faArrowUp,faGamepad,faChartLine,faPenToSquare,faFile,faChessPawn,faArrowLeft,faArrowRight,faChessBoard,faMicrochip} from '@fortawesome/free-solid-svg-icons'
    import { ref } from 'vue';
    import { RouterLink } from 'vue-router';

    const isSideBarOpen = ref(true);    
    const isStatsDropdownOpen = ref(false)

    const handleSidebar = () => {
        isSideBarOpen.value = !isSideBarOpen.value;
        isStatsDropdownOpen.value = false;
    }

    const handleStats = () => {
        isStatsDropdownOpen.value = !isStatsDropdownOpen.value

        if(!isSideBarOpen.value) {
            isSideBarOpen.value = true;
        }
    }

    
</script>