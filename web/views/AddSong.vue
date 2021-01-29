<template lang="pug">
.top-bar
	a(@click="back()")
		span ❮ Back
	h1 Add Song
.list.limit-width
	.field
		.label From URL:
		input(type="text" v-model="url" placeholder="Example: https://www.youtube.com/watch?v=dQw4w9WgXcQ")
		button(v-if="!url && paste" @click="paste()") Paste
		button(v-if="url") OK
	.field
		.label From Files: (WIP)
		input(type="file" multiple)
		//- button(v-if="files") OK
</template>

<script lang="ts">
import { defineComponent, reactive, ref } from "vue";
import { useRouter } from "vue-router";

export default defineComponent({
	setup(props) {
		let router = useRouter();

		let url = ref<string>("");

		let paste = null;
		if(navigator.clipboard && navigator.clipboard.readText){
			paste = function() {
				navigator.clipboard.readText().then(text => url.value = text);
			}
		}

		return reactive({
			back() { router.back(); },
			paste: paste,
			url
		});
	}
})
</script>

<style lang="less">

</style>
