<template lang="pug">
.player-container
	.player.center
		div(ref="original")
</template>

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


<script lang="ts">
import { defineComponent, onMounted, ref, h, onBeforeUnmount } from "vue";

import createPlayer from 'youtube-player'
import getYoutubeId from 'get-youtube-id'
import { YouTubePlayer } from "youtube-player/dist/types";

import _ from 'lodash'

export default defineComponent({
	props: {
		"source": { required: true, type: String },
	},
	emits: ["player"],
	setup(props) {
		const original = ref<HTMLDivElement>(null);
		const parentElement = ref<HTMLElement>(null);
		const player = ref<YouTubePlayer>(null);

		let updateSize = _.debounce(() => {
			player.value.setSize(parentElement.value.clientWidth, parentElement.value.clientHeight);
		}, 100);

		(window as any).updateSize = updateSize;

		onMounted(() => {
			(window as any).YTConfig = {
				host: 'https://www.youtube.com/iframe_api'
			}

			// const host = props.nocookie? 'https://www.youtube-nocookie.com' : 'https://www.youtube.com';

			parentElement.value = original.value.parentElement;

			let el = document.createElement('div');
			parentElement.value.appendChild(el);

			let p = createPlayer(el, {
				// host,
				width:  parentElement.value.clientWidth,
				height: parentElement.value.clientHeight,
				videoId: getYoutubeId(props.source),
			});
			player.value = p;
			p.on('ready', () => {
				updateSize();
			})
			window.addEventListener('resize', updateSize);
		});
		onBeforeUnmount(() => {
			player.value.destroy();
			player.value = null;
			window.removeEventListener('resize', updateSize);
		});

		return { original }
	}
})
</script>
