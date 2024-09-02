<script>
import { VueElement } from 'vue';
import { ref } from 'vue';
import SparklineGraph from '../components/SparklineGraph.vue';
import "ag-grid-community/styles/ag-grid.css"; // Mandatory CSS required by the Data Grid
import "ag-grid-community/styles/ag-theme-quartz.css"; // Optional Theme applied to the Data Grid
import { AgGridVue } from "ag-grid-vue3"; // Vue Data Grid Component

const localStoragePortfolioKey = "stockr-portfolio";

export default {
    name: "App",
    components: {
        AgGridVue, // Add Vue Data Grid component
        SparklineGraph
    },
    data() {
        let portfolio = JSON.parse(localStorage.getItem(localStoragePortfolioKey) ?? "[]");
        return {
            tickr: 'MSFT',
            data: {},
            portfolio
        }
    },
    computed: {
        lastDay() {
            if (this.$data.data.dailyBars && this.$data.data.dailyBars.resultsCount) {
                return this.$data.data.dailyBars.results[this.$data.data.dailyBars.resultsCount - 1];
            }
        },
        dividends() {
            if (this.$data.data.dividends && this.$data.data.dividends.count) {
                let amountArray = Array.from(this.$data.data.dividends.results.map(x => x.amount));
                return amountArray.reverse();
            }
            else return [];
        },
        monthly() {
            if (this.$data.data.monthlyBars && this.$data.data.monthlyBars.resultsCount) {
                return Array.from(this.$data.data.monthlyBars.results.map(x => x.close));
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
        },
        addToPortfolio() {
            // Get from ls
            let portfolioString = localStorage.getItem(localStoragePortfolioKey) ?? "[]";
            let portfolio = JSON.parse(portfolioString);
            portfolio.push({
                tickr: this.$data.tickr
            });

            // Bound field 
            this.portfolio = portfolio;

            // Save back to local storage
            portfolioString = JSON.stringify(portfolio);
            localStorage.setItem(localStoragePortfolioKey, portfolioString);
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

        <div v-if="portfolio.length > 0">
            <h2>Portfolio</h2>
            <div class="p-container">
                <div v-for="(company, ix) in portfolio" :key="ix" class="p-b">
                    <button class="btn btn-secondary" @click="{ tickr = company.tickr; lookupStock() }">{{ company.tickr
                    }}</button>
                </div>
            </div>
        </div>

        <hr />
        <div class="row">
            <p>
                {{ tickr }}

                <button class="btn btn-primary" @click="addToPortfolio">Add to portfolio</button>
            </p>
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

                <!-- Dividendss -->
                <div>
                    <h2>Dividend History</h2>
                    <SparklineGraph :data="dividends"></SparklineGraph>
                </div>

                <!-- Monthly Bars -->
                <div>
                    <h2>Monthly</h2>
                    <SparklineGraph :data="monthly"></SparklineGraph>
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

.p-container {
    display: flex;
}

.p-b {
    flex: 1;
    padding: 5px;
}
</style>