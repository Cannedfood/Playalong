export default
class Looper {
	public jumpTo: (to: number) => void;

	public constructor(jumpTo: (to: number) => void) {
		this.jumpTo = jumpTo;
	}

	private intervalHandle?: NodeJS.Timeout = null;
	private timeoutHandle?: NodeJS.Timeout = null;
	private loopStart?: number = null;
	private start?: number = null;
	private end?: number = null;

	public stopLoop() {
		if(this.timeoutHandle) {
			clearTimeout(this.timeoutHandle);
			this.timeoutHandle = null;
		}
		if(this.intervalHandle) {
			clearInterval(this.intervalHandle);
			this.intervalHandle = null;
		}
	}

	public setLoop(start: number, end: number) {
		this.stopLoop();

		if(start === null || end === null) {
			this.start = null;
			this.end = null;
			return;
		}

		const now = this.now();
		if(this.start === null || this.end === null) {
			this.startLoopInternal({ now, start, end });
		}
		else {
			const playbackPosition = this.start + (
				(now - this.loopStart) // <- Time spent since start of loop
				%
				(this.end - this.start) // <- Loop duration
			);
			const timeRemaining = Math.max(0, end - playbackPosition);

			this.startLoopInternal({ now, start, end, delay: timeRemaining });
		}
	}

	private startLoopInternal({ start, end, now, delay }: { start: number; end: number; now: number; delay?: number; }) {
		this.loopStart = now + (delay || 0);
		this.start = start;
		this.end = end;

		let loopDuration = end - start;
		if(delay > 0) {
			this.timeoutHandle = setTimeout(
				() => {
					this.jumpTo(start);
					this.intervalHandle = setInterval(
						() => this.jumpTo(start),
						loopDuration * 1000
					);
				},
				delay * 1000
			);
		}
		else {
			this.jumpTo(start);
			this.intervalHandle = setInterval(
				() => this.jumpTo(start),
				loopDuration * 1000
			);
		}
	}

	private now() {
		return performance.now() / 1000;
	}
}

