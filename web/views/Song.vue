<template lang="pug">
.top-bar
	a(@click="back()")
		span ❮ Back
	h1 {{song.title}}
	a(v-if="song.id && hasPendingChanges" @click="save()")
		span Save

player(v-if="song.source" :source="song.source")

section-list(
	v-show="mode == 'sections'"
	:song="song"
	:sections="sections"
	@selected-range="setLoop($event.start, $event.end)"
	@editSong="editSong = true"
	@editSection="editSection = $event || newSection()")

edit-section(
	v-if="mode == 'edit-section'"
	:song="song"
	:section="editSection"
	@delete="deleteSection(editSection); hasPendingChanges = true"
	@done="editSection = null; hasPendingChanges = true")

edit-song(
	v-if="mode == 'edit-song'"
	:song="song"
	@done="editSong = false; hasPendingChanges = true;")

</template>

<script lang="ts">
import { ref, computed, defineComponent } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getSong, saveSong, SongSection } from '../Backend'
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
		const hasPendingChanges = ref(false);
		function save() {
			hasPendingChanges.value = false;
			saveSong(song.value.id, song.value);
		}
		function back() {
			if(hasPendingChanges.value && confirm("Save changes?"))
				save();
			router.back();
		}

		// Song data
		const song = asyncValue(getSong(songId), {
			id: null,
			title: "Loading...",
			album: "",
			band: "...",
			bpm: 120,
			startOffset: 0,
			source: null,
			sections: [],
			timeSubdivision: 4,
			timeCount: 4
		});
		const sections = computed(() => song.value.sections);
		function newSection() {
			let section: SongSection = {
				name: "New Section",
				start: 0,
				end: 0,
				timeSubdivision: song.value.timeSubdivision,
				timeCount: song.value.timeCount,
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

		// Playback
		function setLoop(start: number, end: number) {
			console.log("setLoop", start, end);
		}

		return {
			// Top Bar
			back, save, hasPendingChanges,
			// Data/Model
			song, sections,
			newSection, deleteSection,
			// State
			editSection, editSong,
			mode: computed(() => {
				if(editSong.value) return 'edit-song';
				if(editSection.value) return 'edit-section';
				return 'sections';
			}),
			// Playback
			setLoop,
		};
	}
})
</script>
