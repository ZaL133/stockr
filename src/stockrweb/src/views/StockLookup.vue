<script setup>
import { VueElement } from 'vue';
import { ref } from 'vue';

const tickr = ref('');
const data = ref('');

function lookupStock() {
    if (tickr.value) {
        fetch(`https://localhost:7239/StockQuote?ticker=${tickr.value}`)
            .then(r => r.json())
            .then(d => data.value = d);
    }
}
</script>

<template>
    <div class="row">
        <div class="about">
            <h1>Please enter your stock tickr below</h1>
            <input v-model="tickr" placeholder="ticker" class="form-control" />
            <button class="btn btn-default" @click="lookupStock">Lookup</button>
        </div>

        <hr />
        <div class="row">
            <p>{{ tickr }}</p>
            <div v-if="data">
                <div class="form-label">Open: </div>
                <div class="form-control">{{ data.open }}</div>
            </div>
        </div>
    </div>
</template>
  
<style></style>