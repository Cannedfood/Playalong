<template lang="pug">
.inputs
	input(v-if="mode == 'seconds'"  type="number" min="0" step="0.001" v-model="seconds")
	input(v-if="mode == 'beats'"    type="number" min="0" step="0.125" v-model="beats")
	input(v-if="mode == 'measures'" type="number" min="0" step="0.125" v-model="measures")
	button.unit(@click="nextMode()") {{mode}}
</template>

<script lang="ts">
import { computed, defineComponent, reactive } from "vue";

export default defineComponent({
	props: {
		"modelValue": { required: true, type: Number },
		"bpm": { required: true, type: Number },
		"beatsPerMeasure": { required: false, type: Number, default: 4 }
	},
	emits: [ 'update:modelValue' ],
	setup(props, { emit }) {
		function beatsPerSecond() { return props.bpm / 60; }

		const seconds = computed<number>({
			get() { return props.modelValue; },
			set(value) { emit("update:modelValue", +value); }
		});

		const beats = computed<number>({
			get() { return seconds.value * beatsPerSecond() },
			set(value) { seconds.value = (+value) / beatsPerSecond(); }
		});

		const measures = computed<number>({
			get() { return beats.value / props.beatsPerMeasure },
			set(value) { beats.value = (+value) * props.beatsPerMeasure; }
		});

		return reactive({
			seconds, beats, measures,
			mode: 'measures',
			nextMode() {
				switch(this.mode) {
					case 'measures': this.mode = 'beats'; break;
					case 'beats':    this.mode = 'seconds'; break;
					case 'seconds':  this.mode = 'measures'; break;
				}
			}
		});
	}
})
</script>

<style lang="less">

</style>
