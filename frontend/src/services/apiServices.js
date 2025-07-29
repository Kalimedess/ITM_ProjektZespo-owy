import axios from 'axios';
import apiConfig from './apiConfig'; // Use the same config file

// 1. Create a configured axios instance
const apiClient = axios.create({
  baseURL: apiConfig.baseURL,
  withCredentials: true, // Important for sending cookies
  headers: {
    'Content-Type': 'application/json',
  }
});

apiClient.interceptors.request.use(config => {
  // const token = localStorage.getItem('token');
  // if (token) {
  //   config.headers.Authorization = `Bearer ${token}`;
  // }
  return config;
});

// 2. Export methods that use the configured instance
export default {
  // GET request
  get(endpoint, params = {}) {
    return apiClient.get(endpoint, params );
  },

  // POST request
  post(endpoint, data) {
    return apiClient.post(endpoint, data);
  },

  // PUT request
  put(endpoint, data) {
    return apiClient.put(endpoint, data);
  },

  // DELETE request
  delete(endpoint) {
    return apiClient.delete(endpoint);
  },
  getFile(endpoint, params = {}) {
    return apiClient.get(endpoint, {
      params: params,
      responseType: 'blob' // Kluczowy element
    });
  },
    postForFile(endpoint, data = {}) {
    return apiClient.post(endpoint, data, {
      responseType: 'blob' // Kluczowy element
    });
  }
};