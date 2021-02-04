<template lang="pug">
.list
  .field
    .label BPM
    input(type="number" v-model="bpm")
  .field
    .label Time Signature
    .inputs
      input(type="number" v-model="count")
      | /
      input(type="number" v-model="subdivision")
  .field
    .label  Accents
    .inputs
      input(v-for="i in count")
</template>

<script lang="ts">
import { defineComponent, reactive } from "vue";

export default defineComponent({
  setup() {
    let data = {
      bpm: 120,
      subdivision: 4,
      count: 4
    };

    return reactive(data);
  }
})
</script>

<style lang="less">

</style>
