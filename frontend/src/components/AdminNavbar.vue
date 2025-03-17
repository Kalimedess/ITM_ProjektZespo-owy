<template>
    <nav class="w-full bg-secondary py-3 px-6 flex flex-row justify-between items-center border-b-2 border-lgray-accent">
        <div>
            <img :src="logo" class="h-12" alt="ITM logo">
        </div>
        <div class="flex justify-center items-center mr-5">
            <div @click="toggleDropdown" class="cursor-pointer">
                <font-awesome-icon :icon="faCircleUser" class="h-8 text-white hover:text-accent transition-all duration-300"/>
            </div>      
        </div>


        <div v-show="isDropdownOpen" 
            class="absolute top-16 right-0 w-36 bg-secondary rounded-md shadow-lg py-1 z-50 border-solid border-2 border-lgray-accent"
            ref="dropdownMenu">
            <button @click="logout" class="block w-full text-left px-4 py-2 text-sm text-white hover:text-red-600">
                Wyloguj
            </button>
        </div>
    </nav>
</template>


<script setup>
import { ref, onMounted, onUnmounted } from 'vue'
import logo from '@/assets/logos/ITM_poziom_biale.png'
import { faCircleUser } from '@fortawesome/free-solid-svg-icons'
import { useRouter } from 'vue-router'


const isDropdownOpen = ref(false)
const dropdownMenu = ref(null)


const router = useRouter()

const toggleDropdown = () => {
    isDropdownOpen.value = !isDropdownOpen.value
}


const handleClickOutside = (event) => {
    if (dropdownMenu.value && !dropdownMenu.value.contains(event.target) && !event.target.closest('.cursor-pointer')) {
        isDropdownOpen.value = false
    }
}


const logout = () => {
   
    router.push('/')
    
    isDropdownOpen.value = false
}


onMounted(() => {
    document.addEventListener('click', handleClickOutside)
})

onUnmounted(() => {
    document.removeEventListener('click', handleClickOutside)
})
</script>