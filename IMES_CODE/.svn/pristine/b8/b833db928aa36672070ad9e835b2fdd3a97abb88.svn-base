var _bLog = false;
function Queue()
{
	var stack = [];
	this.Add=function(key, value,timeOut, callbackFunc)
	{
		var count=stack.length,
		    index=-1,
			i=0;
		if (count===0)
		{
			stack.push({Key:key,
			            Value:value,
						TimeOut:timeOut,
						CallBackFunc:callbackFunc});			
		}
		else
		{
			//var index=-1
			for(i=0; i<count;++i)
			{
				if (stack[i].Key== key)
				{
					index=i;
					break;
				}
			}
			
			if (index>0)
			{
				stack.splice(index, 1);
			}
			
			stack.push({Key:key,
			            Value:value,
					    TimeOut:timeOut,
						CallBackFunc:callbackFunc});			
			
		}
		
	}
	
	this.Exists=function(key)
	{
		var count=stack.length,
		     i=0;
		if (count===0)
		{
			return false;
         }
        
        if (typeof key == 'undefined') {
           return false;
        }
        
		for(i=0; i<count;++i)
		{
			if (stack[i].Key== key)
			{
			  return true;
			}
		}
		
		return false;
	}

	this.Item = function(key) {
	    var count = stack.length,
		    i=0;
	    if (count === 0) {
	        return null;
	    }

	    if (typeof key == 'undefined') {
	        return null;
	    }
	    
	    if (typeof key === 'number') {
	        return stack[key];
	    }

	    for (i = 0; i < count; ++i) {
	        if (stack[i].Key == key) {
	            return stack[i];
	        }
	    }

	}
	
	this.First=function()
	{
		return stack.length===0? null:stack[0];
	}
	
	this.Last=function()
	{
		return stack.length===0? null:stack[stack.length-1];
	}
	
	this.Remove=function(key)
	{	
		
		var count=stack.length,
		     	i;
		if (count===0)
		{
			return;
		}
		
		//remove		
		if (typeof key== 'undefined')
		{
			stack.shift();
			return;
		}
		else
		{
			//from top to button
			for(i=count-1; i>=0;i--)
			{
				if (stack[i].Key== key)
				{
					stack.splice(i, 1);					
				}
			}		
		
		}
	}
	
	this.RemoveAll=function ()
	{
		stack = [];
	}
	
	this.Count=function()
	{
		return stack.length;
	}
}

var queue = new Queue();


function callbackfun(sn, result, errorCode, errorDesc) {
    //alert(sn + ',' + result + ',' + errorCode + ',' + errorDesc);
    var arr = new Array();
    arr[0] = result;
    arr[1] = errorCode;
    arr[2] = errorDesc;
    return arr;
}

function testAOI() {
    var url = 'http://127.0.0.1:8686/AOI?SN=%sn%&KBPartNo=%kb%&LabelPartNo=%lb%';
		sn = '1234567890',
		kb = 'kb123',
		lb = 'lb1234',
		result;
    result = SendAOICmd(url, sn, kb, lb, 5, callbackfun);
    //alert(result.Result);
    //SendAOICmd(url, sn, kb, lb, 5, callbackfun);
}

function getXmlHttp() {

    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        return new XMLHttpRequest();
    }

    // code for IE6, IE5
    return new ActiveXObject("Microsoft.XMLHTTP");

}

function SendAOI_inner(sn,url, timeOut, callback) {
 
    var xmlhttp,    
	    timeOutFunc,
        abort = 0,
		respText,
		xmlDoc,
		parser;
    xmlhttp=getXmlHttp();
    xmlhttp.open("POST", url, true); //async mode

    xmlhttp.onreadystatechange = function() {        
        if (xmlhttp.readyState == 4) {
            if (timeOutFunc) {
                clearTimeout(timeOutFunc);
            }

            if (abort === 1)
                return;
                
            if (xmlhttp.status == 200) {
				
                respText = xmlhttp.responseText;
                //var xmlDoc;
                if (window.DOMParser) {
                    parser = new DOMParser();
                    xmlDoc = parser.parseFromString(respText, "text/xml");
                    try {
                            queue.Remove(sn);   
                            callback(xmlDoc.getElementsByTagName('SN')[0].textContent,
							         xmlDoc.getElementsByTagName('Result')[0].textContent,
							         xmlDoc.getElementsByTagName('ErrorCode')[0].textContent,
							         xmlDoc.getElementsByTagName('ErrorDescr')[0].textContent);
                        } catch (err)
			           { }	
                }
                else {
                    xmlDoc = new ActiveXObject("Msxml2.DOMDocument");
                    xmlDoc.async = false;
                    xmlDoc.loadXML(respText);
                    try {
                            queue.Remove(sn);
                            callback(xmlDoc.getElementsByTagName('SN')[0].text,
						             xmlDoc.getElementsByTagName('Result')[0].text,
						             xmlDoc.getElementsByTagName('ErrorCode')[0].text,
						             xmlDoc.getElementsByTagName('ErrorDescr')[0].text);
                    } 
                    catch(err)
				    { }	
                }
            }
            else {
                respText = xmlhttp.responseText;
                try {
                       queue.Remove(sn); 
                        callback(sn,
					             'Status',
					             xmlhttp.status ? xmlhttp.status : '',
					            xmlhttp.statusText ? xmlhttp.statusText : '');
				}
				catch(err)
			    {}			
            }
            CheckAndSendInQueue(sn);
        }
    };
	
        
    if (timeOut > 0) {

        timeOutFunc = setTimeout(function() {
						                if (xmlhttp) {
							                abort = 1;
							                xmlhttp.abort();
						                }

						                try {
						                       queue.Remove(sn);
						                        callback(sn,
								                            'TimeOut',
								                            'TimeOut',
								                            'Request web server time out');
						                }
						                catch(err)
						                {}
						                CheckAndSendInQueue(sn);		
						        },
								timeOut * 1000);
         

    }
	
	xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");    
    xmlhttp.send('');  

}

function CheckAndSendInQueue(key)
{
	var data;
	try{

	    queue.Remove(key);
		if (queue.Count()>0)
		{
			data =queue.First();
			//alert('Send URL:' +data.Value);
			SendAOI_inner(data.Key,
						  data.Value, 
						  data.TimeOut, 
						  data.CallBackFunc);
		}
		
	}
	catch(e)
	{
	}
}

function SendAOICmd(url, sn, kbPartNo, lbPartNo, timeOut, callback) {
 
    var xmlhttp,
       timeOutFunc,
       abort = 0;
    xmlhttp = getXmlHttp();
    
    url = url.replace('%sn%', sn);
    url = url.replace('%kb%', kbPartNo);
    url = url.replace('%lb%', lbPartNo); 
	
	if (queue.Count()===0)
	{
		SendAOI_inner(sn,url, timeOut, callback);
	}
	
	queue.Add(sn,url,timeOut,callback); 

}

function SendAOICmd_Sync(url, sn, kbPartNo, lbPartNo, timeOut, callback) {
 
    var xmlhttp,
        timeOutFunc,
        abort = 0,
		respText,
		xmlDoc,
		parser,
		sendText = '';
		
		
    xmlhttp = getXmlHttp();
    
    url = url.replace('%sn%', sn);
    url = url.replace('%kb%', kbPartNo);
    url = url.replace('%lb%', lbPartNo);  
    xmlhttp.open("POST", url, true); //async mode

    xmlhttp.onreadystatechange = function() {        
        if (xmlhttp.readyState == 4) {
            if (timeOutFunc) {
                clearTimeout(timeOutFunc);
            }

            if (abort === 1)
                return;
                
            if (xmlhttp.status == 200) {
                respText = xmlhttp.responseText;
                //var xmlDoc;
                if (window.DOMParser) {
                    parser = new DOMParser();
                    xmlDoc = parser.parseFromString(respText, "text/xml");

                    callback.apply(xmlhttp, [
						xmlDoc.getElementsByTagName('SN')[0].textContent,
						xmlDoc.getElementsByTagName('Result')[0].textContent,
						xmlDoc.getElementsByTagName('ErrorCode')[0].textContent,
						xmlDoc.getElementsByTagName('ErrorDescr')[0].textContent
					]);
                    return;
                }
                else {
                    xmlDoc = new ActiveXObject("Msxml2.DOMDocument");
                    xmlDoc.async = false;
                    xmlDoc.loadXML(respText);
                    callback(xmlDoc.getElementsByTagName('SN')[0].text,
						     xmlDoc.getElementsByTagName('Result')[0].text,
						     xmlDoc.getElementsByTagName('ErrorCode')[0].text,
						     xmlDoc.getElementsByTagName('ErrorDescr')[0].text);
                    return;
                }
            }
            else {
                respText = xmlhttp.responseText;
                callback(sn,
					     'Status',
					     xmlhttp.status ? xmlhttp.status : '',
					    xmlhttp.statusText ? xmlhttp.statusText : '');
				return;		
            }
        }
    };
	
    xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    //var sendText = '<?xml version="1.0" encoding="utf-8"?><Device><SN>%sn%</SN><KBPartNo>%kb%</KBPartNo><LabelPartNo>%lb%</LabelPartNo></Device>';
    //sendText = sendText.replace('%sn%', sn);
    //sendText = sendText.replace('%kb%', kbPartNo);
    //sendText = sendText.replace('%lb%', lbPartNo);
    //var sendText = "";
    if (timeOut > 0) {

        timeOutFunc = setTimeout(function() {
						if (xmlhttp) {
							abort = 1;
							xmlhttp.abort();
						}
						callback(sn,
								'TimeOut',
								'TimeOut',
								'Request web server time out');
						},
								timeOut * 1000);
         

    }
    xmlhttp.send(sendText);

}
function writeLog(errorMsg) {
    if (!_bLog) { return; }
    var now = new Date(),
        homeDir = "C:\\AOI\\Log",
		fileName,
		fso,
		fileHandle;
    //	var fileName=date(yyyyMMddHH
    if (createDir(homeDir)) {
        fileName = now.getFullYear() +""+ now.getMonth() + now.getDay() + now.getHours() + ".log";
        fso = new ActiveXObject("Scripting.FileSystemObject");
        fileHandle = fso.CreateTextFile(homeDir + "\\" + fileName, true);
        fileHandle.WriteLine(errorMsg);
        fileHandle.Close();
    }

}
