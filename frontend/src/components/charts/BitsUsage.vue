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
  
    const svg = d3
      .select(chart.value)
      .append('svg')
      .attr('width', width + margin.left + margin.right)
      .attr('height', height + margin.top + margin.bottom + 20)
      .append('g')
      .attr('transform', `translate(${margin.left},${margin.top})`)
  
    const x = d3
      .scalePoint()
      .domain(props.data.map((d) => d.round))
      .range([0, width])
      .padding(0.5)
  
    const y = d3
      .scaleLinear()
      .domain([0, d3.max(props.data, (d) => d.bits) + 5])
      .range([height, 0])
  
    svg
      .append('g')
      .attr('transform', `translate(0,${height})`)
      .call(d3.axisBottom(x))
      .selectAll('text')
      .style('fill', '#ffffff')
      .attr('transform', 'rotate(0)')
      .style('text-anchor', 'end')
  
    svg
      .append('g')
      .call(d3.axisLeft(y))
      .selectAll('text')
      .style('fill', '#ffffff')
  
    svg.selectAll('path').style('stroke', '#ffffff')
    svg.selectAll('line').style('stroke', '#888888')
  
    const line = d3
      .line()
      .x((d) => x(d.round))
      .y((d) => y(d.bits))
  
    svg
      .append('path')
      .datum(props.data)
      .attr('fill', 'none')
      .attr('stroke', '#38bdf8')
      .attr('stroke-width', 3)
      .attr('d', line)
  
    svg
      .selectAll('circle')
      .data(props.data)
      .enter()
      .append('circle')
      .attr('cx', (d) => x(d.round))
      .attr('cy', (d) => y(d.bits))
      .attr('r', 5)
      .attr('fill', '#38bdf8')
  
    svg
      .append('text')
      .attr('x', width / 2)
      .attr('y', height + 45)
      .attr('text-anchor', 'middle')
      .text('Runda')
      .style('fill', '#ffffff')
      .style('font-size', '14px')
  
    svg
      .append('text')
      .attr('text-anchor', 'middle')
      .attr('transform', `translate(-35, ${height / 2}) rotate(-90)`)
      .text('Zużyte Bity')
      .style('fill', '#ffffff')
      .style('font-size', '14px')
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