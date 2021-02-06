export
function debounce<T>(func: T, wait: number, immediate?: boolean): T {
  var timeout: NodeJS.Timeout;
  return function() {
    var context = this, args = arguments;
    var later = function() {
		timeout = null;
		if (!immediate) (func as any).apply(context, args);
    };
    var callNow = immediate && !timeout;
    clearTimeout(timeout);
    timeout = setTimeout(later, wait);
    if (callNow) (func as any).apply(context, args);
  } as unknown as T;
};
