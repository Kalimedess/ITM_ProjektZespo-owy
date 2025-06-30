<template>
<div v-if="props.isVisible" class="fixed inset-0 flex items-center justify-center z-50">
    <div class="absolute inset-0 bg-black/70" @click="closeModal"></div>
    <div class="bg-primary text-white rounded-lg w-[90%] max-w-5xl relative z-10 border-2 border-accent p-8 animate-jump-in">
        <button 
            @click="closeModal" 
            class="absolute top-2 right-2 w-8 h-8 flex items-center justify-center"
        >
            <font-awesome-icon :icon="faXmark" class="h-5 text-white hover:text-accent transition-all duration-100" />
        </button>
    <div class="grid grid-cols-[0.5fr_1px_1.5fr] gap-6">

    <!-- Lewy panel (menu) -->
    <aside class="border-2 border-lgray-accent rounded-md p-3">
        <ul class="space-y-2 flex flex-col">
            <li class="w-full flex items-center hover:text-accent ml-2"
            :class="{ 'text-accent font-bold': activeView === 'general' }"
            >
                <font-awesome-icon :icon="faGear" class="h-3 mr-2"/>
                <button @click="activeView = 'general' " class="text-left w-full">
                    Ustawienia konta
                </button>
            </li>
            <hr class="border-lgray-accent w-full mx-auto" />
            <li class="w-full flex items-center hover:text-accent ml-2"
            :class="{ 'text-accent font-bold': activeView === 'changePassword' }"
            >
                <font-awesome-icon :icon="faLock" class="h-3 mr-2"/>
                <button @click="activeView = 'changePassword' " class="text-left w-full">
                    Zmień hasło
                </button>
            </li>
            <hr class="border-lgray-accent w-full mx-auto" />
            <li class="w-full">
            </li>
            <li class="w-full flex items-center hover:text-accent ml-2"
            :class="{ 'text-accent font-bold': activeView === 'licenses' }"
            >
                <font-awesome-icon :icon="faIdCard" class="h-3 mr-2"/>
                <button @click="activeView = 'licenses' " class="text-left w-full">
                    Licencje
                </button>
            </li>
            <hr class="border-lgray-accent w-full mx-auto" />
        </ul>
    </aside>


    <div class="bg-lgray-accent"></div>
    <!-- Prawy panel (szczegóły konta) -->
    <section v-if="activeView === 'general'">
      <basicAdminSettings 
        :username="props.username" 
        :email="props.email"
      />
    </section>

    <section v-if="activeView === 'changePassword'">
        <changePassword />
    </section>

    <section v-if="activeView === 'licenses'">
        <userLicenses />
    </section>

  </div>
</div>
    

       
  </div>
</template>


<script setup>
    import { faXmark,faGear,faLock,faIdCard } from '@fortawesome/free-solid-svg-icons';
    import { defineProps,defineEmits,ref } from 'vue';
    import changePassword from '../auth/changePassword.vue';
    import userLicenses from './userLicenses.vue';
    import basicAdminSettings from './basicAdminSettings.vue';

    const activeView = ref('general');

    const props = defineProps({
        isVisible: {
            type: Boolean,
            default: false
        },
        username: {
            type: String,
            default: ''
        },
        email: {
            type: String,
            default: ''
        },
    });

    const emit = defineEmits(['close']);

    const closeModal = () => {
        activeView.value = 'general';
        emit('close');
    };

</script>