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
    },
    xAxisLabel: {
      type: String,
      default: 'Drużyna'
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
  
    const x = d3.scaleBand()
      .domain(props.data.map(d => d.team))
      .range([0, width])
      .padding(0.2)
  
    const y = d3.scaleLinear()
      .domain([0, d3.max(props.data, d => d.avgBits) + 5])
      .range([height, 0])
  
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
  
    svg.selectAll('rect')
  .data(props.data)
  .enter()
  .append('rect')
  .attr('x', d => x(d.team))
  .attr('y', d => y(d.avgBits))
  .attr('width', x.bandwidth())
  .attr('height', d => height - y(d.avgBits))
  .attr('fill', '#a855f7')


    svg.selectAll('text.value')
  .data(props.data)
  .enter()
  .append('text')
  .attr('class', 'value')
  .attr('x', d => x(d.team) + x.bandwidth() / 2)
  .attr('y', d => y(d.avgBits) - 5)
  .attr('text-anchor', 'middle')
  .text(d => d.avgBits)
  .style('fill', '#ffffff')
  .style('font-size', '12px')
  
    svg.append('text')
      .attr('x', width / 2)
      .attr('y', height + 40)
      .attr('text-anchor', 'middle')
      .text(props.xAxisLabel)
      .style('fill', '#ffffff')
      .style('font-size', '14px')
  
    svg.append('text')
      .attr('text-anchor', 'middle')
      .attr('transform', `translate(-35, ${height / 2}) rotate(-90)`)
      .text('Bity')
      .style('fill', '#ffffff')
      .style('font-size', '14px')
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