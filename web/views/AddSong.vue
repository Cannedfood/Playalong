<template lang="pug">
.top-bar
	a(@click="back()")
		span ❮ Back
	h1 Add Song

.list.limit-width(v-if="!song.source")
	.field
		.label From URL:
		input(type="text" v-model="url" placeholder="Example: https://www.youtube.com/watch?v=dQw4w9WgXcQ")
		button(v-if="!url && paste" @click="paste()") Paste
		button(v-if="url" @click="song.source = url") OK
	.field
		.label From Files: (WIP)
		input(type="file" multiple)
		//- button(v-if="files") OK

youtube(v-if="song.source" :source="song.source")
edit-song(v-if="song.source" :song="song" @done="done()")

</template>

<script lang="ts">
import { defineComponent, reactive, ref, watch } from "vue";
import { useRouter } from "vue-router";
import { saveSong, Song } from '../Backend'

import EditSong from './song/EditSong.vue'
import Youtube from './components/YouTube.vue'
import { concatToId } from './components/Util'


export default defineComponent({
	components: { EditSong, Youtube },
	setup(props) {
		let router = useRouter();

		let url = ref<string>("");

		let paste = null;
		if(navigator.clipboard && navigator.clipboard.readText){
			paste = function() {
				navigator.clipboard.readText().then(text => url.value = text);
			}
		}

		let song = ref<Song>({
			id: "",
			title: "",
			band: "",
			bpm: 120,
			startOffset: 0,
			source: "",
			sections: [],
		});

		async function done() {
			let id = concatToId([song.value.band, song.value.title]);
			await saveSong(id, song.value);
			router.replace(`/songs/${id}`)
		}


		let data = reactive({
			back() { router.back(); },
			song,
			paste,
			url,
			done
		});

		return data;
	}
})
</script>

<style lang="less">

</style>
