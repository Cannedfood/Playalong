import _ from 'lodash'

import { Song, SongSection } from './BackendTypes'

let baseUrl = "/api"

function timeout(millis = 300): Promise<void> {
	return new Promise<void>(function(resolve) { setTimeout(resolve, millis); });
}

async function get(endpoint: string) : Promise<any> {
	await timeout();
	let url = baseUrl + endpoint;
	console.log(`Fetch ${url}...`);
	let result = await fetch(baseUrl + endpoint).then(r => r.json());
	console.log("Got " + result);
	return result;
}

export
async function listSongs(): Promise<Song[]> { return get("/songs"); }

export
async function listSongSections(songId: string): Promise<SongSection[]> { return get(`/songs/${songId}/sections`) }

export
async function getSong(songId: string): Promise<Song> { return get(`/songs/${songId}`); }

export
async function saveSections(songId: string, sections: SongSection[]) {
	await timeout();
	// dummySections = sections;
}

export
async function saveSong(song: Song) {
	await timeout();

	// for(let s of dummySongs) {
	// 	if(s.id == song.id) {
	// 		Object.apply(s, song);
	// 		return;
	// 	}
	// }
	// dummySongs.push(song);
}
