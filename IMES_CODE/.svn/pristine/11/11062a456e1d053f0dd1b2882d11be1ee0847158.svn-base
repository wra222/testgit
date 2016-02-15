<%--
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: preview for chart 
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2009-12-12   itc94006         Create 
 * Known issues: ITC-1101-0005 
--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="lineEdit.aspx.cs" Inherits="webroot_aspx_dashboard_lineEdit" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<HEAD runat="server">
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
<script FOR=window EVENT=onload>
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
      <td ><div ><center id=divMain ></center></div></td>
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
    setDefaultLineBaseSetting();
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
            rtn = com.inventec.portal.dashboard.Fa.DashboardManager.GetLineBaseSettingInfo(lineId);
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
         document.getElementById("dFPYTarget").value=LineSettingInfo.FPYTarget;
         document.getElementById("dFPYAlert").value=LineSettingInfo.FPYAlert;
         document.getElementById("dOutputTarget").value=LineSettingInfo.OutputTarget;
    
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
    
    document.getElementById("dFPYTarget").value=lineItem.FPYTarget;
    document.getElementById("dFPYAlert").value=lineItem.FPYAlert;
    document.getElementById("dOutputTarget").value=lineItem.OutputTarget;
    document.getElementById("dStartWorkTimeHour").value=lineItem.StartWorkHour;
    document.getElementById("dStartWorkTimeMinute").value=lineItem.StartWorkMinute;
    document.getElementById("dStopWorkTimeHour").value=lineItem.StopWorkHour;
    document.getElementById("dStopWorkTimeMinute").value=lineItem.StopWorkMinute;

    var StationDisplayValue="";
    if(lineItem.StationDisplay.toLowerCase()=="true")
    {
        StationDisplayValue="Yes";
    }
    else
    {
        StationDisplayValue="No";
    }
    
    setRadioValue(document.all("dStationDisplay"),StationDisplayValue);
 
}



//function setMapRecordValue(blockName,rs){

//    var recordCount = rs.recordcount;
//    if(recordCount<=0) return "";
//    rs.moveFirst();

//    for(var i=0;i<document.all(blockName).all.length;i++){
//        var item=document.all(blockName).all[i];
//        if(item.tagName.toUpperCase()=="INPUT" || item.tagName.toUpperCase()=="TEXTAREA"
//                ||item.tagName.toUpperCase()=="SELECT" ){
//            var fieldName=item.baseName;
//            if(rs.Fields(fieldName)!=null && rs.Fields(fieldName).value!=null){
//                document.all(item.id).value = rs.Fields(fieldName).value;
//            }
//        }
//    }
//}


    
function showWindow(){

    itemArray=new Array();
    //window
    var spans=new Array();
    //span={{行，列，行个数，列个数},{}}=>行数列数以0开头
    spans[0]="0,1,1,5";
    spans[1]="1,1,1,5";
    spans[2]="2,3,1,3";
    spans[3]="3,2,1,5";
    spans[4]="4,3,1,3";
    spans[5]="5,1,1,5";
    spans[6]="6,1,1,2";
    spans[7]="7,1,1,2";
    spans[8]="8,1,1,3";

    var layoutWindowDefOptions = {cellspacing:1,border:"0",cellpadding:0,width:"100%",height:260,
        span:spans,colNum:7,rowNum:9,name:"idWindowDefTable",//heights:heights,
        widths:new Array("26%","12%","5%","16%","31%","5%","5%")};
    var layoutWindowDefHTML= getLayoutHTML(layoutWindowDefOptions);
    
 
    document.getElementById("divMain").innerHTML= layoutWindowDefHTML;
    //alert(strBody)
    /////////////////////
    itemArray[0]={baseName:"StageLable",type:"lable",text:"Stage"};
    itemArray[1]={baseName:"Stage",type:"lable",text:"",nocolon:"yes"}
    
    itemArray[2]={baseName:"Line",type:"lable",text:"Line"};
    itemArray[3]={baseName:"Line",type:"select",need:"y",needPrompt:"Please select the Line!"}
    itemArray[4]={baseName:"needmark1",type:"lable",text:"*",nocolon:"yes"}

    itemArray[5]={baseName:"FPYTarget",type:"lable",text:"FPY Target"};  
    itemArray[6]={baseName:"FPYTarget",maxlength:"9",type:"text",need:"y",needPrompt:"Please input the FPY Target!",onkeypress:"checkOneDecimalPress(this)",style:"ime-mode:disabled;"}
    itemArray[7]={baseName:"FPYTargetUnit",type:"lable",text:"%",nocolon:"yes"}  
    itemArray[8]={baseName:"FPYTargetPrompt",type:"lable",text:"(When FPY >= Target : GREEN)",nocolon:"yes"}  
    itemArray[9]={baseName:"needmark2",type:"lable",text:"*",nocolon:"yes"}
    
    itemArray[10]={baseName:"MidTargetPrompt",type:"lable",text:"(When Alert <= FPY < Target : YELLO)",nocolon:"yes"}
    
    itemArray[11]={baseName:"FPYAlert",type:"lable",text:"FPY Alert"};
    itemArray[12]={baseName:"FPYAlert",maxlength:"9",type:"text",need:"y",needPrompt:"Please input the FPY Alert!",onkeypress:"checkOneDecimalPress(this)",style:"ime-mode:disabled;"}
    itemArray[13]={baseName:"FPYAlertUnit",type:"lable",text:"%",nocolon:"yes"}
    itemArray[14]={baseName:"FPYAlertPrompt",type:"lable",text:"(When FPY < Alert : RED)",nocolon:"yes"}
    itemArray[15]={baseName:"needmark3",type:"lable",text:"*",nocolon:"yes"}

    
    itemArray[16]={baseName:"OutputTarget",type:"lable",text:"Output Target"};  
    itemArray[17]={baseName:"OutputTarget",maxlength:"9",type:"text",need:"y",needPrompt:"Please input the Output Target!",onblur:"IntegerCheck(this)",onkeypress:"checkIntegerPress(this)",style:"ime-mode:disabled;"}
    itemArray[18]={baseName:"needmark3",type:"lable",text:"*",nocolon:"yes"}
    
    
    itemArray[19]={baseName:"StartWork",type:"lable",text:"Start Work"};
    itemArray[20]={baseName:"StartWorkTimeHour",type:"partTimeSelect",maxvalue:"24",unittext:"&nbsp;&nbsp;&nbsp;:"}
    itemArray[21]={baseName:"StartWorkTimeMinute",type:"partTimeSelect",maxvalue:"60",unittext:""}
    
    itemArray[22]={baseName:"StopWork",type:"lable",text:"Stop Work"};
    itemArray[23]={baseName:"StopWorkTimeHour",type:"partTimeSelect",maxvalue:"24",unittext:"&nbsp;&nbsp;&nbsp;:"}
    itemArray[24]={baseName:"StopWorkTimeMinute",type:"partTimeSelect",maxvalue:"60",unittext:""}
    
   
    itemArray[25]={baseName:"StationDisplay",type:"lable",text:"Station Display"}    
    var items=new Array();
    items[0]="Yes";
    items[1]="No";
    itemArray[26]={baseName:"StationDisplay",type:"radio",items:items}    
    
    
    
    setPos("idWindowDefTable","0-0",itemArray,0);
    setPos("idWindowDefTable","0-1",itemArray,1);
    
    setPos("idWindowDefTable","1-0",itemArray,2);
    setPos("idWindowDefTable","1-1",itemArray,3);
    setPos("idWindowDefTable","1-6",itemArray,4);
    
    
    setPos("idWindowDefTable","2-0",itemArray,5);
    setPos("idWindowDefTable","2-1",itemArray,6);
    setPos("idWindowDefTable","2-2",itemArray,7);
    setPos("idWindowDefTable","2-3",itemArray,8);
    setPos("idWindowDefTable","2-6",itemArray,9);
    
    setPos("idWindowDefTable","3-2",itemArray,10);
    
    setPos("idWindowDefTable","4-0",itemArray,11);
    setPos("idWindowDefTable","4-1",itemArray,12);
    setPos("idWindowDefTable","4-2",itemArray,13);
    setPos("idWindowDefTable","4-3",itemArray,14);
    setPos("idWindowDefTable","4-6",itemArray,15);
    
    setPos("idWindowDefTable","5-0",itemArray,16);
    setPos("idWindowDefTable","5-1",itemArray,17);
    setPos("idWindowDefTable","5-6",itemArray,18);
    
    setPos("idWindowDefTable","6-0",itemArray,19);
    setPos("idWindowDefTable","6-1",itemArray,20);
    setPos("idWindowDefTable","6-3",itemArray,21);
    
    setPos("idWindowDefTable","7-0",itemArray,22);
    setPos("idWindowDefTable","7-1",itemArray,23);
    setPos("idWindowDefTable","7-3",itemArray,24);
    
    setPos("idWindowDefTable","8-0",itemArray,25);
    setPos("idWindowDefTable","8-1",itemArray,26);

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
    
    if(parseInt(document.getElementById("dFPYTarget").value)>100)
    {
        alert("The number in FPY Target field should be between 0 and 100.");
        return false;
    }
    
    if(parseInt(document.getElementById("dFPYAlert").value)>100)
    {
        alert("The number in FPY Alert field should be between 0 and 100.");
        return false;
    }
    
    if(OneDecimalCheck(document.getElementById("dFPYTarget").value)==false)
    {
        alert("The number in FPY Target field is not right.");
        return false;
    }
    
    if(OneDecimalCheck(document.getElementById("dFPYAlert").value)==false)
    {
        alert("The number in FPY Alert field is not right.");
        return false;
    }
    
    if(parseFloat(document.getElementById("dFPYAlert").value)>parseFloat(document.getElementById("dFPYTarget").value))
    {
        alert("'FPY Target' field value should not be less than the value of the field 'FPY Alert'.");
        return false;
    }
    
    
    var lineItem=parentWindow.addEditLineObj;
    lineItem.LineID=trimString(getSelectValue(document.getElementById("dLine"),"value"));
    lineItem.Line=trimString(getSelectValue(document.getElementById("dLine"),"text"));
    lineItem.FPYTarget = document.getElementById("dFPYTarget").value; 
    lineItem.FPYAlert = document.getElementById("dFPYAlert").value;                
    lineItem.OutputTarget = document.getElementById("dOutputTarget").value;          
    lineItem.StartWorkHour = document.getElementById("dStartWorkTimeHour").value;    
    lineItem.StartWorkMinute = document.getElementById("dStartWorkTimeMinute").value; 
    lineItem.StopWorkHour = document.getElementById("dStopWorkTimeHour").value;      
    lineItem.StopWorkMinute= document.getElementById("dStopWorkTimeMinute").value;  
    //必须document.all

    if(getRadioValue(document.all("dStationDisplay"))=="Yes")
    {
        lineItem.StationDisplay="True";
    }
    else
    {
        lineItem.StationDisplay="False";
    }
    parentWindow.addEditLineObj=lineItem;
    return true;
}

function IntegerCheck(obj)
{
    var strChange = obj.value;
	var nQty = 0;
	var reExp = /^[0-9]+$/;
	if (reExp.exec(strChange)){
		nQty =  (new Number(strChange)).valueOf();
	}
	else{
	    nQty = "";
	}
	obj.value = nQty;

}

function checkIntegerPress(obj)
{ 
   var key = event.keyCode;

   if(key < 48||key > 57)
   {  
    event.keyCode = 0;
   }
}

function checkOneDecimalPress(obj)
{ 
   var txtval=obj.value;  
  
   var key = event.keyCode;

   if((key < 48||key > 57) && key != 46 )
   {  
    event.keyCode = 0;
   }
   else
   {
    if(key == 46)
    {
     if(txtval.indexOf(".") != -1 || txtval.length == 0)
      event.keyCode = 0;
    } 
//    else
//    { 
//       //限制只输一位小数点
//       var findIndex=txtval.indexOf("."); 
//       if(findIndex != -1 && txtval.length-1!=findIndex)
//       {
//           event.keyCode = 0;
//       }   
//    }       
    
   }

}

function OneDecimalCheck(value)
{
	var nQty = 0;
	var reExp = /^-?[0-9]+(\.[0-9]{1,2})?$/;
	if (reExp.exec(value))
	{
		return true 
	}
	return false;	
}

</script>

</html>

