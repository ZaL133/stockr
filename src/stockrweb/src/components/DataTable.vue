<script setup>

const props = defineProps(['data']);
const headers = props.data.headers;
const records = props.data.records;

</script>
<template>
    <h2>This is your data: </h2>
    <table class="table data-table">
        <thead>
            <tr>
                <th v-for="(header, ix) in headers" :key="ix">
                    {{ header }}
                </th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(record, ix) in records" :key="ix">
                <td v-for="(field, ix) in record.fields" :key="ix">
                    {{ field }}
                </td>
            </tr>
        </tbody>
    </table>
</template>