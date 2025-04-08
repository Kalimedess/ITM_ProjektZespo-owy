<template>
    <footer>
        <div class="text-center text-white w-full bg-secondary text-xs py-2 shadow-[0_-2px_4px_theme(colors.accent/0.5)]">
            Copyright
            <font-awesome-icon :icon="faCopyright" class="h-3 text-white" /> 
            {{ new Date().getFullYear() }}
            ITM Software House. All rights reserved.
        </div>
    </footer>
</template>


<script setup>
     import { faCopyright } from '@fortawesome/free-solid-svg-icons'
     

</script>