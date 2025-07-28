<template>
  <div class="border-2 border-dashed border-gray-600 p-4 rounded-md text-left text-sm">
    <h3 class="text-lg font-bold mb-3">Legenda</h3>
    <div class="space-y-3">
      <!-- Kluczowe decyzje -->
      <div class="flex items-center gap-3">
        <div class="w-8 h-5 rounded-md bg-orange-600 flex-shrink-0"></div>
        <span>Kluczowe decyzje wymagane do przejścia do następnej fazy</span>
      </div>
      <!-- Zwykłe decyzje -->
      <div class="flex items-center gap-3">
        <div class="w-8 h-5 rounded-md bg-indigo-400 flex-shrink-0"></div>
        <span>Zwykłe decyzje</span>
      </div>
      <!-- Połączenie -->
      <div class="flex items-center gap-3">
        <div class="w-8 h-5 rounded-md bg-gray-800 relative flex items-center flex-shrink-0">
          <span class="text-white font-bold absolute left-1">X</span>
          <svg class="w-4 h-4 text-gray-400 absolute left-1/2 -translate-x-1/3" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M10.293 3.293a1 1 0 011.414 0l6 6a1 1 0 010 1.414l-6 6a1 1 0 01-1.414-1.414L14.586 11H3a1 1 0 110-2h11.586l-4.293-4.293a1 1 0 010-1.414z" clip-rule="evenodd" />
          </svg>
           <span class="text-white font-bold absolute right-1">Y</span>
        </div>
        <span>Podjęcie decyzji X jest wymagane aby móc podjąć decyzję Y</span>
      </div>
    </div>
  </div>
</template>