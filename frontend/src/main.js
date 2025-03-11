import './assets/main.css'

import '@/assets/fonts/fonts.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'

import Toast from 'vue-toastification'
import "vue-toastification/dist/index.css";


import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';



//Konfiguracja komunikatów
const toastOptions = {
    position: 'top-right',
    timeout: 3000,
    closeOnClick: true,
    pauseOnFocusLoss: false,
    pauseOnHover: false,
    draggable: true,
    draggablePercent: 0.6,
    showCloseButtonOnHover: false,
    hideProgressBar: false,
    closeButton: 'button',
    icon: true,
    rtl: false
  }

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.use(Toast, toastOptions)

app.component('font-awesome-icon', FontAwesomeIcon);

app.mount('#app')
