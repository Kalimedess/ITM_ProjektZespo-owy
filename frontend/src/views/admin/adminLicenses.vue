<template>
    <div class="flex flex-col h-full bg-primary">
        <RouterView />
  
        <!-- Main content container, stretch to take full screen height -->
        <div class="flex flex-col items-center gap-6 p-60 w-full flex-grow bg-secondary">
          <div class="text-center">
            <p class="mb-2 font-nasalization text-white">Pozostałe licencje gier</p>
            <div class="border-2 border-accent bg-tertiary rounded-md px-6 py-2">X</div>
          </div>
  
          <div class="flex justify-between w-full max-w-md">
            <div class="text-center">
              <p class="mb-2 text-white">Gry w trakcie</p>
              <div class="border-2 border-accent bg-tertiary rounded-md px-6 py-2">X</div>
            </div>
  
            <div class="text-center">
              <p class="mb-2 text-white">Gry rozegrane ogółem</p>
              <div class="border-2 border-accent bg-tertiary rounded-md px-6 py-2">X</div>
            </div>
          </div>
  
          <div>
            <div class="border-2 border-accent bg-tertiary rounded-md px-6 py-2 cursor-pointer hover:bg-accent font-semibold">
              Dokup licencje
            </div>
          </div>
        </div>
      </div>
  </template>
  
  
      
  <script setup>
    import { RouterView } from 'vue-router';

    
  </script>