<script>
import { VueElement } from 'vue';
import { ref } from 'vue';
import "ag-grid-community/styles/ag-grid.css"; // Mandatory CSS required by the Data Grid
import "ag-grid-community/styles/ag-theme-quartz.css"; // Optional Theme applied to the Data Grid
import { AgGridVue } from "ag-grid-vue3"; // Vue Data Grid Component

export default {
    name: "App",
    components: {
        AgGridVue, // Add Vue Data Grid component
    },
    data() {
        return {
            tickr: 'MSFT',
            data: {}
        }
    },
    setup() { },
    methods: {
        lookupStock() {
            let tickerValue = this.$data.tickr;
            if (tickerValue) {
                fetch(`https://localhost:7239/StockQuote/GetStockDetails?ticker=${tickerValue}`)
                    .then(r => r.json())
                    .then(d => {
                        this.$data.data = d;
                    });
            }
        }
    }
};
</script>

<template>
    <div class="row">
        <div class="about">
            <h1>Please enter your stock tickr below</h1>
            <input v-model="tickr" placeholder="ticker" class="form-control" />
            <button class="btn" @click="lookupStock">Lookup</button>
        </div>

        <hr />
        <div class="row">
            <p>{{ tickr }}</p>
            <div v-if="data">
                <div>
                    Open: {{ data.open }}
                    / High: <span class="green"> {{ data.high }}</span>
                    / Low: <span class="red">{{ data.low }}</span>
                </div>
                <div>
                    Close: <span :class="(data.close > data.open) ? 'green' : 'red'">{{ data.close }}</span>
                </div>
            </div>

            <pre>
                {{ data }}
            </pre>
        </div>
    </div>
</template>
  
<style scoped>
.green {
    color: green;
}

.red {
    color: red;
}
</style>