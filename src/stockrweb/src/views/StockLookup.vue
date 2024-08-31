<script setup>
import { VueElement } from 'vue';
import { ref } from 'vue';

const tickr = ref('');
</script>

<template>
    <form @submit="getQuote">
        <div class="about">
            <h1>Please enter your stock tickr below</h1>
            <input v-model="tickr" placeholder="ticker" class="form-control" />
        </div>

        <hr>

        <p>{{ tickr }}</p>
    </form>
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