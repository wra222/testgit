set xmlhttp = CreateObject("MSXML2.XMLHTTP.3.0")
set xmlDoc = CreateObject("Msxml2.DOMDocument")

Url = "http://127.0.0.1/SN"

Data="<?xml version=""1.0"" encoding=""utf-8""?><Device><SN>%sn%</SN><KBPartNo>%kb%</KBPartNo><LabelPartNo>%lb%</LabelPartNo></Device>"

Data=Replace(Data,"%sn%","CUN1234")
Data=Replace(Data,"%kb%","698694-04")
Data=Replace(Data,"%lb%","6054B1146103 ?,6060B1469901?,6Z60B1226101")

xmlhttp.open "POST", Url, False

xmlhttp.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"

xmlhttp.send Data

if xmlhttp.readyState = 4 and xmlhttp.status =200 then
	xmlDoc.async = false
	xmlDoc.loadXML(xmlHTTP.responseText)
	
	sn=xmlDoc.selectSingleNode("//Device/SN").text 
	result=xmlDoc.selectSingleNode("//Device/Result").text 
	errorCode=xmlDoc.selectSingleNode("//Device/ErrorCode").text
	errorDescr=xmlDoc.selectSingleNode("//Device/ErrorDescr").text
	MsgBox "sn=" & sn  & " result=" & result & " errorCode=" & errorCode & " errorDescr=" & errorDescr 
else
    MsgBox xmlhttp.readyState & " " + xmlhttp.status + " " + xmlhttp.statusText
end if
