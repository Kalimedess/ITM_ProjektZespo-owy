<template>
    <div class="w-full text-white">
        <div class="m-4 px-2 py-2 border-2 border-lgray-accent rounded-md bg-tertiary">
            <h1 class="text-center text-white font-nasalization text-3xl mt-2 mb-4">Gry w sesji</h1>
            <homeAdminButtons 
                @open-create-game="showCreateGame = true"/>
            <hr class ="my-4 border-lgray-accent"/>
            <div class="grid grid-cols-1 xl:grid-cols-3 gap-4 mt-6">
                <gameCard
                    v-for="game in games"
                    :key="game.id"
                    :game="game"
                    :color="getGameColor(game.id)"
                />
            </div>
        </div>
        <createGame
            :isVisible="showCreateGame"
            @close="showCreateGame = false"
            />
    </div>

</template>

<script setup>
    import gameCard from '@/components/game/gameCard.vue'
    import createGame from '@/components/game/createGame.vue';
    import homeAdminButtons from '@/components/admin/homeAdminButtons.vue';
    import CreateGame from '@/components/game/createGame.vue';
    import { ref} from 'vue';

    const showCreateGame = ref(false);

    const getGameColor = (gameId) => {
        
        const colors = [
            '#E53E3E',
            '#3182CE',
            '#38A169',
            '#D69E2E',
            '#805AD5',
            '#D53F8C',
            '#DD6B20',
            '#319795',
            '#5A67D8',
        ];

        return colors[gameId - 1 % 9];
    }

    const games = [
  {
    id: 1,
    name: "Gra 1",
    bits: 400,
    round: 5,
  },
  {
    id: 2,
    name: "Gra 2",
    bits: 200,
    round: 3,
  },
  {
    id: 3,
    name: "Gra 3",
    bits: 100,
    round: 1,
  },
  {
    id: 4,
    name: "Gra 4",
    bits: 300,
    round: 2,
  },
  {
    id: 5,
    name: "Gra 5",
    bits: 500,
    round: 4,
  },
  {
    id: 6,
    name: "Gra 6",
    bits: 7800,
    round: 42,
  },
];



</script>