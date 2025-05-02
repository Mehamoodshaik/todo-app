<template>
    <div class="home-container">
      <h1>Select a Provider</h1>
      <div class="card-container">
        <div class="card" @click="selectProvider('api')">
          <img src="https://img.icons8.com/ios-filled/100/database.png" alt="API Icon" />
          <h2>API Provider</h2>
          <p>Connect to backend API server.</p>
        </div>
  
        <div class="card" @click="selectProvider('local')">
          <img src="https://img.icons8.com/ios-filled/100/hdd.png" alt="Local Storage Icon" />
          <h2>Local Storage Provider</h2>
          <p>Save todos in your browser locally.</p>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { useRouter } from 'vue-router'
  
  const router = useRouter()
  
  const selectProvider = (provider) => {
    localStorage.setItem('provider', provider) // Save selected provider
    router.push('/todos') // Go to Todo List
  }
  </script>
  
  <style scoped>
  .home-container {
    text-align: center;
    padding: 2rem;
  }
  
  .card-container {
    display: flex;
    justify-content: center;
    gap: 2rem;
    margin-top: 2rem;
    flex-wrap: wrap;
  }
  
  .card {
    background: linear-gradient(to right, #eef2f3, #8e9eab);
    padding: 2rem;
    border-radius: 12px;
    width: 250px;
    cursor: pointer;
    box-shadow: 0 4px 10px rgba(0,0,0,0.1);
    transition: all 0.3s ease;
    text-align: center;
  }
  
  .card:hover {
    transform: translateY(-5px);
    box-shadow: 0 8px 20px rgba(0,0,0,0.2);
  }
  
  .card img {
    width: 80px;
    height: 80px;
    margin-bottom: 1rem;
  }
  </style>
  