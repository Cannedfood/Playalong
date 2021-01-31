<template lang="pug">
.list.limit-width
	.header Song Settings
		.buttons
			div(@click="$emit('delete')") Del
			div(@click="$emit('done')") Ok
	.field
		.label Title
		input(type="text" v-model="song.title")
	.field
		.label Album
		input(type="text" v-model="song.album")
	.field
		.label Band
		input(type="text" v-model="song.band")
	.field
		.label BPM
		input(type="number" v-model="song.bpm")
		tap-tempo(v-model:bpm="song.bpm")
	.field
		.label Start offset
		input(type="number" v-model="song.startOffset")
</template>

<script lang="ts">
import { defineComponent } from "vue";

import TapTempo from '../components/TapTempo.vue'

export default defineComponent({
	components: { TapTempo },
	props: {
		song: { required: true }
	}
})
</script>

<style lang="less">

</style>
