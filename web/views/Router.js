import { createRouter, createWebHistory } from 'vue-router';

import SongList from './SongList'
import Song from './Song'
import AddSong from './AddSong.vue'

const routes = [
  { path: '/', redirect: '/songs' },
  { path: '/home', redirect: '/songs' },
  { path: '/addSong', name: 'Add Song', component: AddSong },
  { path: '/songs', name: 'Songs', component: SongList },
  { path: '/song/:id', name: 'Song', component: Song },
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
