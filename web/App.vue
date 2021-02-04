<template lang="pug">
router-view

.limit-width(v-if="isMobileFirefox")
  .bg-warn
    h3 Warnings: Firefox Mobile
    p <b>Problem</b>: Embedded YouTube videos don't like this browser. Just the mobile variant, mind you.
    p <b>Solution</b>: Use Chrome Mobile or Firefox Desktop
    p <b>Non-Solution</b>: Switching to Desktop view doesn't work, it just makes this warning go away

</template>

<script>
export default {
  name: 'App',
  computed: {
    isMobileFirefox() { return /Android.+Firefox\//.test(navigator.userAgent); }
  }
}
</script>

<style>
</style>
