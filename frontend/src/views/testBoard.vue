<template>
  <!-- Użyj klas podobnych do tych z admina -->
    <div class="relative flex-grow overflow-hidden w-[800px] h-[600px] mx-auto bg-gray-800 rounded-lg border border-gray-600">
        <gameBoard
        :config="formData"
        :gameMode="true"
        :posX="posX"
        :posY="posY"
        :pawnColor="'#0000ff'"
        />
    </div>

    <div class="flex flex-col items-center mt-4 gap-2">
    <div class="flex justify-center">
        <button @click="moveUp" class="w-12 h-12 bg-blue-600 hover:bg-blue-700 text-white rounded-lg flex items-center justify-center">
        <font-awesome-icon :icon="faArrowUp" size="lg" />
        </button>
    </div>
    
    <div class="flex justify-center gap-2">
        <button @click="moveLeft" class="w-12 h-12 bg-blue-600 hover:bg-blue-700 text-white rounded-lg flex items-center justify-center">
        <font-awesome-icon :icon="faArrowLeft" size="lg" />
        </button>
        
        <button @click="moveDown" class="w-12 h-12 bg-blue-600 hover:bg-blue-700 text-white rounded-lg flex items-center justify-center">
        <font-awesome-icon :icon="faArrowDown" size="lg" />
        </button>
        
        <button @click="moveRight" class="w-12 h-12 bg-blue-600 hover:bg-blue-700 text-white rounded-lg flex items-center justify-center">
        <font-awesome-icon :icon="faArrowRight" size="lg" />
        </button>
    </div>
</div>
</template>

<script setup>
    import gameBoard from '@/components/game/gameBoard.vue';
    import { reactive, ref } from 'vue';
    import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
    import { faArrowRight, faArrowLeft, faArrowUp, faArrowDown } from '@fortawesome/free-solid-svg-icons';

    const posX = ref(0);
    const posY = ref(0);

    const formData = reactive({
        Name: 'Plansza podstawowa', 
        LabelsUp: ['Podstawowa kordynacja', 'Standaryzacja procesów', 'Zintegrowane działania', 'Pełna integracja strategiczna'], 
        LabelsRight: ['Nowicjusz', 'Naśladowca', 'Innowator', 'Lider cyfrowy'], 
        DescriptionDown: 'Poziom integracji wew/zew', 
        DescriptionLeft: 'Zawansowanie Cyfrowe', 
        Rows: 8,
        Cols: 8,
        CellColor: '#fefae0', 
        BorderColor: '#595959', 
        BorderColors: ['#008000', '#FFFF00', '#FFA500', '#FF0000']
    });

    // Uproszczone funkcje ruchu bez sprawdzania granic
    const moveLeft = () => {
        posX.value--;
    };

    const moveRight = () => {
        posX.value++;
    };

    const moveUp = () => {
        posY.value--;
    };

    const moveDown = () => {
        posY.value++;
    };
</script>