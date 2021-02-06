import { Song, SongSection } from '../Backend'

export
function beatsPerSecond(bpm: number) { return bpm / 60; }

export
function measureLength(count: number, basis: number) {
	return 4 * count / basis;
}

export
function secondsToBeats(seconds: number, bpm: number) {
	return seconds * beatsPerSecond(bpm);
}

export
function durationInBeats(section: SongSection, bpm: number) {
	let inSeconds = section.end - section.start;
	return inSeconds * beatsPerSecond(bpm);
}

export
function timestampToMeasure(song: Song, seconds: number) {
	let result = 0;

	for(let sec of song.sections) {
		if(sec.start <= seconds) {
			break;
		}
	}

	return result;
}

export
function measureToTimestamp(song: Song, measures: number) {

}
