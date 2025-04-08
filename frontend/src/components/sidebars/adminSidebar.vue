<template>
    <aside 
    class="h-container bg-secondary text-white flex flex-col  rounded-r-md border-t-2 border-r-2 border-solid border-lgray-accent"
            :class="isSideBarOpen ? 'w-64' : 'w-16' ">
       
        <div class="py-6  flex flex-row justify-between items-center px-4">
            <div></div>
            <h1 v-if="isSideBarOpen" class="text-xl font-bold text-white font-nasalization">DIGITAL WARS</h1>
            <div>
                <button @click="isSideBarOpen = !isSideBarOpen">
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
                        <font-awesome-icon :icon="faHouse" class="h-4 text-accent"/>
                        <span v-if="isSideBarOpen">Home</span>
                    </RouterLink>
                </li>
                
                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer"
                    @click="isDropdownOpen = !isDropdownOpen"
                    v-if="isSideBarOpen">
                    <div class="flex justify-between items-center px-4 py-3 rounded-md"  >
                    <div>
                        <font-awesome-icon :icon="faGamepad" class="h-4 mr-2 text-accent"/>
                        <span>Gry w Sesji</span>
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

                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer"
            @click="isStatsDropdownOpen = !isStatsDropdownOpen">
          <div class="flex justify-between items-center px-4 py-3 rounded-md">
            <div>
              <font-awesome-icon :icon="faChartLine" class="h-4 mr-2 text-accent" />
              Statystyki Gier
            </div>
            <div v-if="isStatsDropdownOpen">
              <font-awesome-icon :icon="faArrowUp" class="h-4" />
            </div>
            <div v-else>
              <font-awesome-icon :icon="faArrowDown" class="h-4" />
            </div>
          </div>

                <div v-show="isStatsDropdownOpen" class="flex flex-col py-1 space-y-1">
            <RouterLink 
              :to="{ path: '/admin/statistics', query: { stat: 'positions' } }"
              class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
              @click.stop>
              Wynik końcowy
            </RouterLink>
          
            <RouterLink 
              :to="{ path: '/admin/statistics', query: { stat: 'bits' } }"
              class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
              @click.stop>
              Zużycie Bitów
            </RouterLink>            
            <RouterLink 
              :to="{ path: '/admin/statistics', query: { stat: 'results' } }"
              class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
              @click.stop>
              Sukcesy vs Porażki
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
                        to="/admin/editBoard"
                    >
                        <font-awesome-icon :icon="faPenToSquare" class="h-4 text-accent"/>
                        <span v-if="isSideBarOpen">Edycja planszy</span>
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

                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer">
                    <RouterLink 
                        class="flex items-center gap-4 px-4 py-3 rounded-md"
                        :class="isSideBarOpen ? '': 'justify-center' "
                        to="/"
                    >
                        <font-awesome-icon :icon="faFileSignature" class="h-4 text-accent"/>
                        <span v-if="isSideBarOpen">Licencje</span>
                    </RouterLink>
                </li>
            </ul>
        </nav>
    </aside>
</template>

<script setup>
    import {faArrowDown,faArrowUp,faGamepad,faChartLine,faPenToSquare,faFile,faFileSignature,faHouse,faArrowLeft,faArrowRight} from '@fortawesome/free-solid-svg-icons'
    import { ref } from 'vue';
    import { RouterLink } from 'vue-router';

    const isDropdownOpen = ref(false)
    const isSideBarOpen = ref(true);    
    const isStatsDropdownOpen = ref(false)
</script>