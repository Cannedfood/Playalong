import { Ref, ref } from "vue";

export
function selection<T = any>() {
	let items = ref([] as T[]);
	return {
		selection: items,
		toggleSelect(item: T) {
			let index = items.value.indexOf(item as any);
			if(index < 0)
				items.value.push(item as any);
			else
				items.value.splice(index, 1);
		},
		isSelected(item) { return items.value.includes(item); }
	}
}

export
function asyncValue<T>(promise: Promise<T>, placeholder: T): Ref<T> {
	let v = ref(placeholder) as Ref<T>;
	promise.then(value => v.value = value);
	return v;
}

export
function makeid(entropyBits: number) {
	const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-<>_|';

	let length = Math.ceil( entropyBits / Math.log2(characters.length) );

	let result           = '';
	for ( var i = 0; i < length; i++ ) {
	   result += characters.charAt(Math.floor(Math.random() * characters.length));
	}
	return result;
}

export
function concatToId(args: string[]) {
	return args.map(v => encodeURIComponent(v.toLowerCase().replace(' ', '-'))).join('--');
}
