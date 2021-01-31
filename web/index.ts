if('serviceWorker' in navigator) {
	navigator.serviceWorker.register("./service-worker.js")
}

import * as Vue from 'vue'

import App from './App.vue'

import router from './views/Router'

import './css/List.less'
import './css/Page.less'
import './css/Cursor.css'
import './css/Fonts.css'

const app = Vue.createApp(App)
	.use(router)
	.mount('#app')
