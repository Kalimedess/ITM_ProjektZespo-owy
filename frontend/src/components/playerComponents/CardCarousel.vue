<template>
  <div class="w-full max-w-xl mx-auto">
    <div class="relative flex items-center">
      <!-- Lewy przycisk -->
      <button
        @click="prevCard"
        class="absolute left-0 z-10 bg-primary text-white p-2 rounded-full shadow"
      >
        ◀
      </button>

      <!-- Karta -->
      <div class="w-full flex justify-center items-center h-48 bg-lgray-accent rounded-xl shadow text-center transition-all duration-300">
        <div class="text-xl font-semibold">
          {{ cards[currentIndex] }}
        </div>
      </div>

      <!-- Prawy przycisk -->
      <button
        @click="nextCard"
        class="absolute right-0 z-10 bg-primary text-white p-2 rounded-full shadow"
      >
        ▶
      </button>
    </div>

    <!-- Wskaźniki -->
    <div class="flex justify-center space-x-2 mt-4">
      <span
        v-for="(card, index) in cards"
        :key="index"
        class="w-3 h-3 rounded-full"
        :class="index === currentIndex ? 'bg-primary' : 'bg-gray-400'"
      ></span>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

const cards = ref([
  'Karta 1: Opis lub zawartość',
  'Karta 2: Coś ciekawego',
  'Karta 3: Jeszcze coś',
])

const currentIndex = ref(0)

const nextCard = () => {
  currentIndex.value = (currentIndex.value + 1) % cards.value.length
}

const prevCard = () => {
  currentIndex.value =
    (currentIndex.value - 1 + cards.value.length) % cards.value.length
}
</script>