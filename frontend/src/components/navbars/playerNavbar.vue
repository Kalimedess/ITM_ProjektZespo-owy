<template>
    <nav class="w-full bg-secondary py-3 px-6 flex flex-row justify-between items-center border-b-2 border-lgray-accent">
        <div>
            <img :src="logo" class="h-12" alt="ITM logo">
        </div>

        <div class="absolute left-1/2 transform -translate-x-1/2">
            <h1 class="text-lg font-semibold text-white">Druzyna 1</h1>
        </div>
    </nav>
</template>


<script setup>

import logo from '@/assets/logos/ITM_poziom_biale.png'

</script>