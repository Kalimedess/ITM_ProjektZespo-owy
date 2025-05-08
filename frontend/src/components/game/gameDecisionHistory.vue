<template>
  <div>
    <h2>Zarządzaj historią decyzji</h2>
    <button @click="loadData">Ładowanie logów</button>

    <div style="margin-top: 20px;">
      <h3>Dodaj Decyzję</h3>
      <form @submit.prevent="createGamelog">
        <input v-model="newGamelog.TeamId" placeholder="TeamId" type="number" required />
        <input v-model="newGamelog.GameId" placeholder="GameId" type="number" required />
        <input v-model="newGamelog.CardId" placeholder="CardId" type="number" required />
        <input v-model="newGamelog.DeckId" placeholder="DeckId" type="number" />
        <input v-model="newGamelog.Date" placeholder="Date (YYYY-MM-DD)" type="text" />
        <input v-model="newGamelog.FeedbackId" placeholder="FeedbackId" type="number" />
        <input v-model="newGamelog.Cost" placeholder="Cost" type="number" />
        <input v-model="newGamelog.Status" placeholder="Status" type="text" />
        <button type="submit">Create</button>
      </form>
    </div>

    <div style="margin-top: 20px;">
      <h3>Usuń Decyzję</h3>
      <form @submit.prevent="deleteGamelog">
        <input v-model="deleteIds.TeamId" placeholder="TeamId" type="number" required />
        <input v-model="deleteIds.GameId" placeholder="GameId" type="number" required />
        <input v-model="deleteIds.CardId" placeholder="CardId" type="number" required />
        <button type="submit">Usuń</button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const emit = defineEmits(['data-loaded']);

const gamelogs = ref([
  { TeamId: 1, GameId: 100, CardId: 501, DeckId: 201, Date: '2025-04-01', FeedbackId: 301, Cost: 10, Status: 'Active' },
  { TeamId: 2, GameId: 101, CardId: 502, DeckId: 202, Date: '2025-04-02', FeedbackId: 302, Cost: 15, Status: 'Inactive' },
  { TeamId: 3, GameId: 102, CardId: 503, DeckId: 203, Date: '2025-04-03', FeedbackId: 303, Cost: 20, Status: 'Active' },
]);

const newGamelog = ref({
  TeamId: '',
  GameId: '',
  CardId: '',
  DeckId: '',
  Date: '',
  FeedbackId: '',
  Cost: '',
  Status: '',
});

const deleteIds = ref({
  TeamId: '',
  GameId: '',
  CardId: '',
});

function loadData() {
  const selectedData = gamelogs.value.map(({ CardId, TeamId }) => ({ CardId, TeamId }));
  emit('data-loaded', { fullData: gamelogs.value, selectedData });
}

function createGamelog() {
  gamelogs.value.push({ ...newGamelog.value });
  loadData(); 
  newGamelog.value = { TeamId: '', GameId: '', CardId: '', DeckId: '', Date: '', FeedbackId: '', Cost: '', Status: '' };
}

function deleteGamelog() {
  gamelogs.value = gamelogs.value.filter(log => 
    !(log.TeamId == deleteIds.value.TeamId && log.GameId == deleteIds.value.GameId && log.CardId == deleteIds.value.CardId)
  );
  loadData(); 
  deleteIds.value = { TeamId: '', GameId: '', CardId: '' };
}
</script>

<style scoped>
form {
  display: flex;
  flex-direction: column;
  gap: 8px;
  margin-bottom: 10px;
}
input {
  padding: 5px;
}
button {
  width: fit-content;
  padding: 5px 10px;
}
</style>

  