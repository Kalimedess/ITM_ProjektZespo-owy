<script setup>
import { useRoute } from 'vue-router'
import { computed } from 'vue'

import BitsUsage from '@/components/charts/AverageBitsUsage.vue'
import DecisionSuccessChart from '@/components/charts/DecisionSuccessByTeamChart.vue'
import StandardDeviationChart from '@/components/charts/StandardDeviationChart.vue'

const route = useRoute()

const selectedStat = computed(() => route.query.stat || 'positions')


const avgBitsUsageByTeam = [
  { team: 'Gra 1', avgBits: 18.5 },
  { team: 'Gra 2', avgBits: 22 },
  { team: 'Gra 3', avgBits: 19.1 }
]


const decisionSuccessByTeam = [
  { team: 'Gra 1', success: 110, failure: 40 },
  { team: 'Gra 2', success: 85, failure: 65 },
  { team: 'Gra 3', success: 90, failure: 60 }
]

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

    <div v-if="selectedStat === 'bits'">
      <BitsUsage :data="avgBitsUsageByTeam" />
    </div>
    <div v-else-if="selectedStat === 'results'">
      <DecisionSuccessChart :data="decisionSuccessByTeam" />
    </div>
    <div v-else-if="selectedStat === 'deviation'">
      <StandardDeviationChart :data="stddevData" />
    </div>
    <div v-else>
      <p class="text-white">Wybierz statystykę z menu bocznego.</p>
    </div>
  </div>
</template>
