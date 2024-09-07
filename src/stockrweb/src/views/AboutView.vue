<script setup>
import DataTable from '../components/DataTable.vue'
</script>
<template>
  <div class="about">
    <h1>This is an about page</h1>

    <hr>

    <DataTable :data="{
      headers: [
        'Ticker',
        'Last'
      ],
      records: [
        {
          fields: [
            'MSFT',
            100
          ]
        }
      ]
    }"></DataTable>
  </div>
</template>
