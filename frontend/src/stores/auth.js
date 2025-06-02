import { defineStore } from 'pinia';
import axios from 'axios';

export const useAuthStore = defineStore('auth', {
  state: () => ({
    isAuthenticated: false,
  }),
  actions: {
    async checkAuth() {
      try {
        await axios.get('http://localhost:5023/api/auth/me', { withCredentials: true });
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
