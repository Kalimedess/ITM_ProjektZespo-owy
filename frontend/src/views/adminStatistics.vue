<script setup>
import { useRoute } from 'vue-router'
import { computed } from 'vue'

import PlayerPosition from '@/components/PlayerPositionChart.vue'
import BitsUsage from '@/components/BitsUsage.vue'
import DecisionSuccessChart from '@/components/DecisionSuccessChart.vue'
import StandardDeviationChart from '@/components/StandardDeviationChart.vue'

const route = useRoute()

const selectedStat = computed(() => route.query.stat || 'positions')

const playerPosition = [
  { name: 'Marcin', position: 8 },
  { name: 'Łukasz', position: 6 },
  { name: 'Szymon', position: 2 },
  { name: 'Bartosz', position: 5 },
  { name: 'Paweł', position: 7 }
]



const bitsData = [
    { round: '1', bits: 12 },
    { round: '2', bits: 18 },
    { round: '3', bits: 8 },
    { round: '4', bits: 24 },
    { round: '5', bits: 15 }
  ]


const decisionStats = {
  success: 14,
  failure: 6
}

const stddevData = [
  { game: 1, positions: 3.2, bits: 4.7 },
  { game: 2, positions: 2.6, bits: 3.8 },
  { game: 3, positions: 1.9, bits: 3.2 },
  { game: 4, positions: 2.1, bits: 2.5 },
  { game: 5, positions: 1.7, bits: 2.1 }
]

</script>

<template>
  <div class="p-8">
    <h2 class="text-2xl font-bold mb-6 text-white">Statystyki gry</h2>

    <div v-if="selectedStat === 'positions'">
      <PlayerPosition :data="playerPosition" />
    </div>

    <div v-else-if="selectedStat === 'bits'">
      <BitsUsage :data="bitsData" />
    </div>
    <div v-else-if="selectedStat === 'results'">
      <DecisionSuccessChart :data="decisionStats" />
    </div>
    <div v-else-if="selectedStat === 'deviation'">
      <StandardDeviationChart :data="stddevData" />
    </div>
    <div v-else>
      <p class="text-white">Wybierz statystykę z menu bocznego.</p>
    </div>
  </div>
</template>
