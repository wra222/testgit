var clkID;
var obUD;
var bodyonclick;
var clock_hour = 0;
var clock_minute = 0;
var obFocusItem = null;

function UDClock_UpClick(clockID)
{
	//switch(clockFocus)
   clkID = formatID(clockID);
//	switch(document.activeElement)
	switch(obFocusItem)
	{
//	case "NONE" :
//		break;
//	case "HOUR" :
	//应刘副理要求，缺省将焦点方在“小时”上。姜昕 11/28
	//case document.all("tHour" + clkID ):
	//case "MINUTE" :
	case document.all("tMinute" + clkID ):
		if(document.all("tMinute" + clkID ).value < 59)
		{			
			document.all("tMinute" + clkID ).value ++;
			if(document.all("tMinute" + clkID ).value.length<2)
				document.all("tMinute" + clkID ).value = "0" + document.all("tMinute" + clkID ).value;
		}
		else
		{
			document.all("tMinute" + clkID ).value = "00";
		}
		break;
	default:
		if(document.all("tHour" + clkID ).value < 23)
		{
			document.all("tHour" + clkID ).value ++;
			if(document.all("tHour" + clkID ).value.length<2)
				document.all("tHour" + clkID ).value = "0" + document.all("tHour" + clkID + "").value;
		}
		else
		{
			document.all("tHour" + clkID ).value = "00";
		}
		break;
	}
	clock_hour = parseInt(document.all("tHour" + clkID ).value);
	clock_minute = parseInt(document.all("tMinute" + clkID ).value);
}
function UDClock_DownClick(clockID)
{
	//switch(clockFocus)
   clkID = formatID(clockID);
//	switch(document.activeElement)
 	switch(obFocusItem)
	{
	//case "NONE" :
	//	break;
	//case "HOUR" :
	//case document.all("tHour" + clkID ):
	//应刘副理要求，缺省将焦点方在“小时”上。姜昕 11/28
	//case "MINUTE" :
	case document.all("tMinute" + clkID ):
		if(document.all("tMinute" + clkID ).value > 0)
		{
			document.all("tMinute" + clkID ).value --;
			if(document.all("tMinute" + clkID ).value.length<2)
				document.all("tMinute" + clkID ).value = "0" + document.all("tMinute" + clkID ).value;
		}
		else
			document.all("tMinute" + clkID ).value = 59;
		break;
	default:
		if(document.all("tHour" + clkID ).value > 0)
		{
			document.all("tHour" + clkID ).value --;
			if(document.all("tHour" + clkID ).value.length<2)
				document.all("tHour" + clkID ).value = "0" + document.all("tHour" + clkID ).value;
		}
		else
			document.all("tHour" + clkID ).value = 23;
		break;
	}
	clock_hour = parseInt(document.all("tHour" + clkID ).value);
	clock_minute = parseInt(document.all("tMinute" + clkID ).value);
}
function createClock(target, backColor, clockID)
{
	var ojTarget = target;
	var content;
   var documentURL = document.URL;
   var calURL;
   var pos;
   pos = documentURL.indexOf("//");
   pos = documentURL.indexOf("/", pos + 2);
   pos = documentURL.indexOf("/", pos + 2);
   calURL = documentURL.substring(0, pos) + "/CommonControl/Calendar/";
//	clockFocus = "NONE";
   clkID = formatID(clockID);
//	document.write("<SCRIPT LANGUAGE=javascript FOR=UDClock"+clkID+" EVENT=DownClick>");
//   document.write("UDClock_DownClick(\"" + clkID + "\")");
//   document.write("</SCRIPT>");
//   document.write("<SCRIPT LANGUAGE=javascript FOR=UDClock"+clkID+" EVENT=UpClick>");
//   document.write("UDClock_UpClick(\"" + clkID + "\")");
//   document.write("</SCRIPT>");
   content = "<table id = 'clockTable" + clkID + "' name='clockTable' style = 'HEIGHT: 20px' border=0 cellspacing=0 nowrap>";
	content += "<tr><td><font face=宋体 size=1>"
	content += "<table style='BACKGROUND-COLOR: " + backColor + ";" +
							 "BORDER-BOTTOM: #c0c0c0 1px solid;" +
							 "BORDER-LEFT: black 2px solid; " +
							 "BORDER-RIGHT: #c0c0c0 1px solid; " +
							 "BORDER-TOP: black 2px solid; " +
							 "HEIGHT: 16px; WIDTH: 36px'" +
					  "cellpadding=0 " +
					  "cellspacing=0>";
	content += "<tr><td>";
	content += "<INPUT id='tHour" + clkID + "' " +
					  "name='tHour" + clkID + " ' " +
					  "style='BACKGROUND-COLOR: " + backColor + ";" +
  					  " 	  BORDER-BOTTOM-WIDTH: 0px;" +
					  "		  BORDER-LEFT-WIDTH: 0px;" +
					  "		  BORDER-RIGHT-WIDTH: 0px; " +
					  "		  BORDER-TOP-WIDTH: 0px; " +
					  "		  HEIGHT: 16px; " +
					  "		  WIDTH: 16px'" +
					  " onfocus='tHour_onfocus(\"" + clkID + "\")' " +
					  " onkeyup = 'tHour_onkeyup(\"" + clkID + "\")' "+
					  " onblur = 'tHour_onblur(\"" + clkID + "\")' "+
					  "	value = '00'" +
//					  " onblur='tHour_onblur(\"" + clkID + "\")' " +			   
			  "	>";
	content += "</td><td>:</td><td>";
	content += "<INPUT id='tMinute" + clkID + "'"+
				  "	   name='tMinute'"+
   				  "	   style='BACKGROUND-COLOR: " + backColor + ";" +
				  "			  BORDER-BOTTOM-WIDTH: 0px;"+
				  "			  BORDER-LEFT-WIDTH: 0px;"+
				  "			  BORDER-RIGHT-WIDTH: 0px; "+
				  "			  BORDER-TOP-WIDTH: 0px; "+
				  "			  HEIGHT: 16px; "+
				  "			  WIDTH: 16px'"+
				  "	   onfocus='tMinute_onfocus(\"" + clkID + "\")' "+
				  "	   onkeyup = 'tMinute_onkeyup(\"" + clkID + "\")' " +
				  "	   onblur = 'tMinute_onblur(\"" + clkID + "\")' " +
				  "	   value = '00'" +
//				  "	   onblur='tMinute_onblur(\"" + clkID + "\")'"+
			  "	></td>";
	content += "</tr></table></FONT></td><td>";
   content += "<table id='UpDown" + clkID +"' width=13 height=16 CELLPADDING=0 CELLSPACING=0 >";
   content += "<tr><td><button id='UpKey"+ clkID +"' onClick='UDClock_UpClick(\"" + clkID + "\")' style='BORDER-LEFT: 0px; BORDER-TOP: 0px; HEIGHT: 10px; WIDTH: 13px' ><IMG src='" + calURL + "UpKey.gif' ></button></td></tr>";
   content += "<tr><td><button id='DownKey"+ clkID +"' onClick='UDClock_DownClick(\"" + clkID + "\")'style='BORDER-LEFT: 0px; BORDER-TOP: 0px; HEIGHT: 10px; WIDTH: 13px' ><IMG src='" + calURL + "DownKey.gif'></button></td></tr>";
   content += "</table>";
//	content += "<OBJECT classid=clsid:603C7E80-87C2-11D1-8BE3-0000F8754DA1 id='UDClock" + clkID + "' "+
//					"	style='HEIGHT: 20px; WIDTH: 16px' VIEWASTEXT >";
	content += "</td></tr></table>";
	target.innerHTML += content;
	if (typeof(bodyonclick) == "undefined")
   {
	   bodyonclick = document.body.onclick;
	   document.body.onclick = clock_onblur;
   }
   clock_hour = 0
   clock_minute = 0
}

function tHour_onfocus(clockID)
{
   var hour;
   clkID = formatID(clockID);
	var hour = parseInt(document.all("tHour" + clkID ).value, 10);
	if(document.all("tHour" + clkID ).value == "")
		hour = 0;
	clock_hour = hour;
	document.all("tHour" + clkID ).select();
	obFocusItem = document.all("tHour" + clkID );
	//obUD = document.all("UDClock" + clkID )
}
function tMinute_onfocus(clockID)
{
   var minute;
   clkID = formatID(clockID);
	var minute = parseInt(document.all("tMinute" + clkID ).value, 10);
	if(document.all("tMinute" + clkID ).value == "")
		minute = 0;
	clock_minute = minute;
	document.all("tMinute" + clkID ).select();
	obFocusItem = document.all("tMinute" + clkID );
	//obUD = document.all("UDClock" + clkID )
}
function clock_onblur()
{
	var e;
// clkID = formatID(clockID);
	e = window.event.srcElement;
	if (e != document.all("tHour" + clkID) && 
	    e != document.all("tMinute" + clkID ) && 
	    e != document.all("DownKey" + clkID) &&
	    e != document.all("UpKey" + clkID))
	{
		document.all("tHour" + clkID ).value = formatString(document.all("tHour" + clkID ).value, 2, "0");
		document.all("tMinute" + clkID ).value = formatString(document.all("tMinute" + clkID ).value, 2, "0");
		obFocusItem = null;
	}
	if(bodyonclick!=null)
	{
  	bodyonclick();
  }
}

function tHour_onkeyup(clockID)
{
   clkID = formatID(clockID);
	var hour = parseInt(document.all("tHour" + clkID ).value, 10);
	if(document.all("tHour" + clkID ).value == "")
		hour = 0;
	var strHour = String(hour);
	
	strHour = formatString(strHour, 2, "0");
	if(hour<24 && hour >=0 && (strHour==document.all("tHour" + clkID ).value || document.all("tHour" + clkID ).value.length <= 1))
	{
		clock_hour = hour;
	}
	else
	{
		document.all("tHour" + clkID ).value = String(clock_hour);
	}
}
function tMinute_onkeyup(clockID)
{
   clkID = formatID(clockID);
	var minute = parseInt(document.all("tMinute" + clkID ).value, 10);
	if(document.all("tMinute" + clkID ).value == "")
		minute = 0;
	var strMinute = String(minute);
	strMinute = formatString(strMinute, 2, "0");

	if(minute<=59 && minute >=0 && (strMinute==document.all("tMinute" + clkID ).value || document.all("tMinute" + clkID ).value.length <= 1))
	{
		clock_minute = minute;
	}
	else
	{
		document.all("tMinute" + clkID ).value = String(clock_minute);
	}
}
function formatString(strOrigin, bits, strStuff)
{
	var num = bits - strOrigin.length;
	var i;
	var strReturn = strOrigin;
	for (i = 0; i < num; i++)
	{
		strReturn = strStuff + strReturn;
	}
	return strReturn;
}
function tHour_onblur(clockID)
{
   clkID = formatID(clockID);
	document.all("tHour" + clkID ).value = formatString(document.all("tHour" + clkID ).value, 2, "0");
}
function tMinute_onblur(clockID)
{
   clkID = formatID(clockID);
	document.all("tMinute" + clkID ).value = formatString(document.all("tMinute" + clkID ).value, 2, "0");
}
function getTime(clockID)
{
   clkID = formatID(clockID);
   var strTime
   if(document.all("tHour" + clkID).disabled == false)
   {
	   strTime = formatString(document.all("tHour" + clkID ).value, 2, "0") + ":" + formatString(document.all("tMinute" + clkID ).value, 2, "0");
	}
	else
	{
	   strTime = String("NoTM");
	}
	return strTime;
}
function putTime(strTime, clockID)
{
	var regTime = /^(0?[0-9]|1[0-9]|2[0-3]):(0?[0-9]|[1-5][0-9])$/;
	var timeArray = regTime.exec(strTime);
   clkID = formatID(clockID);
	if(timeArray)
	{
		document.all("tHour" + clkID ).value = formatString(timeArray[1], 2, "0");
		document.all("tMinute" + clkID ).value = formatString(timeArray[2], 2, "0");
	}
}

function formatID(clockID)
{
	if(typeof(clockID) != "undefined" && typeof(clockID) != "null")
	{
      return(String(clockID))
	}
	else
	{
	   return("");
	}
}
function disableClock(clockID)
{
   clkID = formatID(clockID);
   document.all("tHour" + clkID).disabled = true;
   document.all("tMinute" + clkID).disabled = true;
   document.all("UpKey" + clkID).disabled = true;
   document.all("DownKey" + clkID).disabled = true;
}
function enableClock(clockID)
{
   clkID = formatID(clockID);
   document.all("tHour" + clkID).disabled = false;
   document.all("tMinute" + clkID).disabled = false;
   document.all("UpKey" + clkID).disabled = false;
   document.all("DownKey" + clkID).disabled = false;
}