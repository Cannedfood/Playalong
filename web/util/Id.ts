export
function makeid(entropyBits: number) {
  const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-<>_|';

  let length = Math.ceil( entropyBits / Math.log2(characters.length) );

  let result = '';
  for(var i = 0; i < length; i++) {
    result += characters.charAt(Math.floor(Math.random() * characters.length));
  }
  return result;
}

export
function concatToId(args: string[]) {
  return args.map(v => encodeURIComponent(v.toLowerCase().replace(' ', '-'))).join('--');
}
