import axios from 'axios';

const apiClient = axios.create({
  baseURL: `http://localhost:5023`,
  withCredentials: true
});

apiClient.interceptors.response.use(
  response => response,
  error => {
    if (error.response) {
      console.error('API Error:', error.response.data);
      if (error.response.status === 401) {
        console.error('Unauthorized, redirecting to login might be needed.');
      }
    } else if (error.request) {
      console.error('Network Error:', error.request);
    } else {
      console.error('Axios Error:', error.message);
    }
    return Promise.reject(error);
  }
);

export default apiClient;