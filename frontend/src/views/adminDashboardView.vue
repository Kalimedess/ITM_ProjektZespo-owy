<template>

<div class="flex flex-col min-h-screen bg-primary">
    <AdminNavbar />
    
    
    <div class="flex flex-1 mt-2">
      
      <mySidebar class="flex-shrink-0" />
      
      <!--Content-->
      <div class="flex-1 ml-4 bg-secondary  border-t-2 border-l-2 border-lgray-accent rounded-md shadow-sm text-center">
          <RouterView />
      </div>
    </div>
  </div>

</template>

<script setup>
    import AdminNavbar from '@/components/AdminNavbar.vue';
    import mySidebar from '@/components/mySidebar.vue';
    import { RouterView } from 'vue-router';
</script>

