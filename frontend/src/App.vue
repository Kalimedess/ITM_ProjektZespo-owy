<script setup>
import {  RouterView } from 'vue-router'

import { ref, onMounted } from 'vue';
import { useAuthStore } from '@/stores/auth';
import axios from 'axios';

const message = ref('');

const auth = useAuthStore();

onMounted(async () => {
    await auth.checkAuth();
    try {
        const response = await axios.get('http://localhost:5023/api/test'); //Gdyby nie działało, zobaczyć na jakim porcie działa DOTNET
        message.value = response.data.message;
    } catch (error) {
        console.error('Error fetching data:', error);
    }
});



</script>

<template>
  <RouterView />
</template>

