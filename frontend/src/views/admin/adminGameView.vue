<template>
    <div class="w-full text-white">
        <div class="m-4 px-2 py-2 border-2 border-lgray-accent rounded-md bg-tertiary">
            <h1 class="text-center text-white font-nasalization text-3xl mt-2 mb-4">Stoły</h1>
            <hr class ="my-4 border-lgray-accent"/>
            <div class="grid grid-cols-1 xl:grid-cols-3 gap-4 mt-6">
                <template v-if="gameId">
                  <tableCard
                    v-for="table in tables"
                    :key="table.id"
                    :table="table"
                    :gameId="gameId"
                    :color="table.color"
                    :token="table.token"
                    :gameUrl="`${origin}/player/${table.token}`"
                  />
                </template>
                <template v-else>
                <div class="text-center text-gray-300 mt-4">
                  Brak aktywnej gry.
                </div>
              </template>
            </div>
        </div>
    </div>

</template>

console.log('tableCard props:', { table, game });
<script setup>
  import tableCard from '@/components/game/tableCard.vue'
  import { ref, onMounted, watch } from 'vue'
  import { useRoute } from 'vue-router'
  import apiServices from '@/services/apiServices'
  import apiConfig from '@/services/apiConfig'

  const route = useRoute();
  const gameId = Number(route.params.gameId);

  const props = defineProps(['gameId'])
  const origin = typeof window !== 'undefined' ? window.location.origin : '';

  const tables = ref([])
  
  const getTeams = async () => {
    try {
      console.log('Fetching teams for gameId:', gameId)
      const response = await apiServices.get(apiConfig.admin.games.getTeams(gameId))
      tables.value = response.data
    } catch (error) {
      console.error('Błąd przy pobieraniu drużyn:', error)
    }
  }
          
  onMounted(() => {
    getTeams();
  });

</script>