<template>
    <div>
      <h1>Widok Historii Decyzji</h1>
      <GameLogsLoader @data-loaded="handleDataLoaded" />
  
      <div v-if="fullData.length">
        <h2>Gamelogi</h2>
        <table>
          <thead>
            <tr>
              <th>TeamId</th>
              <th>GameId</th>
              <th>CardId</th>
              <th>DeckId</th>
              <th>Date</th>
              <th>FeedbackId</th>
              <th>Cost</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="log in fullData" :key="`${log.TeamId}-${log.GameId}`">
              <td>{{ log.TeamId }}</td>
              <td>{{ log.GameId }}</td>
              <td>{{ log.CardId }}</td>
              <td>{{ log.DeckId }}</td>
              <td>{{ log.Date }}</td>
              <td>{{ log.FeedbackId }}</td>
              <td>{{ log.Cost }}</td>
              <td>{{ log.Status }}</td>
            </tr>
          </tbody>
        </table>
      </div>
  
      <div v-if="selectedData.length">
        <h2>Podjęte decyzje</h2>
        <ul>
          <li v-for="item in selectedData" :key="`${item.TeamId}-${item.CardId}`">
            CardId: {{ item.CardId }}, TeamId: {{ item.TeamId }}
          </li>
        </ul>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  import GameLogsLoader from '@/components/GameLogsLoader.vue';
  
  const fullData = ref([]);
  const selectedData = ref([]);
  
  function handleDataLoaded(payload) {
    fullData.value = payload.fullData;
    selectedData.value = payload.selectedData;
  }
  </script>
  