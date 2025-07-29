<template>
    <div v-if="show" 
         class="absolute inset-0 bg-black/60 flex items-center justify-center rounded-md">
        <div class="bg-tertiary rounded-lg p-8 flex flex-col items-center border-2 border-lgray-accent">
            <!-- spinner -->
            <div class="relative">
                <div class="w-16 h-16 border-4 border-accent/30 rounded-full animate-spin border-t-accent"></div>
            </div>
            <p class="text-white text-sm text-center mt-4">{{ message }}</p>
        </div>
    </div>
</template>

<script setup>
defineProps({
    show: {
        type: Boolean,
        default: false
    },
    message: {
        type: String,
        default: 'Proszę czekać'
    }
})
</script>