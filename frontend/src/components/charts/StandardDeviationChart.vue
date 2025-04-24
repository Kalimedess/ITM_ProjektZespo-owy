<template>
    <div ref="chart" class="w-full h-96"></div>
  </template>
  
  <script setup>
  import * as d3 from 'd3'
  import { ref, onMounted, watch, nextTick } from 'vue'
  
  const props = defineProps({
    data: {
      type: Array, // np. [{ game: 1, positions: 3.2, bits: 5.1 }, ...]
      required: true
    }
  })
  
  const chart = ref(null)
  
  const drawChart = () => {
    const margin = { top: 40, right: 30, bottom: 50, left: 50 }
    const width = chart.value.clientWidth - margin.left - margin.right
    const height = chart.value.clientHeight - margin.top - margin.bottom
  
    d3.select(chart.value).selectAll('*').remove()
  
    const svg = d3.select(chart.value)
      .append('svg')
      .attr('width', width + margin.left + margin.right)
      .attr('height', height + margin.top + margin.bottom)
      .append('g')
      .attr('transform', `translate(${margin.left},${margin.top})`)
  
    const gameNumbers = props.data.map(d => d.game.toString())
    const metricNames = Object.keys(props.data[0]).filter(k => k !== 'game')
  
    const x = d3.scalePoint()
      .domain(gameNumbers)
      .range([0, width])
      .padding(0.5)
  
    const y = d3.scaleLinear()
      .domain([0, d3.max(props.data.flatMap(d => metricNames.map(m => d[m]))) + 2])
      .range([height, 0])
  
    const color = d3.scaleOrdinal(d3.schemeTableau10).domain(metricNames)
  
    svg.append('g')
      .attr('transform', `translate(0,${height})`)
      .call(d3.axisBottom(x))
      .selectAll('text')
      .style('fill', '#ffffff')
  
    svg.append('g')
      .call(d3.axisLeft(y))
      .selectAll('text')
      .style('fill', '#ffffff')
  
    svg.selectAll('path').style('stroke', '#ffffff')
    svg.selectAll('line').style('stroke', '#888888')
  
    metricNames.forEach(name => {
      const line = d3.line()
        .x(d => x(d.game.toString()))
        .y(d => y(d[name]))
  
      /*svg.append('path')
        .datum(props.data)
        .attr('fill', 'none')
        .attr('stroke', color(name))
        .attr('stroke-width', 2)
        .attr('d', line)
        */
      svg.selectAll(`.dot-${name}`)
        .data(props.data)
        .enter()
        .append('circle')
        .attr('cx', d => x(d.game.toString()))
        .attr('cy', d => y(d[name]))
        .attr('r', 4)
        .attr('fill', color(name))
    })
  
    // Oznaczenia osi
    svg.append('text')
      .attr('x', width / 2)
      .attr('y', height + 40)
      .attr('text-anchor', 'middle')
      .text('Numer gry')
      .style('fill', '#ffffff')
      .style('font-size', '14px')
  
    svg.append('text')
      .attr('text-anchor', 'middle')
      .attr('transform', `translate(-35, ${height / 2}) rotate(-90)`)
      .text('Odchylenie standardowe')
      .style('fill', '#ffffff')
      .style('font-size', '14px')
  
    // Legenda
    const labels = {
      positions: 'Pozycje',
      bits: 'Bity'
    }
  
    const legend = svg.append('g')
      .attr('transform', `translate(${width - 120}, -30)`)
  
    metricNames.forEach((name, i) => {
      const yOffset = i * 20
  
      legend.append('rect')
        .attr('x', 0)
        .attr('y', yOffset)
        .attr('width', 12)
        .attr('height', 12)
        .attr('fill', color(name))
  
      legend.append('text')
        .attr('x', 18)
        .attr('y', yOffset + 10)
        .text(`${labels[name] || name}`)
        .style('fill', '#ffffff')
        .style('font-size', '12px')
    })
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
  