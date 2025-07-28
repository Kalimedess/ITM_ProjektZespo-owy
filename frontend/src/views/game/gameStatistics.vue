<script setup>
import { useRoute } from 'vue-router'
import { computed } from 'vue'

import PositionChart from '@/components/charts/PositionChart.vue'
import BitsUsage from '@/components/charts/AverageBitsUsage.vue'
import ProgressChart from '@/components/charts/ProgressChart.vue'
import DecisionSuccessChart from '@/components/charts/DecisionSuccessByTeamChart.vue'

const route = useRoute()

const selectedStat = computed(() => route.query.stat || 'positions')

const teamPositions = [
  { name: 'Drużyna A', position: 1 },
  { name: 'Drużyna B', position: 2 },
  { name: 'Drużyna C', position: 3 },
  { name: 'Drużyna D', position: 4 },
  { name: 'Drużyna E', position: 5 },
  { name: 'Drużyna F', position: 6 }
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
  { round: 1, 'Drużyna A': 5, 'Drużyna B': 4, 'Drużyna C': 2, 'Drużyna D': 6, 'Drużyna E': 1, 'Drużyna F': 3 },
  { round: 2, 'Drużyna A': 4, 'Drużyna B': 3, 'Drużyna C': 1, 'Drużyna D': 5, 'Drużyna E': 2, 'Drużyna F': 6 },
  { round: 3, 'Drużyna A': 2, 'Drużyna B': 1, 'Drużyna C': 3, 'Drużyna D': 6, 'Drużyna E': 4, 'Drużyna F': 5 },
  { round: 4, 'Drużyna A': 1, 'Drużyna B': 2, 'Drużyna C': 4, 'Drużyna D': 5, 'Drużyna E': 3, 'Drużyna F': 6 },
  { round: 5, 'Drużyna A': 2, 'Drużyna B': 1, 'Drużyna C': 5, 'Drużyna D': 4, 'Drużyna E': 3, 'Drużyna F': 6 },
  { round: 6, 'Drużyna A': 3, 'Drużyna B': 2, 'Drużyna C': 1, 'Drużyna D': 4, 'Drużyna E': 6, 'Drużyna F': 5 },
  { round: 7, 'Drużyna A': 4, 'Drużyna B': 3, 'Drużyna C': 2, 'Drużyna D': 5, 'Drużyna E': 1, 'Drużyna F': 6 },
  { round: 8, 'Drużyna A': 5, 'Drużyna B': 4, 'Drużyna C': 3, 'Drużyna D': 1, 'Drużyna E': 2, 'Drużyna F': 6 },
  { round: 9, 'Drużyna A': 6, 'Drużyna B': 5, 'Drużyna C': 2, 'Drużyna D': 1, 'Drużyna E': 3, 'Drużyna F': 4 },
  { round: 10,'Drużyna A': 5, 'Drużyna B': 6, 'Drużyna C': 1, 'Drużyna D': 2, 'Drużyna E': 3, 'Drużyna F': 4 }
]
</script>

<template>
    <div class="p-8">
      <h2 class="text-2xl font-bold text-white mb-6">Statystyki aktualnej gry</h2>
  
      <div v-if="selectedStat === 'positions'">
        <PositionChart :data="teamPositions" />
      </div>
  
      <div v-else-if="selectedStat === 'bits'">
        <BitsUsage :data="avgBitsUsageByTeam" />
      </div>
  
      <div v-else-if="selectedStat === 'success'">
      <DecisionSuccessChart :data="decisionSuccessByTeam" />
    </div>
  
      <div v-else-if="selectedStat === 'progress'">
        <ProgressChart :data="teamProgress" />
      </div>
  
      <div v-else>
        <p class="text-white">Wybierz statystykę z panelu bocznego.</p>
      </div>
    </div>
  </template>