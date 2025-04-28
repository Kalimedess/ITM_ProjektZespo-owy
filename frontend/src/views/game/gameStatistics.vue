<script setup>
import { useRoute } from 'vue-router'
import { computed } from 'vue'

import PlayerPositionChart from '@/components/charts/PlayerPositionChart.vue'
import BitsUsage from '@/components/charts/AverageBitsUsage.vue'
import PlayerProgressChart from '@/components/charts/PlayerProgressChart.vue'
import DecisionSuccessChart from '@/components/charts/DecisionSuccessByTeamChart.vue'

const route = useRoute()

const selectedStat = computed(() => route.query.stat || 'positions')

const teamPositions = [
  { name: "Drużyna A", position: 8 },
  { name: "Drużyna B", position: 6 },
  { name: "Drużyna C", position: 2 },
  { name: "Drużyna D", position: 5 },
  { name: "Drużyna E", position: 7 }
]

const avgBitsUsageByTeam = [
  { team: 'Drużyna A', avgBits: 16.5 },
  { team: 'Drużyna B', avgBits: 20 },
  { team: 'Drużyna C', avgBits: 15.9 }
]


const decisionSuccessByTeam = [
  { team: 'Drużyna A', success: 40, failure: 10 },
  { team: 'Drużyna B', success: 35, failure: 15 },
  { team: 'Drużyna C', success: 50, failure: 5 }
]

const teamProgress = [
  { round: 1, "Drużyna A": 2, "Drużyna B": 3, "Drużyna C": 1, "Drużyna D": 4, "Drużyna E": 5 },
  { round: 2, "Drużyna A": 3, "Drużyna B": 2, "Drużyna C": 1, "Drużyna D": 5, "Drużyna E": 4 },
  { round: 3, "Drużyna A": 2, "Drużyna B": 4, "Drużyna C": 3, "Drużyna D": 5, "Drużyna E": 1 },
  { round: 4, "Drużyna A": 1, "Drużyna B": 3, "Drużyna C": 2, "Drużyna D": 4, "Drużyna E": 5 },
  { round: 5, "Drużyna A": 4, "Drużyna B": 2, "Drużyna C": 1, "Drużyna D": 5, "Drużyna E": 3 }
]
</script>

<template>
    <div class="p-8">
      <h2 class="text-2xl font-bold text-white mb-6">Statystyki aktualnej gry</h2>
  
      <div v-if="selectedStat === 'positions'">
        <PlayerPositionChart :data="teamPositions" />
      </div>
  
      <div v-else-if="selectedStat === 'bits'">
        <BitsUsage :data="avgBitsUsageByTeam" />
      </div>
  
      <div v-else-if="selectedStat === 'success'">
      <DecisionSuccessChart :data="decisionSuccessByTeam" />
    </div>
  
      <div v-else-if="selectedStat === 'progress'">
        <PlayerProgressChart :data="teamProgress" />
      </div>
  
      <div v-else>
        <p class="text-white">Wybierz statystykę z panelu bocznego.</p>
      </div>
    </div>
  </template>