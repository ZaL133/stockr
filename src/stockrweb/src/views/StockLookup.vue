<script setup>
import { ref } from 'vue';

const tickr = ref('');
</script>

<template>
    <div class="about">
        <h1>Please enter your stock tickr below</h1>
        <input v-model="tickr" placeholder="ticker" />
    </div>

    <hr>

    <p>{{ tickr }}</p>
</template>
  
<style>
@media (min-width: 1024px) {
    .about {
        min-height: 100vh;
        display: flex;
        align-items: center;
    }
}
</style>