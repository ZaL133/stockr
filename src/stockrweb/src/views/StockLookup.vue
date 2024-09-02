<script>
import { VueElement } from 'vue';
import { ref } from 'vue';
import SparklineGraph from '../components/SparklineGraph.vue';
import "ag-grid-community/styles/ag-grid.css"; // Mandatory CSS required by the Data Grid
import "ag-grid-community/styles/ag-theme-quartz.css"; // Optional Theme applied to the Data Grid
import { AgGridVue } from "ag-grid-vue3"; // Vue Data Grid Component

export default {
    name: "App",
    components: {
        AgGridVue, // Add Vue Data Grid component
        SparklineGraph
    },
    data() {
        return {
            tickr: 'MSFT',
            data: {}
        }
    },
    computed: {
        lastDay() {
            if (this.$data.data.aggregateBars && this.$data.data.aggregateBars.resultsCount) {
                return this.$data.data.aggregateBars.results[this.$data.data.aggregateBars.resultsCount - 1];
            }
        },
        dividends() {
            if (this.$data.data.dividends && this.$data.data.dividends.count) {
                return Array.from(this.$data.data.dividends.results.map(x => x.amount));
            }
            else return [];
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
            <div v-if="lastDay">
                <div>
                    <b>Date: </b>{{ (new Date(lastDay.time).toLocaleDateString()) }}
                </div>
                <div>
                    Open: {{ lastDay.open }}
                    / High: <span class="green"> {{ lastDay.high }}</span>
                    / Low: <span class="red">{{ lastDay.low }}</span>
                </div>
                <div>
                    Close: <span :class="(lastDay.close > lastDay.open) ? 'green' : 'red'">{{ lastDay.close }}</span>
                </div>

                <SparklineGraph :data="dividends"></SparklineGraph>
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