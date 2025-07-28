<template>
  <div ref="chart" class="w-full"></div>
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
  const margin = { top: 30, right: 100, bottom: 90, left: 100 }

  const players = Object.keys(props.data[0]).filter(k => k !== 'round')
  const fullWidth = chart.value.clientWidth
  const fullHeight = players.length * 60 + margin.top + margin.bottom

  const width = fullWidth - margin.left - margin.right
  const height = fullHeight - margin.top - margin.bottom

  d3.select(chart.value).selectAll('*').remove()

  const svg = d3.select(chart.value)
    .append('svg')
    .attr('width', fullWidth)
    .attr('height', fullHeight)
    .append('g')
    .attr('transform', `translate(${margin.left},${margin.top})`)

  const rankData = props.data.map(d => {
    const sorted = [...players].sort((a, b) => d[b] - d[a])
    const entry = { round: d.round }
    sorted.forEach((player, idx) => {
      entry[player] = idx + 1
    })
    return entry
  })

  const rounds = rankData.map(d => d.round)
  const x = d3.scaleLinear()
    .domain([d3.min(rounds), d3.max(rounds)])
    .range([0, width])

  const y = d3.scaleLinear()
    .domain([players.length, 1])
    .range([height, 0])

  const color = d3.scaleOrdinal(d3.schemeTableau10)

  // Osie
  svg.append('g')
    .attr('transform', `translate(0,${height})`)
    .call(d3.axisBottom(x).tickValues(rounds).tickFormat(d3.format('d')))
    .selectAll('text')
    .style('fill', '#ffffff')
    .style('font-size', '26px')

  svg.append('g')
    .call(d3.axisLeft(y).ticks(players.length).tickFormat(d => `${d}.`))
    .selectAll('text')
    .style('fill', '#ffffff')
    .style('font-size', '26px')

  svg.selectAll('path').style('stroke', '#ffffff')
  svg.selectAll('line').style('stroke', '#444')

  // Linie i punkty
  players.forEach((player, idx) => {
    const line = d3.line()
      .x(d => x(d.round))
      .y(d => y(d[player]))

    svg.append('path')
      .datum(rankData)
      .attr('fill', 'none')
      .attr('stroke', color(idx))
      .attr('stroke-width', 4)
      .attr('d', line)

    svg.selectAll(`.circle-${player}`)
      .data(rankData)
      .enter()
      .append('circle')
      .attr('cx', d => x(d.round))
      .attr('cy', d => y(d[player]))
      .attr('r', 4)
      .attr('fill', color(idx))

    const last = rankData[rankData.length - 1]
    svg.append('text')
      .attr('x', x(last.round) + 8)
      .attr('y', y(last[player]))
      .attr('fill', color(idx))
      .attr('font-size', '18px')
      .attr('alignment-baseline', 'middle')
      .text(player)
  })

  // Opisy osi
  svg.append('text')
    .attr('x', width / 2)
    .attr('y', height + 70)
    .attr('text-anchor', 'middle')
    .text('Runda')
    .style('fill', '#ffffff')
    .style('font-size', '30px')

  svg.append('text')
    .attr('text-anchor', 'middle')
    .attr('transform', `translate(-60,${height / 2}) rotate(-90)`)
    .text('Pozycja')
    .style('fill', '#ffffff')
    .style('font-size', '30px')
}

onMounted(() => {
  nextTick(drawChart)
})

watch(() => props.data, () => {
  drawChart()
}, { deep: true })
</script>
