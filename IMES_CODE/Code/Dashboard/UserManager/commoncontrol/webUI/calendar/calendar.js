//Calendar controller
// Create by Risun, 2000.7.25

var textoutObj;
var posObj;
var fireObj;
var calDisplaying;
var blCalReady = false;
function fun1()
{
	
	if(blCalReady == false)
	{
		setTimeout("fun1()", 1000);
	}
	else
	{
		window.close();
	}
}
function finishCalFrame()
{
    //alert("finishCalFrame");
    //while(blCalReady == false)
    //{
		//alert("finishCalFrame: " + blCalReady);
    //}
    if(blCalReady == false)
	{
		//setTimeout("fun1()", 5000);
		event.returnValue = "正在读取数据。";
	}
}

function CalDisappear()
{
	var e 
   if(blCalReady == false)
	{
      return;
	}
	if (CalDisappear.caller != null && fireObj != null)
	{
	  	e = window.event.srcElement;
	  	if ((e.id != fireObj.id))
	  	{ 
	  		document.all.position.style.display ="none";
	  	}
  }
	else if (calDisplaying != true)
	{
		document.all.position.style.display = "none";
	}
	else if (calDisplaying == true)
	{
		calDisplaying = false;
	}
}

//------------------------------------------------------------------------
//
//  Function:  initCalFrame
//
//  Synopsis:  initCalFrame function is used insert a <div> tag with
//             a <iframe>, the iframe including the SubFrameCal.htm
//             file is used to show the calendar.
//
//  Arguments: 
//
//  Returns:   none
//
//  Notes:     none
//  Update:    2001-11-1
//------------------------------------------------------------------------
function initCalFrame(controlPath, zOrder)
{
   var content;
   var documentURL = document.URL;
   var calURL;
   var pos;
   var zOd;
   zOd=999
   if(typeof(zOrder)=="number")
   {
      zOd = zOrder
   }
   blCalReady = false;
   if(typeof(controlPath)!="string")
   {
      pos = documentURL.indexOf("//");
      pos = documentURL.indexOf("/", pos + 2);
      pos = documentURL.indexOf("/", pos + 2);
      calURL = documentURL.substring(0, pos) + "/commoncontrol/webUI/calendar/SubFrameCal.htm";
   }
   else
   {//为增加通用性，增加可选参数controlPath以适应控件放在不同路径下。 Risun 2001-10-29
      calURL = controlPath + "SubFrameCal.htm"
   }
   document.write("<div id='position' style='DISPLAY: none; POSITION: absolute; TOP: 0px; LEFT: 0px; z-index: " + String(zOd) + "' ></div>");

   content = "<iframe id='CalFrame' src='" + calURL + "' width = '200' height = '166' scrolling='no' frameBorder='0' style='Z-INDEX:" + String(zOd) + "; margin-top:0px; margin-left:0px' >";
   content += "</iframe>";
   //alert(typeof(window));
   //alert(content);
   document.all.position.innerHTML=content;
}

//------------------------------------------------------------------------
//
//  Function:  showCalFrame
//
//  Synopsis:  showCalFrame function is used to show calendar frame at 
//             HTML page. Allow user to pick a date which is displayed
//             at 'obj' object.
//
//  Arguments: obj - the object used to display the date user picked.
//             frameTop, frameLeft - position of the calendar showwing.
//                                   If they are missed, the calendar will
//                                   will show below the 'obj'.
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------
function showCalFrame(obj, frameTop, frameLeft)
{
	var  ExtrasX = 0;
	var  ExtrasY = 0;
	var  Pele;

  if(blCalReady == false)
	{
      alert("Data loading, wait moment.")
      return
	}
  //====================================================
  //Risun added for multi language (charset), 2001.7.21.
  //----------------------------------------------------
  /* 由于用户可以在日历内部右键改变字符集，故将检测字符集的功能放在htc内部处理，删掉下面内容。
  
  switch(document.charset)
  {
    case "gb2312":
      setCalLanguage("simplified")
      break
    case "big5":
      setCalLanguage("traditional")
      break
    default:
      setCalLanguage("english")
  }
  */
  //checkCharset()
  //----------------------------------------------------
  if (document.all.position.style.display == '')
  {
      document.all.position.style.display = 'none';
  		calDisplaying = false;
      return;
  }
	if (showCalFrame.caller != null)
	{
		fireObj = window.event.srcElement;
	   
	}
	else
	{
		fireObj = null;
		calDisplaying = true;
	}	
	if(showCalFrame.arguments.length > 2)
	{
		ExtrasX = frameLeft;
		ExtrasY = frameTop;
	}
	else
	{
    /*******************************************************/
    // 为保证位置计算正确，将所右parentNode都设成可见，后面再复原
    //  Risun 2001.10.5
    //var tmpObj
    //var visibleArray=new Array()
    //var icount=0
    //tmpObj = obj
    //while(tmpObj!=null)
    //{
   
    //   visibleArray[icount]=tmpObj.style.display
    //   tmpObj.style.display=""
    //   tmpObj=tmpObj.parentNode
    //   icount++
    //}
    /*******************************************************/
		Pele = obj;
		//	alert(Pele.clientWidth)
		
		do
		{
			//if(Pele.style.position != "relative")
			//{
			  ExtrasX += Pele.offsetLeft + Pele.clientLeft;
			  ExtrasY += Pele.offsetTop + Pele.clientTop;
			//}
			//else//补钉，针对表格的SPAN
			//{
			//  ExtrasY += 21
			//}
/*
alert(Pele.tagName + " " + Pele.id + ":\n" + 
      "position: " + Pele.style.position + "\n" +
      "offset: " + String(Pele.offsetLeft) + ", " + String(Pele.offsetTop) + "\n" +
      "client: " + String(Pele.clientLeft) + ", " + String(Pele.clientTop) + "\n" + 
      "style_display: " + String(Pele.style.display) + "\n" +
      "offsetWH: " + String(Pele.offsetWidth) + ", " + String(Pele.offsetHeight) + "\n" +
      "Extras:" + String(ExtrasX) + ", " + String(ExtrasY))
*/
			Pele = Pele.offsetParent;
		} //accumulate values for parents offsets and borders
		while (Pele.tagName != "BODY")

    /*******************************************************/
    // 复原是否可见
    //  Risun 2001.10.5
    //icount=0
    //tmpObj=obj
    //while(tmpObj!=null)
    //{
    //   tmpObj.style.display=visibleArray[icount]
    //   icount++
    //   tmpObj=tmpObj.parentNode
    //}    
    /*******************************************************/
		//*********************************
		
		//应刘副理要求，做的如下改动。
		//功能：将被窗口覆盖的反倒相反的方向。	

		if ((ExtrasY<166)&&(Pele.clientWidth<600))
		{
		   ExtrasX=ExtrasX;
		   ExtrasY=ExtrasY;
		   ExtrasX += Pele.document.parentWindow.screenLeft - window.screenLeft;
       ExtrasY += Pele.document.parentWindow.screenTop - window.screenTop;
		}
		   
		else
		{
		  if (((ExtrasX + 200) < Pele.clientWidth) &&((ExtrasY+166+obj.offsetHeight+3)<Pele.clientHeight))
		  {
		     ExtrasX=ExtrasX;
		     ExtrasY=ExtrasY;
 
		  }
		  /*

		  //判断是否超出了右边界
		  if (((ExtrasX+200)>Pele.clientWidth)&&((ExtrasY+166)<Pele.clientHeight))
		  {
		  	ExtrasX=Pele.clientWidth-200-3;
		  	ExtrasY=ExtrasY;
		  }
		  //判断是否超出了下边界
		  if (((ExtrasY+166+obj.offsetHeight+3)>Pele.clientHeight)&&((ExtrasX+200)<Pele.clientWidth))
		  {
		  	ExtrasX =ExtrasX;
		  	ExtrasY=ExtrasY-166-18-obj.offsetHeight;
		  }
		  //判断是否超出了右边界和下边界
		  if (((ExtrasX+200)>Pele.clientWidth)&&((ExtrasY+166+3+obj.offsetHeight)>Pele.clientHeight))
		  {
		  	ExtrasX=Pele.clientWidth-200-3;
		  	ExtrasY=ExtrasY-166-18-obj.offsetHeight;
		  }
		   */
		  ExtrasX = ExtrasX + Pele.document.parentWindow.screenLeft - window.screenLeft;
		  ExtrasY = ExtrasY + Pele.document.parentWindow.screenTop - window.screenTop;

		}
		//alert("Extras:" + String(ExtrasX) + ", " + String(ExtrasY));
  	//**********************************
	}		

	document.all("CalFrame").width = document.frames("CalFrame").document.all("cal").style.pixelWidth;
	document.all("CalFrame").height = document.frames("CalFrame").document.all("cal").style.pixelHeight + 6;
	//document.all.position.style.top = obj.offsetHeight + ExtrasY + 3;
	document.all.position.style.top = document.body.scrollTop + ExtrasY + obj.offsetHeight + 3;
	document.all.position.style.left = document.body.scrollLeft + ExtrasX;
	/*
alert("position.style: " + document.all.position.style.left + ", " + document.all.position.style.top + "\n" +
      "document.body.scroll: " + document.body.scrollLeft + ", " + document.body.scrollTop + "\n" +
      "Extras: " + ExtrasX + ", " + ExtrasY  + "\n" +
      "obj.offsetHeight:" + obj.offsetHeight)
	*/
	if (obj.tagName == "INPUT")
		setCalDate(obj.value);
	else
		setCalDate(obj.innerText);

    if(!obj.disabled && !obj.readOnly){
        obj.focus();
    }

    document.all.position.style.display = '';
	textoutObj = obj;

}
//------------------------------------------------------------------------
//
//  Function:  setCalLanguage(strLanguage)
//
//  Synopsis:  setCalLanguage function is used to set char's language
//             of the calendar.
//
//  Arguments: strLangauge - language of char: 
//             'english' = English
//             'simplified' = simplified  Chinese
//             'traditional' = traditional Chinese
//
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------
function setCalLanguage(strLanguage)
{
  if(blCalReady == false)
	{
      alert("Data loading, wait moment.")
	}
  document.frames.CalFrame.cal.language=strLanguage
}

//------------------------------------------------------------------------
//
//  Function:  checkCharset()
//
//  Synopsis:  checkCharset function is used to check the charset IE used
//             and set the language property of Calendar htc.
//
//  Arguments: none
//
//
//  Returns:   none
//
//  Notes:     none
//
//------------------------------------------------------------------------
function checkCharset()
{
  //====================================================
  //Risun added for multi language (charset), 2001.7.21.
  //----------------------------------------------------
  switch(document.charset)
  {
    case "gb2312":
      setCalLanguage("simplified")
      break
    case "big5":
      setCalLanguage("traditional")
      break
    default:
      setCalLanguage("english")
  }
  //----------------------------------------------------

}
/*
function showCalWindow(obj)
{
	var szUrl;
	var szFeatures;

	szUrl = 'http://dtmanager-001/TTLManager/Calendar/SubCal.htm';
	szFeatures = 'dialogWidth:250px; dialogHeight:250px; status:0; help:0';

	var str;
	str = window.showModalDialog(szUrl, 0, szFeatures);
	if(str!=null)
		obj.value = str;
}

function initCalendar()
{
   var content;
   document.write("<div id='position' style='DISPLAY: none; POSITION: absolute; TOP: 0px; LEFT: 0px;' ></div>");
   //content = "<div id='Add' style='HEIGHT: 0px; LEFT: 0px; POSITION: static; TOP: 0px; WIDTH: 0px;'>";
   content="<table class='Dialog' border='0' cellPadding='2' cellSpacing='0' id='Dialog1'>";
   content+="<tr><td LANGUAGE='javascript' onclick='Calendar_onclick()'>";
   content+="		<CENTER>";
   content+="		<IE:Calendar id='cal' style='width : 200; height : 133; border : 1px solid black;' > </IE:Calendar>";
   content+="		</CENTER>";
   content+="	</td></tr>";
   content+="</table>";
   //content+="</div>";
   document.all.position.innerHTML=content;
   body.onclick = "alert('body_onclick')";
}
*/
function Calendar_onclick()
{
   var e = window.event.srcElement

   if (e.tagName == "TD") 
   {
      if ( !e.day ) return  // The calendar is read only
      textoutObj.value = cal.year + "-" + cal.month + "-" + cal.day;
      document.all.position.style.display = 'none';
   }
}
/*
function showCalendar(textout)
{
   textoutObj = textout;
   document.all.position.style.top = document.body.scrollTop + (document.body.clientHeight - parseInt(document.all.cal.style.height)) / 2;
   document.all.position.style.left = document.body.scrollLeft + (document.body.clientWidth - parseInt(document.all.cal.style.width)) / 2;
   //document.all.Add.style.display = '';
   document.all.position.style.display = '';      
}
*/
//document.write("<SCRIPT FOR=window EVENT=onclick language='JavaScript'>");
//document.write("CalDisappear();");
//document.write("</SCRIPT>");
