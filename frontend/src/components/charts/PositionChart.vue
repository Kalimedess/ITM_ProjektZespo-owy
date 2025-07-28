<template>
  <div ref="chart" class="w-full h-[600px] flex items-center justify-center"></div>
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
  d3.select(chart.value).selectAll('*').remove()
  
  const svgWidth = 1000
  const svgHeight = 700
  const podiumWidth = 250
  const podiumHeight = 320
  const spacing = 80
  const topOffset = 100
  const depth = 20
  
  const sorted = [...props.data].sort((a, b) => a.position - b.position)
  const [first, second, third, ...others] = sorted
  
  const svg = d3
    .select(chart.value)
    .append('svg')
    .attr('width', svgWidth)
    .attr('height', svgHeight)
  
  const centerX = svgWidth / 2
  
  const render3DBox = (x, y, width, height, baseColor) => {
    // Front
    svg.append('rect')
      .attr('x', x)
      .attr('y', y)
      .attr('width', width)
      .attr('height', height)
      .attr('fill', baseColor)
      .attr('stroke', '#ffffff')
      .attr('stroke-width', 2)
    
    // Right side
    svg.append('polygon')
      .attr('points', `
        ${x + width},${y}
        ${x + width + depth},${y - depth}
        ${x + width + depth},${y + height - depth}
        ${x + width},${y + height}
      `)
      .attr('fill', d3.color(baseColor).darker(0.7))
      .attr('stroke', '#ffffff')
      .attr('stroke-width', 1)
    
    // Top side
    svg.append('polygon')
      .attr('points', `
        ${x},${y}
        ${x + width},${y}
        ${x + width + depth},${y - depth}
        ${x + depth},${y - depth}
      `)
      .attr('fill', d3.color(baseColor).brighter(0.5))
      .attr('stroke', '#ffffff')
      .attr('stroke-width', 1)
  }
  
  // 2nd place
  const secondX = centerX - podiumWidth - podiumWidth / 2
  const secondY = topOffset + 100
  render3DBox(secondX, secondY, podiumWidth, podiumHeight - 100, 'silver')
  
  svg.append('text')
    .attr('x', secondX + podiumWidth / 2)
    .attr('y', secondY - 35)
    .attr('text-anchor', 'middle')
    .attr('font-size', '22px')
    .attr('font-weight', 'bold')
    .attr('fill', '#ffffff')
    .attr('stroke', '#000000')
    .attr('stroke-width', 0.5)
    .text(`${second?.name || 'N/A'}`)
  
  // Medal dla 2. miejsca
  svg.append('circle')
    .attr('cx', secondX + podiumWidth / 2)
    .attr('cy', secondY + 30)
    .attr('r', 15)
    .attr('fill', 'silver')
    .attr('stroke', '#ffffff')
    .attr('stroke-width', 2)
  
  svg.append('text')
    .attr('x', secondX + podiumWidth / 2)
    .attr('y', secondY + 35)
    .attr('text-anchor', 'middle')
    .attr('font-size', '14px')
    .attr('font-weight', 'bold')
    .attr('fill', '#000000')
    .text('2')
  
  // 1st place
  const firstX = centerX - podiumWidth / 2
  const firstY = topOffset
  render3DBox(firstX, firstY, podiumWidth, podiumHeight, 'gold')
  
  svg.append('text')
    .attr('x', firstX + podiumWidth / 2)
    .attr('y', firstY - 35)
    .attr('text-anchor', 'middle')
    .attr('font-size', '24px')
    .attr('font-weight', 'bold')
    .attr('fill', '#ffffff')
    .attr('stroke', '#000000')
    .attr('stroke-width', 0.5)
    .text(`${first?.name || 'N/A'}`)
  
  // Medal dla 1. miejsca
  svg.append('circle')
    .attr('cx', firstX + podiumWidth / 2)
    .attr('cy', firstY + 30)
    .attr('r', 18)
    .attr('fill', 'gold')
    .attr('stroke', '#ffffff')
    .attr('stroke-width', 2)
  
  svg.append('text')
    .attr('x', firstX + podiumWidth / 2)
    .attr('y', firstY + 36)
    .attr('text-anchor', 'middle')
    .attr('font-size', '16px')
    .attr('font-weight', 'bold')
    .attr('fill', '#000000')
    .text('1')
  
  // 3rd place
  const thirdX = centerX + podiumWidth / 2
  const thirdY = topOffset + 160
  render3DBox(thirdX, thirdY, podiumWidth, podiumHeight - 160, '#cd7f32')
  
  svg.append('text')
    .attr('x', thirdX + podiumWidth / 2)
    .attr('y', thirdY - 35)
    .attr('text-anchor', 'middle')
    .attr('font-size', '22px')
    .attr('font-weight', 'bold')
    .attr('fill', '#ffffff')
    .attr('stroke', '#000000')
    .attr('stroke-width', 0.5)
    .text(`${third?.name || 'N/A'}`)
  
  // Medal dla 3. miejsca
  svg.append('circle')
    .attr('cx', thirdX + podiumWidth / 2)
    .attr('cy', thirdY + 30)
    .attr('r', 15)
    .attr('fill', '#cd7f32')
    .attr('stroke', '#ffffff')
    .attr('stroke-width', 2)
  
  svg.append('text')
    .attr('x', thirdX + podiumWidth / 2)
    .attr('y', thirdY + 35)
    .attr('text-anchor', 'middle')
    .attr('font-size', '14px')
    .attr('font-weight', 'bold')
    .attr('fill', '#ffffff')
    .text('3')
  
  //  Pozostałe miejsca
  const othersStartY = topOffset + podiumHeight + 80
  
  svg.selectAll('.others')
    .data(others)
    .enter()
    .append('text')
    .attr('x', secondX)
    .attr('y', (d, i) => othersStartY + i * 32)
    .attr('text-anchor', 'start') 
    .attr('fill', '#ffffff')
    .attr('font-size', '22px')
    .attr('font-weight', 'bold')
    .attr('stroke', '#000000')
    .attr('stroke-width', 0.3)
    .text((d) => `${d.position}. ${d.name}`)
}

onMounted(() => nextTick(drawChart))
watch(() => props.data, () => drawChart(), { deep: true })
</script>