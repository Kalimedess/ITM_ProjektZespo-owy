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

    const games = [{
        id:1,
        name: "Gra 1",
        color: "red",
        bits:400,
        round:5,
    },
    {
        id:2,
        name: "Gra 2",
        color: "blue",
        bits:200,
        round:3,
    },
    {
        id:3,
        name: "Gra 3",
        color: "green",
        bits:100,
        round:1,
    },
    {
        id:4,
        name: "Gra 4",
        color: "yellow",
        bits:300,
        round:2,
    },
    {
        id:5,
        name: "Gra 5",
        color: "purple",
        bits:500,
        round:4,
    }];



</script>