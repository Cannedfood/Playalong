<template lang="pug">
.list.limit-width
	.header Song Settings
		.buttons
			fa-icon(icon="trash" @click="$emit('delete')")
			fa-icon(icon="check" @click="$emit('done')")
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
		.label Time Signature
		.inputs
			input(type="number" v-model="song.timeCount")
			div /
			input(type="number" v-model="song.timeSubdivision")
	.field
		.label Start offset
		input(type="number" v-model="song.startOffset")
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { Song } from "../../Backend";

import TapTempo from '../components/TapTempo.vue'

export default defineComponent({
	components: { TapTempo },
	props: {
		song: { required: true, type: Object as () => Song }
	}
})
</script>

<style lang="less">

</style>
