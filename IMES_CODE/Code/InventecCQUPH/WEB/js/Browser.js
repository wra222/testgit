/* Reference from jQuery */
var userAgent = navigator.userAgent.toLowerCase();
browser = {
    version: (userAgent.match(/.+(?:rv|it|ra|ie)[\/: ]([\d.]+)/) || [0, '0'])[1],
    safari: /webkit/.test(userAgent),
    opera: /opera/.test(userAgent),
    msie: /msie/.test(userAgent) && !/opera/.test(userAgent),
    mozilla: /mozilla/.test(userAgent) && !/(compatible|webkit)/.test(userAgent)
};

/* Another method for IE version */
/*
if (document.documentMode == 10)
    alert('IE 10');
else if (document.documentMode == 9)
    alert('IE 9');
else if (window.postMessage)
    alert('IE 8');
else if (window.XMLHttpRequest)
    alert('IE 7');
else if (document.compatMode)
    alert('IE 6');
else if (window.createPopup)
    alert('IE 5.5');
else if (window.attachEvent)
    alert('IE 5');
else if (document.all) {
    alert('IE 4');
}
*/