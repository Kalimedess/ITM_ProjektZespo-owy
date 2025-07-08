<template>
    <div ref="chart" class="w-full h-96"></div>
  </template>
  
  <script setup>
  import * as d3 from 'd3'
  import { ref, onMounted, watch, nextTick } from 'vue'
  
  const props = defineProps({
    data: {
      type: Object, // np. { success: 12, failure: 8 }
      required: true
    }
  })
  
  const chart = ref(null)
  
  const drawChart = () => {
    const width = chart.value.clientWidth
    const height = chart.value.clientHeight
    const radius = Math.min(width, height) / 2 - 40
  
    d3.select(chart.value).selectAll('*').remove()
  
    const svg = d3.select(chart.value)
      .append('svg')
      .attr('width', width)
      .attr('height', height)
      .append('g')
      .attr('transform', `translate(${width / 2},${height / 2})`)
  
    const data = props.data
    const color = d3.scaleOrdinal()
      .domain(['success', 'failure'])
      .range(['#4ade80', '#f87171']) // zielony, czerwony
  
    const pie = d3.pie().value(d => d[1])
    const data_ready = pie(Object.entries(data))
  
    const arc = d3.arc()
      .innerRadius(0)
      .outerRadius(radius)
  
    svg
      .selectAll('path')
      .data(data_ready)
      .enter()
      .append('path')
      .attr('d', arc)
      .attr('fill', d => color(d.data[0]))
      .attr('stroke', '#1e293b')
      .style('stroke-width', '2px')
  
    // Legenda
    const labels = {
      success: 'Sukcesy',
      failure: 'Porażki'
    }
  
    const legend = svg.append('g')
      .attr('transform', `translate(${radius + 20}, ${-radius + 10})`)
  
    Object.entries(data).forEach(([key, value], i) => {
      const yOffset = i * 20
  
      legend.append('rect')
        .attr('x', 0)
        .attr('y', yOffset)
        .attr('width', 12)
        .attr('height', 12)
        .attr('fill', color(key))
  
      legend.append('text')
        .attr('x', 18)
        .attr('y', yOffset + 10)
        .text(`${labels[key] || key}: ${value}`)
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
  