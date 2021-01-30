<template lang="pug">
.top-bar
	a(@click="back()")
		span ❮ Back
	h1 {{song.title}}
	a(v-if="song.id" @click="save()")
		span Save
.player-container
	.player.center
		player(v-if="song.source" :source="song.source")

section-list(
	v-show="mode == 'sections'"
	:song="song"
	:sections="sections"
	@editSong="editSong = true"
	@editSection="editSection = $event || newSection()")

edit-section(
	v-if="mode == 'edit-section'"
	:song="song"
	:section="editSection"
	@delete="deleteSection(editSection); updated = true"
	@done="editSection = null; updated = true")

edit-song(
	v-if="mode == 'edit-song'"
	:song="song"
	@done="editSong = false; updated = true;")

</template>

<script lang="ts">
import { ref, computed, defineComponent } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getSong, getSongSections, saveSongSections, saveSong, SongSection } from '../Backend'
import { asyncValue } from './components/Util'

import Player from './components/YouTube.vue'

import SectionList from './song/SectionList.vue'
import EditSong    from './song/EditSong.vue'
import EditSection from './song/EditSection.vue'

export default defineComponent({
	components: {
		Player,
		SectionList, EditSong, EditSection
	},
	setup() {
		const route = useRoute();
		const router = useRouter();
		const songId = route.params.id as string;

		// State
		const editSong    = ref(false);
		const editSection = ref(null);

		// Navigation / Saving
		const updated = ref(false);
		function save() {
			saveSong(song.value.id, song.value);
			saveSongSections(song.value.id, sections.value);
		}
		function back() {
			if(updated.value && confirm("Save changes?"))
				save();
			router.back();
		}

		// Song data
		const song = asyncValue(getSong(songId), {
			id: null,
			title: "Loading...",
			band: "...",
			bpm: 120,
			startOffset: 0,
			source: null
		});
		const sections = asyncValue(getSongSections(songId), []);
		function newSection() {
			let section = {
				name: "New Section",
				start: 0,
				end: 0
			};
			if(sections.value.length > 0) {
				section.end = sections.value[sections.value.length - 1].end;
				section.start = section.end + 1;
			}
			sections.value.push(section)
			return section;
		}

		function deleteSection(section: SongSection) {
			confirm(`Are you sure you want to delete the section '${section.name}'?`)
			const index = sections.value.indexOf(section);
			if(index >= 0) {
				sections.value.splice(index, 1);
			}
			if(editSection.value == section)
				editSection.value = null;
		}

		return {
			// Top Bar
			back, save, updated,
			// Data/Model
			song, sections,
			newSection, deleteSection,
			// State
			editSection, editSong,
			mode: computed(() => {
				if(editSong.value) return 'edit-song';
				if(editSection.value) return 'edit-section';
				return 'sections';
			})
		};
	}
})
</script>

<style>
.player-container {
	background: #333;
}
.player {
	background: black;
	height: 60vw;
	max-height: 30vh;
	width: 100%;
	max-width: 53.3333333vh;

	margin-left: auto;
	margin-right: auto;
}
</style>
