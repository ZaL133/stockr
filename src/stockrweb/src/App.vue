<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <nav class="nav">
      <RouterLink to="/">Home</RouterLink>
      <RouterLink to="/about">About</RouterLink>
      <RouterLink to="/stock">Stock</RouterLink>
    </nav>
  </header>

  <RouterView />
</template>

<style scoped></style>
