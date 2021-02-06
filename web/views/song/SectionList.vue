<template lang="pug">
.list.limit-width
  .header Song
    .buttons
      fa-icon(icon="cog" @click="$emit('editSong')")
  .item.clicky(
    v-for="section of sections"
    @click="toggleSelect(section)"
    :class="{ highlight: isSelected(section) }"
  )
    .title {{section.name}}
    .subtitle {{timestamp(section.start)}} - {{timestamp(section.end)}} ({{section.end - section.start}}s)
    .buttons
      fa-icon(icon="edit" @click.stop="$emit('editSection', section)")
  .item.clicky-bg(@click="$emit('editSection', null)")
    .title.text-center
      fa-icon(icon="plus")
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { Song, SongSection } from "../../Backend";
import { listSelect } from '../../util/Vuetil'

export default defineComponent({
  props: {
    "song": { required: true, type: Object as () => Song },
    "sections": { required: true, type: Array as () => SongSection[] }
  },
  emits: ["selected-range", "editSection", "editSong"],
  setup(_, {emit}) {
    const { toggleSelect, isSelected, selection } = listSelect<SongSection>();

    return {
      toggleSelect: function(section: SongSection) {
        toggleSelect(section);
        if(selection.value.length == 0) {
          emit('selected-range', { start: null, end: null });
        }
        else {
          emit('selected-range', {
            start: selection.value.map(v => v.start).reduce((a, b) => Math.min(a, b), 1e7),
            end: selection.value.map(v => v.end).reduce((a, b) => Math.max(a, b), 0),
          });
        }
      },
      isSelected,
      beat(n: number) {

      },
      timestamp(n: number) {
        let minutes = Math.floor(n / 60);
        let seconds = n - minutes;
        return `${minutes}:${seconds}`;
      }
    };
  }
})
</script>
