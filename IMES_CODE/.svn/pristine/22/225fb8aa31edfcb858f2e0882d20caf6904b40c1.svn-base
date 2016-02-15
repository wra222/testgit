<%--
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: preview for chart 
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2009-12-12   itc94006         Create 
 * Known issues: ITC-1101-0011 
--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="stationEdit.aspx.cs" Inherits="webroot_aspx_dashboard_stationEdit" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Edit Station</title>
</head>
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

button{
	font-family: "Arial", "Helvetica", "sans-serif";
	font-size: 14px;
}

legend {margin: -10px 0 0 0;position:relative;font-size:11pt;} 

label {
	font-family: "Arial", "Helvetica", "sans-serif";
	font-size: 12px;
	font-weight: bolder;
}

</style>


<body style="overflow:auto">

<form id="form2" runat="server">
<div></div>
</form>
    
<table id="idcontent" width="100%" border="0" cellpadding="0" cellspacing="5" >
    <tr>
      <td  ></td>
    </tr>
    <tr>
      <td ><div><center id=divMain1 style="width :94%"></center></div></td>
    </tr>
    <tr>
      <td  ></td>
    </tr>
    <tr>
      <td >
      	  <fieldset style="height:100%;width:100%">
            <legend >Display Fields</legend>
             <div ><center id=divMain2 ></center></div>
          </fieldset>
      </td>
    </tr>
    
    <tr Style="height:7px">
      <td  ></td>
    </tr>
    <tr id="idButtonBlock" >
	    <td align="right">
	     <div style="width :94%">
	            <button id="btnOK" style="width:90px" onclick="btnClick()">OK</button>&nbsp;&nbsp;
	            <button id="btnCancel" style="width:90px" onclick="btnClick()">Cancel</button>&nbsp;&nbsp;
	     </div>
	    </td>
    </tr>
    
</table>
</body>

<SCRIPT FOR=dDefect  EVENT=onclick()>

EnabledObjects();
EnabledFactorOfFPY();

</SCRIPT>

<SCRIPT FOR=dYieldRate  EVENT=onclick()>

EnabledFactorOfFPY();

</SCRIPT>

<script language="JavaScript">

var parentWindow;
var itemArray;
//////////
parentWindow = window.dialogArguments;
//!!! need change
if(parentWindow.dlgTitleForLineEdit!=null){
  window.document.title=parentWindow.dlgTitleForStationEdit;
}

function onOptionChange()
{
//alert(event.srcElement.id)
}

init();

////////////////
function init(){

    showWindow();
    initControlValue();
    EnabledObjects();
    EnabledFactorOfFPY();
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
    var stationItem=parentWindow.addEditStationObj;
    document.getElementById("dLableLine").innerHTML=stationItem.Line;
    setSelectedOption(document.getElementById("dStation"),parentWindow.stationListDataTable,"datatable",false,false,stationItem.StationId);
    //初始化时为空
    if(stationItem.StationId=="")
    {
        document.getElementById("dStation").selectedIndex=-1;
    }    
    
    setCheckBoxValue(document.getElementById("dQuantity"), stationItem.Quantity);
    setCheckBoxValue(document.getElementById("dWIP"), stationItem.WIP);
    setCheckBoxValue(document.getElementById("dDefect"), stationItem.Defect);
    setCheckBoxValue(document.getElementById("dYieldRate"), stationItem.YieldRate);
    
    setCheckBoxValue(document.getElementById("dFactorOfFPY"), stationItem.FactorOfFPY);
    
    document.getElementById("dYieldTarget").value=stationItem.YieldTarget;
 
}
    
function showWindow(){

    itemArray=new Array();
    //window
    var spans1=new Array();
        
    var layoutWindowDefOptions1 = {cellspacing:0,border:"0",cellpadding:0,width:"100%",height:56,
    span:spans1,colNum:3,rowNum:2,name:"idWindowDefTable1",//heights:heights,
    widths:new Array("30%","65","5%")};
    
    var layoutWindowDefHTML1= getLayoutHTML(layoutWindowDefOptions1);
    document.getElementById("divMain1").innerHTML= layoutWindowDefHTML1;   
    
    
    var spans2=new Array();
    //span={{行，列，行个数，列个数},{}}=>行数列数以0开头
    spans2[0]="2,2,1,3";
//    spans2[1]="1,1,1,4";
//    spans2[2]="2,1,1,3";
//    spans2[3]="3,1,1,4";
//    spans2[4]="6,1,1,2";

    var layoutWindowDefOptions2 = {cellspacing:0,border:"0",cellpadding:0,width:"100%",height:120,
        span:spans2,colNum:5,rowNum:4,name:"idWindowDefTable2",//heights:heights,
        widths:new Array("22%","25%","43","5%","5%")};
    var layoutWindowDefHTML2= getLayoutHTML(layoutWindowDefOptions2);
    
 
    document.getElementById("divMain2").innerHTML= layoutWindowDefHTML2;
    
    //alert(strBody)
    /////////////////////
    itemArray[0]={baseName:"LineLable",type:"lable",text:"Line"};
    itemArray[1]={baseName:"Line",type:"lable",text:"",nocolon:"yes"}
    
    itemArray[2]={baseName:"Station",type:"lable",text:"Station"};
    itemArray[3]={baseName:"Station",type:"select",need:"y",needPrompt:"Please select the Station!",style:"width:232"}
    itemArray[4]={baseName:"StationNeedmark",type:"lable",text:"*",nocolon:"yes"}
    
    itemArray[5]={baseName:"Quantity",type:"checkbox",item:"Quantity"}
    itemArray[6]={baseName:"WIP",type:"checkbox",item:"WIP"}
    itemArray[7]={baseName:"Defect",type:"checkbox",item:"Defect"}
    itemArray[8]={baseName:"YieldRate",type:"checkbox",item:"Yield Rate"}
    
    itemArray[9]={baseName:"YieldTarget",type:"lable",text:"Yield Target"};  
    //!!!数字型  
    itemArray[10]={baseName:"YieldTarget",maxlength:"9",type:"text", onkeypress:"checkOneDecimalPress(this)",style:"ime-mode:disabled;width:136"}
    itemArray[11]={baseName:"YieldTargetmark",type:"lable",text:"%",nocolon:"yes"}
    //itemArray[12]={baseName:"YieldTargetNeedmark",type:"lable",text:"*",nocolon:"yes"}
    
    itemArray[12]={baseName:"FactorOfFPY",type:"checkbox",item:"Factor of FPY formula"}
    
    setPos("idWindowDefTable1","0-0",itemArray,0);
    setPos("idWindowDefTable1","0-1",itemArray,1);
    setPos("idWindowDefTable1","1-0",itemArray,2);
    setPos("idWindowDefTable1","1-1",itemArray,3);
    setPos("idWindowDefTable1","1-2",itemArray,4);
    
    setPos("idWindowDefTable2","0-0",itemArray,5);
    setPos("idWindowDefTable2","1-0",itemArray,6);
    setPos("idWindowDefTable2","2-0",itemArray,7);    
    setPos("idWindowDefTable2","2-1",itemArray,8);
    
    setPos("idWindowDefTable2","2-2",itemArray,12);
    
    setPos("idWindowDefTable2","3-1",itemArray,9);
    setPos("idWindowDefTable2","3-2",itemArray,10);
    setPos("idWindowDefTable2","3-3",itemArray,11);   
    //setPos("idWindowDefTable2","3-4",itemArray,12); 
    
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
     
    
    if(getCheckBoxValue(document.getElementById("dYieldRate")) ==true)
    {    
        if(trimString(getObjectValue(document.getElementById("dYieldTarget")))=="")
        {
            alert("Please input the Yield Target!");
            return false;
        }
        
        if(parseInt(document.getElementById("dYieldTarget").value)>100)
        {
            alert("The number in Yield Target field should be between 0 and 100.");
            return false;
        }    
        
        if(OneDecimalCheck(document.getElementById("dYieldTarget").value)==false)
        {
            alert("The number in Yield Target field is not right.");
            return false;
        }
        
    }
    
    var stationItem=parentWindow.addEditStationObj;
    stationItem.StationId=trimString(getSelectValue(document.getElementById("dStation"),"value"));
    stationItem.Station=trimString(getSelectValue(document.getElementById("dStation"),"text"));
    
    if(getCheckBoxValue(document.getElementById("dQuantity")) ==true)
    {
        stationItem.Quantity ="True";
    }
    else
    {
        stationItem.Quantity ="False";
    }
    
    if(getCheckBoxValue(document.getElementById("dDefect")) ==true)
    {
        stationItem.Defect ="True";
    }
    else
    {
        stationItem.Defect ="False";
    }
        
    if(getCheckBoxValue(document.getElementById("dWIP")) ==true)
    {
        stationItem.WIP ="True";
    }
    else
    {
        stationItem.WIP ="False";
    }
    
    if(getCheckBoxValue(document.getElementById("dYieldRate")) ==true)
    {
        stationItem.YieldRate ="True";
    }
    else
    {
        stationItem.YieldRate ="False";
    }       
             
    if(getCheckBoxValue(document.getElementById("dFactorOfFPY")) ==true)
    {
        stationItem.FactorOfFPY="True";
    }  
    else 
    {
        stationItem.FactorOfFPY="False";
    }       
             
    stationItem.YieldTarget = trimString(document.getElementById("dYieldTarget").value);          
    parentWindow.addEditStationObj=stationItem;
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

function EnabledFactorOfFPY()
{
   var yieldRateChecked=getCheckBoxValue(document.getElementById("dYieldRate"))

    if(yieldRateChecked==false)
    {
        setCheckBoxValue(document.getElementById("dFactorOfFPY"), "False");
        document.getElementById("dFactorOfFPY").disabled=true;
    }
    else
    {
        document.getElementById("dFactorOfFPY").disabled=false;
    }

}

function EnabledObjects()
{
    var defectChecked=getCheckBoxValue(document.getElementById("dDefect"))

    if(defectChecked==false)
    {
        setCheckBoxValue(document.getElementById("dYieldRate"), "False");
        document.getElementById("dYieldTarget").value="";
        document.getElementById("dYieldRate").disabled=true;
        document.getElementById("dYieldTarget").disabled=true;
    }
    else
    {
        document.getElementById("dYieldRate").disabled=false;
        document.getElementById("dYieldTarget").disabled=false;
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

