import { defineStore } from 'pinia';
import apiServices from '@/services/apiServices';
import apiConfig from '@/services/apiConfig';

export const useAuthStore = defineStore('auth', {
  state: () => ({
    isAuthenticated: false,
  }),
  actions: {
    async checkAuth() {
      try {
        await apiServices.get(apiConfig.auth.me)
        this.isAuthenticated = true;
      } catch {
        this.isAuthenticated = false;
      }
    },
    setAuthenticated(value) {
      this.isAuthenticated = value;
    },
  },
});
