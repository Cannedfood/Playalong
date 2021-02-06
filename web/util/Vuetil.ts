import { Ref, ref } from "vue";

export
function listSelect<T = any>() {
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
