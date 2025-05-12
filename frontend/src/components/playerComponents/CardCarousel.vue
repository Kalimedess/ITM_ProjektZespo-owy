<template>
  <div class="w-full max-w-xl mx-auto mt-10">
    <div class="relative flex items-center">
      <!-- Lewy przycisk -->
      <button
        @click="prevCard"
        class="absolute left-0 z-10 bg-primary text-white p-3 rounded-full shadow-lg hover:scale-110 transition-transform duration-200"
      >
        <-
      </button>

      <!-- Karta -->
      <div
        class="w-full flex flex-col justify-center items-center h-52 bg-green-400 text-gray-800 rounded-2xl shadow-2xl px-6 text-center transition-all duration-300"
      >
        <h2 class="text-xl font-bold mb-2">
          {{ cards[currentIndex].title }}
        </h2>
        <p class="text-sm text-gray-600">
          {{ cards[currentIndex].description }}
        </p>
      </div>

      <!-- Prawy przycisk -->
      <button
        @click="nextCard"
        class="absolute right-0 z-10 bg-primary text-white p-3 rounded-full shadow-lg hover:scale-110 transition-transform duration-200"
      >
        ->
      </button>
    </div>

    <!-- Wskaźniki -->
    <div class="flex justify-center space-x-2 mt-4">
      <span
        v-for="(card, index) in cards"
        :key="index"
        @click="goToCard(index)"
        class="w-4 h-4 rounded-full cursor-pointer transition-all duration-300"
        :class="index === currentIndex ? 'bg-primary scale-110' : 'bg-gray-300'"
      ></span>
    </div>

    <!-- Przyciski wybrania akcji -->
    <div class="flex justify-center mt-6">
      <button
      @click="sendCardSelection"
      class="bg-black text-white px-6 py-2 rounded-full shadow-md hover:bg-gray-800 transition-colors duration-200"
      >
        Wybierz kartę
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

// Lista kart (każda zawiera tytuł i opis)
const cards = ref([
  { title: 'Karta 1', description: 'Opis pierwszej karty' },
  { title: 'Karta 2', description: 'Coś ciekawego do przeczytania' },
  { title: 'Karta 3', description: 'Jeszcze inna zawartość' },
  { title: 'Karta 4', description: 'Opis czwartej karty' },
  { title: 'Karta 5', description: 'Bonusowa informacja' },
  { title: 'Karta 6', description: 'Dodatkowe szczegóły' },
  { title: 'Karta 7', description: 'Notatka końcowa' },
  { title: 'Karta 8', description: 'Zakończenie lub finał' }
])

// Aktualnie widoczna karta (indeks w tablicy)
const currentIndex = ref(0)

// Przejście do następnej karty
const nextCard = () => {
  currentIndex.value = (currentIndex.value + 1) % cards.value.length
}

// Przejście do poprzedniej karty
const prevCard = () => {
  currentIndex.value =
    (currentIndex.value - 1 + cards.value.length) % cards.value.length
}

// Przejście do wybranej karty (np. po kliknięciu wskaźnika)
const goToCard = (index) => {
  currentIndex.value = index
}

// Funkcja wysyłająca wybraną kartę do backendu
const sendCardSelection = async () => {
  // Pobierz dane bieżącej karty
  const selectedCard = cards.value[currentIndex.value]

  // Obiekt do wysłania
  const payload = {
    title: selectedCard.title,
    description: selectedCard.description,
    selectedAt: new Date().toISOString() // znacznik czasu
  }

  try {
    // Wysyłka danych do API (ścieżkę /api/wybrana-karta zmienić pod backend)
    const response = await fetch('/api/wybrana-karta', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(payload)
    })

    if (!response.ok) throw new Error('Błąd serwera: ' + response.status)

    // Odczytaj odpowiedź (jeśli backend coś odsyła)
    const data = await response.json()
    console.log('Karta wysłana:', data)

  } catch (error) {
    console.error('Błąd przy wysyłaniu karty:', error)
  }
}
</script>