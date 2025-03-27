<template>
    <div class="w-full h-full flex-1 flex flex-col justify-center items-center text-center text-white" >
      <h1 class="m-8 font-nasalization text-7xl">Drzewo decyzji</h1>
      <img :src="drzewko" class="w-100 border-2 border-lgray-accent p-2 overflow-hidden rounded-md" alt="ITM logo" >
    </div>
  </template>
  
  <script setup>
  import drzewko from '@/assets/viewPNGs/DrzewoDecyzji.png'
  </script>