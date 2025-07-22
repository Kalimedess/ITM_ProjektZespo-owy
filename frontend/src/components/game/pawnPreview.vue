<template>
  <div ref="container" class="flex justify-center items-center h-full w-full cursor-pointer" @click="emit('click')"
    title="Kliknij aby zmienić kolor">
    <svg 
      viewBox="0 0 300 300"
      class="w-full h-full bg-white"
    >
      <g ref="pawn"></g>
    </svg>
  </div>
</template>

<script setup>
import { ref, onMounted, watch, defineEmits } from 'vue';
import * as d3 from "d3";

const emit = defineEmits(['click']);

const props = defineProps({
  pawnColor: {
    type: String,
    default: '#0000ff'
  },
});

const container = ref(null);
const pawn = ref(null);

// Rysowanie pionka
const drawPawn = () => {
  const pawnGroup = d3.select(pawn.value);
  pawnGroup.selectAll("*").remove();

  const centerX = 150; 
  const centerY = 150;

  const pawnPath = "M 225.5,294.5 C 231.979,295.491 238.646,295.824 245.5,295.5C 245.5,311.167 245.5,326.833 245.5,342.5C 179.833,342.5 114.167,342.5 48.5,342.5C 48.5,326.5 48.5,310.5 48.5,294.5C 55.1667,294.5 61.8333,294.5 68.5,294.5C 68.8256,290.116 68.4922,285.783 67.5,281.5C 59.5866,274.592 56.7533,265.925 59,255.5C 62.8813,244.72 70.548,238.72 82,237.5C 104.247,207.195 115.413,173.195 115.5,135.5C 92.123,118.375 84.2897,95.7084 92,67.5C 105.287,36.9129 128.453,24.4129 161.5,30C 194.896,41.2769 209.063,64.4436 204,99.5C 200.388,115.436 191.722,127.769 178,136.5C 178.299,166.031 185.633,193.698 200,219.5C 204.448,226.282 209.281,232.782 214.5,239C 235.289,244.415 241.123,256.915 232,276.5C 226.213,280.998 224.047,286.998 225.5,294.5 Z";

  const scale = 0.7;

  const originalCenterX = 147;
  const originalCenterY = 186; 

  pawnGroup
    .append("path")
    .attr("class", "pawn")
    .attr("d", pawnPath)
    .attr("fill", props.pawnColor)
    .attr("stroke", "#333")
    .attr("stroke-width", 1.5 / scale)
    .attr("transform", `
      translate(${centerX}, ${centerY})
      scale(${scale})
      translate(${-originalCenterX}, ${-originalCenterY})
    `);
};

// Lifecycle hooks
onMounted(() => {
  drawPawn();
});

watch(() => props.pawnColor, () => {
  drawPawn();
});
</script>