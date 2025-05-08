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
    const margin = { top: 30, right: 30, bottom: 50, left: 50 }
    const width = chart.value.clientWidth - margin.left - margin.right
    const height = chart.value.clientHeight - margin.top - margin.bottom
  
    d3.select(chart.value).selectAll('*').remove()
  
    const svg = d3.select(chart.value)
      .append('svg')
      .attr('width', width + margin.left + margin.right)
      .attr('height', height + margin.top + margin.bottom)
      .append('g')
      .attr('transform', `translate(${margin.left},${margin.top})`)
  
    const players = Object.keys(props.data[0]).filter(key => key !== 'round')
  
    // Skala X - linear
    const rounds = props.data.map(d => d.round)
  
    const x = d3.scaleLinear()
      .domain([d3.min(rounds), d3.max(rounds)])
      .range([0, width])
  
    const y = d3.scaleLinear()
      .domain([0, 20])
      .range([height, 0])
  
    // Osie
    svg.append('g')
      .attr('transform', `translate(0,${height})`)
      .call(
        d3.axisBottom(x)
          .tickValues(rounds) // pokaz TYLKO 1, 2, 3
          .tickFormat(d3.format('d')) // bez przecinków
      )
      .selectAll('text')
      .style('fill', '#ffffff')
  
    svg.append('g')
      .call(d3.axisLeft(y))
      .selectAll('text')
      .style('fill', '#ffffff')
  
    svg.selectAll('path').style('stroke', '#ffffff')
    svg.selectAll('line').style('stroke', '#888888')
  
    const color = d3.scaleOrdinal(d3.schemeTableau10)
  
    players.forEach((player, idx) => {
      const line = d3.line()
        .x(d => x(d.round))
        .y(d => y(d[player]))
  
      svg.append('path')
        .datum(props.data)
        .attr('fill', 'none')
        .attr('stroke', color(idx))
        .attr('stroke-width', 2)
        .attr('d', line)
  
      svg.selectAll(`circle-${player}`)
        .data(props.data)
        .enter()
        .append('circle')
        .attr('cx', d => x(d.round))
        .attr('cy', d => y(d[player]))
        .attr('r', 4)
        .attr('fill', color(idx))
    })
  
    // Opisy osi
    svg.append('text')
      .attr('x', width / 2)
      .attr('y', height + 40)
      .attr('text-anchor', 'middle')
      .text('Runda')
      .style('fill', '#ffffff')
  
    svg.append('text')
      .attr('text-anchor', 'middle')
      .attr('transform', `translate(-30,${height / 2}) rotate(-90)`)
      .text('Postęp')
      .style('fill', '#ffffff')
  
    // Legenda
    const legend = svg.selectAll('.legend')
      .data(players)
      .enter().append('g')
      .attr('transform', (d, i) => `translate(0, ${i * 20})`)
  
    legend.append('rect')
      .attr('x', width - 18)
      .attr('width', 12)
      .attr('height', 12)
      .attr('fill', (d, i) => color(i))
  
    legend.append('text')
      .attr('x', width - 24)
      .attr('y', 6)
      .attr('dy', '0.35em')
      .style('text-anchor', 'end')
      .style('fill', '#ffffff')
      .text(d => d)
  }
  
  onMounted(() => {
    nextTick(() => {
      drawChart()
    })
  })
  
  watch(() => props.data, () => {
    drawChart()
  }, { deep: true })
  </script>
  