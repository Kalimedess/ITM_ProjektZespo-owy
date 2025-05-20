<template>
  <div class="w-full max-w-xl mx-auto mt-10">
    <div class="relative">
      
      <div
        @mousedown="startHold"
        @mouseup="cancelHold"
        @mouseleave="cancelHold"
        :class="[
          'w-full h-64 bg-gradient-to-br from-lime-400 via-lime-500 to-lime-600 rounded-2xl shadow-2xl text-white transition-transform duration-200 ease-in-out',
          cardClicked ? 'scale-105' : 'scale-100'
        ]"
      >
        <div class="flex h-full">
          <!-- Lewy przycisk -->
          <button
            @click.stop="prevCard"
            class="w-12 flex items-center justify-center hover:bg-black/20 rounded-l-2xl transition"
            aria-label="Poprzednia karta"
          >
            <p><</p>
          </button>

          <!-- Środek karty -->
          <div class="flex-1 flex flex-col items-center justify-center px-6 text-center">
            <h2 class="text-2xl font-bold mb-2">
              {{ cards[currentIndex].title }}
            </h2>
            <p class="text-base text-white/90 italic">
              {{ cards[currentIndex].description }}
            </p>
          </div>

          <!-- Prawy przycisk -->
          <button
            @click.stop="nextCard"
            class="w-12 flex items-center justify-center hover:bg-black/20 rounded-r-2xl transition"
            aria-label="Następna karta"
          >
            <p>></p>
          </button>
        </div>
      </div>

      <!-- Wskaźniki (zawijane) -->
      <div class="flex flex-wrap justify-center gap-2 mt-6">
        <button
          v-for="(card, index) in cards"
          :key="index"
          @click="goToCard(index)"
          class="w-8 h-8 text-sm font-medium rounded-full flex items-center justify-center transition-all duration-300 border-2"
          :class="index === currentIndex
            ? 'bg-primary text-white border-white scale-110'
            : 'bg-gray-200 text-gray-700 border-gray-400 hover:bg-gray-300'"
        >
          {{ index + 1 }}
        </button>
      </div>

      <!-- Przyciski akcji -->
      <div class="flex justify-center mt-6">
        <button
          @click="sendCardSelection"
          class="bg-black text-white px-6 py-2 rounded-full shadow-md hover:bg-gray-800 transition-colors duration-200"
        >
          Wybierz kartę
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

// Lista kart (każda zawiera tytuł i opis)
const cards = ref([
  { title: 'Karta 1', description: 'Opis pierwszej karty' },
  { title: 'Karta 2', description: 'Opis drugiej karty' },
  { title: 'Karta 3', description: 'Opis trzeciej karty' },
  { title: 'Karta 4', description: 'Opis czwartej karty' },
  { title: 'Karta 5', description: 'Opis piątej karty' },
  { title: 'Karta 6', description: 'Opis szóstej karty' },
  { title: 'Karta 7', description: 'Opis siódmej karty' },
  { title: 'Karta 8', description: 'Opis ósmej karty' },
  { title: 'Karta 9', description: 'Opis dziewiątej karty' },
  { title: 'Karta 10', description: 'Opis dziesiątej karty' },
  { title: 'Karta 11', description: 'Opis jedenastej karty' },
  { title: 'Karta 12', description: 'Opis dwunastej karty' },
  { title: 'Karta 13', description: 'Opis trzynastej karty' },
  { title: 'Karta 14', description: 'Opis czternastej karty' },
  { title: 'Karta 15', description: 'Opis piętnastej karty' },
  { title: 'Karta 16', description: 'Opis szesnastej karty' },
  { title: 'Karta 17', description: 'Opis siedemnastej karty' },
  { title: 'Karta 18', description: 'Opis osiemnastej karty' },
  { title: 'Karta 19', description: 'Opis dziewiętnastej karty' },
  { title: 'Karta 20', description: 'Opis dwudziestej karty' },
  { title: 'Karta 21', description: 'Opis dwudziestej pierwszej karty' },
  { title: 'Karta 22', description: 'Opis dwudziestej drugiej karty' },
  { title: 'Karta 23', description: 'Opis dwudziestej trzeciej karty' },
  { title: 'Karta 24', description: 'Opis dwudziestej czwartej karty' },
  { title: 'Karta 25', description: 'Opis dwudziestej piątej karty' },
  { title: 'Karta 26', description: 'Opis dwudziestej szóstej karty' },
  { title: 'Karta 27', description: 'Opis dwudziestej siódmej karty' },
  { title: 'Karta 28', description: 'Opis dwudziestej ósmej karty' },
  { title: 'Karta 29', description: 'Opis dwudziestej dziewiątej karty' },
  { title: 'Karta 30', description: 'Opis trzydziestej karty' },
  { title: 'Karta 31', description: 'Opis trzydziestej pierwszej karty' },
  { title: 'Karta 32', description: 'Opis trzydziestej drugiej karty' },
  { title: 'Karta 33', description: 'Opis trzydziestej trzeciej karty' },
  { title: 'Karta 34', description: 'Opis trzydziestej czwartej karty' },
  { title: 'Karta 35', description: 'Opis trzydziestej piątej karty' },
  { title: 'Karta 36', description: 'Opis trzydziestej szóstej karty' },
  { title: 'Karta 37', description: 'Opis trzydziestej siódmej karty' },
  { title: 'Karta 38', description: 'Opis trzydziestej ósmej karty' },
  { title: 'Karta 39', description: 'Opis trzydziestej dziewiątej karty' },
  { title: 'Karta 40', description: 'Opis czterdziestej karty' },
  { title: 'Karta 41', description: 'Opis czterdziestej pierwszej karty' },
  { title: 'Karta 42', description: 'Opis czterdziestej drugiej karty' },
  { title: 'Karta 43', description: 'Opis czterdziestej trzeciej karty' },
  { title: 'Karta 44', description: 'Opis czterdziestej czwartej karty' },
  { title: 'Karta 45', description: 'Opis czterdziestej piątej karty' },
  { title: 'Karta 46', description: 'Opis czterdziestej szóstej karty' },
  { title: 'Karta 47', description: 'Opis czterdziestej siódmej karty' },
  { title: 'Karta 48', description: 'Opis czterdziestej ósmej karty' },
  { title: 'Karta 49', description: 'Opis czterdziestej dziewiątej karty' },
  { title: 'Karta 50', description: 'Opis pięćdziesiątej karty' }
])

const currentIndex = ref(0)

const cardClicked = ref(false)
let holdTimeout = null

const startHold = () => {
  holdTimeout = setTimeout(() => {
    cardClicked.value = true
  }, 500) // ustawianie czasu powiększania kart 0,5 sekundy
}

const cancelHold = () => {
  clearTimeout(holdTimeout)
  cardClicked.value = false
}

const nextCard = () => {
  currentIndex.value = (currentIndex.value + 1) % cards.value.length
}

const prevCard = () => {
  currentIndex.value = (currentIndex.value - 1 + cards.value.length) % cards.value.length
}

const goToCard = (index) => {
  currentIndex.value = index
}

</script>