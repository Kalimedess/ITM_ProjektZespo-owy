<template>
    <div class="flex flex-row justify-center gap-3 items-center mt-5 mb-5">
        <div class="flex gap-2">
            <button 
                class="border-2 border-lgray-accent py-2 px-4 rounded-md text-center hover:border-accent transition-colors duration-300 flex items-center"
                @click="openCreateGame">
                <font-awesome-icon :icon="faPlus" class="h-4 text-accent mr-2"/> 
                Stwórz nową grę
            </button>
        </div>
        <div class="flex gap-2">
            <button 
                @click="stopAllGames"
                class="border-2 border-lgray-accent py-2 px-4 rounded-md text-center hover:border-accent transition-colors duration-300 flex items-center">
                <font-awesome-icon :icon="faCircleStop" class="h-4 text-accent mr-2"/> 
                Zatrzymaj wszystkie gry
            </button>
            <button
                @click="endAllGames"  
                class="border-2 border-lgray-accent py-2 px-4 rounded-md text-center hover:border-accent transition-colors duration-300 flex items-center">
                <font-awesome-icon :icon="faPowerOff" class="h-4 text-accent mr-2"/> 
                Zakończ wszystkie gry
            </button>
        </div>
    </div>
</template>

<script setup>
  import { ref } from 'vue';
  import { faPowerOff,faPlus } from '@fortawesome/free-solid-svg-icons'
  import { faCircleStop } from '@fortawesome/free-regular-svg-icons'
  import { useToast } from 'vue-toastification';
  import apiServices from '@/services/apiServices';
  import apiConfig from '@/services/apiConfig';

     const toast = useToast();

     const emit = defineEmits(['openCreateGame','update-status']);

     const openCreateGame = () => {
        emit('openCreateGame');
     };

     const isStoppingGames = ref(false);
    const isEndingGames = ref(false);

    // Funkcja do zatrzymywania wszystkich gier
    const stopAllGames = async () => {
      if (isStoppingGames.value) return;
      if (!confirm('Czy na pewno chcesz zatrzymać wszystkie aktywne gry?')) {
        return;
      }

      isStoppingGames.value = true;
      try {
        const response = await apiServices.post(apiConfig.games.stopAll);

        if (response.status === 200 || response.status === 204) {
          toast.success('Wszystkie gry zostały zatrzymane.');
        } else {
          toast.error(`Nie udało się zatrzymać gier. Serwer odpowiedział: ${response.status}`);
        }
      } catch (error) {
        console.error('Błąd podczas zatrzymywania gier:', error.response?.data || error.message);
        toast.error(`Błąd podczas zatrzymywania gier: ${error.response?.data?.message || error.message || 'Nieznany błąd'}`);
      } finally {
        isStoppingGames.value = false;
      }
      emit('update-status');
    };

    // Funkcja do kończenia wszystkich gier
    const endAllGames = async () => {
      if (isEndingGames.value) return;

      if (!confirm('JESTEŚ PEWIEN, że chcesz ZAKOŃCZYĆ WSZYSTKIE gry? Tej operacji NIE MOŻNA cofnąć i może spowodować utratę danych postępu gier.')) {
        return;
      }

      isEndingGames.value = true;
      try {
        const response = await apiServices.post(apiConfig.games.endAll);

        if (response.status === 200 || response.status === 204) {
          toast.success('Wszystkie gry zostały zakończone.');
        } else {
          toast.error(`Nie udało się zakończyć gier. Serwer odpowiedział: ${response.status}`);
        }
      } catch (error) {
        console.error('Błąd podczas kończenia gier:', error.response?.data || error.message);
        toast.error(`Błąd podczas kończenia gier: ${error.response?.data?.message || error.message || 'Nieznany błąd'}`);
      } finally {
        isEndingGames.value = false;
      }
      emit('update-status');
    };

     
</script>