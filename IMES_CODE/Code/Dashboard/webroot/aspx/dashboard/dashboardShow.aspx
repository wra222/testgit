
<%--
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: preview for chart 
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2009-12-12   itc94006         Create 
 * Known issues: ITC-1101-0003 
 * Known issues: ITC-1101-0008
--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dashboardShow.aspx.cs" Inherits="webroot_aspx_dashboard_dashboardShow" %>

<html >
<head runat="server">
    <title>无标题页</title>
</head>

    <script type="text/javascript" src="../../commoncontrol/jquery.js"></script>
    <script type="text/javascript" src="../../commoncontrol/jquery-json.js"></script>
    <script type="text/javascript" src="../../commoncontrol/json.js"></script>
    
<!-- #include file="dashboardUtility.aspx" --> 
<style type="text/css">
body { background-color:black;width:99%;margin:10px;padding:0;overflow-x:auto;}
.firstLine{float:left;min-width:1024px;width:100%;_width:expression( document.body.clientWidth < 1024? '1020px':'100%' );margin:5px 0 5px 0;padding:0;}
.displayName{float:left;font-family:"Agency FB";font-size:36pt;font-weight:bold;color:#FFF;overflow:hidden;width:650px;}   

.clock{float:right;font-family:"Agency FB";font-size:36pt;font-weight:bold;color:#FFF;width:290px;}
.alert{position:relative;float:left;min-width:1024px;width:100%;_width:expression( document.body.clientWidth < 1024? '1020px':'100%' );margin:5px 0 5px 0;padding:0;background:url('images\alertBack.bmp') repeat-x;height:29px;}
.alertFont{font-family:"Arial Narrow";font-size:17pt;font-weight:bold;color:#D20000}

.stageFrame{overflow:hidden;min-width:1024px;width:100%;_width:expression( document.body.clientWidth < 1024? '1020px':'100%' );margin:10px 0 10px 0;padding:0;}
.stageLeft{float:left;width:12px;height:40px;background:url("images\stageBoxLeft.bmp") left no-repeat;margin:0;padding:0;}
.stageRight{float:left;width:12px;height:40px;background:url("images\stageBoxRight.bmp") left no-repeat;margin:0;padding:0;}
.stageMiddle{float:left;overflow:hidden;width:97.6%;height:40px;border-width:2px 0px 2px 0px;border-color:#fff;border-style:solid;margin:0;padding:0;text-align: center;}
.stageMiddle table{table-layout:fixed;width:99%;height:70%;margin:2px 1% 0 0;padding:0;border-spacing:0px;}

.stgBullet{text-align:right;font-family:Wingdings;font-size:16pt;color:#A6A600;}
.stageInfoName,.stageRate
{text-align:left;font-family:"Arial Narrow";font-size:16pt;font-weight:bold;color:#FFF;}
.stageInfoValue,.stageRateValue
{position:relative;text-align:left;font-family:"Arial Narrow";font-size:16pt;color:#FFF;height:27px;margin:0;padding:0}
.stageRateValue{width:303px;border-style:solid; background-color:#2F2F2F;border-width:2px;border-color:#292C29 #5C5E5C #5C5E5C #292C29;}
.stageRateBox{position:relative;margin:0;padding:0;}
.stageRateBar{position:absolute;overflow:hidden;z-index:-20;background:#35712F;height:23px;margin:0;padding:0;}
.stageRateTime{position:absolute;overflow:hidden;margin:0;padding:0;background:#BB9900;border-style:solid;border-width:0px 1px 0px 1px;border-color:#000000;width:4px;height:22px;top:0px;z-index:-10;}
.stageRageGrid{position:absolute;overflow:hidden;margin:0;padding:0;border-style:solid;border-width:0px 1px 0px 0px;border-color:#888;height:22px;top:0px;z-index:-10;}

.lineFrame{float:left;min-width:1024px;width:100%;_width:expression( document.body.clientWidth < 1024? '1020px':'100%' );margin:5px 0 5px 0;padding:0;}

h3{background:url('images\LineBack.bmp') repeat-x;width:99.8%;height:35px;margin:0 0.1% 0 0.1%;padding:0px;}
h3 table{table-layout:fixed;width:99%;height:70%;margin:2px 1% 0 0;padding:0;border-spacing:0px;}
h3 table tr{margin:0;padding:0;height:80%}
h3 table tr td{margin:0;padding:0;}
.lineName,.lineFPY,.lineTarget,.lineOutput,.lineRate
{width:7%;text-align:right;font-family:"Arial Narrow";font-size:17pt;font-weight:bold;color:#FFF;}
.lineName{width:5%}

.lineNameValue,.lineTargetValue,.lineOutputValue,.lineRateValue
{position:relative;background:#033100;border-style:solid; border-width:2px;border-color:#005500 #009300 #009300 #005500;text-align:right;margin:0;padding:0;font-family:"Arial Narrow";font-size:15pt;font-weight:bold;color:#FFF;text-align:right;z-index:0}
.lineRateValue{text-align:left;}
.lineRateValue{width:203px}
.lineNameValue{width:164px;text-align:left;}

.lineFPYValue
{position:relative;background:#033100;border-style:solid; border-width:2px;border-color:#005500 #009300 #009300 #005500;text-align:right;margin:0;padding:0;font-family:"Arial Narrow";font-size:15pt;font-weight:bold;color:#00FF00;text-align:right;z-index:0}

.lineFPYAlertValue
{position:relative;background:#033100;border-style:solid; border-width:2px;border-color:#005500 #009300 #009300 #005500;text-align:right;margin:0;padding:0;font-family:"Arial Narrow";font-size:15pt;font-weight:bold;color:#FFFF00;text-align:right;z-index:0}

.lineFPYAlarmValue
{position:relative;background:#033100;border-style:solid; border-width:2px;border-color:#005500 #009300 #009300 #005500;text-align:right;margin:0;padding:0;font-family:"Arial Narrow";font-size:15pt;font-weight:bold;color:#FF0000;text-align:right;z-index:0}

.lineRateBox{position:relative;margin:0;padding:0;}
.lineRateBar{position:absolute;overflow:hidden;z-index:-20;background:#009900;height:22px;margin:0;padding:0;}
.lineRateTime{position:absolute;overflow:hidden;margin:0;padding:0;background:#BB9900;border-style:solid;border-width:0px 1px 0px 1px;border-color:#000000;width:4px;height:22px;top:0px;z-index:-10;}


.stationFrame{float:left;min-width:1024px;width:100%;_width:expression( document.body.clientWidth < 1024? '1020px':'100%' );margin:0 0 0px 0;padding:0;}

.stationHDLeft  {float:left;width:0.8%;height:32px;background:url("images\head_left2.JPG") left no-repeat;margin:0;padding:0;}
.stationHDMiddle{float:left;width:98.3%;height:32px;background:url("images\head_mid.JPG") repeat-x;margin:0;padding:0;text-align: center;}
.stationHDRight {float:left;width:0.8%;height:32px;background:url("images\head_right2.JPG") right no-repeat;margin:0;padding:0;}
h4{width:98%;height:32px;text-align: center;margin:0px 1% 0px 1%;padding:3px 0 0 0;}
h4 table{width:100%;margin:0;padding:0;}
h4 table thead{width:100%;margin:0;padding:0;}
h4 table thead tr{margin:0;padding:0;}
.THStation, .THQuantity,.THDefect,.THWIP,.THYieldRate{font-family:"Arial Narrow"; font-size:13pt;color:#3A783C;margin:0;padding:0px 0px 0px 0px;text-align:right;}
.THStation{text-align:left;width:27%;}
.THQuantity{width:18%;}
.THDefect{width:18%;}
.THWIP{width:18.5%;}
.THYieldRate{width:18.5%;padding-right:0px;}

.stationBDMiddle{float:left;width:99.7%;background:url("images\body1x1.jpg") repeat;margin:0;padding:0;text-align:center;}

.stationBTMLeft {float:left;width:1%;height:8px;background:url("images\bottom_left1.JPG") left top no-repeat;margin:0;padding:0;}
.stationBTMMiddle{float:left;width:96.58%;height:8px;background:url("images\bottom_mid1.JPG") repeat-x;margin:0;padding:0;text-align: center;}
.stationBTMRight{float:left;width:2%;height:8px;background:url("images\bottom_right1.JPG") right top no-repeat;margin:0;padding:0;}

.stationTable{table-layout:fixed;float:left;margin:0px 0.1% 0px 0.6%;padding:0px 0px 0px 0px;width:97.5%;height:100%;border-spacing:0px;}
.stationTr{}
.stationTd{padding:0px 10px 0px 10px;width:18%;border-bottom:#006600 1px solid;color:#00FF00;text-align:Right;}
.stationTdFirst{padding:0px 10px 0px 10px;width:14%;border-bottom:#006600 1px solid;color:#00FF00;text-align:Right;}
.stationTdName{padding:0px 10px 0px 10px;width:32%;border-bottom:#006600 1px solid;color:#00FF00;text-align:left bottom;}
.stationTdAlarm{padding:0px 10px 0px 10px;width:18%;border-bottom:#006600 1px solid;color:#00FF00;text-align:Right;background-color:#600000;color:#FFFF00}
.stationFont{font-family:"Arial Narrow";font-size:16pt;}
.stationNameSpan{overflow:hidden;width:256px;}
.stationBullet{vertical-align:bottom;padding:0px;margin:0px;border-width:0px;width:24px;height:24px}

table {border-collapse:separate;}

</style>    

<script language="javascript" type="text/jscript">

//var ttt=""
//ttt+="begin time"+new Date().toString()+"; ";
var windowId = changeNull('<%=Request.Params["uuid"] %>');

var stageType = changeNull('<%=Request.Params["stageType"] %>');

//var fromwhere=changeNull('<%=Request.Params["fromwhere"] %>');
//!!! need change
//windowId='bd1fadc2940b4bfeb74b75cc47ea2d84'
//var isFirstF11=true;

function changeNull(val)
{
    if ("undefined" == val || null == val)
    {
        val = "";
    }
    
    return val;
}

//当前记录的当前window设置的更新时间
var showStepInfo=new Object();
showStepInfo.CurrentWindowSettingUdt="";
var windowData;

var idRefreshTimeShowTimer=null;
var idRefreshWindowTimer=null;

//line station数据的高度
var oneLineHeight=45;
var oneStationValueHeight=27;
var oneStationHeadAndFootHeight=57;

var oneClockHeight=82;
var oneAlertheight=40;
var oneStageHeight=61;

var dashboardWindowBottomRemain=null;
var dashboardWindowBottomRemainValue=0;

resetShowNumber();
function resetShowNumber()
{
    showStepInfo.isRefeshWindowShow=true;
    showStepInfo.NextLineRowNumber=0;
    showStepInfo.NextStationRowNumber=0;
}


</script>

<body Scroll="NO" >
    <form id="form1" runat="server">
    </form>
    <div id ="dDivContent">
	    <div class="firstLine" id="dDivFirstLine">
		    <div class="displayName" id="dDisplayName"onclick="Showtop10Click()" NOWRAP></div>
		    <div class="clock" id="dClock" NOWRAP></div>
	    </div>  
	    <div class="alert" id="dDivAlert">
		    <marquee  direction="left" behavior="scroll" scrollamount="3" scrolldelay="100"><font class="alertFont" id="dAlertMessage">Alert message: Welcome to Inventec Google昨天才刚刚将Chrome Dev分支更新到4.0.249.22，今天又推送了4.0.249.25版更新，不过这次更新仅仅面向Windows用户，修复了Windows平台两个严重的崩溃问题。</font></marquee>
	    </div>  
	    
        <div class="stageFrame" id="dDivStage">	
        
      <%--      <div class="stageLeft"></div>
            <div class="stageMiddle">
                <table width ="100%">
                    <tr><td class="stageDN"><font class="stgBullet">&#xB5;</font>&nbsp;Goal:</td>
                    <td class="stageDNValue" id="dDn" ></td>
                    <td class="stageUNPA"><font class="stgBullet">&#xB5;</font>&nbsp;SA&nbsp;Input:</td>
                    <td class="stageUNPAValue" id="dUnpa"></td>
                    <td class="stageOutput"><font class="stgBullet">&#xB5;</font>&nbsp;SA&nbsp;Output:</td>
                    <td class="stageOutputValue" id="dOutput"></td>
                    <td class="stageRate"><font class="stgBullet">&#xB5;</font>&nbsp;Rate:</td>
                    <td class="stageRateValue" id ="dStageRateBox">
                         <div class="stageRateBox" ><div class="stageRageGrid" style="width:30px"></div>
                         <div class="stageRageGrid" style="width:60px"></div>
                         <div class="stageRageGrid" style="width:90px"></div>
                         <div class="stageRageGrid" style="width:120px"></div>
                         <div class="stageRageGrid" style="width:150px"></div>
                         <div class="stageRageGrid" style="width:180px"></div>
                         <div class="stageRageGrid" style="width:210px"></div>
                         <div class="stageRageGrid" style="width:240px"></div>
                         <div class="stageRageGrid" style="width:270px"></div>
                         <div class="stageRateBar" style="background:#882020;width:0px;" id="dStageRateBar"></div>
                         <font id="dStagePercent"></font>
                         <div class="stageRateTime" style="left:0px" id="dStageRateTime"></div>
                         </div></td>
                     </tr>
                  </table>
                  </div>
             <div class="stageRight"></div>--%>
	    </div>	    
    
        <div id="dDivLineAndStation"> 
        </div>
	    
    </div>
</body>


<script FOR="window" EVENT="onload">
//ttt+="onload "+new Date().toString()+"; ";
dashboardWindowBottomRemain="<%=dashboardWindowBottomRemain%>";
try
{
    dashboardWindowBottomRemainValue=parseInt(dashboardWindowBottomRemain);
}
catch(e)
{
    alert("DashboardShowBottomRemain value set in web.config is not correct.");
}

if ("undefined" == dashboardWindowBottomRemainValue || null == dashboardWindowBottomRemainValue)
{
    dashboardWindowBottomRemainValue=0;
}

StartWindowShow();

</SCRIPT>

<script FOR="window" EVENT="onunload">

ClearTimer(idRefreshWindowTimer);
ClearTimer(idRefreshTimeShowTimer);

</SCRIPT>


<script language="javascript" type="text/javascript">

    var rateBoxWidth = 300;
    function Showtop10Click() {
        var dlgFeature = "dialogHeight:600px;dialogWidth:1010px;center:yes;status:no;help:no";
        var RefreshTime = windowData.refreshTime;
        //alert(RefreshTime);
        var newref = "showtop10issue.aspx?uuid=" + windowId + "&stageType=" + stageType + "&RefreshTime=" + RefreshTime;
        var dlgReturn = window.showModalDialog(newref, window, dlgFeature);
 
        //var newref = "showtop10issue.aspx?uuid=" + windowId + "&stageType=" + stageType
        //window.open(newref);
       
    }
function GetStageHtml(stageInfo)
{
    var bodyString="";
    bodyString+="<div class=\"stageLeft\"></div>";
	bodyString+="<div class=\"stageMiddle\">";
    bodyString+="<table><tr>";

    if(stageInfo.isSaStage=="True")
    {
        bodyString+="<td class=\"stageInfoName\" style=\"width:8%\" ><font class=\"stgBullet\">&#xB5;</font>&nbsp;Goal:</td>";
        var goalValue="";
        
        if(stageInfo.isGoalDisplay=="True")
        {
            goalValue=stageInfo.goal;
        } 
        //var goalValue="22222";
        bodyString+="<td class=\"stageInfoValue\" id=\"dGoalValue\" style=\"width:9%\" >"+goalValue+"</td>";
        ///////
        bodyString+="<td class=\"stageInfoName\" style=\"width:12%\" ><font class=\"stgBullet\">&#xB5;</font>&nbsp;SA&nbsp;Input:</td>";

        var saInputValue="";
        
        if(stageInfo.isSaInputDisplay =="True")
        {
            saInputValue=stageInfo.saInput;
        } 
        //var saInputValue="22222";
        bodyString+="<td class=\"stageInfoValue\" id=\"dSaInputValue\" style=\"width:9%\" >"+saInputValue+"</td>";    
        ///////    
        bodyString+="<td class=\"stageInfoName\" style=\"width:13%\" ><font class=\"stgBullet\">&#xB5;</font>&nbsp;SA&nbsp;Output:</td>";

        var saOutputValue="";
        
        if(stageInfo.isSaOutputDisplay =="True")
        {
            saOutputValue=stageInfo.saOutput;
        } 
        //var saOutputValue="22222";
        bodyString+="<td class=\"stageInfoValue\" id=\"dSaOutputValue\" style=\"width:9%\" >"+saOutputValue+"</td>";    
        
        rateBoxWidth=300;

   }
   else
   {
        bodyString+="<td class=\"stageInfoName\" style=\"width:6%\"><font class=\"stgBullet\">&#xB5;</font>&nbsp;DN:</td>";
        
        var dnValue="";
        //var dnValue="22222";//tmp        
        if(stageInfo.isDnDisplay=="True")
        {
            dnValue=stageInfo.dn;
        } 
        bodyString+="<td class=\"stageInfoValue\" id=\"dDnValue\" style=\"width:6%\" >"+dnValue+"</td>";
        ///////
        bodyString+="<td class=\"stageInfoName\" style=\"width:8%\" ><font class=\"stgBullet\">&#xB5;</font>&nbsp;FA&nbsp;In:</td>";

        var faInputValue="";
        //var faInputValue="22222";
        if(stageInfo.isFaInputDisplay =="True")
        {
            faInputValue=stageInfo.faInput;
        } 
        bodyString+="<td class=\"stageInfoValue\" id=\"dFaInputValue\" style=\"width:6%\" >"+faInputValue+"</td>";    
        ///////    
        bodyString+="<td class=\"stageInfoName\" style=\"width:10%\" ><font class=\"stgBullet\">&#xB5;</font>&nbsp;FA&nbsp;Out:</td>";

        var faOutputValue="";
        //var faOutputValue="22222";
        if(stageInfo.isFaOutputDisplay =="True")
        {
            faOutputValue=stageInfo.faOutput;
        } 
        bodyString+="<td class=\"stageInfoValue\" id=\"dFaOutputValue\" style=\"width:6%\" >"+faOutputValue+"</td>";      
        
        ///////
         bodyString+="<td class=\"stageInfoName\" style=\"width:8%\" ><font class=\"stgBullet\">&#xB5;</font>&nbsp;PA&nbsp;In:</td>";

        var paInputValue="";
        //var paInputValue="22222";
        if(stageInfo.isPaInputDisplay =="True")
        {
            paInputValue=stageInfo.paInput;
        } 
        bodyString+="<td class=\"stageInfoValue\" id=\"dPaInputValue\" style=\"width:6%\" >"+paInputValue+"</td>";    
        ///////    
        bodyString+="<td class=\"stageInfoName\" style=\"width:10%\" ><font class=\"stgBullet\">&#xB5;</font>&nbsp;PA&nbsp;Out:</td>";

        var paOutputValue="";
        //var paOutputValue="22222";
        if(stageInfo.isPaOutputDisplay =="True")
        {
            paOutputValue=stageInfo.paOutput;
        } 
        bodyString+="<td class=\"stageInfoValue\" id=\"dPaOutputValue\" style=\"width:6%\" >"+paOutputValue+"</td>";   
        rateBoxWidth=190;
   
   }
       
    var rateBoxWidthAll= rateBoxWidth+3;
    var perBoxWidth=rateBoxWidth/10;
    bodyString+="<td class=\"stageRate\"><font class=\"stgBullet\">&#xB5;</font>&nbsp;Rate:</td>";
    bodyString+="<td class=\"stageRateValue\" id =\"dStageRateBox\" style=\"width:"+rateBoxWidthAll+"px\" >";
	bodyString+="<div class=\"stageRateBox\" >";
	var tmpBoxWidth=perBoxWidth;
	bodyString+="<div class=\"stageRageGrid\" style=\"width:"+tmpBoxWidth+"px\"></div>";
	tmpBoxWidth=2*perBoxWidth
	bodyString+="<div class=\"stageRageGrid\" style=\"width:"+tmpBoxWidth+"px\"></div>";
	tmpBoxWidth=3*perBoxWidth
	bodyString+="<div class=\"stageRageGrid\" style=\"width:"+tmpBoxWidth+"px\"></div>";
	tmpBoxWidth=4*perBoxWidth
	bodyString+="<div class=\"stageRageGrid\" style=\"width:"+tmpBoxWidth+"px\"></div>";
	tmpBoxWidth=5*perBoxWidth
	bodyString+="<div class=\"stageRageGrid\" style=\"width:"+tmpBoxWidth+"px\"></div>";
	tmpBoxWidth=6*perBoxWidth
	bodyString+="<div class=\"stageRageGrid\" style=\"width:"+tmpBoxWidth+"px\"></div>";
	tmpBoxWidth=7*perBoxWidth
	bodyString+="<div class=\"stageRageGrid\" style=\"width:"+tmpBoxWidth+"px\"></div>";
	tmpBoxWidth=8*perBoxWidth
	bodyString+="<div class=\"stageRageGrid\" style=\"width:"+tmpBoxWidth+"px\"></div>";
	tmpBoxWidth=9*perBoxWidth
	bodyString+="<div class=\"stageRageGrid\" style=\"width:"+tmpBoxWidth+"px\"></div>";
	
    var percentStr=stageInfo.rate+"%("+stageInfo.targetRate+"%)"
                    
    var dStageRateBarPercent=parseInt(stageInfo.rate)*rateBoxWidth/100;
      
    var dStageRateTimePercent=parseInt(stageInfo.targetRate)*rateBoxWidth/100;
    
    if(dStageRateBarPercent>rateBoxWidth)
    {
        dStageRateBarPercent=rateBoxWidth;
    }
    if(dStageRateTimePercent>rateBoxWidth)
    {
        dStageRateTimePercent=rateBoxWidth;
    }         
	
    bodyString+="<div class=\"stageRateBar\"";
    
    if(stageInfo.isRateOk=="True")
    {
        bodyString+=" style=\"background:#35712F;width:"+dStageRateBarPercent+"px;\"";
    }          
    else
    {
        bodyString+=" style=\"background:#882020;width:"+dStageRateBarPercent+"px;\"";          
    }      
   
    bodyString+=" id=\"dStageRateBar\"></div><font id=\"dStagePercent\">"+percentStr+"</font>";
    bodyString+="<div class=\"stageRateTime\" style=\"left:"+dStageRateTimePercent+"px\" id=\"dStageRateTime\"></div>";
							       
    bodyString+="</div></td></tr></table></div>";          
    bodyString+="<div class=\"stageRight\"></div>";



    return bodyString;
}

function GetLineHtml(lineNumber, lineShowingInfo)
{
    //dLineFrame+  lineNumber   ->dLineFrame0
    //id="dLineFrame'+lineNumber+'LineNameValue"    ->dLineFrame0LineNameValue
    //id="dLineFrame'+lineNumber+'LineFPYValue"
    //id="dLineFrame'+lineNumber+'LineTargetValue"
    //id="dLineFrame'+lineNumber+'LineOutputValue"
    //id="dLineFrame'+lineNumber+'LineRateBar"  ->style.width   200px->100% ，<!--Line rate bar color -- normal = "background:#116611"; alarm = "background:#DD0000". 
    //id="dLineFrame'+lineNumber+'LineRateBarPercentText" 显示的文字  82%(65%)
    //id="dLineFrame'+lineNumber+'LineRateTime"  显示target style.left
    var bodyString="";
    bodyString+='<div class="lineFrame" id="dLineFrame'+lineNumber +'">';
    bodyString+='<h3>';
	bodyString+='<table style="table-layout:fixed">';
	bodyString+='<tr style="height:26px">';
	bodyString+='<td class="lineName">Line:</td>';
	bodyString+='<td class="lineNameValue" id="dLineFrame'+lineNumber+'LineNameValue">'+lineShowingInfo.lineName+'</td>';
    bodyString+='<td class="lineFPY">FPY:</td>';
    
    var lineFPY=lineShowingInfo.fPY+"%";
    bodyString+='<td '
   
    if(lineShowingInfo.isFPYOk=="True")
    {
        bodyString+='class="lineFPYValue"';
    }
    else if(lineShowingInfo.isFPYOk=="JustPass")
    {
        bodyString+='class="lineFPYAlertValue"';
    }
    else
    {
        bodyString+='class="lineFPYAlarmValue"';
    }
    
    bodyString+=' id="dLineFrame'+lineNumber+'LineFPYValue">'+lineFPY+'</td>';
    bodyString+='<td class="lineTarget">Target:</td>';
    bodyString+='<td class="lineTargetValue" id="dLineFrame'+lineNumber+'LineTargetValue">'+lineShowingInfo.target+'</td>';
    bodyString+='<td class="lineOutput">Output:</td>';
    bodyString+='<td class="lineOutputValue" id="dLineFrame'+lineNumber+'LineOutputValue">'+lineShowingInfo.output+'</td>';
    bodyString+='<td class="lineRate">Rate:</td>'
    bodyString+='<td class="lineRateValue">'
    bodyString+='<div class=lineRateBox>'
    bodyString+='<div class="lineRateBar" '
    
    var rateBarWidth=parseInt(lineShowingInfo.rate)*200/100;
    if(rateBarWidth>200)
    {
        rateBarWidth=200;
    }
    
    if(lineShowingInfo.isRateOk=="True")
    {
        bodyString+='style="background:#116611;width:'+rateBarWidth+'px;"'
    }
    else
    {
        bodyString+='style="background:#DD0000;width:'+rateBarWidth+'px;"'
    } 
    
    var lineRatePercent=lineShowingInfo.rate+"%("+lineShowingInfo.targetRate+"%)";
    bodyString+=' id="dLineFrame'+lineNumber+'LineRateBar"></div><font id="dLineFrame'+lineNumber+'LineRateBarPercentText">'+lineRatePercent+'</font>';
//							<!--Line rate bar color -- normal = "background:#116611"; alarm = "background:#DD0000". 
//								  Line rate bar width -- 0% = "width:0px"; 100% = "width:200px"-->
    var rateTargetWidth=parseInt(lineShowingInfo.targetRate)*200/100;  
    if(rateTargetWidth>200)
    {
        rateTargetWidth=200;
    }
    
    bodyString+='<div class="lineRateTime" style="left:'+rateTargetWidth+'px" id="dLineFrame'+lineNumber+'LineRateTime"></div>';
//							<!--Line Rate time line position -- 0% = "left:0px"; 100% = "left:200px"-->
    bodyString+='</div>';
    bodyString+='</td>';
    bodyString+='</tr>';
    bodyString+='</table>';
    bodyString+='</h3>';
    bodyString+='</div>';
    
    //alert(bodyString)
    return bodyString;
}

function GetStationHead()
{
    var bodyString="";   
    bodyString+='<div class="stationFrame">'; 
	bodyString+='<div class="stationHDLeft"> </div>';
	bodyString+='<div class="stationHDMiddle">';
	bodyString+='<h4>';
	bodyString+='<table>';
	bodyString+='<thead>';
	bodyString+='<tr>';
	bodyString+='<th class="THStation"  >Station</th>';
	bodyString+='<th class="THQuantity" >Quantity</th>';
	bodyString+='<th class="THDefect"   >Defect</th>';
	bodyString+='<th class="THWIP"      >WIP</th>';
	bodyString+='<th class="THYieldRate">Yield Rate</th>';
	bodyString+='</tr>';
	bodyString+='</thead>';
	bodyString+='</table>';
	bodyString+='</h4>';
	bodyString+='</div>';
	bodyString+='<div class="stationHDRight"></div>';
	return bodyString;

}

function GetStationFoot()
{
    var bodyString=""; 
    bodyString+='<div class="stationBTMLeft">  </div>';
	bodyString+='<div class="stationBTMMiddle"> </div>';
	bodyString+='<div class="stationBTMRight"> </div>';
	bodyString+='</div>';
    return bodyString;
}
//!!!
//id="dLine'+lineNumber+'StationInfo"  需要改变高度 26*行数
function GetStationMidHead(lineNumber,stationCount)
{
    //这个Style的高度会根据总的station长度而改变
    var bodyString=""; 
	//bodyString+='<div class="stationBDMiddle" style="height:130px" id="dLine'+lineNumber+'StationInfo">'
	var blockHeight=stationCount*oneStationValueHeight;
	
	bodyString+='<div class="stationBDMiddle" id="dLine'+lineNumber+'StationInfo" style="height:'+blockHeight+'px">'
	bodyString+='<TABLE class="stationTable" cellspacing="0">'
    return bodyString;			
}

function GetStationMidFoot()
{
    var bodyString="";
    bodyString+='</TABLE>';
	bodyString+='</div>';
    return bodyString;	
}


function GetStationHtml(lineNumber,stationNumber,stationShowingInfo)
{
    var bodyString="";
    bodyString+='<TR class="stationTr">';
    bodyString+='<TD class="stationTdName" NOWRAP><IMG class="stationBullet" SRC="';

    if(stationShowingInfo.isYieldRateOk=="True"||stationShowingInfo.isYieldRateDsp =="False")
    {
        bodyString+='images\\greenBall.bmp';
    }
    else
    {
        bodyString+='images\\redBall.bmp';
    }
    
    bodyString+='">&nbsp;<span class="stationNameSpan"><font class="stationFont">'+stationShowingInfo.stationName+'</font></span></TD>';
    /////////

	bodyString+='<TD class="stationTdFirst"><font class="stationFont">';
	if(stationShowingInfo.isQuantityDsp=="True")
	{
	    bodyString+=stationShowingInfo.quantity;
	}
	else
	{
	    bodyString+="&nbsp";
	}
	bodyString+='</font></TD>';
	///////
	
	bodyString+='<TD class="stationTd"><font class="stationFont">';
	if(stationShowingInfo.isDefectDsp=="True")
	{
	    bodyString+=stationShowingInfo.defect;
	}
	else
	{
	    bodyString+="&nbsp";
	}
	bodyString+='</font></TD>';
	
	////////
	
	
	bodyString+='<TD class="stationTd"><font class="stationFont">';
	if(stationShowingInfo.isWIPDsp=="True")
	{
	    bodyString+=stationShowingInfo.wIP;
	}
	else
	{
	    bodyString+="&nbsp";
	}
	bodyString+='</font></TD>';
	/////////////
	bodyString+='<TD class="';
	if(stationShowingInfo.isYieldRateOk=="True"||stationShowingInfo.isYieldRateDsp =="False")
	{
	    bodyString+='stationTd';
	}
	else
	{
	    bodyString+='stationTdAlarm';
	}
	
	
	bodyString+='"><font class="stationFont">';
 
	if(stationShowingInfo.isYieldRateDsp =="True")
	{
	    //产量为0时，stationShowingInfo.yieldRate字串为空字串，不显示文字
	    if(stationShowingInfo.yieldRate!="")
	    {
	      bodyString+=stationShowingInfo.yieldRate+"%";
	    }
	    else
	    {
	        bodyString+="&nbsp";
	    }
	}
	else
	{
	    bodyString+="&nbsp";
	}
	
	bodyString+='</font></TD>';
	
	
	bodyString+='</TR>';
    return bodyString;	
}

function StartWindowShow()
{
//     ttt+="StartWindowShow "+new Date().toString()+"; ";
 //    var rtn;    
    try
    {     
         $.ajax({
            url: 'dashboardShowData.aspx',
            type: 'POST',
//            contentType: "application/json",
//            dataType: 'json',
            data: {
                windowId: windowId,
                stageType:stageType
            }, 
            error: function(){
                //alert("Can't get data from server.");
                RefreshWindowData(5);
            },
            success: function(resultdata){
                 windowData=null;
                 windowData=JSON.parse(resultdata);
                 resultdata=null
                 if(showStepInfo.CurrentWindowSettingUdt!=windowData.currentWindowSettingUdt)
                 {
                     resetShowNumber();
                     showStepInfo.CurrentWindowSettingUdt=windowData.currentWindowSettingUdt;
                 }
                 showWindowData();   
            }
         });
     
     }
     catch(e)
     {
        //alert("Can't get data from server.");
        RefreshWindowData(5);
        return;
     }
     
//     if (rtn.error != null) 
//     {
//         alert(rtn.error.Message);
//         //!!!如何处理
//         return;
//     }
//     else 
//     { 
//         windowData=null;
//         //windowData=rtn.value;
//         windowData=eval(resultdata);
//     }
//     
//     if(windowData==null)
//     {
//        return;
//     }

//     rtn=null;
//     //Windows dashboard设定变动，重设初始值，重头显示
//     if(showStepInfo.CurrentWindowSettingUdt!=windowData.CurrentWindowSettingUdt)
//     {
//         resetShowNumber();
//         showStepInfo.CurrentWindowSettingUdt=windowData.CurrentWindowSettingUdt;
//     }
////     ttt+="showWindowData "+new Date().toString()+"; ";
//     showWindowData(); 
 
}

function RefreshTimerShow(timeShowString, currentSecond)
{

    ClearTimer(idRefreshTimeShowTimer);
    document.getElementById("dClock").innerHTML="";
    document.getElementById("dClock").innerHTML=timeShowString;
    //下次取显示时间的间隔毫秒数
    var nextGetTime=(1000-parseInt(currentSecond))
    idRefreshTimeShowTimer=setTimeout("GetTimeShowData()",nextGetTime);
    
}

function GetTimeShowData()
{

   try
     {
        $.ajax({
            url: 'DashboardShowTimerData.aspx',
            type: 'POST',
        //            contentType: "application/json",
        //            dataType: 'json',
            data:{}, 
            error: function(){
                //alert("Can't get data from server.");
            },
            success: function(resultdata){
                
                 var currentTimeInfo=null;
                 currentTimeInfo=JSON.parse(resultdata);
                 
                 resultdata=null
                 if(currentTimeInfo!=null)
                 {
                    RefreshTimerShow(currentTimeInfo.TimeShowString, currentTimeInfo.Second);
                 }                                    
            }
         });
     
     }
     catch(e)
     {
        //alert("Can't get data from server.");
        return;
     }

/*     var rtn;
     try
     {
        rtn = com.inventec.portal.dashboard.DashboardShowManager.GetCurrentTimeInfo();
     }
     catch(e)
     {
        alert("Can't get data from server.");
        return;
     }
     
     var currentTimeInfo=null;
     if (rtn.error != null) 
     {
         alert(rtn.error.Message);
         //!!!如何处理
         return;
     }
     else 
     { 
         currentTimeInfo=rtn.value;
     }

     rtn=null;
     if(currentTimeInfo!=null)
     {
        RefreshTimerShow(currentTimeInfo.TimeShowString, currentTimeInfo.Second);
     }
  */   
}

function ClearTimer(timerId)
{
    if(timerId!=null)
    {    
        window.clearTimeout(timerId)
    }
}

function RefreshWindowData(rereshTime)
{
    ClearTimer(idRefreshWindowTimer);
    //下次取显示数据的间隔毫秒数   

    if(rereshTime!="" && rereshTime!="0")
    {
        var timeMs=parseInt(rereshTime) * 1000;
        idRefreshWindowTimer=setTimeout("StartWindowShow()",timeMs);
    }
}

function showWindowData()
{

     //document.getElementById("dDivContent").style.visibility="hidden";
    if(showStepInfo.isRefeshWindowShow==true)
     {
        window.document.title=windowData.windowName;

        document.getElementById("dDisplayName").innerHTML="";
        document.getElementById("dClock").innerHTML="";
        document.getElementById("dAlertMessage").innerHTML="";       
        
        
        document.getElementById("dDisplayName").innerHTML=htmlEncode(windowData.displayName);
        document.getElementById("dClock").innerHTML=htmlEncode(windowData.nowTimeShowString);
        document.getElementById("dAlertMessage").innerHTML=htmlEncode(windowData.alertMessage);       
     }
     
     var stageInfo=windowData.stageInfo;

     if(stageInfo.isStageDisplay=="True")
     {
         document.getElementById("dDivStage").innerHTML=GetStageHtml(stageInfo);
                 
         document.getElementById("dDivStage").style.display="";
//         //////////
//         if(stageInfo.isDnDisplay=="True")
//         {
//            document.getElementById("dDn").innerHTML=""
//            document.getElementById("dDn").innerHTML=stageInfo.dN;
//         }
//         else
//         {
//            document.getElementById("dDn").innerHTML="";
//         }
//            
//         
//         if(stageInfo.isUnPaDisplay=="True")
//         {
//            document.getElementById("dUnpa").innerHTML=""
//            document.getElementById("dUnpa").innerHTML=stageInfo.uNPA;
//         }
//         else
//         {
//            document.getElementById("dUnpa").innerHTML="";
//         }
//         if(stageInfo.isOutputDisplay=="True")
//         {         
//            document.getElementById("dOutput").innerHTML=""
//            document.getElementById("dOutput").innerHTML=stageInfo.output;
//         }
//         else
//         {
//            document.getElementById("dOutput").innerHTML="";
//         }
         /////////
//         if(document.getElementById("dStageRateBox")==null)
//         {
//            return;
//         }
         
         if(stageInfo.isRateDisplay=="True")
         {
              document.getElementById("dStageRateBox").style.visibility="visible";
//              ////////
//              var percentStr=stageInfo.rate+"%("+stageInfo.targetRate+"%)"
//              document.getElementById("dStagePercent").innerHTML=""
//              document.getElementById("dStagePercent").innerHTML=percentStr;
//                            
//              var dStageRateBarPercent=parseInt(stageInfo.rate)*rateBoxWidth/100;
//              if(dStageRateBarPercent>rateBoxWidth)
//              {
//                dStageRateBarPercent=rateBoxWidth;
//              }
//              
//              document.getElementById("dStageRateBar").style.width=dStageRateBarPercent+"px";
//              
//              var dStageRateTimePercent=parseInt(stageInfo.targetRate)*rateBoxWidth/100;
//              if(dStageRateTimePercent>rateBoxWidth)
//              {
//                dStageRateTimePercent=rateBoxWidth;
//              }              
//              
//              document.getElementById("dStageRateTime").style.left=dStageRateTimePercent+"px";  
//              
//              if(stageInfo.isRateOk=="True")
//              {
//                document.getElementById("dStageRateBar").style.background="#35712F";
//              }          
//              else
//              {
//                document.getElementById("dStageRateBar").style.background="#882020"              
//              }
              ////////
         }
         else
         {
             document.getElementById("dStageRateBox").style.visibility="hidden";
         }
         
     }
     else
     {
         document.getElementById("dDivStage").style.display="none";
     }
      
     document.getElementById("dDivLineAndStation").innerHTML="";
     //removeChildrenRecursively(document.getElementById("dDivLineAndStation"));
     document.getElementById("dDivLineAndStation").innerHTML=showLineAndStation();
     
     try
     {
        CollectGarbage(); 
     }
     catch(e)
     {
     
     }
//     document.getElementById("dDivContent").style.visibility="visible"; 
     //刷新时间
    
     RefreshTimerShow(windowData.nowTimeShowString, windowData.nowTimeSecond);
     
     //设置window dashboard数据刷新
     RefreshWindowData(windowData.refreshTime);
    

}

function removeChildrenRecursively(node) {
    if (!node) return;
    while (node.hasChildNodes()) {
        removeChildrenRecursively(node.firstChild);
        node.removeChild(node.firstChild);
    }
}

function showLineAndStation()
{

     var lineStationBody="";  
     
     var tmpOneStageHeight=oneStageHeight;
     if(document.getElementById("dDivStage").style.display=="none")
     {
         tmpOneStageHeight=0;
     }     
     
     //剩余高度
     var leftHeight =document.body.clientHeight
                     -oneClockHeight
                     -oneAlertheight
                     -tmpOneStageHeight
                     +10    //前面区块都是计算的上边缘开始，因此需要最后补充上 14,后面-4（clientHeight比实际高度多4），合成10
                     -dashboardWindowBottomRemainValue;
                    
     var lineAndStationInfos=windowData.stageInfo.lineShowingInfos;
    
     var startlineNumber=showStepInfo.NextLineRowNumber;
     //debugger
     for(var lineNumber=startlineNumber;lineNumber<lineAndStationInfos.length;lineNumber++)
     {
         if(leftHeight<oneLineHeight)
         {
             return lineStationBody;
         }     

         if( lineAndStationInfos[lineNumber].isInWorkTime=="False" )
         {
             //上回显示一半时，再显示时下班了，那么新的station应该清0
             showStepInfo.NextStationRowNumber=0;
             if(lineNumber!=lineAndStationInfos.length-1)
             {
                  showStepInfo.NextLineRowNumber=lineNumber+1;
             }
             else
             {
                  showStepInfo.NextLineRowNumber=0; 
             }            
             continue;        
         }
         
         lineStationBody+=GetLineHtml(lineNumber,lineAndStationInfos[lineNumber]);
         leftHeight=leftHeight-oneLineHeight;
         
         if(lineAndStationInfos[lineNumber].isStationDsp!="True" || lineAndStationInfos[lineNumber].stationShowingInfos.length==0)
         {
             
             if(lineNumber!=lineAndStationInfos.length-1)
             {
                  showStepInfo.NextLineRowNumber=lineNumber+1;
             }
             else
             {
                  showStepInfo.NextLineRowNumber=0; 
             }            
             continue;
         }
         
         if(leftHeight<oneStationHeadAndFootHeight+oneStationValueHeight)
         {
             //showStepInfo.NextLineRowNumber=lineNumber;
             return lineStationBody;
         } 
         
         lineStationBody+=GetStationHead();
         //将station的head foot高度都减去
         leftHeight=leftHeight-oneStationHeadAndFootHeight;                  
         //debugger
         var startStationNumber=showStepInfo.NextStationRowNumber;
     
         var stationNumber=startStationNumber;
         
         var isLineGoNext=true;
         for(stationNumber=startStationNumber;stationNumber<lineAndStationInfos[lineNumber].stationShowingInfos.length;stationNumber++)
         {  
             if(leftHeight<oneStationValueHeight)
             {
                 isLineGoNext=false;
                 break;
             }
             leftHeight=leftHeight-oneStationValueHeight;
         }
         
         //可以显示的station个数
         var showStationCount=stationNumber-startStationNumber;
         if(stationNumber<lineAndStationInfos[lineNumber].stationShowingInfos.length)
         {
            showStepInfo.NextStationRowNumber=stationNumber;
         }
         else
         {
            showStepInfo.NextStationRowNumber=0;
         }
         
         //leftHeight=leftHeight-oneStationValueHeight*showStationCount;
         lineStationBody+=GetStationMidHead(lineNumber,showStationCount);

         for(var i=startStationNumber;i<startStationNumber+showStationCount;i++)
         {  
             lineStationBody+=GetStationHtml(lineNumber,stationNumber,lineAndStationInfos[lineNumber].stationShowingInfos[i])
         }

         lineStationBody+=GetStationMidFoot(); 
         lineStationBody+=GetStationFoot(); 

         if(isLineGoNext==true)
         {   
             if(lineNumber!=lineAndStationInfos.length-1)
             {
                  showStepInfo.NextLineRowNumber=lineNumber+1;
             }
             else
             {
                  showStepInfo.NextLineRowNumber=0; 
             }
         }
     }     
     return lineStationBody;
}


with (window.document) {
  //oncontextmenu=onselectstart=ondragstart=function(){
	oncontextmenu=ondragstart=function(){
		event.cancelBubble=true;
		/*if (event.srcElement.tagName != undefined) {
			 var sSrcTagName=event.srcElement.tagName.toLowerCase();
			 if (sSrcTagName=="textarea" || sSrcTagName=="input"||sSrcTagName=="editbox") return true;
       		 }*/
        	return false;
   	}
	
	onkeydown=function() {

		var charCode = event.keyCode;
		var iKeyCode=event.keyCode;
		
//		if(isFirstF11==true && fromwhere=="main")
//		{
//		    if(iKeyCode==122)
//		    {
//	            try
//                {                
//	                var WshShell =new ActiveXObject("WScript.Shell");
//                    var ref=window.location.href  
//                    //var re = /dashboardmain.aspx/gi;
//                    var re=new RegExp("dashboardShow.aspx(.*)","gi")
//                    var newref = ref.replace(re, "dashboardShow.aspx?uuid="+windowId );
//                    var execname="IEXPLORE.exe -k "+ref;
//                    WshShell.run(execname);
//                                    
//                    event.cancelBubble = true;
//		            window.close();	
//                    
//                    
//                }
//                catch(e)
//                {
//                    alert("Running activex control was forbidden in browser set.");
//                    return false;
//                }
//                isFirstF11=false;
//		        return false;
//		    }
//		
//		}
//		
//		return true;	
        if(iKeyCode==122)
        {
            return true;
        }
   	
        if (iKeyCode==27 || iKeyCode==112 || (iKeyCode==78 && event.ctrlKey) || (iKeyCode==37 && event.altKey) || (iKeyCode==39 && event.altKey)) return false;

        if (iKeyCode>112 && iKeyCode < 124){
            event.keyCode = 0;
            event.cancelBubble=true;
            event.returnValue =false
            return false
        }
        if (iKeyCode == 8) {
            if (event.srcElement.tagName != undefined) {
                var sSrcTagName = event.srcElement.tagName.toLowerCase();
                if (sSrcTagName == "textarea" || sSrcTagName == "input" || event.srcElement.contentEditable) {
                    event.cancelBubble = false;
                    return true
                } else { 
                    event.cancelBubble = true;
                    return false
                }

            }
        }
    }
}

</script>

</html>
