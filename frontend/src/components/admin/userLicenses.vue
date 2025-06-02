<template>
    <div class="flex items-center justify-center mb-6">
        <h2 class="text-2xl font-nasalization">Licencje</h2>
      </div>

      <div class="flex justify-center mb-6">
        <font-awesome-icon :icon="faCircleUser" class="h-20" />
      </div>

      <hr class="border-lgray-accent mb-4" />

      <div class="mb-4 flex justify-between items-start">
          <p class="font-bold ml-2">Gry w trakcie</p>
          <p class="mr-2">{{ licencesData.gamesInSession }}</p>
      </div>
      <hr class="border-lgray-accent mb-4" />

       <div class="mb-4 flex justify-between items-start">
          <p class="font-bold ml-2">Gry rozegrane</p>
          <p class="mr-2">{{ licencesData.gamesInTotal }}</p>
      </div>
      <hr class="border-lgray-accent mb-4" />

      <div class="mb-4 flex justify-between items-start">
          <p class="font-bold ml-2">Pozostałe licencje</p>
          <p class="mr-2">{{ licencesData.licensesLeft }}</p>
      </div>
      <hr class="border-lgray-accent mb-6" />

      <button 
          class="bg-tertiary hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60 mb-5"
        >
          Dokup więcej licencji
      </button>


</template>

<script setup>
    import { ref } from 'vue';

    const licencesData = ref({
        gamesInSession:6,
        gamesInTotal: 10,
        licensesLeft: 4,
    });



</script>