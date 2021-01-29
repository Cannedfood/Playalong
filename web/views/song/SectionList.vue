<template lang="pug">
.list.limit-width
	.header Song
		.buttons
			a(@click="$emit('editSong')") Options
	.item.clicky(
		v-for="section of sections"
		@click="toggleSelect(section)"
		:class="{ highlight: isSelected(section) }"
	)
		.label {{section.name}}
		.buttons
			div(@click.stop="$emit('editSection', section)") Edit
	.item.clicky(@click="$emit('editSection', null)")
		.title Add
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { SongSection } from "../../Backend";
import { selection } from '../components/Util'

export default defineComponent({
	props: {
		"song": { required: true },
		"sections": { required: true }
	},
	setup() {
		const { toggleSelect, isSelected } = selection<SongSection>();
		return {
			toggleSelect, isSelected
		};
	}
})
</script>
