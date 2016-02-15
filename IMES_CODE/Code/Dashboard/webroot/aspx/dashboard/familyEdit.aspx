<%@ Page Language="C#" AutoEventWireup="true" CodeFile="familyEdit.aspx.cs" Inherits="webroot_aspx_dashboard_FamilyEdit" %>

<%--
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: preview for chart 
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2011-11-25   itc94006        Create 
 * Known issues:
--%>
<%string strContextPath = Request.ApplicationPath;%>
<script type="text/javascript" language="javascript" src="<%=strContextPath%>/webroot/commoncontrol/function/commonFunction.js"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Edit Family</title>
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
      <td ><div><center id="divMain1" style="width :94%"></center></div></td>
    </tr>
    <tr>
      <td  ></td>
    </tr>
       
    <tr style="height:7px">
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
    if(event.srcElement.id=="dFamily")
    {    
       setSeriesOptionsByFamily();
       document.getElementById("dSeries").selectedIndex=-1;
    }
}

init();

////////////////
function init(){

    showWindow();
    initControlValue();
    //EnabledObjects();
    //EnabledFactorOfFPY();
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
    setSelectedOption(document.getElementById("dFamily"),parentWindow.stationListDataTable,"datatable",false,false,stationItem.Family);
    //初始化时为空
    if(stationItem.Family=="")
    {
        document.getElementById("dFamily").selectedIndex=-1;
    }    
    
    document.getElementById("dPlanQty").value=stationItem.PlanQty; 
    setSeriesOptionsByFamily(stationItem.Series);
    if(stationItem.Series=="")
    {
        document.getElementById("dSeries").selectedIndex=-1;
    }   
 
}
    
function showWindow(){

    itemArray=new Array();
    //window
    var spans1=new Array();
        
    var layoutWindowDefOptions1 = {cellspacing:0,border:"0",cellpadding:0,width:"100%",height:120,
    span:spans1,colNum:3,rowNum:4,name:"idWindowDefTable1",//heights:heights,
    widths:new Array("30%","65","5%")};
    
    var layoutWindowDefHTML1= getLayoutHTML(layoutWindowDefOptions1);
    document.getElementById("divMain1").innerHTML= layoutWindowDefHTML1;   
    
    //alert(strBody)
    /////////////////////
    itemArray[0]={baseName:"LineLable",type:"lable",text:"Line"};
    itemArray[1]={baseName:"Line",type:"lable",text:"",nocolon:"yes"}
    
    itemArray[2]={baseName:"Family",type:"lable",text:"Family"};
    itemArray[3]={baseName:"Family",type:"select",need:"y",needPrompt:"Please select the Family!",style:"width:232"}
    itemArray[4]={baseName:"FamilyNeedmark",type:"lable",text:"*",nocolon:"yes"}
    
    itemArray[5]={baseName:"Series",type:"lable",text:"Series"};
    itemArray[6]={baseName:"Series",type:"select",need:"y",needPrompt:"Please select the Series!",style:"width:232"}
    itemArray[7]={baseName:"SeriesNeedmark",type:"lable",text:"*",nocolon:"yes"}
    
    itemArray[8]={baseName:"PlanQty",type:"lable",text:"Plan Qty"};  
    itemArray[9]={baseName:"PlanQty",maxlength:"6",type:"text",need:"y",needPrompt:"Please input the Plan Qty!", onkeypress:"checkIntegerPress(this)", onpaste:"return false", style:"ime-mode:disabled;width:136"}
    itemArray[10]={baseName:"PlanQtyNeedmark",type:"lable",text:"*",nocolon:"yes"}
   
    setPos("idWindowDefTable1","0-0",itemArray,0);
    setPos("idWindowDefTable1","0-1",itemArray,1);
    setPos("idWindowDefTable1","1-0",itemArray,2);
    setPos("idWindowDefTable1","1-1",itemArray,3);
    setPos("idWindowDefTable1","1-2",itemArray,4);
    
    setPos("idWindowDefTable1","2-0",itemArray,5);
    setPos("idWindowDefTable1","2-1",itemArray,6);
    setPos("idWindowDefTable1","2-2",itemArray,7);    
    
    setPos("idWindowDefTable1","3-0",itemArray,8);
    setPos("idWindowDefTable1","3-1",itemArray,9);
    setPos("idWindowDefTable1","3-2",itemArray,10);
    
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
        
    if(parseInt(document.getElementById("dPlanQty").value)<=0)
    {
        alert("The Plan Qty value should be larger than 0.");
        return false;
    }
    
    var stationItem=parentWindow.addEditStationObj;
    stationItem.Family=trimString(getSelectValue(document.getElementById("dFamily"),"value"));
    stationItem.Series=trimString(getSelectValue(document.getElementById("dSeries"),"text"));
    stationItem.PlanQty=trimString(document.getElementById("dPlanQty").value);    
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

function setSeriesOptionsByFamily(defaultValue)
{
    var familyValue=trimString(getSelectValue(document.getElementById("dFamily"),"value"));
    
    var rtnSeriesList;  
    var seriesListDataTable;
      
    try
    {
       
       var familys=parentWindow.existFamilys;
       var seriesis=parentWindow.existSeriesList;      

       familyValue.toJSON = function(){return toJSON(this);};
       familys.toJSON = function(){return toJSON(this);};	
       seriesis.toJSON = function(){return toJSON(this);};	
       rtnSeriesList = com.inventec.portal.dashboard.Smt.DashboardManager.GetSeriesListByFamily(familyValue,familys,seriesis);
    }
    catch(e)
    {
       alert("Can't get data from server.");
       return;
    }
         
    if (rtnSeriesList.error != null) 
    {
        alert(rtnSeriesList.error.Message);
        return;
    }
    else 
    { 
        seriesListDataTable=rtnSeriesList.value;
    }
    
    setSelectedOption(document.getElementById("dSeries"),seriesListDataTable,"datatable",false,false,defaultValue);

    
}

</script>

</html>

