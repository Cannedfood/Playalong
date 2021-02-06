<template lang="pug">
.top-bar
  a(@click="back()")
    span ❮ Back
  h1 {{song.title}}
  a(v-if="song.id && hasPendingChanges" @click="save()")
    span Save

player(v-if="song.source" :source="song.source" ref="playerElement")

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
import { ref, computed, defineComponent, onBeforeUnmount } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getSong, saveSong, SongSection } from '../Backend'
import { asyncValue } from '../util/Vuetil'

import Looper from '../util/Looper'

import Player from './components/YouTube.vue'

import SectionList from './song/SectionList.vue'
import EditSong    from './song/EditSong.vue'
import EditSection from './song/EditSection.vue'

export default defineComponent({
  components: {
    Player,
    SectionList, EditSong, EditSection
  },
  setup(props) {
    const route = useRoute();
    const router = useRouter();
    const songId = route.params.id as string;

    const playerElement = ref(null);

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
        bpm: song.value.bpm,
        timeSubdivision: song.value.timeSubdivision,
        timeCount: song.value.timeCount,
      };
      if(sections.value.length > 0) {
        section.start = sections.value[sections.value.length - 1].end;
        section.end   = section.start + 4 * 4 * song.value.timeCount / song.value.timeSubdivision / (song.value.bpm / 60);
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
    let looper = new Looper(seconds => playerElement.value.player.seekTo(seconds));

    onBeforeUnmount(() => looper.stopLoop());

    return {
      // Top Bar
      back, save, hasPendingChanges,
      // Data/Model
      playerElement,
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
      setLoop: (start, end) => looper.setLoop(start, end),
    };
  }
})
</script>
