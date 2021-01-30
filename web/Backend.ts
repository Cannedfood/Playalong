// GENERATED FILE 30-01-2021 10:23:54
// This file is being generated by GenerateTypescript.cs

// == Standard declarations ======================================
export var baseUrl = ''

function timeout(millis = 300): Promise<void> {
	return new Promise<void>(function(resolve) { setTimeout(resolve, millis); });
}

async function get (url: string): Promise<any>             { return await fetch(baseUrl + url).then(v => v.json()); }
async function post(url: string, data: any): Promise<any>  { return await fetch(baseUrl + url, { method: 'POST', body: JSON.stringify(data) }); }
async function del (url: string): Promise<any>             { return await fetch(baseUrl + url, { method: 'DELETE' }); }

// == Endpoint: PlayalongApiV1 '/api/playalong/v1/' ======================================
export
interface Song {
	id: string;
	title: string;
	band: string;
	bpm: number;
	startOffset: number;
	source: string;
}

export
interface SongSection {
	name: string;
	start: number;
	end: number;
}

export function getSongs(): Promise<Song[]> { return get(`/api/playalong/v1/songs`); }
export function getSong(id: string): Promise<Song> { return get(`/api/playalong/v1/songs/${id}`); }
export function saveSong(id: string, song: Song): Promise<void> { return post(`/api/playalong/v1/songs/${id}`, song); }
export function deleteSong(id: string): Promise<void> { return del(`/api/playalong/v1/songs/${id}`); }
export function getSongSections(id: string): Promise<SongSection[]> { return get(`/api/playalong/v1/songs/${id}/sections`); }
export function saveSongSections(id: string, songSections: SongSection[]): Promise<void> { return post(`/api/playalong/v1/songs/${id}/sections`, songSections); }
