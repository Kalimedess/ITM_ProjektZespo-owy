<template>
  <svg :viewBox="viewBox" class="w-full h-full">
    <!-- Definicja grotu strzałki -->
    <defs>
      <marker
        id="arrowhead"
        viewBox="0 0 10 10"
        refX="9"
        refY="5"
        markerWidth="6"
        markerHeight="6"
        orient="auto-start-reverse"
      >
        <!-- Grot strzałki będzie dziedziczył kolor po linii -->
        <path d="M 0 0 L 10 5 L 0 10 z" fill="currentColor"></path>
      </marker>
    </defs>

    <!-- Rysowanie krawędzi (linii) -->
    <g class="edges">
      <path
        v-for="edge in edges"
        :key="`${edge.from}-${edge.to}`"
        :d="getEdgePath(edge)"
        :stroke="getEdgeColor(edge)"
        fill="none"
        stroke-width="3"
        :style="{ color: getEdgeColor(edge) }"
        marker-end="url(#arrowhead)"
      ></path>
    </g>

    <!-- Rysowanie węzłów -->
    <g class="nodes">
      <g v-for="node in nodes" :key="node.id" :transform="`translate(${node.x}, ${node.y})`">
        <rect
          :x="-nodeWidth / 2"
          :y="-nodeHeight / 2"
          :width="nodeWidth"
          :height="nodeHeight"
          :fill="node.type === 'key' ? '#F97316' : '#C4B5FD'"
          rx="5"
          ry="5"
          stroke="#1E293B"
          stroke-width="2"
        ></rect>
        <text
          fill="white"
          font-size="20"
          font-weight="bold"
          text-anchor="middle"
          dominant-baseline="central"
        >
          {{ node.id }}
        </text>
      </g>
    </g>
  </svg>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  nodes: { type: Array, required: true },
  edges: { type: Array, required: true },
  // NOWY PROP: Przyjmujemy mapę kolorów
  edgeColors: { type: Object, default: () => ({}) },
});

const nodeWidth = 80;
const nodeHeight = 50;

// Oblicz dynamicznie viewBox na podstawie pozycji węzłów
const viewBox = computed(() => {
  if (props.nodes.length === 0) {
    return '0 0 1000 1000'; // Domyślny, gdy nie ma danych
  }
  
  const padding = 100;
  const minX = Math.min(...props.nodes.map(n => n.x)) - nodeWidth / 2 - padding;
  const minY = Math.min(...props.nodes.map(n => n.y)) - nodeHeight / 2 - padding;
  const maxX = Math.max(...props.nodes.map(n => n.x)) + nodeWidth / 2 + padding;
  const maxY = Math.max(...props.nodes.map(n => n.y)) + nodeHeight / 2 + padding;
  
  const width = maxX - minX;
  const height = maxY - minY;

  return `${minX} ${minY} ${width} ${height}`;
});

const nodesById = computed(() => {
  return props.nodes.reduce((acc, node) => {
    acc[node.id] = node;
    return acc;
  }, {});
});

// ZAKTUALIZOWANA FUNKCJA DO RYSOWANIA ŚCIEŻEK
function getEdgePath(edge) {
  const fromNode = nodesById.value[edge.from];
  const toNode = nodesById.value[edge.to];

  if (!fromNode || !toNode) return '';

  const startX = fromNode.x;
  const startY = fromNode.y;
  const endX = toNode.x;
  const endY = toNode.y;

  // Oblicz punkty kontrolne dla krzywej Béziera, aby uzyskać ładny łuk
  // To sprawi, że linia będzie wygięta w pionie
  const midY = startY + (endY - startY) / 2;
  const controlPoint1 = `${startX} ${midY}`;
  const controlPoint2 = `${endX} ${midY}`;

  // M = Move To, C = Cubic Bezier Curve
  return `M ${startX} ${startY} C ${controlPoint1}, ${controlPoint2}, ${endX} ${endY}`;
}

// NOWA FUNKCJA DO POBIERANIA KOLORU
function getEdgeColor(edge) {
  return props.edgeColors[edge.from] || '#A1A1AA'; // Zwróć kolor lub domyślny szary
}
</script>