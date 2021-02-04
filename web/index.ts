if('serviceWorker' in navigator) {
  navigator.serviceWorker.register("./service-worker.js")
}

import * as Vue from 'vue'

import App from './App.vue'

import router from './views/Router'

import './css/List.less'
import './css/Page.less'
import './css/Cursor.less'
import './css/Fonts.css'

import { library } from '@fortawesome/fontawesome-svg-core'
import { faEdit, faTrash, faCog, faCheck, faPlus } from '@fortawesome/free-solid-svg-icons'

library.add(faEdit, faTrash, faCog, faCheck, faPlus);

import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

const app = Vue.createApp(App)
  .use(router)
  .component('fa-icon', FontAwesomeIcon)
  .mount('#app')
