    '(sn,kb,lb)
	'--- for test Queue
	dim guitars,arrayItems, arrayKeys
	set guitars=CreateObject("Scripting.Dictionary") 
	guitars.Add "e", "Epiphone" 
	guitars.Add "f", "Fender" 
	guitars.Add "g", "Gibson" 
	guitars.Add "h", "Harmony" 
	arrayItems = guitars.Items
	arrayKeys = guitars.Keys
	MsgBox guitars.count & "," & arrayItems(0) & "," & arrayKeys(0)
	guitars.Remove("e") 
	MsgBox guitars.count & "," & arrayItems(3) & "," & arrayKeys(3)
     dim sn,kb,lb
     sn="5CG1280XX4"
     kb="6060B1435501"
     lb="6060B1435501"

     set xmlhttp = CreateObject("MSXML2.XMLHTTP.3.0")
     set xmlDoc = CreateObject("Msxml2.DOMDocument")

     Url = "http://127.0.0.1:8686/AOI?SN=%sn%&KBPartNo=%kb%&LabelPartNo=%lb%"
     'Url="http://127.0.0.1:0680/testtestSNKBPartNo?SN=%sn%&KBPartNo=%kb%&LabelPartNo=%lb%"

     Data="<?xml version=""1.0"" encoding=""utf-8""?><Device><SN>%sn%</SN><KBPartNo>%kb%</KBPartNo><LabelPartNo>%lb%</LabelPartNo></Device>"
     'Data="<?xml version=""1.0"" encoding=""utf-8""?><Device><SN>%sn%</SN><KBPartNo>%kb%</KBPartNo><LabelPartNo>%lb%</LabelPartNo></Device>"
	 'Data=""
     Url=Replace(Url,"%sn%",trim(sn))
     Url=Replace(Url,"%kb%",trim(kb))
     Url=Replace(Url,"%lb%",trim(lb))
	 Data=Replace(Data,"%sn%",trim(sn))
     Data=Replace(Data,"%kb%",trim(kb))
     Data=Replace(Data,"%lb%",trim(lb))
    
     xmlhttp.open "POST", Url, False

     xmlhttp.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	

     MsgBox  Url
     xmlhttp.send Data
    'inputbox "","",Data
     if xmlhttp.readyState = 4 and xmlhttp.status =200 then
        MsgBox  "Success!!"
		xmlDoc.async = false
		xmlDoc.loadXML(xmlHTTP.responseText)
		
		sn=xmlDoc.selectSingleNode("//Device/SN").text 
		result=xmlDoc.selectSingleNode("//Device/Result").text 
		errorCode=xmlDoc.selectSingleNode("//Device/ErrorCode").text
		errorDescr=xmlDoc.selectSingleNode("//Device/ErrorDescr").text
        dim ret(3)
         MsgBox "result=" & result & " errorCode=" & errorCode & " errorDescr=" & errorDescr
         ret(1) =result 
         ret(2) =errorCode 
         ret(3) =errorDescr 
         AOI_Test = ret
        'AOI_Test="result=" & result & " errorCode=" & errorCode & " errorDescr=" & errorDescr
         
		MsgBox "sn=" & sn  & " result=" & result & " errorCode=" & errorCode & " errorDescr=" & errorDescr 
    else
       '(xmlhttp.statusText)
        MsgBox xmlhttp.readyState & " " & xmlhttp.status & " " & xmlhttp.statusText
        
    end if

'end function