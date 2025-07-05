<template>
  <div class="w-full border-2 rounded-md bg-tertiary p-4 flex flex-col gap-4 text-white hover:transform hover:-translate-y-2 transition-all duration-300"
       :style="{borderColor: props.color}"
       @dblclick="router.push('/admin/game')">
      <div class="flex flex-row items-center justify-between">
        <div class="flex items-center min-w-0 flex-1 mr-2 overflow-hidden">
      <font-awesome-icon :icon="faGamepad" class="h-5 mr-2 flex-shrink-0"
                      :style="{ color: props.color }"/> 
      <span class="text-lg font-semibold break-words"
            :style="{ color: props.color }">
        {{ game.name }}
      </span>
    </div>
      <span class="text-sm px-2 py-1 rounded-full"
            :style="{backgroundColor: props.color}">
        ID: {{ game.id }}
      </span>
    </div>
    
    <div class="flex flex-row gap-2">
      <button 
        v-if="game.status === 'During' || game.status === 'Paused' "
        @click="requestStatusChange(game.id, game.status === 'During' ? 'Paused' : 'During')"
        class="flex flex-auto items-center justify-center border-2 border-lgray-accent py-2 px-3 rounded-md hover:border-accent transition-colors duration-300" @dblclick.stop>
        <font-awesome-icon :icon="game.status === 'During' ? faCircleStop : faCirclePlay" class="h-4 text-accent mr-2"/>
        {{ game.status === 'During' ? 'Wstrzymaj grę' : 'Wznów grę' }}
      </button>
    
       <button 
        @click=""
        class="flex flex-auto items-center justify-center border-2 border-lgray-accent py-2 px-3 rounded-md hover:border-accent transition-colors duration-300" @dblclick.stop>
          <font-awesome-icon :icon="faPowerOff" class="h-4 mr-2 text-accent"/>  
          Zakończ grę
      </button>
    </div>
    
    <RouterLink 
        :to="`/admin/game`"
        @dblclick.stop
        class="w-full flex items-center justify-center border-2 border-lgray-accent py-2 px-3 rounded-md hover:border-accent transition-colors duration-300">
        <font-awesome-icon :icon="faMagnifyingGlass" class="h-4 mr-2 text-accent"/>     
        Otwórz grę
    </RouterLink>
  </div>
</template>

<script setup>
  import { faGamepad, faCircleStop, faMagnifyingGlass,faCirclePlay, faPowerOff } from '@fortawesome/free-solid-svg-icons';
  import { RouterLink,useRouter } from 'vue-router';

  const router = useRouter();


  const props = defineProps({
    game: {
      type: Object,
      required: true
    },
    color: {
      type: String,
      required: true
    }
  });

  const emit = defineEmits(['update-status']);

  const requestStatusChange = (gameId, newStatus) => {

      emit('update-status', { gameId, newStatus });
  };
</script>