<template>
    <nav class="w-full bg-secondary py-3 px-6 flex flex-row justify-between items-center border-b-2 border-lgray-accent">
        <div>
            <RouterLink to="/">
                <img :src="logo" class="h-12" alt="ITM logo">
            </RouterLink>
        </div>
        <div class="flex justify-center items-center mr-5">
            <div @click="toggleDropdown" class="cursor-pointer">
                <font-awesome-icon :icon="faCircleUser" class="h-8 text-white hover:text-accent transition-all duration-300"/>
            </div>
            <div class="text-white ml-3 hidden sm:block">
              {{ username }}
            </div>      
        </div>


        <div v-show="isDropdownOpen" 
            class=" flex flex-col items-center absolute top-16 right-16 w-48 bg-secondary rounded-md shadow-lg py-1 z-50 border-solid border-2 border-lgray-accent"
            ref="dropdownMenu">
            <button @click="isVisible = true" class="flex items-center w-full px-4 py-2 text-sm text-white hover:text-gray-500">
                <span>Ustawienia konta</span>
                <font-awesome-icon :icon="faGear" class="ml-2"/>
            </button>
            <hr class="border-lgray-accent w-[90%]">
            <button @click="logout" class="flex items-center w-full px-4 py-2 text-sm text-white hover:text-red-600 ">
                <span>Wyloguj się</span>
                <font-awesome-icon :icon="faRightFromBracket" class="ml-2"/>
            </button>
        </div>
    </nav>

    <adminSettings :isVisible="isVisible" @close="isVisible = false" :username="username" :email="email"/>
</template>


<script setup>
import { ref, onMounted, onUnmounted } from 'vue'
import axios from 'axios'
import logo from '@/assets/logos/ITM_poziom_biale.png'
import { faCircleUser,faRightFromBracket,faGear } from '@fortawesome/free-solid-svg-icons'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { RouterLink } from 'vue-router';
import adminSettings from '../admin/adminSettings.vue'

const authStore = useAuthStore()


const isDropdownOpen = ref(false)
const dropdownMenu = ref(null)
const router = useRouter()
const username = ref('')
const email = ref('')
const isVisible = ref(false)

const toggleDropdown = () => {
    isDropdownOpen.value = !isDropdownOpen.value
}


const handleClickOutside = (event) => {
    if (dropdownMenu.value && !dropdownMenu.value.contains(event.target) && !event.target.closest('.cursor-pointer')) {
        isDropdownOpen.value = false
    }
}


const logout = async () => {
  try {
    await axios.post('http://localhost:5023/api/auth/logout', {}, {
      withCredentials: true
    });
    authStore.setAuthenticated(false);
    router.push('/');
    isDropdownOpen.value = false;
  } catch (error) {
    console.error('Błąd podczas wylogowywania:', error);
  }
};

const fetchUser = async () => {
    try {
        const res = await axios.get('http://localhost:5023/api/auth/me', { withCredentials: true })
        username.value = res.data.name
        email.value = res.data.email
    } catch (err) {
        console.error('Nie udało się pobrać danych użytkownika')
    }
}

onMounted(() => {
    document.addEventListener('click', handleClickOutside)
    fetchUser()
})

onUnmounted(() => {
    document.removeEventListener('click', handleClickOutside)
})

</script>