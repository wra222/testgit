﻿/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: formula editor
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2008-12-17   itc98047        replace old cal with this one.
 * 2013-05-02   Li,Vincent      support all browser
 * Known issues:Any restrictions about this file 
 *              1 xxxxxxxx
 *              2 xxxxxxxx
 */

function Initial() {
    var userAgent = navigator.userAgent.toLowerCase();
    if (/msie/.test(userAgent)) 
    {
        var _VersionInfo="Version:2.1&#13.2.1作者hy&#13;2.0作者:walkingpoison&#13;1.0作者: F.R.Huang(meizz)&#13;MAIL: meizz@hzcnc.com"	//版本信息

		//==================================================== WEB 页面显示部分 =====================================================
		var strFrame;		//存放日历层的HTML代码
		document.writeln('<iframe id=meizzDateLayer Author=wayx frameborder=0 bgcolor=red style="position: absolute; width: 160px; height: 182px;z-index: 9998; display: none"></iframe>');
		strFrame='<style>';
		strFrame+='INPUT.button{BORDER-RIGHT: #999999 1px solid;BORDER-TOP: white 1px solid;BORDER-LEFT: white 1px solid;';
		strFrame+='BORDER-BOTTOM: #999999 1px solid;BACKGROUND-COLOR:rgb(169,228,234);font-family:宋体;}';
		strFrame+='TD{FONT-SIZE: 9pt;font-family:宋体;}';
		strFrame+='</style>';
		strFrame+='<scr' + 'ipt>';
		strFrame+='var datelayerx,datelayery;	/*存放日历控件的鼠标位置*/';
		strFrame+='var bDrag;	/*标记是否开始拖动*/';
		strFrame+='function document.onmousemove()	/*在鼠标移动事件中，如果开始拖动日历，则移动日历*/';
		strFrame+='{if(bDrag && window.event.button==1)';
		strFrame+='	{var DateLayer=parent.document.all.meizzDateLayer.style;';
		strFrame+='		DateLayer.posLeft += window.event.clientX-datelayerx;/*由于每次移动以后鼠标位置都恢复为初始的位置，因此写法与div中不同*/';
		strFrame+='		DateLayer.posTop += window.event.clientY-datelayery;}}';
		strFrame+='function DragStart()		/*开始日历拖动*/';
		strFrame+='{var DateLayer=parent.document.all.meizzDateLayer.style;';
		strFrame+='	datelayerx=window.event.clientX;';
		strFrame+='	datelayery=window.event.clientY;';
		strFrame+='	bDrag=true;}';
		strFrame+='function DragEnd(){		/*结束日历拖动*/';
		strFrame+='	bDrag=false;}';
		strFrame+='</scr' + 'ipt>';
		strFrame+='<div style="z-index:9999;position: absolute; left:0; top:0;" onselectstart="return false"><span id=tmpSelectYearLayer Author=wayx style="z-index: 9999;position: absolute;top: 3; left: 19;display: none"></span>';
		strFrame+='<span id=tmpSelectMonthLayer Author=wayx style="z-index: 9999;position: absolute;top: 3; left: 78;display: none"></span>';
		strFrame+='<table style="border:1 outset" cellspacing=0 cellpadding=0 width="160" height=160 bordercolor="#A9A9A9" bgcolor=#999999 Author="wayx">';
		strFrame+='  <tr Author="wayx"><td   height=20 Author="wayx" bgcolor=#FFFFFF><table border=0 cellspacing=1 cellpadding=0 width=100% Author="wayx" height=20>';
		strFrame+='      <tr align=center Author="wayx"><td width=16 align=center bgcolor="rgb(169,228,234)" style="font-size:12px;cursor: hand;color: #ffffff" ';
		strFrame+='        onclick="parent.meizzPrevM()" title="向前翻 1 月" Author=meizz><b Author=meizz>&lt;</b>';
		strFrame+='        </td><td width=60 align=center style="font-size:12px;cursor:default" Author=meizz ';
		strFrame+='onmouseover="style.backgroundColor=\'#D4D0C8\'" onmouseout="style.backgroundColor=\'white\'" ';
		strFrame+='onclick="parent.tmpSelectYearInnerHTML(this.innerText.substring(0,4))" title="点击这里选择年份"><span Author=meizz id=meizzYearHead></span></td>';
		strFrame+='<td width=48 align=center style="font-size:12px;cursor:default" Author=meizz onmouseover="style.backgroundColor=\'#D4D0C8\'" ';
		strFrame+=' onmouseout="style.backgroundColor=\'white\'" onclick="parent.tmpSelectMonthInnerHTML(this.innerText.length==3?this.innerText.substring(0,1):this.innerText.substring(0,2))"';
		strFrame+='        title="点击这里选择月份"><span id=meizzMonthHead Author=meizz></span></td>';
		strFrame+='        <td width=16 bgcolor="rgb(169,228,234)" align=center style="font-size:12px;cursor: hand;color: #ffffff" ';
		strFrame+='         onclick="parent.meizzNextM()" title="向后翻 1 月" Author=meizz><b Author=meizz>&gt;</b></td></tr>';
		strFrame+='    </table></td></tr>';
		strFrame+='  <tr Author="wayx"><td height=18 Author="wayx">';
		strFrame+='<table style="border:1;width=\"100%\"" cellspacing=0 cellpadding=0 bgcolor="rgb(169,228,234)" ' + (bMoveable? 'onmousedown="DragStart()" onmouseup="DragEnd()"':'');
		strFrame+=' BORDERCOLORLIGHT=#999999 BORDERCOLORDARK=#FFFFFF width=100% height=18 Author="wayx" style="cursor:' + (bMoveable ? 'move':'default') + '">';
		strFrame+='<tr Author="wayx" align=center valign=bottom><td style="font-size:12px;color:#800080" Author=meizz>日</td>';
		strFrame+='<td style="font-size:12px;color:#800080" Author=meizz>一</td><td style="font-size:12px;color:#800080" Author=meizz>二</td>';
		strFrame+='<td style="font-size:12px;color:#800080" Author=meizz>三</td><td style="font-size:12px;color:#800080" Author=meizz>四</td>';
		strFrame+='<td style="font-size:12px;color:#800080" Author=meizz>五</td><td style="font-size:12px;color:#800080" Author=meizz>六</td></tr>';
		strFrame+='</table></td></tr><!-- Author:F.R.Huang(meizz) http://www.meizz.com/ mail: meizz@hzcnc.com 2002-10-8 -->';
		strFrame+='  <tr Author="wayx"><td     Author="wayx">';
		strFrame+='    <table style="border:1" cellspacing=2 cellpadding=0 BORDERCOLORLIGHT=#999999 BORDERCOLORDARK=#FFFFFF bgcolor=#fff8ec  Author="wayx">';
		var n=0; for (j=0;j<5;j++){ strFrame+= ' <tr align=center Author="wayx">'; for (i=0;i<7;i++){
		strFrame+='<td width=21 height=18 id=meizzDay'+n+'  Author=meizz onclick=parent.meizzDayClick(this.innerText,0)></td>';n++;}
		strFrame+='</tr>';}
		strFrame+='      <tr align=center Author="wayx">';
		for (i=35;i<39;i++)strFrame+='<td width=21 height=18 id=meizzDay'+i+'  Author=wayx onclick="parent.meizzDayClick(this.innerText,0)"></td>';
		strFrame+='        <td colspan=3 align=right Author=meizz><span onclick=parent.closeLayer() style="font-size:12px;cursor: hand"';
		strFrame+='         Author=meizz title="' + _VersionInfo + '"><u>关闭</u></span>&nbsp;</td></tr>';
		strFrame+='    </table></td></tr><tr Author="wayx"><td Author="wayx">';
		strFrame+='        <table border=0 cellspacing=1 cellpadding=0 width=100% Author="wayx" bgcolor="rgb(169,228,234)">';
		strFrame+='          <tr Author="wayx"><td Author=meizz align=left><input Author=meizz type=button class=button value="<<" title="向前翻 1 年" onclick="parent.meizzPrevY()" ';
		strFrame+='             onfocus="this.blur()" style="font-size: 12px; height: 17px"><input Author=meizz class=button title="向前翻 1 月" type=button ';
		strFrame+='             value="< " onclick="parent.meizzPrevM()" onfocus="this.blur()" style="font-size: 12px; height: 17px"></td><td ';
		strFrame+='             Author=meizz align=center><input Author=meizz type=button class=button value=Today onclick="parent.meizzToday()" ';
		strFrame+='             onfocus="this.blur()" title="当前日期" style="font-size: 12px; height: 17px; cursor:hand"></td><td ';
		strFrame+='             Author=meizz align=right><input Author=meizz type=button class=button value=" >" onclick="parent.meizzNextM()" ';
		strFrame+='             onfocus="this.blur()" title="向后翻 1 月" class=button style="font-size: 12px; height: 17px"><input ';
		strFrame+='             Author=meizz type=button class=button value=">>" title="向后翻 1 年" onclick="parent.meizzNextY()"';
		strFrame+='             onfocus="this.blur()" style="font-size: 12px; height: 17px"></td>';
		strFrame+='</tr></table></td></tr></table></div>';

		window.frames.meizzDateLayer.document.writeln(strFrame);
		window.frames.meizzDateLayer.document.close();		//解决ie进度条不结束的问题

		//==================================================== WEB 页面显示部分 ======================================================
		return window.frames.meizzDateLayer.document.all;
    } else {
		var _VersionInfo="Version:2.1&#13.2.1作者hy&#13;2.0作者:walkingpoison&#13;1.0作者: F.R.Huang(meizz)&#13;MAIL: meizz@hzcnc.com"	//版本信息

		//==================================================== WEB 页面显示部分 =====================================================
		var strFrame;		//存放日历层的HTML代码
		//document.writeln('<iframe id="meizzDateLayer" Author=wayx frameborder=0 bgcolor=red style="position: absolute; width: 160px; height: 182px;z-index: 9998; display: none"></iframe>');
		var odatelayer;
		var iframe = document.createElement("iframe");
		iframe.setAttribute("id", "meizzDateLayer");
		iframe.setAttribute("Author", "wayx");
		iframe.setAttribute("frameborder", "0px");
		iframe.setAttribute("bgcolor", "red");
		iframe.setAttribute("style", "position: absolute; width:165px; height:198px;z-index: 9998; display: none;");


		strFrame='<style>';
		strFrame+='INPUT.button{BORDER-RIGHT: #999999 1px solid;BORDER-TOP: white 1px solid;BORDER-LEFT: white 1px solid;';
		strFrame+='BORDER-BOTTOM: #999999 1px solid;BACKGROUND-COLOR:rgb(169,228,234);font-family:宋体;}';
		strFrame+='TD{FONT-SIZE: 9pt;font-family:宋体;}';
		strFrame+='</style>';
		//strFrame+='<scr' + 'ipt>';
		//strFrame+='var datelayerx,datelayery;	/*存放日历控件的鼠标位置*/';
		//strFrame+='var bDrag;	/*标记是否开始拖动*/';
		//strFrame+='function document.onmousemove()	/*在鼠标移动事件中，如果开始拖动日历，则移动日历*/';
		//strFrame+='{if(bDrag && window.event.button==1)';
		//strFrame += '	{var DateLayer=parent document.getElementsByTagName("*").meizzDateLayer.style;';
		//strFrame+='		DateLayer.posLeft += window.event.clientX-datelayerx;/*由于每次移动以后鼠标位置都恢复为初始的位置，因此写法与div中不同*/';
		//strFrame+='		DateLayer.posTop += window.event.clientY-datelayery;}}';
		//strFrame+='function DragStart()		/*开始日历拖动*/';
		//strFrame += '{var DateLayer=parent document.getElementsByTagName("*").meizzDateLayer.style;';
		//strFrame+='	datelayerx=window.event.clientX;';
		//strFrame+='	datelayery=window.event.clientY;';
		//strFrame+='	bDrag=true;}';
		//strFrame+='function DragEnd(){		/*结束日历拖动*/';
		//strFrame+='	bDrag=false;}';
		//strFrame+='</scr' + 'ipt>';
		strFrame += '<script> ';
		strFrame += ' var datelayerx,datelayery;';
		strFrame += ' var bDrag;';
		strFrame += '  document.onmousemove=function(){	';
		strFrame += '  var event; if (window.event) { event = window.event;  } else {event = arguments.callee.caller.arguments[0];} if (bDrag && event.button==1)';
		strFrame += '	{var DateLayer=parent.document.getElementsByTagName("*").meizzDateLayer.style;';
		strFrame += '		DateLayer.left = (parseInt(DateLayer.left,10) + event.clientX-datelayerx).toString() +"px"; ';
		strFrame += '		DateLayer.top = (parseInt(DateLayer.top,10) +event.clientY-datelayery).toString()+"px"; }}; ';
		strFrame += '  function DragStart(e){';
		strFrame += ' var event; if (window.event) { event = window.event;  } else {event = e;} var DateLayer=parent.document.getElementsByTagName("*").meizzDateLayer.style;';
		strFrame += '	datelayerx=event.clientX;';
		strFrame += '	datelayery=event.clientY;';
		strFrame += '	bDrag=true; }';
		strFrame += '  function DragEnd(){';
		strFrame += '	bDrag=false;} ';
		strFrame += ' </script>';
		strFrame+='<div style="z-index:9999;position: absolute; left:0px; top:0px;" onselectstart="return false;"><span id=tmpSelectYearLayer Author=wayx style="z-index: 9999;position: absolute;top:3px; left: 19px;display: none"></span>';
		strFrame+='<span id=tmpSelectMonthLayer Author=wayx style="z-index: 9999;position: absolute;top: 3; left: 78;display: none"></span>';
		strFrame+='<table style="border:1 outset" cellspacing=0 cellpadding=0 width="160px" height="160px" bordercolor="#A9A9A9" bgcolor=#999999 Author="wayx">';
		strFrame+='  <tr Author="wayx"><td   height=20 Author="wayx" bgcolor=#FFFFFF><table border=0 cellspacing=1 cellpadding=0 width=100% Author="wayx" height=20>';
		strFrame+='      <tr align=center Author="wayx"><td width=16 align=center bgcolor="rgb(169,228,234)" style="font-size:12px;cursor: hand;color: #ffffff" ';
		strFrame+='        onclick="parent.meizzPrevM()" title="向前翻 1 月" Author=meizz><b Author=meizz>&lt;</b>';
		strFrame+='        </td><td width="60px" align=center style="font-size:12px;cursor:default" Author=meizz ';
		strFrame+='onmouseover="style.backgroundColor=\'#D4D0C8\'" onmouseout="style.backgroundColor=\'white\'" ';
		strFrame+='onclick="parent.tmpSelectYearInnerHTML(innerText(this).substring(0,4))" title="点击这里选择年份"><span Author=meizz id=meizzYearHead></span></td>';
		strFrame+='<td width=48 align=center style="font-size:12px;cursor:default" Author=meizz onmouseover="style.backgroundColor=\'#D4D0C8\'" ';
		strFrame += ' onmouseout="style.backgroundColor=\'white\'" onclick="parent.tmpSelectMonthInnerHTML(innerText(this).length==3?innerText(this).substring(0,1):innerText(this).substring(0,2))"';
		strFrame+='        title="点击这里选择月份"><span id=meizzMonthHead Author=meizz></span></td>';
		strFrame+='        <td width=16 bgcolor="rgb(169,228,234)" align=center style="font-size:12px;cursor: hand;color: #ffffff" ';
		strFrame+='         onclick="parent.meizzNextM()" title="向后翻 1 月" Author=meizz><b Author=meizz>&gt;</b></td></tr>';
		strFrame+='    </table></td></tr>';
		strFrame+='  <tr Author="wayx"><td height=18 Author="wayx">';
		strFrame+='<table style="border:1;width=\"100%\"" cellspacing=0 cellpadding=0 bgcolor="rgb(169,228,234)" ' + (bMoveable? 'onmousedown="DragStart(event)" onmouseup="DragEnd(event)"':'');
		strFrame+=' BORDERCOLORLIGHT=#999999 BORDERCOLORDARK=#FFFFFF width=100% height=18 Author="wayx" style="cursor:' + (bMoveable ? 'move':'default') + '">';
		strFrame+='<tr Author="wayx" align=center valign=bottom><td style="font-size:12px;color:#800080" Author=meizz>日</td>';
		strFrame+='<td style="font-size:12px;color:#800080" Author=meizz>一</td><td style="font-size:12px;color:#800080" Author=meizz>二</td>';
		strFrame+='<td style="font-size:12px;color:#800080" Author=meizz>三</td><td style="font-size:12px;color:#800080" Author=meizz>四</td>';
		strFrame+='<td style="font-size:12px;color:#800080" Author=meizz>五</td><td style="font-size:12px;color:#800080" Author=meizz>六</td></tr>';
		strFrame+='</table></td></tr><!-- Author:F.R.Huang(meizz) http://www.meizz.com/ mail: meizz@hzcnc.com 2002-10-8 -->';
		strFrame+='  <tr Author="wayx"><td     Author="wayx">';
		strFrame+='    <table style="border:1" cellspacing=2 cellpadding=0 BORDERCOLORLIGHT=#999999 BORDERCOLORDARK=#FFFFFF bgcolor=#fff8ec  Author="wayx">';
		var n=0; for (j=0;j<5;j++){ strFrame+= ' <tr align=center Author="wayx">'; for (i=0;i<7;i++){
		strFrame += '<td width="21px" height="18px" id=meizzDay' + n + '  Author=meizz onclick=parent.meizzDayClick(innerText(this),0)></td>'; n++;
		}
		strFrame+='</tr>';}
		strFrame+='      <tr align=center Author="wayx">';
		for (i = 35; i < 39; i++) strFrame += '<td width="21px" height="18px" id=meizzDay' + i + '  Author=wayx onclick="parent.meizzDayClick(innerText(this),0)"></td>';
		strFrame+='        <td colspan=3 align=right Author=meizz><span onclick=parent.closeLayer() style="font-size:12px;cursor: hand"';
		strFrame+='         Author=meizz title="' + _VersionInfo + '"><u>关闭</u></span>&nbsp;</td></tr>';
		strFrame+='    </table></td></tr><tr Author="wayx"><td Author="wayx">';
		strFrame+='        <table border=0 cellspacing=1 cellpadding=0 width=100% Author="wayx" bgcolor="rgb(169,228,234)">';
		strFrame+='          <tr Author="wayx"><td Author=meizz align=left><input Author=meizz type=button class=button value="<<" title="向前翻 1 年" onclick="parent.meizzPrevY()" ';
		strFrame+='             onfocus="this.blur()" style="font-size: 12px; height: 17px"><input Author=meizz class=button title="向前翻 1 月" type=button ';
		strFrame+='             value="< " onclick="parent.meizzPrevM()" onfocus="this.blur()" style="font-size: 12px; height: 17px"></td><td ';
		strFrame+='             Author=meizz align=center><input Author=meizz type=button class=button value=Today onclick="parent.meizzToday()" ';
		strFrame+='             onfocus="this.blur()" title="当前日期" style="font-size: 12px; height: 17px; cursor:hand"></td><td ';
		strFrame+='             Author=meizz align=right><input Author=meizz type=button class=button value=" >" onclick="parent.meizzNextM()" ';
		strFrame+='             onfocus="this.blur()" title="向后翻 1 月" class=button style="font-size: 12px; height: 17px"><input ';
		strFrame+='             Author=meizz type=button class=button value=">>" title="向后翻 1 年" onclick="parent.meizzNextY()"';
		strFrame+='             onfocus="this.blur()" style="font-size: 12px; height: 17px"></td>';
		strFrame+='</tr></table></td></tr></table></div>';

		document.body.appendChild(iframe);
		var objDoc = (iframe.contentWindow || iframe.contentDocument);
		if (objDoc.document) {
			objDoc = objDoc.document;
		}
		objDoc.open();
		objDoc.writeln(strFrame);
		objDoc.close();
		return objDoc.getElementsByTagName("*"); 
        
    }
}
//==================================================== 参数设定部分 =======================================================
var bMoveable=true;		//设置日历是否可以拖动
odatelayer = Initial();

//window.frames.meizzDateLayer.document.writeln(strFrame);
//window.frames.meizzDateLayer.document.close();		//解决ie进度条不结束的问题

//==================================================== WEB 页面显示部分 ======================================================
function innerText(obj, value){
        if(document.all){
            if(typeof(value)=="undefined"){
                return obj.innerText;
            }else{
                obj.innerText=value;
            }
        }else{
            if(typeof(value)=="undefined"){
                return obj.textContent;
            }else{
                return obj.textContent=value;
            }
        }
    }
var outObject;
var outButton;		//点击的按钮
var outDate="";		//存放对象的日期
//var odatelayer=window.frames.meizzDateLayer.document.all;		//存放日历对象
//var odatelayer = window.frames.meizzDateLayer.document.getElementsByTagName("*");
function showCalendar(showDataArea, showTop, showLeft) {
    var evt;
    if (window.event) {
        evt = window.event;
    } else {
    evt = arguments.callee.caller.arguments[0];
    }

    setday(evt, showDataArea, showTop, showLeft);
}

function initCalFrame()
{}

function CalDisappear()
{}
 
//function setday(tt,obj) //主调函数
function setday(evt ,showDataArea, showTop, showLeft) //主调函数
{
//	if (arguments.length >  2){alert("对不起！传入本控件的参数太多！");return;}
//	if (arguments.length == 0){alert("对不起！您没有传回本控件任何参数！");return;}
    //var dads  = document.all.meizzDateLayer.style;
    var dads = document.getElementsByTagName("*").meizzDateLayer.style;
    var tt;
    if (window.event) {
        tt = window.event.srcElement;
    } else {        
        tt = evt.target;            
    }
    //var tt = window.event.srcElement;
    
	var th = tt;
	var ttop  = tt.offsetTop;     //TT控件的定位点高
	var thei  = tt.clientHeight;  //TT控件本身的高
	var tleft = tt.offsetLeft;    //TT控件的定位点宽
	var ttyp = tt.type;          //TT控件的类型
	//alert(" tt.offsetTop:" + tt.offsetTop + " tt.clientHeight:" + tt.clientHeight + " tt.offsetLeft:" + tt.offsetLeft + " tt.type:" + tt.type);
	//while (tt = tt.offsetParent){ttop+=tt.offsetTop; tleft+=tt.offsetLeft;}
	while (tt = tt.offsetParent){ttop+=tt.offsetTop-tt.scrollTop; tleft+=tt.offsetLeft;}
	//alert("ttop:" + ttop + "tleft:" + tleft);
	if (showTop != null && showLeft != null)
	{
	    dads.top  = showTop;
	    dads.left = showLeft;
    } else {
    dads.top = ((ttyp == "image") ? ttop + thei : ttop + thei + 6).toString() + 'px';
	    dads.left = tleft.toString()+'px';
    	
    
    }

    //add by lzy

    var redundantWidth = (parseInt(dads.left, 10) + 160) - document.body.clientWidth;
    //alert("redundantWidth:" + redundantWidth);
	if (redundantWidth > 0) {

	    var posLeft = dads.posLeft;
	   
	    posLeft -= redundantWidth;
	    dads.left = posLeft.toString() +'px';
	}

	var redundantHeight = (parseInt(dads.top, 10) + 182) - document.body.clientHeight;
	//alert("redundantHeight:" + redundantHeight);
	
	if (redundantHeight > 0) {
	    var posTop = parseInt(dads.top,10);
	   
	    posTop -= redundantHeight;
	    dads.top = posTop.toString() +'px';
	}
	var obj = null;
	if (arguments.length > 0 && typeof(showDataArea) == "string") 
//	    obj = eval(showDataArea);
        obj = document.getElementById(showDataArea);	    
	else
	    obj = th.previousSibling;
//end	    
	outObject = (obj.tagName != "INPUT") ? th : obj;
	outButton = (obj.tagName != "INPUT") ? null : th;	//设定外部点击的按钮
//	outObject = (arguments.length == 1) ? th : obj;
//	outButton = (arguments.length == 1) ? null : th;	//设定外部点击的按钮
	
	
	
	//根据当前输入框的日期显示日历的年月
	var reg = /^(\d+)-(\d{1,2})-(\d{1,2})$/;
	var r = outObject.value.match(reg);
	if(r!=null){
		r[2]=r[2]-1;
		var d= new Date(r[1], r[2],r[3]);
		if(d.getFullYear()==r[1] && d.getMonth()==r[2] && d.getDate()==r[3]){
			outDate=d;		//保存外部传入的日期
		}
		else outDate="";
			meizzSetDay(r[1],r[2]+1);
	}
	else{
		outDate="";
		meizzSetDay(new Date().getFullYear(), new Date().getMonth() + 1);
	}
	dads.display = '';
	
	evt = window.event ? window.event : evt;
	evt.returnValue = false;
}

var MonHead = new Array(12);    		   //定义阳历中每个月的最大天数
    MonHead[0] = 31; MonHead[1] = 28; MonHead[2] = 31; MonHead[3] = 30; MonHead[4]  = 31; MonHead[5]  = 30;
    MonHead[6] = 31; MonHead[7] = 31; MonHead[8] = 30; MonHead[9] = 31; MonHead[10] = 30; MonHead[11] = 31;

var meizzTheYear=new Date().getFullYear(); //定义年的变量的初始值
var meizzTheMonth=new Date().getMonth()+1; //定义月的变量的初始值
var meizzTheDay=new Date().getMonth();//定义日的变量的初始值
var meizzWDay=new Array(39);               //定义写日期的数组


//function document.onclick() //任意点击时关闭该控件	//ie6的情况可以由下面的切换焦点处理代替
document.onclick = function(e) {

     if (window.event) {
        with (window.event) {
            if (srcElement.getAttribute("Author") == null && srcElement != outObject && srcElement != outButton)
                closeLayer();
        }
    } else {

    with (e) {
            if (target.getAttribute("Author") == null && target != outObject && target != outButton)
                closeLayer();
        }
    }
};

//function document.onkeyup()		//按Esc键关闭，切换焦点关闭
document.onkeyup = function() {

    var evt = window.event || arguments.callee.caller.arguments[0];

    // if (window.event.keyCode==27){
    if (evt.keyCode == 27) {
        if (outObject) outObject.blur();
        closeLayer();
    }
    else if (document.activeElement)
        if (document.activeElement.getAttribute("Author") == null && document.activeElement != outObject && document.activeElement != outButton) {
        closeLayer();
    }
};

function meizzWriteHead(yy,mm)  //往 head 中写入当前的年与月
{
    innerText(odatelayer.meizzYearHead, yy + " 年");
    innerText(odatelayer.meizzMonthHead, mm + " 月");
	//odatelayer.meizzYearHead.innerText  = yy + " 年";
    //odatelayer.meizzMonthHead.innerText = mm + " 月";
  }

function tmpSelectYearInnerHTML(strYear) //年份的下拉框
{
  if (strYear.match(/\D/)!=null){alert("年份输入参数不是数字！");return;}
  var m = (strYear) ? strYear : new Date().getFullYear();
  if (m < 1000 || m > 9999) {alert("年份值不在 1000 到 9999 之间！");return;}
  var n = m - 10;
  if (n < 1000) n = 1000;
  if (n + 100 > 1000) n = 1950;
  var s = "<select Author=meizz name=tmpSelectYear  style='position:relative;top:-3px;font-size: 12px' size='10' "
     s += "onblur='document.all.tmpSelectYearLayer.style.display=\"none\"' "
     s += "onchange='document.all.tmpSelectYearLayer.style.display=\"none\";"
     s += "parent.meizzTheYear = this.value; parent.meizzSetDay(parent.meizzTheYear,parent.meizzTheMonth)'>\r\n";
  var selectInnerHTML = s;
  for (var i = n; i < n + 100; i++)
  {
    if (i == m)
       {selectInnerHTML += "<option Author=wayx value='" + i + "' selected>" + i + "年" + "</option>\r\n";}
    else {selectInnerHTML += "<option Author=wayx value='" + i + "'>" + i + "年" + "</option>\r\n";}
  }
  selectInnerHTML += "</select>";
 // alert("selectInnerHTML YearInnerHTML:" + selectInnerHTML);
  odatelayer.tmpSelectYearLayer.style.display="";
  odatelayer.tmpSelectYearLayer.innerHTML = selectInnerHTML;
  odatelayer.tmpSelectYear.focus(); returnValue
}

function tmpSelectMonthInnerHTML(strMonth) //月份的下拉框
{
  if (strMonth.match(/\D/)!=null){alert("月份输入参数不是数字！");return;}
  var m = (strMonth) ? strMonth : new Date().getMonth() + 1;
  var s = "<select Author=meizz name=tmpSelectMonth style='position:relative;top:-3px;left:8px;font-size: 12px' width='100%'"
     s += "onblur='document.all.tmpSelectMonthLayer.style.display=\"none\"' "
     s += "onchange='document.all.tmpSelectMonthLayer.style.display=\"none\";"
     s += "parent.meizzTheMonth = this.value; parent.meizzSetDay(parent.meizzTheYear,parent.meizzTheMonth)'>\r\n";
  var selectInnerHTML = s;
  for (var i = 1; i < 13; i++)
  {
    if (i == m)
       {selectInnerHTML += "<option Author=wayx value='"+i+"' selected>"+i+"月"+"</option>\r\n";}
    else {selectInnerHTML += "<option Author=wayx value='"+i+"'>"+i+"月"+"</option>\r\n";}
  }
  selectInnerHTML += "</select>";
 // alert("selectInnerHTML MonthInnerHTML:" + selectInnerHTML);
  odatelayer.tmpSelectMonthLayer.style.display="";
  odatelayer.tmpSelectMonthLayer.innerHTML = selectInnerHTML;
  odatelayer.tmpSelectMonth.focus();
}

function closeLayer()               //这个层的关闭
  {
      //document.all.meizzDateLayer.style.display="none";
      document.getElementsByTagName("*").meizzDateLayer.style.display = "none";
  }

function IsPinYear(year)            //判断是否闰平年
  {
    if (0==year%4&&((year%100!=0)||(year%400==0))) return true;else return false;
  }

function GetMonthCount(year,month)  //闰年二月为29天
  {
    var c=MonHead[month-1];if((month==2)&&IsPinYear(year)) c++;return c;
  }
function GetDOW(day,month,year)     //求某天的星期几
  {
    var dt=new Date(year,month-1,day).getDay()/7; return dt;
  }

function meizzPrevY()  //往前翻 Year
  {
    if(meizzTheYear > 999 && meizzTheYear <10000){meizzTheYear--;}
    else{alert("年份超出范围（1000-9999）！");}
    meizzSetDay(meizzTheYear,meizzTheMonth);
  }
function meizzNextY()  //往后翻 Year
  {
    if(meizzTheYear > 999 && meizzTheYear <10000){meizzTheYear++;}
    else{alert("年份超出范围（1000-9999）！");}
    meizzSetDay(meizzTheYear,meizzTheMonth);
  }
function appendZero(n){return(("00"+ n).substr(("00"+ n).length-2));}//日期自动补零程序
function meizzToday()  //Today Button
  {
  var today;
    meizzTheYear = new Date().getFullYear();
    meizzTheMonth = new Date().getMonth()+1;
    meizzTheToday=new Date().getDate();
    if(outObject){
		outObject.value=meizzTheYear + "-" + appendZero(meizzTheMonth) + "-" + appendZero(meizzTheToday);
    }
    closeLayer();
  }
function meizzPrevM()  //往前翻月份
  {
    if(meizzTheMonth>1){meizzTheMonth--}else{meizzTheYear--;meizzTheMonth=12;}
    meizzSetDay(meizzTheYear,meizzTheMonth);
  }
function meizzNextM()  //往后翻月份
  {
    if(meizzTheMonth==12){meizzTheYear++;meizzTheMonth=1}else{meizzTheMonth++}
    meizzSetDay(meizzTheYear,meizzTheMonth);
  }

function meizzSetDay(yy,mm)   //主要的写程序**********
{
  meizzWriteHead(yy,mm);
  //设置当前年月的公共变量为传入值
  meizzTheYear=yy;
  meizzTheMonth=mm;

  for (var i = 0; i < 39; i++){meizzWDay[i]=""};  //将显示框的内容全部清空
  var day1 = 1,day2=1,firstday = new Date(yy,mm-1,1).getDay();  //某月第一天的星期几
  for (i=0;i<firstday;i++)meizzWDay[i]=GetMonthCount(mm==1?yy-1:yy,mm==1?12:mm-1)-firstday+i+1	//上个月的最后几天
  for (i = firstday; day1 < GetMonthCount(yy,mm)+1; i++){meizzWDay[i]=day1;day1++;}
  for (i=firstday+GetMonthCount(yy,mm);i<39;i++){meizzWDay[i]=day2;day2++}
  for (i = 0; i < 39; i++)
  { var da = eval("odatelayer.meizzDay"+i)     //书写新的一个月的日期星期排列
    if (meizzWDay[i]!="")
      {
		//初始化边框
		da.borderColorLight="#999999";
		da.borderColorDark="#FFFFFF";
		if(i<firstday)		//上个月的部分
		{
			da.innerHTML="<b><font color='darkgray'>" + meizzWDay[i] + "</font></b>";
			da.title=(mm==1?12:mm-1) +"月" + meizzWDay[i] + "日";
			da.onclick = Function("meizzDayClick(innerText(this),-1)");
			if(!outDate)
				da.style.backgroundColor = ((mm==1?yy-1:yy) == new Date().getFullYear() &&
					(mm==1?12:mm-1) == new Date().getMonth()+1 && meizzWDay[i] == new Date().getDate()) ?
					 "#D4D0C8":"white";
			else
			{
				da.style.backgroundColor =((mm==1?yy-1:yy)==outDate.getFullYear() && (mm==1?12:mm-1)== outDate.getMonth() + 1 &&
				meizzWDay[i]==outDate.getDate())? "#00ffff" :
				(((mm==1?yy-1:yy) == new Date().getFullYear() && (mm==1?12:mm-1) == new Date().getMonth()+1 &&
				meizzWDay[i] == new Date().getDate()) ? "#D4D0C8":"white");
				//将选中的日期显示为凹下去
				if((mm==1?yy-1:yy)==outDate.getFullYear() && (mm==1?12:mm-1)== outDate.getMonth() + 1 &&
				meizzWDay[i]==outDate.getDate())
				{
					da.borderColorLight="#FFFFFF";
					da.borderColorDark="#FF9900";
				}
			}
		}
		else if (i>=firstday+GetMonthCount(yy,mm))		//下个月的部分
		{
			da.innerHTML="<b><font color='darkgray'>" + meizzWDay[i] + "</font></b>";
			da.title=(mm==12?1:mm+1) +"月" + meizzWDay[i] + "日";
			da.onclick = Function("meizzDayClick(innerText(this),1)");
			if(!outDate)
				da.style.backgroundColor = ((mm==12?yy+1:yy) == new Date().getFullYear() &&
					(mm==12?1:mm+1) == new Date().getMonth()+1 && meizzWDay[i] == new Date().getDate()) ?
					 "#D4D0C8":"white";
			else
			{
				da.style.backgroundColor =((mm==12?yy+1:yy)==outDate.getFullYear() && (mm==12?1:mm+1)== outDate.getMonth() + 1 &&
				meizzWDay[i]==outDate.getDate())? "#00ffff" :
				(((mm==12?yy+1:yy) == new Date().getFullYear() && (mm==12?1:mm+1) == new Date().getMonth()+1 &&
				meizzWDay[i] == new Date().getDate()) ? "#D4D0C8":"white");
				//将选中的日期显示为凹下去
				if((mm==12?yy+1:yy)==outDate.getFullYear() && (mm==12?1:mm+1)== outDate.getMonth() + 1 &&
				meizzWDay[i]==outDate.getDate())
				{
					da.borderColorLight="#FFFFFF";
					da.borderColorDark="#FF9900";
				}
			}
		}
		else		//本月的部分
		{
			da.innerHTML="<b>" + meizzWDay[i] + "</b>";
			da.title=mm +"月" + meizzWDay[i] + "日";
			da.onclick = Function("meizzDayClick(innerText(this),0)"); 	//给td赋予onclick事件的处理
			//如果是当前选择的日期，则显示亮蓝色的背景；如果是当前日期，则显示暗黄色背景
			if(!outDate)
				da.style.backgroundColor = (yy == new Date().getFullYear() && mm == new Date().getMonth()+1 && meizzWDay[i] == new Date().getDate())?
					"#FF9900":"white";
			else
			{ 
				da.style.backgroundColor =(yy==outDate.getFullYear() && mm== outDate.getMonth() + 1 && meizzWDay[i]==outDate.getDate())?
					"#00ffff":((yy == new Date().getFullYear() && mm == new Date().getMonth()+1 && meizzWDay[i] == new Date().getDate())?
					"#FF9900":"white");
				//将选中的日期显示为凹下去
				if(yy==outDate.getFullYear() && mm== outDate.getMonth() + 1 && meizzWDay[i]==outDate.getDate())
				{
					da.borderColorLight="#FFFFFF";
					da.borderColorDark="#FF9900";
				}
			}
		}
        da.style.cursor="hand"
      }
    else{da.innerHTML="";da.style.backgroundColor="";da.style.cursor="default"}
  }
}

function meizzDayClick(n,ex)  //点击显示框选取日期，主输入函数*************
{
  var yy=meizzTheYear;
  var mm = parseInt(meizzTheMonth)+ex;	//ex表示偏移量，用于选择上个月份和下个月份的日期
	//判断月份，并进行对应的处理
	if(mm<1){
		yy--;
		mm=12+mm;
	}
	else if(mm>12){
		yy++;
		mm=mm-12;
	}

  if (mm < 10){mm = "0" + mm;}
  if (outObject)
  {
    if (!n) {//outObject.value="";
      return;}
    if ( n < 10){n = "0" + n;} 
    
    outObject.value= yy + "-" + mm + "-" + n ; //注：在这里你可以输出改成你想要的格式
    closeLayer();
    
    //add by lzy
    if (typeof(afterDateChangeCallback) == 'function')
        window.setTimeout("afterDateChangeCallback(eval('outObject'))", 1);
      //  afterDateChangeCallback(outObject);
  }
  else {closeLayer(); alert("您所要输出的控件对象并不存在！");}
}

