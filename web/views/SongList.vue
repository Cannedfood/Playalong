<template lang="pug">
.top-bar
  h1 Songs
.list.limit-width(v-if="songs")
  //- .field
  //-   input(type="text" placeholder="Search...")
  .header Songs
  router-link.item.very-clicky(v-for="song in songs" :to="`/song/${song.id}`")
    .title {{song.title}}
    .subtitle {{[song.band, song.album].filter(v=>v).join(' - ')}}
  router-link.button.clicky-bg(to="/addSong")
    .title.text-center
      fa-icon(icon="plus")
</template>

<script lang="ts">
import { defineComponent, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { getSongs, Song } from '../Backend'

export default defineComponent({
  name: 'SongList',
  setup() {
    let router = useRouter();

    let model = reactive({
      songs: null,
      selectSong(song: Song) { router.push({ path: `/song/${song.id}` }) }
    });

    getSongs().then(v => { model.songs = v });

    return model;
  }
})
</script>

<style>
ol {
  display: flex;
  flex-flow: column;
}
li {
  display: block;
  width: 100%;
  min-height: 2em;
  border: 1px solid lightgray;
}
</style>
