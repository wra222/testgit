var IMESUti = IMESUti || {};
IMESUti.Message=( function() {
    
   //********** private **********
   var noSupportMessage = "Your browser cannot support WebSocket!"; 
   var ws; 
   var xmlhttp; 
   var subScribeData; 
   var timerFunc;
   var timeout=5000;//timer 1000 million second timeout 
   var wsClosed=0;
   function GetNow()
	{
			var now = new Date(); 
			var datetime = now.getFullYear()+'/'+(now.getMonth()+1)+'/'+now.getDate(); 
				datetime += ' '+now.getHours()+':'+now.getMinutes()+':'+now.getSeconds(); 
			return datetime;	
	}
	
	function getXmlHttp() 
	{
			if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari	    
				return new XMLHttpRequest();
				}

				// code for IE6, IE5
			return new ActiveXObject("Microsoft.XMLHTTP");
	}
	
	function CheckJSONObject() {
             if (typeof (JSON) == 'undefined') 
			{ 
				$('script').append($("<script type='text/javascript' src='../JS/json2.js'>"));
			} 				
    }
	
	function PostHttp(url,subject, data,SuccessCallBack, ErrorCallBack) {
			if (!xmlhttp)
			{
				xmlhttp=getXmlHttp();
			}
			xmlhttp.open('POST', url, true); //async mode

			xmlhttp.onreadystatechange = function() {        
				if (xmlhttp.readyState == 4) {
					var responseText = xmlhttp.responseText;
					if (xmlhttp.status == 200) {						
						if (typeof (SuccessCallBack) === 'function') 
						{ 
							SuccessCallBack(url,subject, data, responseText);
						}
					}else {						
						if (typeof (ErrorCallBack) === 'function')
						{			
							ErrorCallBack(url,subject,data,xmlhttp.status);
						}
					}					
				}
			};
			
			xmlhttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=utf-8');    
			//xmlhttp.send('{"Cmd":"P","Subject":"Test","Context":"test1234"}');			
			var dataObj={ Cmd:'P',
					   Subject:subject,
					   Context:data,
					   Timestamp:GetNow()			
						};
					
			CheckJSONObject();		
			var sendData= JSON.stringify(dataObj);	
			xmlhttp.send(sendData);				
	}			
	
	 function connectSocketServer(wsUrl,OnMessageCallBack,onOpenCallBack, onCloseCallBack, onErrorCallBack) {
                var support = 'MozWebSocket' in window ? 'MozWebSocket' : ('WebSocket' in window ? 'WebSocket' : null);

                if (support == null) {
                    alert(noSupportMessage);
                    return;
                }
                
                // create a new websocket and connect ws://10.96.183.193:2012/
                ws = new window[support](wsUrl);

                // when data is comming from the server, this metod is called
                ws.onmessage = function (evt) {                    
					CheckJSONObject();
					var rawData=evt.data;
					var obj=JSON.parse(rawData);
					if (typeof (OnMessageCallBack) === 'function')
					{			
						OnMessageCallBack(rawData,obj.Context);
					}					
                };

                // when the connection is established, this method is called
                ws.onopen = function () {
					if (timerFunc) {
						clearInterval(timerFunc);
						timerFunc = null;
					}
					
					if (typeof (onOpenCallBack) === 'function')
					{			
						onOpenCallBack();
					}
					if (subScribeData){
						var data={
							Cmd:'S',
							Subject:subScribeData.Subject,					   
							Timestamp:GetNow()			
						};
						CheckJSONObject();
						var sendData =JSON.stringify(data);	
						ws.send(sendData);
						if (typeof (subScribeData.OnSent) === 'function')
						{
						    subScribeData.OnSent(wsUrl, sendData, subScribeData.Subject);
						}
						
						//subScribeData=null;
					}	
                };

                // when the connection is closed, this method is called
                ws.onclose = function () {
					ws = null;
                    if (typeof (onCloseCallBack) === 'function')
					{	
						if (!timerFunc) //no timer function case call client function
						{		
							onCloseCallBack();
						}
					}
					
					if (!timerFunc && wsClosed === 0)
					{
						timerFunc = setInterval(function(){
											if (subScribeData){
												try{
													connectSocketServer(subScribeData.Wsurl,
																		subScribeData.OnMessage,
																		subScribeData.onOpen,
																		subScribeData.onClose,
																		subScribeData.onError
																		);
												}
												catch(err)
												{}
											}
											else
											{
											  clearInterval(timerFunc);
											  timerFunc = null;
											}
									},
									timeout);
					}								
                };
				
				ws.onerror= function(evt){
					if (typeof (onErrorCallBack) === 'function')
					{			
						onErrorCallBack(evt);
					}
				};
            }
	function sendMessage(wsUrl,subject,OnSentCallBack,OnMessageCallBack,onOpenCallBack, onCloseCallBack, onErrorCallBack) {
				
				wsClosed= 0; //reset manual closed 				
                if (!ws) {
					connectSocketServer(wsUrl,OnMessageCallBack,onOpenCallBack, onCloseCallBack,onErrorCallBack);
					subScribeData={
						Wsurl:wsUrl,
						Subject:subject,
						OnSent:OnSentCallBack,
						OnMessage:OnMessageCallBack,
						onOpen:onOpenCallBack,
						onClose:onCloseCallBack,
						onError:onErrorCallBack
					};
				}else{
					var data={
					   Cmd:'S',
					   Subject:subject,					   
					   Timestamp:GetNow()			
					};
					CheckJSONObject();

					var sendData =JSON.stringify(data);
					if (ws.readyState==1) //Connected
					{		
						ws.send(sendData);
						if (typeof (OnSentCallBack) === 'function')
						{			
							OnSentCallBack(wsUrl, sendData, subject);
						}

					}
					else
					{
						if (ws.readyState==2||ws.readyState==3) //closing socket
						{
							if (!timerFunc && wsClosed === 0)
							{
								timerFunc = setInterval(
									function(){
											if (subScribeData){
												try{
													connectSocketServer(wsUrl,
																		OnMessageCallBack,
																		onOpenCallBack,
																		onCloseCallBack,
																		onErrorCallBack
																		);
												}
												catch(err)
												{}
											}
											else
											{
											  clearInterval(timerFunc);
											  timerFunc = null;
											}
									},
									timeout);
							}
						}
						
					}
					
					subScribeData={
						Wsurl:wsUrl,
						Subject:subject,
						OnSent:OnSentCallBack,
						OnMessage:OnMessageCallBack,
						onOpen:onOpenCallBack,
						onClose:onCloseCallBack,
						onError:onErrorCallBack
					};				
					
				}
     }
	function closeWS()
	{
		wsClosed= 1;
		if (ws) 
		{
			ws.close();
			ws=null;
        }		
	}
     //********** private ********** 
    return  {
			 Publish:function(url,subject,data,SuccessFunction, ErrorFunction)
			 {
				 PostHttp(url,subject,data,SuccessFunction, ErrorFunction)
			 },
			 Subscribe:function(wsUrl,subject,OnSentFunction, OnMessageFunction, OnOpenFunction, onCloseFunction,onErrorFunction)
			 {
				 sendMessage(wsUrl,subject,OnSentFunction,OnMessageFunction, OnOpenFunction, onCloseFunction,onErrorFunction)
			 },
			 DisconnectWS:function()
			 {
				closeWS();
			 }
    };
}
)();