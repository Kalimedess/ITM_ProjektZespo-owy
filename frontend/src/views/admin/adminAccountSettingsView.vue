<template>
    <h1 class="text-center text-white">Ustawienia konta</h1>
  
    <div class="flex flex-col w-full h-full bg-secondary text-white">
      <RouterView />
  
      <div class="flex flex-col items-center gap-6 py-6">

        <form action="changeMail" method="POST" class="flex flex-col w-60 ">
          <label for="email">Zmień swój email:</label>
          <input
            type="email"
            id="email"
            name="email"
            required
            pattern="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
            title="Wpisz poprawny email"
            placeholder="Nowy email"
          />
          <input
            type="submit"
            value="Wyślij"
            class="border-2 border-accent rounded-md px-6 py-2 bg-tertiary hover:bg-accent"
          />
        </form>
  
        <form action="changePassword" method="POST" class="flex flex-col w-60">
          <label for="email">Zmień swoje hasło:</label>
          <input
            type="email"
            id="email"
            name="email"
            required
            pattern="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
            title="Wpisz poprawny email"
            placeholder="Email"
          />
          <input
            type="password"
            id="password"
            name="password"
            placeholder="Nowe hasło"
          />
          <input
            type="submit"
            value="Wyślij"
            class="border-2 border-accent rounded-md px-6 py-2 bg-tertiary hover:bg-accent"
          />
        </form>
      </div>
    </div>
  </template>
  
      
  <script setup>
    import { RouterView } from 'vue-router';
  </script>