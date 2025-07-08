<template>
    <aside 
    class="h-container bg-secondary text-white flex flex-col  rounded-r-md border-t-2 border-r-2 border-solid border-lgray-accent"
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
                        <font-awesome-icon :icon="faHouse" class="h-4 text-accent"/>
                        <span v-if="isSideBarOpen">Home</span>
                    </RouterLink>
                </li>
                
                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer"
                        @click="handleGameStats">

                    <div class="flex items-center px-4 py-3 rounded-md" :class="isSideBarOpen ? 'justify-between' : 'justify-center' ">
                        <div>
                            <font-awesome-icon :icon="faChartLine" class="h-4 text-accent" :class="isSideBarOpen ? 'mr-4 ' : 'mr-0' "/>
                            <span v-if="isSideBarOpen">Statystyki stołów</span>
                        </div>
                            <font-awesome-icon v-if="isSideBarOpen" :icon="isGameStatsDropdownOpen  ? faArrowUp : faArrowDown" class="h-4"/>   
                    </div>

                    <div v-show="isGameStatsDropdownOpen" class="flex flex-col py-1 space-y-1">
                        <RouterLink 
                            :to="{ path: '/admin/game', query: { stat: 'positions' } }"
                            class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
                            @click.stop>
                            Pozycje końcowe
                        </RouterLink>

                        <RouterLink 
                            :to="{ path: '/admin/game', query: { stat: 'success' } }"
                            class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
                            @click.stop>
                            Skuteczność decyzji
                        </RouterLink>

                        <RouterLink 
                            :to="{ path: '/admin/game', query: { stat: 'bits' } }"
                            class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
                            @click.stop>
                            Średnie zużycie bitów na rundę
                        </RouterLink>

                       

                        <RouterLink 
                            :to="{ path: '/admin/game', query: { stat: 'progress' } }"
                            class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
                            @click.stop>
                            Ranking drużyn na przestrzeni rund
                        </RouterLink>
                    </div>
                </li>

                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer"
            @click="handleGameManager">
          <div class="flex items-center px-4 py-3 rounded-md" :class="isSideBarOpen ? 'justify-between' : 'justify-center' ">
            <div>
              <font-awesome-icon :icon="faGamepad" class="h-4  text-accent" :class="isSideBarOpen ? 'mr-4' : 'mr-0' " />
                <span v-if="isSideBarOpen">Panel sterowania grą</span>
            </div>
                <font-awesome-icon v-if="isSideBarOpen" :icon="isGameManagerDropdownOpen ? faArrowUp : faArrowDown" class="h-4" />
          </div>

                <div v-show="isGameManagerDropdownOpen" class="flex flex-col py-1 space-y-1">
            <RouterLink 
              :to="{ path: '/admin/game/editbits'}"
              class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
              @click.stop>
              Zmień ilość bitów drużyny
            </RouterLink>
          
            <RouterLink 
              :to="{ path: '/admin/game', query: { stat: 'alwaysSucseed' } }"
              class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
              @click.stop>
              Zagrana karta zawsze się powodzi
            </RouterLink>            
            <RouterLink 
              :to="{ path: '/admin/game', query: { stat: 'changeGameName' } }"
              class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
              @click.stop>
              Zmień nazwę gry
            </RouterLink>
            <RouterLink 
              :to="{ path: '/admin/game', query: { stat: 'ban/unban_card' } }"
              class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
              @click.stop>
              Zablokuj/Odblokuj kartę
            </RouterLink>
            </div>
                </li>
                
                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer"
                    @click="handleGameTableManager">
                    <div class="flex items-center px-4 py-3 rounded-md" :class="isSideBarOpen ? 'justify-between' : 'justify-center' ">
                    <div>
                        <font-awesome-icon :icon="faUsers" class="h-4 text-accent" :class="isSideBarOpen ? 'mr-4' : 'mr-0' "/>
                        <span v-if="isSideBarOpen">Panel sterowania stołami</span>
                    </div>
                        <font-awesome-icon v-if="isSideBarOpen" :icon="isGameTableManagerDropdownOpen ? faArrowUp : faArrowDown" class="h-4" />
                    </div>


                    <div v-show="isGameTableManagerDropdownOpen" class="flex flex-col py-1 space-y-1">
                        <router-link 
                        to="/" 
                        class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
                        @click.stop
                        >
                        Stół 1
                        </router-link>
                        <router-link 
                        to="/" 
                        class="px-4 py-2 hover:bg-[#1c2942] rounded-md transition-all duration-200"
                        @click.stop
                        >
                        Stół 2
                        </router-link>
                    </div>
                </li>
          

                <li class="border-2 border-lgray-accent rounded-md hover:border-accent transition-colors duration-300 cursor-pointer">
                    <RouterLink 
                        class="flex items-center gap-4 px-4 py-3 rounded-md"
                        :class="isSideBarOpen ? '': 'justify-center' "
                        to="/admin/game"
                    >
                        <font-awesome-icon :icon="faPenToSquare" class="h-4 text-accent"/>
                        <span v-if="isSideBarOpen">Chat</span>
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
                        <span v-if="isSideBarOpen">Wygeneruj grę</span>
                    </RouterLink>
                </li>
            </ul>
        </nav>
    </aside>
</template>

<script setup>
    import { icon } from '@fortawesome/fontawesome-svg-core';
import {faArrowDown,faArrowUp,faGamepad,faChartLine,faPenToSquare,faFile,faFileSignature,faHouse,faArrowLeft,faArrowRight, faUsers} from '@fortawesome/free-solid-svg-icons'
    import { ref } from 'vue';
    import { RouterLink } from 'vue-router';

    const isSideBarOpen = ref(true);    
    const isGameManagerDropdownOpen = ref(false)
    const isGameStatsDropdownOpen = ref(false)
    const isGameTableManagerDropdownOpen = ref(false);

    const handleSidebar = () => {

        isSideBarOpen.value = !isSideBarOpen.value;
        isGameStatsDropdownOpen.value = false;
        isGameManagerDropdownOpen.value = false;
        isGameTableManagerDropdownOpen.value = false;
    }

    const handleGameStats = () => {

        isGameStatsDropdownOpen.value = !isGameStatsDropdownOpen.value;

        if(!isSideBarOpen.value) {
            isSideBarOpen.value = true;
        }
    }

    const handleGameManager = () => {

        isGameManagerDropdownOpen.value = !isGameManagerDropdownOpen.value;

        if(!isSideBarOpen.value) {
            isSideBarOpen.value = true;
        }
    }

    const handleGameTableManager = () => {

        isGameTableManagerDropdownOpen.value = !isGameTableManagerDropdownOpen.value;

        if(!isSideBarOpen.value) {
            isSideBarOpen.value = true;
        }
    }
</script>