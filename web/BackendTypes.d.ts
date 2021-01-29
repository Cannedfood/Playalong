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

