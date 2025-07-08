<template>
    <div class="flex items-center justify-center mb-6">
        <h2 class="text-2xl font-nasalization">Ustawienia konta</h2>
      </div>

      <div class="flex justify-center mb-6">
        <font-awesome-icon :icon="faCircleUser" class="h-20" />
      </div>

      <hr class="border-lgray-accent mb-4" />

      <form @submit.prevent="handleChangeAccountSettings">
        <div class="mb-4">
            <label for="username" class="font-bold ml-2 mb-2">Nazwa</label>
            <input 
                id="username"
                v-model="usernameEdit"
                type="text"
                class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 text-white w-full"
                />
        </div>

        <hr class="border-lgray-accent mb-4" />

        <div class="mb-4">
            <label for="email" class="font-bold ml-2 mb-2">E-mail</label>
            <input 
                    id="email"
                    v-model="emailEdit"
                    type="email"
                    class="bg-tertiary border-2 border-lgray-accent rounded-md px-3 py-2 text-white w-full"
                />
        </div>

        <hr class="border-lgray-accent mb-4" />

        <button 
            type="submit" 
            class="bg-tertiary hover:bg-accent text-white w-full py-4 rounded-lg font-medium transition-all duration-300 shadow-sm hover:shadow-lg shadow-accent/40 hover:shadow-accent/60 mb-5"
            >
            Zapisz zmiany
        </button>

      </form>
</template>

<script setup>
    import { faCircleUser } from '@fortawesome/free-solid-svg-icons';
    import { ref, watch,defineProps } from 'vue';

    const props = defineProps({
        username: {
            type: String,
            default: ''
        },
        email: {
            type: String,
            default: ''
        },
    });

    const usernameEdit = ref(props.username);
    const emailEdit = ref(props.email);

    watch(() => props.username, val => usernameEdit.value = val);
    watch(() => props.email, val => emailEdit.value = val);

    const handleChangeAccountSettings = async () => {
        // Tu będzie logika do zmiany ustawień konta
    };
</script>