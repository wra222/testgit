<%--
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: preview for chart 
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2009-12-12   itc94006         Create 
 * Known issues: ITC-1101-0012
 * Known issues: ITC-1101-0013
--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dashboardSetting.aspx.cs" Inherits="webroot_aspx_dashboard_dashboardSetting" %>

<%@ Import Namespace="com.inventec.system" %>
<script type="text/javascript" src="../../commoncontrol/tableEdit/TableData.js"></script>
<script type="text/javascript" src="../../commoncontrol/tableEdit/TableEdit.js"></script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<fis:header id="header2" runat="server"/>
<!-- #include file="dashboardUtility.aspx" --> 
<style type="text/css">


*{
	font-family: "Arial", "Helvetica", "sans-serif";
	font-size: 14px;
}

button{
	font-family: "Arial", "Helvetica", "sans-serif";
	font-size: 14px;
}

legend {
    font-size: x-small; 
    font-style: normal; 
    font-family: 'Times New Roman', Times, serif;
}
</style>

<script language="javascript" type="text/javascript">
   
    var windowId = changeNull('<%=Request.Params["uuid"] %>');
    //!!!从session及令牌中取得
    var editor=window.parent.editor;
    //alert(editor)
    //alert(windowId)
   
    function changeNull(val)
    {
        if ("undefined" == val || null == val)
        {
            val = "";
        }
        
        return val;
    }
    
</script>

<body >
    <form id="form1" runat="server">
    </form>
    <table cellpadding="0" border="0" cellspacing="0"  width="100%">
   <tr>
    <td class="title" style="height:20px" NOWRAP>
        <label id="headName" style="width:400px;overflow:hidden" NOWRAP> </label>
    </td>
	</tr>
	<tr id="Tr1" Style="height:8px">
	    <td></td>
    </tr>   
    <tr id="idWindowBlock" Style="display:none">
	    <td>
	    <fieldset style="height:90%;width:100%">
            <legend >Window Setup</legend><div id= "divWindow"></div></fieldset>
	    </td>
    </tr>   
    <tr id="Tr2" Style="height:8px">
	    <td></td>
    </tr> 
    <tr id="idLineBlock" Style="display:none">
	    <td>
	    <fieldset style="width: 100%"><legend>Line & Station</legend>
	    <div id= "divLine">
	     <table >
	     <tr>
	        <td colspan=2>
	            Selected Lines:
	        </td> 
	     </tr>
	     <tr >
	        <td>
	            <div id='divLineTable'>
	            </div>
	        </td>
	        <td  style="padding-left:3px" >
	             <table id ="idLineButtonTable" width="100%">
	             <tr><td> <button id="btnLineAdd" style="width:90px" onclick="btnClick()">Add</button> </td></tr>	      
	             <tr><td> <button id="btnLineEdit" style="width:90px" onclick="btnClick()">Edit</button> </td></tr>	 
	             <tr><td> <button id="btnLineDelete" style="width:90px" onclick="btnClick()">Delete</button> </td></tr>	 
	             <tr><td> <button id="btnLineMoveUp" style="width:90px" onclick="btnClick()">Move Up</button> </td></tr>	 
	             <tr><td> <button id="btnLineMoveDown" style="width:90px" onclick="btnClick()">Move Down</button> </td></tr>	        
	             </table>
	        </td> 	        
	     </tr>
	     </table>
	    
	    </div>
	    <div id= "divStation">
	     <table >
	     <tr>
	        <td>
	            Selected Stations:
	        </td>
	     </tr>
	     <tr>
	        <td>
		        <div id='divStationTable'>
	            </div>        
	        </td> 
	        <td  style="padding-left:3px" >
	             <table id ="Table1" width="100%">
	             <tr><td> <button id="btnStationAdd" style="width:90px" onclick="btnClick()">Add</button> </td></tr>	      
	             <tr><td> <button id="btnStationEdit" style="width:90px" onclick="btnClick()">Edit</button> </td></tr>	 
	             <tr><td> <button id="btnStationDelete" style="width:90px" onclick="btnClick()">Delete</button> </td></tr>	 
	             <tr><td> <button id="btnStationMoveUp" style="width:90px" onclick="btnClick()">Move Up</button> </td></tr>	 
	             <tr><td> <button id="btnStationMoveDown" style="width:90px" onclick="btnClick()">Move Down</button> </td></tr>	        
	             </table>
	        </td>  	        
	     </tr>
	    <tr id="Tr3" Style="height:5px">
	        <td></td>
        </tr>
	     
	     </table>
	     
	    </div>
	    </fieldset>
	    </td>
    </tr> 
    <tr id="idButtonBlock" Style="display:none">
	    <td align="right"><br/>
	     <div>
	            <button id="btnSave" style="width:105px" onclick="saveClick()">Save</button>&nbsp;&nbsp;
	            <button id="btnDisplay" style="width:105px" onclick="displayClick()">Save & Display</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	        </div>
	    </td>
    </tr>
    </table>
</body>
   <fis:footer id="footer1" runat="server"/>    
  

<script FOR=window EVENT=onload>

   if(initPage()==false)
   {
       return;
   }
   document.getElementById("headName").style.width=parseInt(document.body.clientWidth)-1;
   
   document.getElementById("dWindowName").style.width=parseInt(document.body.clientWidth*0.79);
   document.getElementById("dDisplayName").style.width=parseInt(document.body.clientWidth*0.79);
   document.getElementById("dAlertMessage").style.width=parseInt(document.body.clientWidth*0.79);
   
   
   if(getEditData.WindowLineInfos.length>0) 
   {
       lineTable.HighLightRow(0); 
   } 
   
   if(windowId=="")
   {
       setDefaultStageWorkTime();
   }

</SCRIPT>
       
<script type="text/javascript" language="javascript">

var isDoing=false;

//!!!need check 和数据库一致
var stageName;

//!!!need 
//document.getElementById('headName').innerHTML=windowName;
var lineTable = null;
var stationTable=null;

var dlgTitleForLineEdit="Edit Line";
var operationForLineEdit="new";

var dlgTitleForStationEdit="Edit Station";
var operationForStationEdit="new";

//打开的对话框大小 
var dlgLineFeature = "dialogHeight:360px;dialogWidth:435px;center:yes;status:no;help:no";
var dlgStationFeature = "dialogHeight:310px;dialogWidth:400px;center:yes;status:no;help:no";

//对应编辑数据的Object
var addEditLineObj;
var addEditStationObj;

var lineListDataTable=null;
var stationListDataTable=null;

var itemArray=new Array();
var getEditData;

var SA;
var FA;

//initPage();

function onOptionChange()
{

}

//新增或者新变换一个stage选择时调用
//从数据库取是否有该stage的start work time, stop work time;
//若有，替换当前的缺省的start work time, stop work time数据,若没有使用省缺值
function setDefaultStageWorkTime()
{
    var stageId= stageName;
    if(stageId!="")
    {
         var workTimeInfo=null;
         var rtn;    
         try
         {
            rtn = com.inventec.portal.dashboard.Fa.DashboardManager.GetStageWorkTimeInfo(stageId);
         }
         catch(e)
         {
            alert("Can't get data from server.");
            return;
         }         
         
         if (rtn.error != null) 
         {
             alert(rtn.error.Message);
             return;
         }
         else 
         { 
             workTimeInfo=rtn.value;
         }  
         
         if(workTimeInfo.length==2)
         {
             document.getElementById("dStartWorkTimeHour").value=workTimeInfo[0].Hour;
             document.getElementById("dStartWorkTimeMinute").value=workTimeInfo[0].Minute;
             document.getElementById("dStopWorkTimeHour").value=workTimeInfo[1].Hour;
             document.getElementById("dStopWorkTimeMinute").value=workTimeInfo[1].Minute;
         }
         else
         {
             document.getElementById("dStartWorkTimeHour").value="08";
             document.getElementById("dStartWorkTimeMinute").value="00";
             document.getElementById("dStopWorkTimeHour").value="08";
             document.getElementById("dStopWorkTimeMinute").value="00";      
         
         }
    }
}

function initPage()
{
     stageName="<%=stageName%>";
     var rtn;    
     try
     {
        rtn = com.inventec.portal.dashboard.Fa.DashboardManager.GetDashboardWindowSetting(windowId);
     }
     catch(e)
     {
        alert("Can't get data from server.");
        return false;
     }
          
     if (rtn.error != null) 
     {
         alert(rtn.error.Message);
         window.parent.location.href="dashboardmain.aspx?editor=" + editor;
         return false;

     }
     else 
     { 
         getEditData=rtn.value;
     }
     
     if(getEditData==null)
     {
        return false;
     }

     var windowName=getEditData.WindowName;
     document.getElementById('headName').innerHTML=htmlEncode(windowName);
     //显示主设定画面
     initPageObject(); 
     initLineAndStation(0);
     
     initControlValue();
     
     document.getElementById("idWindowBlock").style.display="";
     document.getElementById("idLineBlock").style.display="";
     document.getElementById("idButtonBlock").style.display="";
     
     return true;
     //testId();

}


//var allId=""
//function testId(){

//    for(var i=0;i<document.all.length;i++){
//        var item=document.all[i];
//        if(item.tagName.toUpperCase()=="INPUT" || item.tagName.toUpperCase()=="TEXTAREA"
//                ||item.tagName.toUpperCase()=="SELECT" ){
//                
//                allId=allId +item.id+" "
//        }
//    }
//    
//    alert(allId)
//}

function initControlValue()
{
     var windowData=getEditData;
  
     document.getElementById("dWindowName").value=windowData.WindowName;
     document.getElementById("dDisplayName").value=windowData.DisplayName;
     document.getElementById("dAlertMessage").value=windowData.AlertMessage;
     document.getElementById("dHour").value=windowData.Hour;
     document.getElementById("dMinute").value=windowData.Minute;
     document.getElementById("dSecond").value=windowData.Second;
     
     
     var DataSourceTypeValue="";
     //type "0"或"1"
     if(windowData.DataSourceType=="0")
     {
         DataSourceTypeValue="Real";
     }
     else
     {
         DataSourceTypeValue="Unreal";
     }    
     
     //注意，一定是all, 它没有id
     setRadioValue(document.all("dDataSourceType"),DataSourceTypeValue);     
     
     var defaultStageId=windowData.StageId;
     
     document.getElementById("dStageId").value=stageName;
//     setSelectedOption(document.getElementById("dStageId"),selectStageOption,"arry",false,false,defaultStageId)
     
     
     var isStageDisplayValue="";
     if(windowData.IsStageDisplay.toLowerCase()=="true")
     {
         isStageDisplayValue="Yes";
     }
     else
     {
         isStageDisplayValue="No";
     }   
     
     //注意，一定是all, 它没有id
     setRadioValue(document.all("dIsStageDisplay"),isStageDisplayValue); 
     
     document.getElementById("dStartWorkTimeHour").value=windowData.StartWorkTimeHour;
     document.getElementById("dStartWorkTimeMinute").value=windowData.StartWorkTimeMinute;
     document.getElementById("dStopWorkTimeHour").value=windowData.StopWorkTimeHour;
     document.getElementById("dStopWorkTimeMinute").value=windowData.StopWorkTimeMinute; 
     
     if(defaultStageId!=null && defaultStageId!="")
     {
          changeDisplayCheckByStageChange(true); 
     }
     else
     {
          changeDisplayCheckByStageChange(false); 
     }
      
}


function changeDisplayCheckByStageChange(isSetCheckByGetWindowsData)
{     
     var windowData=getEditData;

//     if(stageId==SA)
//     {
//         if(isSetCheckByGetWindowsData==true)
//         {
//             setCheckBoxValue(document.getElementById("dIsGoalDisplay"), windowData.IsGoalDisplay);
//             setCheckBoxValue(document.getElementById("dIsSaInputDisplay"), windowData.IsSaInputDisplay);
//             setCheckBoxValue(document.getElementById("dIsSaOutputDisplay"), windowData.IsSaOutputDisplay);
//             setCheckBoxValue(document.getElementById("dIsSaRateDisplay"), windowData.IsRateDisplay);
//             
//             
//         }
//         else
//         {
//             setCheckBoxValue(document.getElementById("dIsGoalDisplay"), "True");
//             setCheckBoxValue(document.getElementById("dIsSaInputDisplay"), "True");
//             setCheckBoxValue(document.getElementById("dIsSaOutputDisplay"), "True");
//             setCheckBoxValue(document.getElementById("dIsSaRateDisplay"), "True");       
//         }
         
//         setCheckBoxValue(document.getElementById("dIsDnDisplay"), "True");
//         setCheckBoxValue(document.getElementById("dIsFaInputDisplay"), "True");
//         setCheckBoxValue(document.getElementById("dIsFaOutputDisplay"), "True");
//         setCheckBoxValue(document.getElementById("dIsPaInputDisplay"), "True");
//         setCheckBoxValue(document.getElementById("dIsPaOutputDisplay"), "True");
//         setCheckBoxValue(document.getElementById("dIsFaRateDisplay"), "True");
//         document.getElementById("idStageDefTable2").style.display="";
//         document.getElementById("idStageDefTable3").style.display="none";
//    }
//    else
//    {    
//         setCheckBoxValue(document.getElementById("dIsGoalDisplay"), "True");
//         setCheckBoxValue(document.getElementById("dIsSaInputDisplay"), "True");
//         setCheckBoxValue(document.getElementById("dIsSaOutputDisplay"), "True");
//         setCheckBoxValue(document.getElementById("dIsSaRateDisplay"),"True");
         
         if(isSetCheckByGetWindowsData==true)
         {
             setCheckBoxValue(document.getElementById("dIsDnDisplay"), windowData.IsDnDisplay);
             setCheckBoxValue(document.getElementById("dIsFaInputDisplay"), windowData.IsFaInputDisplay);
             setCheckBoxValue(document.getElementById("dIsFaOutputDisplay"), windowData.IsFaOutputDisplay);
             setCheckBoxValue(document.getElementById("dIsPaInputDisplay"), windowData.IsPaInputDisplay);
             setCheckBoxValue(document.getElementById("dIsPaOutputDisplay"), windowData.IsPaOutputDisplay);
             setCheckBoxValue(document.getElementById("dIsFaRateDisplay"), windowData.IsRateDisplay);
         }
         else
         {
             setCheckBoxValue(document.getElementById("dIsDnDisplay"), "True");
             setCheckBoxValue(document.getElementById("dIsFaInputDisplay"), "True");
             setCheckBoxValue(document.getElementById("dIsFaOutputDisplay"),"True");
             setCheckBoxValue(document.getElementById("dIsPaInputDisplay"), "True");
             setCheckBoxValue(document.getElementById("dIsPaOutputDisplay"), "True");
             setCheckBoxValue(document.getElementById("dIsFaRateDisplay"), "True");       
         }
         document.getElementById("idStageDefTable2").style.display="none";
         document.getElementById("idStageDefTable3").style.display="";
//    }
}


///////

//初始化selectedLineNum为0
function initLineAndStation(selectedLineNum)
{
   var lineList=getEditData.WindowLineInfos;
   var lineRs=getLineRs(lineList);
   
   var stationRs;
   var stationList;
   
   showLineTable(lineRs);   

   if(lineList.length> selectedLineNum)
   {
       stationList=lineList[selectedLineNum];
   }
   else
   {
     var rtn;    
     try
     {
        rtn = com.inventec.portal.dashboard.Fa.DashboardManager.GetNewStationList();
     }
     catch(e)
     {
        alert("Can't get data from server.");
        return;
     }
          
     if (rtn.error != null) 
     {
         alert(rtn.error.Message);
         return;
     }
     else 
     { 
         stationList=rtn.value;
     }    
   }
   
   if(stationList==null)
   {
       return;
   }
   stationRs=getStationRs(stationList);   
   showStationTable(stationRs);  
   
//    不允许在此设置，表格控件不允许    
//    if(lineList.length> selectedLineNum)
//    {
//        lineTable.HighLightRow(selectedLineNum); 
//    } 

}

function showLineTable(rs)
{

//    var tableWidth = parseInt(document.body.clientWidth*0.80);
    var tableWidth = parseInt(document.body.clientWidth*0.96)-88;
    var clientH =  parseInt((document.body.clientHeight-414)*0.49);
    
    if (lineTable == null){
        lineTable = new clsTable(rs, "lineTable");
        if (clientH > 50){
    	    lineTable.Height = clientH;
        } else {
    	    lineTable.Height = 50;
        }
	    lineTable.TableWidth = tableWidth;    	    
	    var tmpWidths = new Array(10,11,12,12,16,12,12,12);  
        lineTable.Widths = getTableWidthsFixColumn(tmpWidths,tableWidth);
        lineTable.FieldsType = new Array( 0, 0, 0, 0, 0, 0, 0, 0);
        lineTable.HideColumn = new Array(true,true,true,true,true,true,true,true);
        //lineTable.UseSort = "TDCData";
	    lineTable.Divide = "<%=Constants.COL_DELIM%>";
	    lineTable.ScreenWidth = -1;
	    lineTable.AfterNew = "onLineTableAfterNew()";
        //lineTable.BeforeSave = "MyBeforeSave()";

	    //lineTable.UpDown = "fUpdateIssueDetail()";
	    lineTable.UseHTML = false;
    }
    
    lineTable.rs_main = rs;
    divLineTable.innerHTML = lineTable.Display();
    
    //HideWait();

}

function showStationTable(rs)
{
//    var tableWidth = parseInt(document.body.clientWidth*0.80);
    var tableWidth = parseInt(document.body.clientWidth*0.96)-88;
    //var clientH =  parseInt(document.body.clientHeight*0.28);
    var clientH =  parseInt((document.body.clientHeight-414)*0.49);
    
    if (stationTable == null){
        stationTable = new clsTable(rs, "stationTable");
        if (clientH > 50){
    	    stationTable.Height = clientH;
        } else {
    	    stationTable.Height = 50;
        }

	    stationTable.TableWidth = tableWidth;    	    
	    var tmpWidths = new Array(15,12,12,12,12,12);  
        stationTable.Widths = getTableWidthsFixColumn(tmpWidths,tableWidth);
        stationTable.FieldsType = new Array( 0, 0, 0, 0, 0, 0);
        stationTable.HideColumn = new Array(true,true,true,true,true,true);
        //lineTable.UseSort = "TDCData";
	    stationTable.Divide = "<%=Constants.COL_DELIM%>";
	    stationTable.ScreenWidth = -1;
	    //lineTable.AfterNew = "MyAfterNew()";
        //lineTable.BeforeSave = "MyBeforeSave()";

	    //lineTable.UpDown = "fUpdateIssueDetail()";
	    stationTable.UseHTML = false;
    }
    
    stationTable.rs_main = rs;
    divStationTable.innerHTML = stationTable.Display();
    
    //HideWait();

}

function getTableWidthsFixColumn(widths,tableWidth){

    var width=tableWidth;
    width = width-19;
    var total=0;
    for(var i=0;i<widths.length;i++){
	    total=total+widths[i];
    }
    if(total == 0){
        total=1;
    }
    var returnArray =new Array();
    var count=0;
    var countWidth=0;
    for(var i=0;i<widths.length;i++){
        count=i;
        returnArray[i] = parseInt(widths[i]*width/total);
        countWidth+=returnArray[i];
    }

    returnArray[count]=parseInt(returnArray[count]+width-countWidth);
    return returnArray;
}

//将line列表变为数据源
function getLineRs(lineList) {

    var rsSite = new ActiveXObject("ADODB.Recordset");
   
    rsSite.Fields.Append("Stage", 202,500);
    rsSite.Fields.Append("Name", 202,500);
    rsSite.Fields.Append("FPY Target", 202,500);
    rsSite.Fields.Append("FPY Alert", 202,500);
    rsSite.Fields.Append("Output Target", 202,500);
    rsSite.Fields.Append("Start Work", 202,500);
    rsSite.Fields.Append("Stop Work", 202,500);
    rsSite.Fields.Append("Station Dsp",202,500);
    rsSite.Open();

    for (var i = 0; i < lineList.length; i++)
    {
        var lineItem=lineList[i];
        rsSite.AddNew();
        rsSite.Fields(0)=lineItem.Stage;
        rsSite.Fields(1)=lineItem.Line;
        
        if(lineItem.FPYTarget!="")
        {
            rsSite.Fields(2)=lineItem.FPYTarget+"%";
        }
        else
        {
            rsSite.Fields(2)="";
        }
        
        if(lineItem.FPYAlert!="")
        {
            rsSite.Fields(3)=lineItem.FPYAlert+"%";
        }
        else
        {
            rsSite.Fields(3)="";
        }
        
        rsSite.Fields(4)=lineItem.OutputTarget;
        rsSite.Fields(5)=lineItem.StartWorkHour+":"+lineItem.StartWorkMinute;
        rsSite.Fields(6)=lineItem.StopWorkHour+":"+lineItem.StopWorkMinute;
        
        //!!!need check
        if(lineItem.StationDisplay.toLowerCase()=="true")
        {
            rsSite.Fields(7)="Yes";
        }
        else
        {
            rsSite.Fields(7)="No";
        }
        rsSite.Update();
    }

    return rsSite;
}  

//将station列表变为数据源
function getStationRs(stationList) {

    var rsSite = new ActiveXObject("ADODB.Recordset");
   
    rsSite.Fields.Append("Name", 202,500);
    rsSite.Fields.Append("Quantity", 202,500);
    rsSite.Fields.Append("Defect", 202,500);
    rsSite.Fields.Append("WIP", 202,500);
    rsSite.Fields.Append("Yield Target", 202,500);
    rsSite.Fields.Append("Factor of FPY", 202,500);
    rsSite.Open();

    for (var i = 0; i < stationList.length; i++)
    {
        var stationItem=stationList[i];
        rsSite.AddNew();
        rsSite.Fields(0)=stationItem.Station;
        
        //!!!need check
        if(stationItem.Quantity.toLowerCase()=="true")
        {
            rsSite.Fields(1)="V";
        }
        else
        {
            rsSite.Fields(1)="X";
        }        
        
        //!!!need check
        if(stationItem.Defect.toLowerCase()=="true")
        {
            rsSite.Fields(2)="V";
        }
        else
        {
            rsSite.Fields(2)="X";
        }
         //!!!need check
        if(stationItem.WIP.toLowerCase()=="true")
        {
            rsSite.Fields(3)="V";
        }
        else
        {
            rsSite.Fields(3)="X";
        }

        if(stationItem.YieldTarget!="")
        {
            rsSite.Fields(4)=stationItem.YieldTarget+"%";
        }
        else
        {
            rsSite.Fields(4)="";
        }
        
        if(stationItem.FactorOfFPY.toLowerCase()=="true")
        {
            rsSite.Fields(5)="V";
        }
        else
        {
            rsSite.Fields(5)="X";
        }
        rsSite.Update();
    }
    return rsSite;
}               

function initPageObject()
{
    //window
    var spans=new Array();
    //span={{行，列，行个数，列个数},{}}=>行数列数以0开头
    spans[0]="0,1,1,4";
    spans[1]="1,1,1,4";
    spans[2]="2,1,1,4";
    spans[3]="4,1,1,2";
    spans[4]="5,1,1,2";
  
    var heights=new Array(25,25,25,25,25,25);
    
    var layoutWindowDefOptions = {cellspacing:0,border:"0",cellpadding:0,width:"100%",//height:paramHeight,
        span:spans,colNum:6,rowNum:6,name:"idWindowDefTable",heights:heights,
        widths:new Array("18%","18%","18%","18%","25%","3%")};
    var layoutWindowDefHTML= getLayoutHTML(layoutWindowDefOptions);
    
    //stage
    var spans1=new Array();
    var heights1=new Array();
    heights1[0]=25;
    var layoutStageDefOptions = {cellspacing:0,border:"0",cellpadding:0,width:"100%",//height:paramHeight,
        span:spans1,colNum:8,rowNum:1,name:"idStageDefTable",heights:heights1,
        widths:new Array("16%","24%","11%","6%","12%","11%","6%","16%")};

    var layoutStageDefHTML= getLayoutHTML(layoutStageDefOptions);     
    
    //stage各字段是否显示 sa
    var layoutStageDefOptions2 = {cellspacing:0,border:"0",cellpadding:0,width:"100%",//height:paramHeight,
        span:spans1,colNum:6,rowNum:1,name:"idStageDefTable2",heights:heights1,
        widths:new Array("16%","9%","12%","12%","12%","39%")};

    var layoutStageDefHTML2= getLayoutHTML(layoutStageDefOptions2);
    
    //fa的显示
    var spans3=new Array();
    var layoutStageDefOptions3 = {cellspacing:0,border:"0",cellpadding:0,width:"100%",//height:paramHeight,
        span:spans1,colNum:8,rowNum:1,name:"idStageDefTable3",heights:heights1,
        widths:new Array("16%","9%","12%","12%","12%","12%","12%","15%")};

    var layoutStageDefHTML3= getLayoutHTML(layoutStageDefOptions3);
    
    var strBody=layoutWindowDefHTML+ "<hr />"+layoutStageDefHTML+layoutStageDefHTML2+layoutStageDefHTML3;    
    document.getElementById("divWindow").innerHTML= strBody;
    idStageDefTable3.style.display="none";
    //alert(strBody)
    /////////////////////

    itemArray[0]={baseName:"WindowName",type:"lable",text:"Window Name"};
    itemArray[1]={baseName:"WindowName",need:"y",needPrompt:"Please input the Window Name!",save:"y",maxlength:"128",type:"text"}
    itemArray[2]={baseName:"WindowNameNeed",type:"lable",text:"*",nocolon:"yes"};
    
    itemArray[3]={baseName:"DisplayName",type:"lable",text:"Display Name"};
    itemArray[4]={baseName:"DisplayName",need:"y",needPrompt:"Please input the Display Name!",save:"y",maxlength:"128",type:"text"}
    itemArray[5]={baseName:"DisplayNameNeed",type:"lable",text:"*",nocolon:"yes"};

    itemArray[6]={baseName:"AlertMessage",type:"lable",text:"Alert Message"};    
    itemArray[7]={baseName:"AlertMessage",save:"y",maxlength:"512",type:"text"}
    
    itemArray[8]={baseName:"RefreshTime",type:"lable",text:"Refresh Time"};
     
    itemArray[9]={baseName:"Hour",type:"partTimeSelect",maxvalue:"24",unittext:"hour(s)"}
    itemArray[10]={baseName:"Minute",type:"partTimeSelect",maxvalue:"60",unittext:"minute(s)"}
    itemArray[11]={baseName:"Second",type:"partTimeSelect",maxvalue:"60",unittext:"second(s)"}
    
    itemArray[12]={baseName:"DataSourceType",type:"lable",text:"Data Source"}    
    var items=new Array();
    items[0]="Real";
    items[1]="Unreal";
    itemArray[13]={baseName:"DataSourceType",type:"radio",items:items}
    
    itemArray[14]={baseName:"StageId",type:"lable",text:"Stage"}  
    itemArray[15]={baseName:"StageId",type:"text",readOnly:"true"}
    itemArray[16]={baseName:"StageIdNeed",type:"lable",text:"*",nocolon:"yes"};
    ////////////////////////////////
    
    itemArray[17]={baseName:"IsStageDisplay",type:"lable",text:"Stage Display"};
    var items1=new Array();
    items1[0]="Yes";
    items1[1]="No";
    itemArray[18]={baseName:"IsStageDisplay",type:"radio",items:items1}
    
    itemArray[19]={baseName:"StartWork",type:"lable",text:"Start Work"}
    itemArray[20]={baseName:"StartWorkTimeHour",type:"partTimeSelect",maxvalue:"24",unittext:"&nbsp;:"}
    itemArray[21]={baseName:"StartWorkTimeMinute",type:"partTimeSelect",maxvalue:"60",unittext:""}
    
    itemArray[22]={baseName:"StopWork",type:"lable",text:"Stop Work"}
    itemArray[23]={baseName:"StopWorkTimeHour",type:"partTimeSelect",maxvalue:"24",unittext:"&nbsp;:"}
    itemArray[24]={baseName:"StopWorkTimeMinute",type:"partTimeSelect",maxvalue:"60",unittext:""}
    
    itemArray[25]={baseName:"DisplayFields",type:"lable",text:"Display Fields"}
    
    itemArray[26]={baseName:"IsGoalDisplay",type:"checkbox",item:"Goal"}
    itemArray[27]={baseName:"IsSaInputDisplay",type:"checkbox",item:"SA Input"}
    itemArray[28]={baseName:"IsSaOutputDisplay",type:"checkbox",item:"SA Output"}
    itemArray[29]={baseName:"IsSaRateDisplay",type:"checkbox",item:"Rate"} //!!!
    
    
    itemArray[30]={baseName:"IsDnDisplay",type:"checkbox",item:"DN"}
    itemArray[31]={baseName:"IsFaInputDisplay",type:"checkbox",item:"FA Input"}
    itemArray[32]={baseName:"IsFaOutputDisplay",type:"checkbox",item:"FA Output"}
    itemArray[33]={baseName:"IsPaInputDisplay",type:"checkbox",item:"PA Input"}
    itemArray[34]={baseName:"IsPaOutputDisplay",type:"checkbox",item:"PA Output"}
    itemArray[35]={baseName:"IsFaRateDisplay",type:"checkbox",item:"Rate"} //!!!
    
    setPos("idWindowDefTable","0-0",itemArray,0);
    setPos("idWindowDefTable","0-1",itemArray,1);
    setPos("idWindowDefTable","0-5",itemArray,2);
    setPos("idWindowDefTable","1-0",itemArray,3);
    setPos("idWindowDefTable","1-1",itemArray,4);
    setPos("idWindowDefTable","1-5",itemArray,5);
    setPos("idWindowDefTable","2-0",itemArray,6);
    setPos("idWindowDefTable","2-1",itemArray,7);
    
    setPos("idWindowDefTable","3-0",itemArray,8);
    setPos("idWindowDefTable","3-1",itemArray,9);
    setPos("idWindowDefTable","3-2",itemArray,10);
    setPos("idWindowDefTable","3-3",itemArray,11);
    
    setPos("idWindowDefTable","4-0",itemArray,12);
    setPos("idWindowDefTable","4-1",itemArray,13);
    
    setPos("idWindowDefTable","5-0",itemArray,14);
    setPos("idWindowDefTable","5-1",itemArray,15);
    setPos("idWindowDefTable","5-5",itemArray,16);
    
    /////////
    setPos("idStageDefTable","0-0",itemArray,17);
    setPos("idStageDefTable","0-1",itemArray,18);
    setPos("idStageDefTable","0-2",itemArray,19);
    setPos("idStageDefTable","0-3",itemArray,20);
    setPos("idStageDefTable","0-4",itemArray,21);
    setPos("idStageDefTable","0-5",itemArray,22);
    setPos("idStageDefTable","0-6",itemArray,23);
    setPos("idStageDefTable","0-7",itemArray,24);    
    
    setPos("idStageDefTable2","0-0",itemArray,25);
    setPos("idStageDefTable2","0-1",itemArray,26);
    setPos("idStageDefTable2","0-2",itemArray,27);
    setPos("idStageDefTable2","0-3",itemArray,28);
    setPos("idStageDefTable2","0-4",itemArray,29);      
    
    setPos("idStageDefTable3","0-0",itemArray,25);
    setPos("idStageDefTable3","0-1",itemArray,30);
    setPos("idStageDefTable3","0-2",itemArray,31);
    setPos("idStageDefTable3","0-3",itemArray,32);
    setPos("idStageDefTable3","0-4",itemArray,33);  
    setPos("idStageDefTable3","0-5",itemArray,34);  
    setPos("idStageDefTable3","0-6",itemArray,35);

}

function setAllObjWidths(areaName){

    for(var i=0;i<document.all(areaName).all.length;i++){
    	var item=document.all(areaName).all[i];
        if(item.tagName.toUpperCase()!="BUTTON"){
      		var item=document.all(areaName).all[i];
        	item.style.width=item.offsetWidth;
        }
    }
}

function dealBtnClick()
{
   switch(event.srcElement.id)
   {
   case "btnLineAdd" :
         operationForLineEdit="new";

         var rtn;    
         try
         {
            rtn = com.inventec.portal.dashboard.Fa.DashboardManager.GetNewLine();
         }
         catch(e)
         {
            alert("Can't get data from server.");
            return;
         }
              
         if (rtn.error != null) 
         {
             alert(rtn.error.Message);
             return;
         }
         else 
         { 
             addEditLineObj=rtn.value;
         } 
        
        if(addEditLineObj==null)
        {
            return;
        }
                
        addEditLineObj.Stage=stageName;    
        var stageId= stageName;
        if(stageId=="")
        {
            alert("Please check data for [Dashboard_Stage] table, the stage can't be null!");
            return;
        }
        //var rtnLineList = com.inventec.portal.dashboard.DashboardManager.GetLineListByStage(stageId);
        var existLines=new Array();
        for (var i=0;i<getEditData.WindowLineInfos.length;i++)
        {
            existLines[i]=getEditData.WindowLineInfos[i].LineID;
        }
        var rtnLineList;    
        try
        {
           rtnLineList =  com.inventec.portal.dashboard.Fa.DashboardManager.GetLineListByStageEclipse(existLines);
        }
        catch(e)
        {
           alert("Can't get data from server.");
           return;
        }
     
        if (rtnLineList.error != null) 
        {
            alert(rtnLineList.error.Message);
            return;
        }
        else 
        { 
            lineListDataTable=rtnLineList.value;
        } 
      
        if(lineListDataTable==null)
        {
            return;
        }
        
 	    var dlgReturn=window.showModalDialog("lineEdit.aspx", window, dlgLineFeature);

 	    if(dlgReturn=="OK")
 	    {
 	        var currentHighlightRow= getEditData.WindowLineInfos.length;
 	        getEditData.WindowLineInfos[currentHighlightRow]=addEditLineObj; 
 	        var lineRs=getLineRs(getEditData.WindowLineInfos);
 	        showLineTable(lineRs);
 	        var stationList=addEditLineObj.WindowLineStationInfos;
            var stationRs=getStationRs(stationList); 

            showStationTable(stationRs);
            lineTable.HighLightRow(currentHighlightRow);
 	        
 	    }
 	    
 	    break;  
   case "btnLineEdit":

        var rownum=lineTable.GetRowNumber();
        if(rownum <0 || rownum >= lineTable.rs_main.recordcount||lineTable.IsEmpty==true)
        {
            return;
        }         
        
        operationForLineEdit="edit";
        var lineNumForLineEdit=rownum;
        addEditLineObj=getEditData.WindowLineInfos[rownum];
        
        var stageId= stageName;
        if(stageId=="")
        {
            alert("Please check data for [Dashboard_Stage] table, the stage can't be null!");
            return;
        }
        //var rtnLineList = com.inventec.portal.dashboard.DashboardManager.GetLineListByStage(stageId);
        
        var existLines=new Array();
        for (var i=0;i<getEditData.WindowLineInfos.length;i++)
        {
            if(i!=rownum)
            {
                existLines[existLines.length]=getEditData.WindowLineInfos[i].LineID;
            }            
        }
       
        var rtnLineList;
        try
        {
           rtnLineList = com.inventec.portal.dashboard.Fa.DashboardManager.GetLineListByStageEclipse(existLines);
        }
        catch(e)
        {
           alert("Can't get data from server.");
           return;
        }
        
        if (rtnLineList.error != null) 
        {
            alert(rtnLineList.error.Message);
            return;
        }
        else 
        { 
            lineListDataTable=rtnLineList.value;
        }
        if(lineListDataTable==null)
        {
            return;
        }
        
        lineIdBeforeEdit=addEditLineObj.LineID;
        var dlgReturn=window.showModalDialog("lineEdit.aspx", window, dlgLineFeature);
 	    if(dlgReturn=="OK")
 	    {
 	        lineIdAfterEdit=addEditLineObj.LineID;
 	        
 	        var currentHighlightRow=lineNumForLineEdit; 	        
 	        var stationList;
 	        if(lineIdAfterEdit==lineIdBeforeEdit)
 	        {
 	            stationList=addEditLineObj.WindowLineStationInfos;
 	        }
 	        else
 	        {
                 var rtn;    
                 try
                 {
                    rtn = com.inventec.portal.dashboard.Fa.DashboardManager.GetNewStationList();
                 }
                 catch(e)
                 {
                    alert("Can't get data from server.");
                    return;
                 }
                 
                 if (rtn.error != null) 
                 {
                     alert(rtn.error.Message);
                     return;
                 }
                 else 
                 { 
                     stationList=rtn.value;
                 }	
                 
                 if(stationList==null)
                 {
                     return;
                 }
                 
                 addEditLineObj.WindowLineStationInfos= stationList;      
 	        }

            //!!!note
 	        getEditData.WindowLineInfos[currentHighlightRow]=addEditLineObj;
 	        
 	        var lineRs=getLineRs(getEditData.WindowLineInfos);
 	        showLineTable(lineRs);
 	         	       
            var stationRs=getStationRs(stationList);   
            showStationTable(stationRs);
            
            lineTable.HighLightRow(currentHighlightRow);              
 	        
 	    }        
        break;
        
   case "btnLineDelete":
        var rownum=lineTable.GetRowNumber();
        if(rownum <0 || rownum >= lineTable.rs_main.recordcount||lineTable.IsEmpty==true)
        {
            return;
        }   
         
        getEditData.WindowLineInfos=RemoveFromArray(getEditData.WindowLineInfos,rownum);
 	        
        //高亮行前移             
        if(rownum>0)
        {
            rownum=rownum-1;
        } 
        
        initLineAndStation(rownum);
        
        if(getEditData.WindowLineInfos.length>0)
        {
            lineTable.HighLightRow(rownum); 
        }
        break;
   case "btnLineMoveUp":
        var rownum=lineTable.GetRowNumber();
        if(rownum <0 || rownum >= lineTable.rs_main.recordcount||lineTable.IsEmpty==true)
        {
            return;
        }     
        if(rownum==0 || lineTable.rs_main.recordcount<2)
        {
            return;
        }

        var tmpSave=getEditData.WindowLineInfos[rownum-1];
        getEditData.WindowLineInfos[rownum-1]=getEditData.WindowLineInfos[rownum];
        getEditData.WindowLineInfos[rownum]=tmpSave;
        
        var lineRs=getLineRs(getEditData.WindowLineInfos);
 	    showLineTable(lineRs);
        
        //高亮移动后的行
        rownum=rownum-1;
        //因为HighLightRow会触发AfterNew,会自动更新station
        lineTable.HighLightRow(rownum); 
        break;
   case "btnLineMoveDown":
   
        var rownum=lineTable.GetRowNumber();
        if(rownum <0 || rownum >= lineTable.rs_main.recordcount||lineTable.IsEmpty==true)
        {
            return;
        }  
        if(rownum>=lineTable.rs_main.recordcount-1 || lineTable.rs_main.recordcount<2)
        {
            return;
        }

        var tmpSave=getEditData.WindowLineInfos[rownum+1];
        getEditData.WindowLineInfos[rownum+1]=getEditData.WindowLineInfos[rownum];
        getEditData.WindowLineInfos[rownum]=tmpSave;
        
        var lineRs=getLineRs(getEditData.WindowLineInfos);
 	    showLineTable(lineRs);
        
        //高亮移动后的行
        rownum=rownum+1;
        //因为HighLightRow会触发AfterNew,会自动更新station
        lineTable.HighLightRow(rownum); 
        break; 
        
   case "btnStationAdd" :
         operationForStationEdit="new";
        
         var rownumLine=lineTable.GetRowNumber();
         if(rownumLine <0 || rownumLine >= lineTable.rs_main.recordcount||lineTable.IsEmpty==true)
         {
             return;
         }           

         var rtn;    
         try
         {
            rtn = com.inventec.portal.dashboard.Fa.DashboardManager.GetNewStation();
         }
         catch(e)
         {
            alert("Can't get data from server.");
            return;
         }
                  
         if (rtn.error != null) 
         {
             alert(rtn.error.Message);
             return;
         }
         else 
         { 
             addEditStationObj=rtn.value;
         } 

         if(addEditStationObj==null)
         {
             return;
         }         
         
        //取得station列表 
        var lineId= getEditData.WindowLineInfos[rownumLine].LineID;  
        
        var existStations=new Array();
        for (var i=0;i<getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos.length;i++)
        {
            existStations[i]=getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos[i].StationId;
        }
        
        var rtnStationList;    
        try
        {
           rtnStationList = com.inventec.portal.dashboard.Fa.DashboardManager.GetStationListByLineEclipse(lineId,existStations);
        }
        catch(e)
        {
           alert("Can't get data from server.");
           return;
        }             
        
        //var rtnStationList = com.inventec.portal.dashboard.DashboardManager.GetStationListByLine(lineId);
        if (rtnStationList.error != null) 
        {
            alert(rtnStationList.error.Message);
            return;
        }
        else 
        { 
            stationListDataTable=rtnStationList.value;
        } 
        
         if(stationListDataTable==null)
         {
             return;
         }   
        
        //!!!还需要考虑把line的值带进去
        addEditStationObj.Line=getEditData.WindowLineInfos[rownumLine].Line;
        var dlgReturn=window.showModalDialog("stationEdit.aspx", window, dlgStationFeature);
        
 	    if(dlgReturn=="OK")
 	    {
 	        var currentHighlightRow= getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos.length;
 	        getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos[currentHighlightRow]=addEditStationObj;  	       
 	        var stationList=getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos;
            var stationRs=getStationRs(stationList);
            showStationTable(stationRs);
            stationTable.HighLightRow(currentHighlightRow); 	        
 	    }        
        
 	    break;  
   case "btnStationEdit":
   
        var rownum=stationTable.GetRowNumber();
        if(rownum <0 || rownum >= stationTable.rs_main.recordcount||stationTable.IsEmpty==true)
        {
            return;
        }  
           
        var rownumLine=lineTable.GetRowNumber();
        if(rownumLine <0 || rownumLine >= lineTable.rs_main.recordcount||lineTable.IsEmpty==true)
        {
            return;
        }                     
           
        //取得station列表 
        var lineId= getEditData.WindowLineInfos[rownumLine].LineID; 
        
        var existStations=new Array();
        for (var i=0;i<getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos.length;i++)
        {
            if(i!=rownum)
            {
                existStations[existStations.length]=getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos[i].StationId;
            }
        }

        //var rtnStationList = com.inventec.portal.dashboard.DashboardManager.GetStationListByLine(lineId);
        var rtnStationList;    
        try
        {
           rtnStationList = com.inventec.portal.dashboard.Fa.DashboardManager.GetStationListByLineEclipse(lineId,existStations); 
        }
        catch(e)
        {
           alert("Can't get data from server.");
           return;
        }
             
        if (rtnStationList.error != null) 
        {
            alert(rtnStationList.error.Message);
            return;
        }
        else 
        { 
            stationListDataTable=rtnStationList.value;
        }    
           
        if(stationListDataTable==null)
        {
            return;
        }  
                    
        operationForStationEdit="edit";
        var lineNumForStationEdit=rownum;
        addEditStationObj=getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos[lineNumForStationEdit];
        var dlgReturn=window.showModalDialog("stationEdit.aspx", window, dlgStationFeature);

 	    if(dlgReturn=="OK")
 	    {	        
 	        var currentHighlightRow=lineNumForStationEdit; 	
 	        //!!! note        
            getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos[currentHighlightRow]=addEditStationObj;
 	        var stationList=getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos;
            var stationRs=getStationRs(stationList);
            showStationTable(stationRs);
            stationTable.HighLightRow(currentHighlightRow);              
        }
        break;
        
   case "btnStationDelete":
        var rownum=stationTable.GetRowNumber();
        if(rownum <0 || rownum >= stationTable.rs_main.recordcount||stationTable.IsEmpty==true)
        {
            return;
        }  
           
        var rownumLine=lineTable.GetRowNumber();
        if(rownumLine <0 || rownumLine >= lineTable.rs_main.recordcount||lineTable.IsEmpty==true)
        {
            return;
        }     
        
        getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos=RemoveFromArray(getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos,rownum);
        
        var stationRs=getStationRs(getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos);   
        showStationTable(stationRs);
            
        //高亮行前移             
        if(rownum>0)
        {
            rownum=rownum-1;
        } 
        
        if(getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos.length>0)
        {
            stationTable.HighLightRow(rownum); 
        }    
   
        break;
   case "btnStationMoveUp":
        var rownum=stationTable.GetRowNumber();
        if(rownum <0 || rownum >= stationTable.rs_main.recordcount||stationTable.IsEmpty==true)
        {
            return;
        }  
           
        var rownumLine=lineTable.GetRowNumber();
        if(rownumLine <0 || rownumLine >= lineTable.rs_main.recordcount||lineTable.IsEmpty==true)
        {
            return;
        }     
        
        if(rownum==0 || stationTable.rs_main.recordcount<2)
        {
            return;
        }
                
        var tmpSave=getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos[rownum-1];
        getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos[rownum-1]=getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos[rownum];
        getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos[rownum]=tmpSave;
        
        var stationRs=getStationRs(getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos);
        showStationTable(stationRs);
        
        //高亮移动后的行
        rownum=rownum-1;
        //因为HighLightRow会触发AfterNew,会自动更新station
        stationTable.HighLightRow(rownum); 

        break;
   case "btnStationMoveDown":
   
        var rownum=stationTable.GetRowNumber();
        if(rownum <0 || rownum >= stationTable.rs_main.recordcount||stationTable.IsEmpty==true)
        {
            return;
        }  
           
        var rownumLine=lineTable.GetRowNumber();
        if(rownumLine <0 || rownumLine >= lineTable.rs_main.recordcount||lineTable.IsEmpty==true)
        {
            return;
        }     
        
        if(rownum>=stationTable.rs_main.recordcount-1 || stationTable.rs_main.recordcount<2)
        {
            return;
        }
                
        var tmpSave=getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos[rownum+1];
        getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos[rownum+1]=getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos[rownum];
        getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos[rownum]=tmpSave;
        
        var stationRs=getStationRs(getEditData.WindowLineInfos[rownumLine].WindowLineStationInfos);
        showStationTable(stationRs);
        
        //高亮移动后的行
        rownum=rownum+1;
        //因为HighLightRow会触发AfterNew,会自动更新station
        stationTable.HighLightRow(rownum); 
        break;                                 
   }
}

function btnClick()
{
    if(isDoing==true)
    {
        return false;
    }
    isDoing=true;
    
    dealBtnClick();
    isDoing=false;
}

function saveClick()
{
    //ShowWait();
    if(isDoing==true)
    {
        return false;
    }
    isDoing=true;
    for(var i=0;i<itemArray.length;i++)
    {
        var item=itemArray[i];

        if(item.type!="select" && item.type!="text" )
        {
            continue;
        }
        
        var object=document.getElementById("d"+item.baseName);
        
        if(object==null )
        {
            continue;
        }
        
        var value="";
        if(object.tagName.toUpperCase()=="INPUT" || object.tagName.toUpperCase()=="TEXTAREA" ){
             value = trimString(getObjectValue(object));
        }
        else if(object.tagName.toUpperCase()=="SELECT"){
            value = trimString(getSelectValue(object,"value"));
        }
        if(item.need=="y" && value ==""){
            alert(item.needPrompt);
            focusObject(object);
            //HideWait();
            isDoing=false;
            return false;
        }
    }

    
    var dataItem=getEditData;

    dataItem.WindowName = trimString(document.getElementById("dWindowName").value);   
    dataItem.DisplayName = trimString(document.getElementById("dDisplayName").value);   
    dataItem.AlertMessage = trimString(document.getElementById("dAlertMessage").value);   
    dataItem.Hour = trimString(getSelectValue(document.getElementById("dHour"),"text"));
    dataItem.Minute = trimString(getSelectValue(document.getElementById("dMinute"),"text"));
    dataItem.Second = trimString(getSelectValue(document.getElementById("dSecond"),"text"));
    
    dataItem.StageId = stageName;
    dataItem.StageName = stageName;

    //必须document.all
    if(getRadioValue(document.all("dDataSourceType"))=="Real")
    {
        dataItem.DataSourceType="0";
    }
    else
    {
        dataItem.DataSourceType="1";
    }
    
    if(getRadioValue(document.all("dIsStageDisplay"))=="Yes")
    {
        dataItem.IsStageDisplay="True";
    }
    else
    {
        dataItem.IsStageDisplay="False";
    }
    
    dataItem.StartWorkTimeHour = trimString(getSelectValue(document.getElementById("dStartWorkTimeHour"),"text"));
    dataItem.StartWorkTimeMinute = trimString(getSelectValue(document.getElementById("dStartWorkTimeMinute"),"text"));
    dataItem.StopWorkTimeHour = trimString(getSelectValue(document.getElementById("dStopWorkTimeHour"),"text"));
    dataItem.StopWorkTimeMinute = trimString(getSelectValue(document.getElementById("dStopWorkTimeMinute"),"text"));
    

    if(getCheckBoxValue(document.getElementById("dIsDnDisplay")) ==true)
    {
        dataItem.IsDnDisplay ="True";
    }
    else
    {
        dataItem.IsDnDisplay ="False";
    }
    if(getCheckBoxValue(document.getElementById("dIsFaInputDisplay")) ==true)
    {
        dataItem.IsFaInputDisplay ="True";
    }
    else
    {
        dataItem.IsFaInputDisplay ="False";
    }

    if(getCheckBoxValue(document.getElementById("dIsFaOutputDisplay")) ==true)
    {
        dataItem.IsFaOutputDisplay ="True";
    }
    else
    {
        dataItem.IsFaOutputDisplay ="False";
    }   
    if(getCheckBoxValue(document.getElementById("dIsPaInputDisplay")) ==true)
    {
        dataItem.IsPaInputDisplay ="True";
    }
    else
    {
        dataItem.IsPaInputDisplay ="False";
    }  
    if(getCheckBoxValue(document.getElementById("dIsPaOutputDisplay")) ==true)
    {
        dataItem.IsPaOutputDisplay ="True";
    }
    else
    {
        dataItem.IsPaOutputDisplay ="False";
    }  
    if(getCheckBoxValue(document.getElementById("dIsFaRateDisplay")) ==true)
    {
        dataItem.IsRateDisplay ="True";
    }
    else
    {
        dataItem.IsRateDisplay ="False";
    }  
    dataItem.IsGoalDisplay  = "False";    
    dataItem.IsSaInputDisplay  = "False"; 
    dataItem.IsSaOutputDisplay = "False"; 

    dataItem.Editor=editor;
    
    windowId=dataItem.WindowId;
    dataItem.toJSON = function(){return toJSON(this);};	

    var rtn;    
    try
    {
       rtn = com.inventec.portal.dashboard.Fa.DashboardManager.SaveDashboardWindowSetting(dataItem);
    }
    catch(e)
    {
       isDoing=false;
       alert("Can't get data from server.");
       return;
    }
         
    if (rtn.error != null) 
    {
        alert(rtn.error.Message);
        //HideWait();
        isDoing=false;
        return false;
    }
    else 
    { 
        windowId=rtn.value;
    } 
    
    if(windowId==null)
    {
        isDoing=false;
        return;
    }       
    //alert("Save successful!"); 
    //!!! need check
    //window.parent.frames("menu").tree.freshNodeRoot();   
    //window.parent.frames("menu").tree.focusClientNode(3, true)
    //setTimeout('window.parent.frames("menu").tree.focusClientNode(5, true)',1000);
    window.parent.frames("menu").tree.insertNode1(windowId); 
    //HideWait(); 
    
    setTimeout('NotDoing()', 1000);  
    return true;
}

function NotDoing()
{
   isDoing=false; 
}

function displayClick()
{

   if(saveClick()==false)
   {
       return;
   }
   //windowId应该刚刚被设置过值
   //!!!showDisplay(windowId);
   window.parent.showDisplay(windowId);
}

function getCloneObj(obj)
{
    var returnValue=obj;

    return returnValue;
}

//控制station的button
function onLineTableAfterNew(){

    var rownum=lineTable.GetRowNumber();
    
    var stationList;

    if(rownum <0 || rownum >= lineTable.rs_main.recordcount||lineTable.IsEmpty==true)
    {
         var rtn;    
         try
         {
            rtn = com.inventec.portal.dashboard.Fa.DashboardManager.GetNewStationList();
         }
         catch(e)
         {
            alert("Can't get data from server.");
            return;
         }
              
         if (rtn.error != null) 
         {
             alert(rtn.error.Message);
             return;
         }
         else 
         { 
             stationList=rtn.value;
         } 
         
         if(stationList==null)
         {
            return;
         }
         
         disabledStationButton(true);
    }  
//    else if(getEditData.WindowLineInfos[rownum].StationDisplay=="False")
//    {
//        disabledStationButton(true);
//        stationList=getEditData.WindowLineInfos[rownum].WindowLineStationInfos;
//    }
    else
    {
        disabledStationButton(false);
        stationList=getEditData.WindowLineInfos[rownum].WindowLineStationInfos;
    }    
    
    
    var stationRs=getStationRs(stationList);
    showStationTable(stationRs); 
    
    if(stationList.length>0) 
    {
        stationTable.HighLightRow(0); 
    }     
    
}

function disabledStationButton(isDisabled)
{
    document.getElementById("btnStationAdd").disabled=isDisabled;
    document.getElementById("btnStationEdit").disabled=isDisabled;
    document.getElementById("btnStationDelete").disabled=isDisabled;
    document.getElementById("btnStationMoveUp").disabled=isDisabled;
    document.getElementById("btnStationMoveDown").disabled=isDisabled;

}

function RemoveFromArray(obj, index)
{
    var returnObj=new Array();
    for(var i=0;i<obj.length;i++)
    {
        if(i!=index)
        {
            returnObj[returnObj.length]=obj[i];
        }
    }
    return returnObj;    
}

</script> 
</html>
