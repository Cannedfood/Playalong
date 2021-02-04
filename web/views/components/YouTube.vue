<template lang="pug">
.player-container
  .player.center
    div(ref="original")
</template>

<style lang="less">
.player-container {
  background: #333;
}
.player {
  background: black;
  height: 60vw;
  width: 100%;
  max-height: calc(20cm / 16 * 9);
  max-width: 20cm;

  margin-left: auto;
  margin-right: auto;
}
</style>


<script lang="ts">
import { defineComponent, onMounted, ref, h, onBeforeUnmount } from "vue";

import createPlayer from 'youtube-player'
import getYoutubeId from 'get-youtube-id'
import { YouTubePlayer } from "youtube-player/dist/types";

function debounce<T>(func: T, wait: number, immediate?: boolean): T {
  var timeout: NodeJS.Timeout;
  return function() {
    var context = this, args = arguments;
    var later = function() {
      timeout = null;
      if (!immediate) (func as any).apply(context, args);
    };
    var callNow = immediate && !timeout;
    clearTimeout(timeout);
    timeout = setTimeout(later, wait);
    if (callNow) (func as any).apply(context, args);
  } as unknown as T;
};

export default defineComponent({
  props: {
    "source": { required: true, type: String },
  },
  emits: ["player"],
  setup(props, { emit }) {
    const original = ref<HTMLDivElement>(null);
    const parentElement = ref<HTMLElement>(null);
    const player = ref<YouTubePlayer>(null);

    let updateSize = debounce(() => {
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

      emit('player', player);
    });
    onBeforeUnmount(() => {
      player.value.destroy();
      player.value = null;
      window.removeEventListener('resize', updateSize);
    });

    return { original, player }
  }
})
</script>
