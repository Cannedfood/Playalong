<template lang="pug">
button(@click="tap()") Tap
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
	emits: ["update:bpm", "update:seconds"],
	setup(_, {emit}) {
		let measurements = [];
		let lastAverage = null;
		return {
			tap() {
				let now = performance.now();
				if(lastAverage) {
					while(measurements.length && measurements[0] < now - (8 * 1000))
						measurements.shift();
				}
				measurements.push(performance.now());
				if(measurements.length > 2) {
					let total = 0;
					for(let i = 0; i < measurements.length - 1; i++) {
						total += measurements[i + 1] - measurements[i];
					}
					lastAverage = total / (measurements.length - 1);

					let averageSeconds = lastAverage / 1000;
					emit('update:seconds', averageSeconds);
					emit('update:bpm', Math.round(60 / averageSeconds));
				}
			}
		}
	}
})
</script>
