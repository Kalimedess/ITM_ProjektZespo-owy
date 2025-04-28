<template>
  <div class="flex flex-wrap justify-center gap-8">
    <div 
      v-for="(teamData, index) in data" 
      :key="index" 
      class="w-64 h-80 flex flex-col items-center bg-[#1c2942] p-4 rounded-lg shadow-lg">
      
      <div ref="chartsContainer" :id="`chart-${index}`" class="w-full h-48"></div>
      
      <p class="mt-4 text-lg text-white font-bold">{{ teamData.team }}</p>
      
      <div class="text-sm text-white mt-2">
        <p>Sukcesy: <span class="text-green-400 font-bold">{{ teamData.success }}</span></p>
        <p>Porażki: <span class="text-red-400 font-bold">{{ teamData.failure }}</span></p>
      </div>
    </div>
  </div>
</template>

<script setup>
import * as d3 from 'd3'
import { ref, onMounted, nextTick, watch } from 'vue'

const props = defineProps({
  data: {
    type: Array, // [{team: ..., success: ..., failure: ...}, ...]
    required: true
  }
})

const chartsContainer = ref(null)

const drawCharts = () => {
  props.data.forEach((teamData, index) => {
    const selector = `#chart-${index}`
    d3.select(selector).selectAll('*').remove()

    const width = 150
    const height = 150
    const radius = Math.min(width, height) / 2 - 10

    const svg = d3.select(selector)
      .append('svg')
      .attr('width', width)
      .attr('height', height)
      .append('g')
      .attr('transform', `translate(${width / 2},${height / 2})`)

    const pie = d3.pie().value(d => d[1])
    const data_ready = pie(Object.entries({ success: teamData.success, failure: teamData.failure }))

    const arc = d3.arc()
      .innerRadius(0)
      .outerRadius(radius)

    const color = d3.scaleOrdinal()
      .domain(['success', 'failure'])
      .range(['#4ade80', '#f87171']) // Zielony - Sukcesy, Czerwony - Porażki

    svg
      .selectAll('path')
      .data(data_ready)
      .enter()
      .append('path')
      .attr('d', arc)
      .attr('fill', d => color(d.data[0]))
      .attr('stroke', '#1e293b')
      .style('stroke-width', '2px')
  })
}

onMounted(() => {
  nextTick(() => {
    drawCharts()
  })
})

watch(() => props.data, () => {
  drawCharts()
}, { deep: true })
</script>