<script setup>
import { VueElement } from 'vue';
import { ref } from 'vue';

const tickr = ref('');
const data = ref('');

function lookupStock() {
    if (tickr.value) {
        fetch(`https://localhost:7239/StockQuote?ticker=${tickr.value}`)
            .then(r => r.json())
            .then(data => console.log(data));
    }
}
</script>

<template>
    <div class="about">
        <h1>Please enter your stock tickr below</h1>
        <input v-model="tickr" placeholder="ticker" class="form-control" />
        <button class="btn btn-default" @click="lookupStock">Lookup</button>
    </div>

    <hr>

    <p>{{ tickr }}</p>
    <p>{{ data }}</p>
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