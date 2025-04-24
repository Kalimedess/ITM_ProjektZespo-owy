<template>
        <div v-if="props.isVisible" class="fixed inset-0 flex items-center justify-center z-50">
            <div class="absolute inset-0  bg-black/70" @click="closeModal"></div>

            <div class="flex flex-col justify-start items-center bg-primary text-white rounded-lg w-96 relative z-10 border-2 border-accent p-12 animate-jump-in">


                <button 
                    @click="closeModal" 
                    class="absolute top-2 right-2 w-8 h-8 flex items-center justify-center"
                >
                    <font-awesome-icon :icon="faXmark" class="h-5 text-white  hover:text-accent transition-all duration-100" />
                </button>
                
                <div class=" animate-fade">
                    <h2 class="text-2xl font-bold mb-8 text-center">Wprowadź kod i dołącz do gry 🎮</h2>
                    <form>
                        <div class="mb-8">
                            <input 
                            type="text" 
                            id="login" 
                            v-model="code" 
                            placeholder="Wprowadz kod..."
                            class="w-full px-3 py-3 bg-tertiary border border-gray-700 rounded-md text-white focus:outline-none focus:border-accent"
                            required
                            />
                        </div>

                        <button 
                            type="submit" 
                            class="bg-tertiary hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60 mb-5"
                        >
                            Dołącz do gry
                        </button>
                    </form>
                </div>
            </div>
        </div>
</template>

<script setup>

    import { defineProps,ref,defineEmits } from 'vue';
    import {faXmark} from '@fortawesome/free-solid-svg-icons'

    const props = defineProps({

        isVisible: {
            type: Boolean,
            default: false, 
        }
    });

    const code = ref('');

    const emit = defineEmits(['close']);

    const closeModal = () => {

        emit('close');

        code.value = '';

    };

</script>