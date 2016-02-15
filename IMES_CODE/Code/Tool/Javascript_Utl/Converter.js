function toBinString (arr) {
    var uarr = new Uint8Array(arr.map(function(x){return parseInt(x,2)}));
    var strings = [], chunksize = 0xffff;
    // There is a maximum stack size. We cannot call String.fromCharCode with as many arguments as we want
    for (var i=0; i*chunksize < uarr.length; i++){
        strings.push(String.fromCharCode.apply(null, uarr.subarray(i*chunksize, (i+1)*chunksize)));
    }
    return strings.join('');
}

function ByteArray2HexString(byteArray)
{
	var ret = '',
	  i = 0,
	  len = byteArray.length;
	while (i < len) {
	  var h = ('00'+byteArray[i].toString(16)).substr(-2);	 
	  ret += h;
	  i++;
	}
	return ret.toUpperCase();
}

function Bytes2HexString (arr) {
    var uarr = new Array(arr.map(function(x){return ('00'+x.toString(16)).substr(-2);}));
      
    return uarr.join('').toUpperCase();
}