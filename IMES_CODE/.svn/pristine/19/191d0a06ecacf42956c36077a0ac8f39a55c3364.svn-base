<%@ Page Language="C#" AutoEventWireup="true" CodeFile="saLineEdit.aspx.cs" Inherits="webroot_aspx_dashboard_saLineEdit" %>
<%--
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: preview for chart 
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2011-11-24   itc94006         Create 
 * Known issues:
--%>


<html xmlns="http://www.w3.org/1999/xhtml">
<HEAD id="HEAD1" runat="server">
    <title>Edit Line</title>
</HEAD>
<!-- #include file="dashboardUtility.aspx" --> 
<style type="text/css">

*{
	font-family: "Arial", "Helvetica", "sans-serif";
	font-size: 12px;
}

body{
	margin: 0;
	padding: 0;
	overflow: auto;
	background-color: rgb(231,240,247);
}

legend {margin: -10px 0 0 0;position:relative;font-size:11pt;} 

label {
	font-family: "Arial", "Helvetica", "sans-serif";
	font-size: 12px;
	font-weight: bolder;
}

button{
	font-family: "Arial", "Helvetica", "sans-serif";
	font-size: 14px;
}

</style>
<%

%>
<script language="javascript">
//    function checkWinAndDataLoad(){
//       if(bDataLoad==true && bWindowLoad==true ){
//           if(parentWindow.operation=="modify"){
//               focusObject("dTSubCategory_name");
//           }else{
//               focusObject("dTCategory_id");
//           }
//           var tHeight=parseInt(window.dialogHeight)-(document.body.clientHeight-document.all("idcontent").clientHeight-document.all("dDiv").offsetHeight);
//           window.dialogHeight=tHeight+"px";
//       }
//    }
</script>
<SCRIPT FOR=window EVENT=onload>
//bWindowLoad=true;
//checkWinAndDataLoad();
</SCRIPT>

<body style="overflow:auto">

<form id="form2" runat="server">
<div></div>
</form>
    
<table id="idcontent" width="100%" border="0" cellpadding="1" cellspacing="5" >
    <tr>
      <td  ></td>
    </tr>
    <tr>
      <td ><div ><center id="divMain" ></center></div></td>
    </tr>
     <tr>
      <td  ></td>
    </tr>
    <tr>
      <td >
      	  <fieldset style="height:100%;width:100%">
            <legend >Option Fields</legend>
             <div ><center id="divMain2" ></center></div>
          </fieldset>
      </td>
    </tr>
    <tr height="10px">
      <td  ></td>
    </tr>
    <tr id="idButtonBlock" >
	    <td align="right">
	     <div>
	            <button id="btnOK" style="width:90px" onclick="btnClick()">OK</button>&nbsp;&nbsp;
	            <button id="btnCancel" style="width:90px" onclick="btnClick()">Cancel</button>&nbsp;&nbsp;
	     </div>
	    </td>
    </tr>
    
</table>
<%--<div id ="dDiv" style="display:none" ><input id="currentUUID" ></input></div>--%>
</body>

<script language="JavaScript">
//var bWindowLoad=false;
//var bDataLoad=false;
//var winWidth =420;//parseInt(window.external.dialogWidth)-80;
//var winHeight =330;//parseInt(window.external.dialogHeight)-300;

///////////////////////////////////

//////////////////
////父窗口传过来的Edit UUID,
//var uuid="";

////当reset时重取2个表格中的数据, 用refreshType标识第几次取数据//
//var type_init=0;
//var type_reset=1;
//var refreshType=type_init;
///////////////
//var idFieldName="TSubCategory_id";  //存放item id的字段名

////提示文字
//var promptCancel="The data you entered will not be saved if you click \"Cancel\". Are you sure you want to continue?";
//var promptSaveSucceed="Save successful!";
////必输项物件中的提示
//var nameNeedSubCategory="Please input the sub-category!";
//var nameNeedCategory="Please select the category name!";

//var maxLengthPrompt="It cannot be saved because the characters exceed the limit.";

////map物件数组
//var itemArray=new Array();
////map物件数组相应的Label
//var labels=new Array();

////区分提交是的按钮saveQuit或saveNew////////
//var flag_saveQuit=0;
//var flag_saveNew=1;
//var commitFlag;

var parentWindow;
var itemArray;
//////////
parentWindow = window.dialogArguments;
//!!! need change
if(parentWindow.dlgTitleForLineEdit!=null){
  window.document.title=parentWindow.dlgTitleForLineEdit;
}

init();

function onOptionChange()
{
    switch(event.srcElement.id)
    {
        case "dLine":
            setDefaultLineBaseSetting();
            break;
        case "dShift":
            var shiftValue= getSelectValue(document.getElementById("dShift"),"value");
            if(shiftValue=="DAY")
            {
                 document.getElementById("dStartWorkTimeHour").value="08";
                 document.getElementById("dStartWorkTimeMinute").value="00";       
                 document.getElementById("dStopWorkTimeHour").value="20";
                 document.getElementById("dStopWorkTimeMinute").value="30";                     
            }
            else
            {
                 document.getElementById("dStartWorkTimeHour").value="20";
                 document.getElementById("dStartWorkTimeMinute").value="30";       
                 document.getElementById("dStopWorkTimeHour").value="08";
                 document.getElementById("dStopWorkTimeMinute").value="00";
            }            
            break;            
            
    }
}


//新增或者新变换一个line选择时调用
//从数据库取是否有该line的start work time, stop work time, target等信息;
//若有，替换当前的缺省的start work time, stop work time数据,若没有使用省缺值
function setDefaultLineBaseSetting()
{

    var lineId= getSelectValue(document.getElementById("dLine"),"value");
    
    if(lineId!="")
    {
         var LineSettingInfo=null;
         var rtn;    
         try
         {
            rtn = com.inventec.portal.dashboard.Smt.DashboardManager.GetLineBaseSettingInfo(lineId);
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
             LineSettingInfo=rtn.value;
         }  
         
         document.getElementById("dStartWorkTimeHour").value=LineSettingInfo.StartWorkHour;
         document.getElementById("dStartWorkTimeMinute").value=LineSettingInfo.StartWorkMinute;
         document.getElementById("dStopWorkTimeHour").value=LineSettingInfo.StopWorkHour;
         document.getElementById("dStopWorkTimeMinute").value=LineSettingInfo.StopWorkMinute;
         document.getElementById("dShift").value=LineSettingInfo.Shift;
    
    }
}

////////////////
function init(){

    showWindow();
    initControlValue();
}

var allId=""
function testId(){

    for(var i=0;i<document.all.length;i++){
        var item=document.all[i];
        if(item.tagName.toUpperCase()=="INPUT" || item.tagName.toUpperCase()=="TEXTAREA"
                ||item.tagName.toUpperCase()=="SELECT" ){
                
                allId=allId +item.id+" "
        }
    }
    
    alert(allId)
}

function initControlValue()
{
    var lineItem=parentWindow.addEditLineObj;
    
    document.getElementById("dLableStage").innerHTML=lineItem.Stage

    setSelectedOption(document.getElementById("dLine"),parentWindow.lineListDataTable,"datatable",false,false,lineItem.LineID);
    //初始化时为空
    if(lineItem.LineID=="")
    {
        document.getElementById("dLine").selectedIndex=-1;
    }    
    
    document.getElementById("dShift").value=lineItem.Shift;
    document.getElementById("dStartWorkTimeHour").value=lineItem.StartWorkHour;
    document.getElementById("dStartWorkTimeMinute").value=lineItem.StartWorkMinute;
    document.getElementById("dStopWorkTimeHour").value=lineItem.StopWorkHour;
    document.getElementById("dStopWorkTimeMinute").value=lineItem.StopWorkMinute;
    
    var selectShiftOption=new Array();
    selectShiftOption[0]=new Array("DAY","DAY");
    selectShiftOption[1]=new Array("NIGHT","NIGHT");

    setSelectedOption(document.getElementById("dShift"),selectShiftOption,"array",false,false,lineItem.Shift)

    setCheckBoxValue(document.getElementById("dInput"), lineItem.IsInputDsp);
    setCheckBoxValue(document.getElementById("dDefect"), lineItem.IsDefectDsp);
    setCheckBoxValue(document.getElementById("dYieldRate"), lineItem.IsYieldRateDsp);
    setCheckBoxValue(document.getElementById("dICTInput"), lineItem.IsICTInputDsp);
    setCheckBoxValue(document.getElementById("dICTDefect"), lineItem.IsICTDefectDsp);
    setCheckBoxValue(document.getElementById("dICTYieldRate"), lineItem.IsICTYieldRateDsp);
    
    
}

  
function showWindow(){

    itemArray=new Array();
    //window
    var spans=new Array();
    //span={{行，列，行个数，列个数},{}}=>行数列数以0开头
    spans[0]="0,1,1,5";
    spans[1]="1,1,1,5";
    spans[2]="2,1,1,5";   
    spans[3]="3,1,1,2";
    spans[4]="4,1,1,2";


    var layoutWindowDefOptions = {cellspacing:1,border:"0",cellpadding:0,width:"100%",height:180,
        span:spans,colNum:7,rowNum:5,name:"idWindowDefTable",//heights:heights,
        widths:new Array("26%","12%","5%","16%","31%","5%","5%")};
    var layoutWindowDefHTML= getLayoutHTML(layoutWindowDefOptions);
    document.getElementById("divMain").innerHTML= layoutWindowDefHTML;
    
    var spans2=new Array();
    //span={{行，列，行个数，列个数},{}}=>行数列数以0开头
    var layoutWindowDefOptions2 = {cellspacing:0,border:"0",cellpadding:0,width:"100%",height:60,
        span:spans2,colNum:3,rowNum:2,name:"idWindowDefTable2",//heights:heights,
        widths:new Array("32%","32%","36")};
    var layoutWindowDefHTML2= getLayoutHTML(layoutWindowDefOptions2);    
 
    document.getElementById("divMain2").innerHTML= layoutWindowDefHTML2;  
        
    //alert(strBody)
    /////////////////////
    itemArray[0]={baseName:"StageLable",type:"lable",text:"Stage"};
    itemArray[1]={baseName:"Stage",type:"lable",text:"",nocolon:"yes"}
    
    itemArray[2]={baseName:"Line",type:"lable",text:"Line"};
    itemArray[3]={baseName:"Line",type:"select",need:"y",needPrompt:"Please select the Line!"}
    itemArray[4]={baseName:"needmark1",type:"lable",text:"*",nocolon:"yes"}
 
    itemArray[5]={baseName:"Shift",type:"lable",text:"Shift"};
    itemArray[6]={baseName:"Shift",type:"select",need:"y",needPrompt:"Please select the Shift!"}
    itemArray[7]={baseName:"needmark2",type:"lable",text:"*",nocolon:"yes"}
    
    itemArray[8]={baseName:"StartWork",type:"lable",text:"Start Work"};
    itemArray[9]={baseName:"StartWorkTimeHour",type:"partTimeSelect",maxvalue:"24",unittext:"&nbsp;&nbsp;&nbsp;:"}
    itemArray[10]={baseName:"StartWorkTimeMinute",type:"partTimeSelect",maxvalue:"60",unittext:""}
    
    itemArray[11]={baseName:"StopWork",type:"lable",text:"Stop Work"};
    itemArray[12]={baseName:"StopWorkTimeHour",type:"partTimeSelect",maxvalue:"24",unittext:"&nbsp;&nbsp;&nbsp;:"}
    itemArray[13]={baseName:"StopWorkTimeMinute",type:"partTimeSelect",maxvalue:"60",unittext:""}
    
    itemArray[14]={baseName:"Input",type:"checkbox",item:"Input"}
    itemArray[15]={baseName:"Defect",type:"checkbox",item:"Defect"}
    itemArray[16]={baseName:"YieldRate",type:"checkbox",item:"Yield&nbspRate"}
    itemArray[17]={baseName:"ICTInput",type:"checkbox",item:"ICT&nbspInput"}
    itemArray[18]={baseName:"ICTDefect",type:"checkbox",item:"ICT&nbspDefect"}
    itemArray[19]={baseName:"ICTYieldRate",type:"checkbox",item:"ICT&nbspYield&nbspRate"}
    
    setPos("idWindowDefTable","0-0",itemArray,0);
    setPos("idWindowDefTable","0-1",itemArray,1);
    
    setPos("idWindowDefTable","1-0",itemArray,2);
    setPos("idWindowDefTable","1-1",itemArray,3);
    setPos("idWindowDefTable","1-6",itemArray,4);
        
    setPos("idWindowDefTable","2-0",itemArray,5);
    setPos("idWindowDefTable","2-1",itemArray,6);
    setPos("idWindowDefTable","2-6",itemArray,7);
    
    setPos("idWindowDefTable","3-0",itemArray,8);
    setPos("idWindowDefTable","3-1",itemArray,9);
    setPos("idWindowDefTable","3-3",itemArray,10);
    
    setPos("idWindowDefTable","4-0",itemArray,11);
    setPos("idWindowDefTable","4-1",itemArray,12);
    setPos("idWindowDefTable","4-3",itemArray,13);

    setPos("idWindowDefTable2","0-0",itemArray,14);
    setPos("idWindowDefTable2","0-1",itemArray,15);
    setPos("idWindowDefTable2","0-2",itemArray,16);
    setPos("idWindowDefTable2","1-0",itemArray,17);
    setPos("idWindowDefTable2","1-1",itemArray,18);   
    setPos("idWindowDefTable2","1-2",itemArray,19);  
    
}

function btnClick(){
    switch(event.srcElement.id){
        case "btnOK" :   //主附件
        	if(dealSave()==false)
        	{
        	    return;
        	}
        	window.returnValue = "OK";
            window.close();	
        break;
        case "btnCancel" :
        	window.returnValue = "Cancel";
            window.close();	
        break;
    }
}

function dealSave(){
    
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
            return false;
        }
    }
    
    
    var lineItem=parentWindow.addEditLineObj;
    lineItem.LineID=trimString(getSelectValue(document.getElementById("dLine"),"value"));
    lineItem.Line=trimString(getSelectValue(document.getElementById("dLine"),"text"));
    lineItem.Shift = document.getElementById("dShift").value; 
    lineItem.StartWorkHour = document.getElementById("dStartWorkTimeHour").value;    
    lineItem.StartWorkMinute = document.getElementById("dStartWorkTimeMinute").value; 
    lineItem.StopWorkHour = document.getElementById("dStopWorkTimeHour").value;      
    lineItem.StopWorkMinute= document.getElementById("dStopWorkTimeMinute").value;  
    //必须document.all
    

    if(getCheckBoxValue(document.all("dInput"))==true )
    {
        lineItem.IsInputDsp="True";
    }
    else
    {
        lineItem.IsInputDsp="False";
    }
    
    if(getCheckBoxValue(document.all("dDefect"))==true)
    {
        lineItem.IsDefectDsp="True";
    }
    else
    {
        lineItem.IsDefectDsp="False";
    }
    
    if(getCheckBoxValue(document.all("dYieldRate"))==true)
    {
        lineItem.IsYieldRateDsp="True";
    }
    else
    {
        lineItem.IsYieldRateDsp="False";
    }
    
    if(getCheckBoxValue(document.all("dICTInput"))==true)
    {
        lineItem.IsICTInputDsp="True";
    }
    else
    {
        lineItem.IsICTInputDsp="False";
    }
    
    if(getCheckBoxValue(document.all("dICTDefect"))==true)
    {
        lineItem.IsICTDefectDsp="True";
    }
    else
    {
        lineItem.IsICTDefectDsp="False";
    }
    
    if(getCheckBoxValue(document.all("dICTYieldRate"))==true)
    {
        lineItem.IsICTYieldRateDsp="True";
    }
    else
    {
        lineItem.IsICTYieldRateDsp="False";
    }    
    parentWindow.addEditLineObj=lineItem;

    return true;

}

</script>

</html>

