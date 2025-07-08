<template>
    <div class="w-full text-white">
        <div class="m-4 px-2 py-2 border-2 border-lgray-accent rounded-md bg-tertiary">
            <h1 class="text-center text-white font-nasalization text-3xl mt-2 mb-4">Stoły</h1>
            <hr class ="my-4 border-lgray-accent"/>
            <div class="grid grid-cols-1 xl:grid-cols-3 gap-4 mt-6">
                <tableCard
                v-for="table in tables"
                :key="table.id"
                :table="table"
                :color="getGameColor(table.id)"
                />
            </div>
        </div>
    </div>

</template>


<script setup>
    import tableCard from '@/components/game/tableCard.vue'

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

    const tables = [
  {
    id: 1,
    name: "Stół 1",
    bits: 400,
  },
  {
    id: 2,
    name: "Stół 2",
    bits: 200,
    round: 3,
  },
  {
    id: 3,
    name: "Stół 3",
    bits: 100,
  },
  {
    id: 4,
    name: "Stół 4",
    bits: 500,
  },
  {
    id: 5,
    name: "Stół 5",
  },
];



</script>