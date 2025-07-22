<template>
    <div class="w-full text-white">
        <div class="m-4 px-2 py-2 border-2 border-lgray-accent rounded-md bg-tertiary">
            <h1 class="text-center text-white font-nasalization text-3xl mt-2 mb-4">Stoły</h1>
            <hr class ="my-4 border-lgray-accent"/>
            <div class="grid grid-cols-1 xl:grid-cols-3 gap-4 mt-6">
                <tableCard
                v-if="game"
                v-for="table in tables"
                :key="table.id"
                :table="table"
                :game="game"
                :color="getGameColor(table.id)"
                :gameUrl="`${origin}/player/${table.token}`"
                />
            </div>
        </div v-else class="text-center text-gray-300 mt-4">
    </div>

</template>

console.log('tableCard props:', { table, game });
<script setup>
    import tableCard from '@/components/game/tableCard.vue'
    import { ref, onMounted, watch } from 'vue'
    import axios from 'axios'
    import { useRoute } from 'vue-router'

    const props = defineProps(['gameId'])
    const origin = typeof window !== 'undefined' ? window.location.origin : '';

    const getGameColor = (gameId) => {
        
        const colors = [
            '#E53E3E',
            '#3182CE',
            '#38A169',
            '#D69E2E',
            '#805AD5',
            '#D53F8C',
            '#DD6B20',
            '#319795',
            '#5A67D8',
        ];

        return colors[gameId - 1 % 9];
    }

  const tables = ref([])
  const game = ref(null)
  



const getTeams = async () => {
  try {
    console.log('Fetching teams for gameId:', props.gameId)
    const response = await axios.get(`http://localhost:5023/api/adminpanel/teams/by-game/${props.gameId}`, {
      withCredentials: true
    })
    tables.value = response.data
  } catch (error) {
    console.error('Błąd przy pobieraniu drużyn:', error)
  }
}
      

const getGame = async () => {
  try {
    const response = await axios.get(`http://localhost:5023/api/games/${props.gameId}`, {
      withCredentials: true
    })
    game.value = response.data
  } catch (error) {
    console.error('Błąd przy pobieraniu gry:', error)
  }
}

onMounted(() => {
  getTeams();
  getGame(); 
});

</script>