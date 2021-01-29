<template lang="pug">
.list.limit-width
	.header Section Settings
		.buttons
			div(@click="$emit('delete')") Del
			div(@click="$emit('done')") Ok
	.field
		.label Title
		input(type="text" v-model="section.name")
	.field
		.label Start
		.inputs
			.input
				input(type="number" min="0" step="0.001" v-model="section.start")
				.unit seconds
			.input
				input(type="number" min="0" step="0.125" :value="section.start / beatsPerSecond" @input="section.start = (+$event.target.value) * beatsPerSecond")
				.unit beats
	.field
		.label End
		.inputs
			.input
				input(type="number" min="0" step="0.001" v-model="section.end")
				.unit seconds
			.input
				input(type="number" min="0" step="0.125" :value="section.end / beatsPerSecond" @input="section.end = (+$event.target.value) * beatsPerSecond")
				.unit beats
</template>

<script lang="ts">
import { computed, defineComponent } from "vue";
import { Song, SongSection } from "../../Backend";

export default defineComponent({
	props: {
		"section": { required: true, type: Object as () => SongSection },
		"song": { required: true, type: Object as () => Song },
	},
	setup(props) {
		return {
			beatsPerSecond: computed(() => props.song.bpm / 60),
			fromBeats(beats: number) { return beats * this.beatsPerSecond; },
			toBeats(seconds: number) { return seconds / this.beatsPerSecond; }
		}
	}
})
</script>
