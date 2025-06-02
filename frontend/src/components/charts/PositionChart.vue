<template>
  <div ref="chart" class="w-full h-96"></div>
</template>

<script setup>
import * as d3 from 'd3'
import { ref, onMounted, watch, nextTick } from 'vue'

const props = defineProps({
  data: {
    type: Array,
    required: true
  }
})

const chart = ref(null)


const drawChart = () => {
  const margin = { top: 20, right: 30, bottom: 40, left: 40 }
  const width = chart.value.clientWidth - margin.left - margin.right
  const height = chart.value.clientHeight - margin.top - margin.bottom


  d3.select(chart.value).selectAll('*').remove()

  const svg = d3
    .select(chart.value)
    .append('svg')
    .attr('width', width + margin.left + margin.right)
    .attr('height', height + margin.top + margin.bottom+40)
    .append('g')
    .attr('transform', `translate(${margin.left},${margin.top})`)

  const x = d3
    .scaleBand()
    .domain(props.data.map((d) => d.name))
    .range([0, width])
    .padding(0.2)

  const y = d3.scaleLinear().domain([0, 30]).range([height, 0])


  svg
    .append('g')
    .call(d3.axisLeft(y))
    .selectAll('text')
    .style('fill', '#ffffff') // etykiety osi Y

  svg
    .append('g')
    .attr('transform', `translate(0,${height})`)
    .call(d3.axisBottom(x))
    .selectAll('text')
    .attr('transform', 'rotate(-40)')
    .style('text-anchor', 'end')
    .style('fill', '#ffffff') // etykiety osi X
  
  svg.append('text')
    .attr('text-anchor', 'middle')
    .attr('transform', `translate(${-30}, ${height / 2}) rotate(-90)`)
    .text('Pozycja')
    .style('fill', '#ffffff')
    .style('font-size', '14px')

  svg.append('text')
    .attr('text-anchor', 'middle')
    .attr('x', width / 2)
    .attr('y', height + 60)
    .text('Drużyna')
    .style('fill', '#ffffff')
    .style('font-size', '14px')

  // Linie osi
  svg.selectAll('path').style('stroke', '#ffffff') // linie osi
  svg.selectAll('line').style('stroke', '#888888') // linie pomocnicze

  // Słupki
  svg
    .selectAll('rect')
    .data(props.data)
    .enter()
    .append('rect')
    .attr('x', (d) => x(d.name))
    .attr('y', (d) => y(d.position))
    .attr('width', x.bandwidth())
    .attr('height', (d) => height - y(d.position))
    .attr('fill', '#a855f7') 
}

onMounted(() => {
  nextTick(() => {
    drawChart()
  })
})

watch(
  () => props.data,
  () => {
    drawChart()
  },
  { deep: true }
)
</script>

