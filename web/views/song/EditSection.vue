<template lang="pug">
.list.limit-width
  .header Section Settings
    .buttons
      fa-icon(icon="trash" @click="$emit('delete')") Del
      fa-icon(icon="check" @click="$emit('done')") Ok
  .field
    .label Title
    input(type="text" v-model="section.name")
  .field
    .label Start
    time-input(v-model="section.start" :bpm="section.bpm")
  .field
    .label Duration
    time-input(v-model="duration" :bpm="section.bpm")
  .field
    .label End
    time-input(v-model="section.end" :bpm="section.bpm")
  .field
    .label Time Signature
    .inputs
      input(type="number" v-model="section.timeCount")
      div /
      input(type="number" v-model="section.timeSubdivision")
  .field
    .label BPM
    .inputs
      input(type="number" v-model="section.bpm")
      tap-tempo(v-model:bpm="section.bpm")

</template>

<script lang="ts">
import { computed, defineComponent } from "vue";
import { Song, SongSection } from "../../Backend";

import TimeInput from '../components/TimeInput.vue'
import TapTempo from '../components/TapTempo.vue'

export default defineComponent({
  components: { TimeInput, TapTempo },
  props: {
    "section": { required: true, type: Object as () => SongSection },
    "song": { required: true, type: Object as () => Song },
  },
  setup({ section }) {
    let duration = computed<number>({
      get() { return section.end - section.start },
      set(value) { section.end = section.start + value; }
    })

    return { duration }
  }
})
</script>
