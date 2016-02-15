<%--
/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for UnitWeight (for Docking) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI Unit Weight for Docking
 * UC:CI-MES12-SPEC-PAK-UC Unit Weight for Docking            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-29  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/
--%>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UnitWeight.aspx.cs" Inherits="PAK_UnitWeight" Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="~/CommonControl/WeightTypeControl.ascx" TagName="WeightTypeControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="~/CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager runat="server" ID="SM" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="~/Docking/Service/WebServiceUnitWeight.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="divUnitWeight" style="z-index: 0;">
        <fieldset style="width: 95%" align="center">
            <legend id="lblProductInfo" runat="server" style="color: Blue" class="iMes_label_13pt">
            </legend>
            <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
                <tr style="height: 30px" align="left">
                    <td width="16%">
                        <asp:Label ID="lblCustSN" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="24%">
                        <asp:Label runat="server" ID="lblCustSN_Display" Width="90%" Text="" CssClass="iMes_textbox_input_Disabled"></asp:Label>
                    </td>
                    <td width="16%">
                        <asp:Label ID="lblProdId" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="24%">
                        <asp:Label runat="server" ID="lblProdId_Display" Width="90%" Text="" CssClass="iMes_textbox_input_Disabled"></asp:Label>
                    </td>
                </tr>
                <tr style="height: 30px" align="left">
                    <td width="16%">
                        <asp:Label ID="lblModel" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="24%">
                        <asp:Label runat="server" ID="lblModel_Display" Width="90%" Text="" CssClass="iMes_textbox_input_Disabled"></asp:Label>
                    </td>
                    <td width="16%">
                        <asp:Label ID="lblStdWeight" runat="server" class="iMes_label_13pt"> </asp:Label>
                    </td>
                    <td width="24%">
                        <asp:Label runat="server" ID="lblStdWeight_Display" Width="90%" Text="" CssClass="iMes_textbox_input_Disabled"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
        <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center" style="height: 100px">
            <tr height="100" valign="middle">
                <td style="text-align: center" colspan="3">
                    <asp:Label ID="lblPAQC" runat="server" class="iMes_label_30pt_Red"
                        Font-Bold="True" ForeColor="Red"> </asp:Label>
                </td>
            </tr>
            <tr height="100" valign="top">
                <td width="60%" align="left" style="text-align: center">
                    <asp:Label ID="lblUnitWeight" runat="server" class="iMes_label_30pt_Black"> </asp:Label>
                </td>
                <td width="5%">
                </td>
                <td width="35%" align="left" style="text-align: left">
                    <asp:Label ID="lblUnitWeightValue" runat="server" class="iMes_label_30pt_Red_Underline"
                        Font-Bold="True" ForeColor="Red"> </asp:Label>
                </td>
            </tr>
        </table>
        <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center" style="height: 50px">
            <tr valign="middle">
                <td width="16%" align="left">
                    <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                </td>
                <td align="left">
                    <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                        CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                        ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                </td>
            </tr>
        </table>
        <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
            <tr>
                <td>
                </td>
                <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </tr>
        </table>
    </div>

    <script language="javascript" for="objMSComm" event="OnComm">  

// MSComm1????? OnComm ????? MSComm1_OnComm()??
objMSComm_OnComm();
  
    </script>

    <script language="javascript" type="text/javascript">
 


var msgSuccess='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var msgSystemError =  '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var msgPrintSettingPara='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';  
var msgInput='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "msgInputSN") %>';  

var msgNoCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgNoCustSN").ToString() %>';
var msgInputNull = '<%=this.GetLocalResourceObject(Pre + "_msgInputNull").ToString() %>';
var msgInvalidSN =  '<%=this.GetLocalResourceObject(Pre + "_msgInvalidSN").ToString() %>';
var msgCustSNErr ='<%=this.GetLocalResourceObject(Pre + "_msgCustSNErr").ToString() %>';
var msgUnitWeightNull ='<%=this.GetLocalResourceObject(Pre + "_msgUnitWeightNull").ToString() %>';

var inputControl =getCommonInputObject();
var editor = "<%=UserId%>";
var customer = '<%=Customer %>';
var station = '<%=Request["Station"] %>';
var pcode ='<%=Request["PCode"] %>';  
var accountId='<%=Request["AccountId"] %>';
var login ='<%=Request["Login"] %>';
var WeightFilePath ="<%=UnitWeightFilePath%>";

var model ="";
var productID = "";
var custSN = "";
var actWeight;
var hostname = getClientHostName();
var NotShowMessage=true;
var sourcePlayTimes=2

document.body.onload = function() {
    try {

        setCommPara("U",WeightFilePath);
        callNextInput();
    }
    catch (e) {
    if(NotShowMessage===true){
    ShowErrorInfo(e.description,NotShowMessage,sourcePlayTimes);
    }
    else{
        alert(e.description);
    }
    }
};



function onFailed(result) {
     endWaitingCoverDiv();
     ResetPage();
     if(NotShowMessage===true){
     ShowErrorInfo(result.get_message(),NotShowMessage,sourcePlayTimes);
     }
     else{
     ShowMessage(result.get_message());
     ShowInfo(result.get_message());
     }
 }
 
var weightBuffer="";
function objMSComm_OnComm()
{
    var objMSComm = document.getElementById("objMSComm");
   if(objMSComm.CommEvent != 2)
   { 
        //????????,???   
		 return;
	}
    else if(objMSComm.CommEvent==2)//???????  
    {
        weightBuffer= objMSComm.Input;
        //ShowInfo(weightBuffer);
        var result;
        var idx;
  	    if(weighttype=="")
	    {      
	        idx = weightBuffer.indexOf("\r\n");
            result = weightBuffer.substring(0, idx);
        }
		else
		{
		    idx = weightBuffer.indexOf(weighttype);
		    result = weightBuffer.substring(idx);
		    idx = result.indexOf("\r\n");
            result = result.substring(0, idx);
		}

       var weight = getNumber(result);
       if (weight === false) {
         document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText = "";
       } else {
         document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText = weight;
       } 

    }
    
}  

function getNumber(str) 
{
	var result = false;
	if (typeof (str) == 'string')
	{
		    result = str.replace(/[^\d\.]/g, '');
		    result = isNaN(result) ? false : parseFloat(result);

	}

	return result;
}


function processDataEntry(inputStr)
{
    ShowInfo("");
    clearInfo();
    var inputData = inputStr.trim();
    actWeight=document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText; 
    if (inputData == "") 
    {
     if(NotShowMessage===true){
     ShowErrorInfo(msgInputNull,NotShowMessage,sourcePlayTimes);
     }
     else{
        alert(msgInputNull);
     }
        clearActualWeight();
        callNextInput();
        return;
    }
    //??IMEI??
    if (actWeight==null || actWeight=="")
    {
        if (Boolean( <%=isTestWeight%> ))
        {
            alert("TEST!");
            actWeight = "100.00";
            document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText="100.00";
        }
    }
    if (actWeight=="")
    {  
     if(NotShowMessage===true){
     ShowErrorInfo(msgUnitWeightNull,NotShowMessage,sourcePlayTimes);
     }
     else{
        alert(msgUnitWeightNull);
     }
        callNextInput();
        return ;
    }
    if (inputData == "7777") {
        ResetPage();
        return;
    }
     
    inputCustSN(inputData);
}

 function inputCustSN(inputData)
 {
     //if(inputData.substr(0,2) == "CN" && inputData.length == 10) 
	 if ((inputData.length == 10) && CheckCustomerSN(inputData))
     {
         custSN = inputData;
         inputCustomerSN();
     }
     //else if (inputData.substr(0,3) == "SCN" && inputData.length == 11) 
	 else if ((inputData.length == 11) && CheckCustomerSN(inputData))
     {
         custSN = inputData.substr(1,10);   //'SCN' ???11???,?10?????Customer S/N :(CN********)
         inputCustomerSN();
     }
     else
     {
       if(NotShowMessage===true){
       ShowErrorInfo(msgNoCustSN,NotShowMessage,sourcePlayTimes);
       }
       else{
        alert(msgNoCustSN);
        }
        clearActualWeight();
        callNextInput();
      }
 }

 function inputCustomerSN() 
 {
      beginWaitingCoverDiv();
      var line = "";
      WebServiceUnitWeight.inputCustSN(custSN, actWeight,line, editor, station, customer,onCustSucceed,onCustFailed);        
 }


 function onCustSucceed(result)
{
     endWaitingCoverDiv();
     
     if (result == null) {
        ShowInfo("");
        if(NotShowMessage===true){
         ShowErrorInfo(msgSystemError,NotShowMessage,sourcePlayTimes);
         }
         else{
        alert(msgSystemError);
        }
        clearActualWeight();
//      ShowInfo(msgSystemError);
     }
     else if (result[0] == "CHK020") {
        clearActualWeight();
        if(NotShowMessage===true){
         ShowErrorInfo(result[1],NotShowMessage,sourcePlayTimes);
         }
        else{
        ShowErrorMessage(result[1]);
        }
     }
     else if (result[0]==SUCCESSRET){
        document.getElementById("<%=lblCustSN_Display.ClientID %>").innerText = custSN;
        productID = result[1][0]; 
        document.getElementById("<%=lblProdId_Display.ClientID %>").innerText = result[1][0];     
        document.getElementById("<%=lblModel_Display.ClientID %>").innerText = result[1][1];
        model = result[1][1];
        
        //	Standard Weight – IMES_GetData..ModelWeight.UnitWeight(????decimal(10,2)) – ??????,??????NULL (????????????????????)
        if (result[1][2] == -1)
        {
            document.getElementById("<%=lblStdWeight_Display.ClientID %>").innerText = "NULL";
        }
        else
        {
            document.getElementById("<%=lblStdWeight_Display.ClientID %>").innerText = result[1][2];
        }       
        if (result[1][3] == true)
        {
            document.getElementById("<%=lblPAQC.ClientID %>").innerText = '<%=this.GetLocalResourceObject(Pre + "_lblPAQC").ToString() %>';
        }
        Save();        
     }
     else {
        ShowInfo("");
        var content = result[0];
        if(NotShowMessage===true){
         ShowErrorInfo(content,NotShowMessage,sourcePlayTimes);
         }
        else{
        alert(content);
        }
        clearActualWeight();
//        ShowInfo(content);
    }
    
     callNextInput(); 
}

 function onCustFailed(result) {
     endWaitingCoverDiv();
     ExitPage();
     clearActualWeight();
     clearInfo();
     if(NotShowMessage===true){
     ShowErrorInfo(result.get_message(),NotShowMessage,sourcePlayTimes);
     }
     else{
     ShowMessage(result.get_message());
     ShowInfo(result.get_message());
     }
     callNextInput();
 }
 
function Save()
{
    try{
        WebServiceUnitWeight.save(productID, onSaveSuceed, onSaveFailed);
     }
     catch(e) 
     {
         alert(e);
         clearActualWeight();
     }
}

function onSaveSuceed(result)
{
     endWaitingCoverDiv();
     
     if (result == null) {
        ShowInfo("");
     if(NotShowMessage===true){
     ShowErrorInfo(msgSystemError,NotShowMessage,sourcePlayTimes);
     }
     else{
        alert(msgSystemError);
        ShowInfo(msgSystemError);
     }
        ResetPage();
     }
     else if (result[0]==SUCCESSRET){
         if (document.getElementById("<%=lblPAQC.ClientID %>").innerText == "")
         {
            ShowSuccessfulInfoFormat(true, "ProductID", productID);
         }
         else
         {
            ShowSpecialSuccessfulInfoFormatDetail(true, "", "ProductID", productID, '<%=this.GetLocalResourceObject(Pre + "_msgPAQCDispaly").ToString() %>');
         }
         
        /*
         * Answer to: ITC-1414-0085
         * Description: Clear actual weight after saving data.
        */
         clearActualWeight();
     }
     else {
        ShowInfo("");
        var content = result[0];
        if(NotShowMessage===true){
        ShowErrorInfo(content,NotShowMessage,sourcePlayTimes);
        }
        else{
        alert(content);
        ShowInfo(content);
        }
        ResetPage();
    }
}

function onSaveFailed(result) {
     endWaitingCoverDiv();
     ResetPage();
     if(NotShowMessage===true){
        ShowErrorInfo(result.get_message(),NotShowMessage,sourcePlayTimes);
        }
     else{
     ShowMessage(result.get_message());
     ShowInfo(result.get_message());
     }
     
 }
 
// exit page
 function ExitPage()
 {
    if(custSN!="")
    {
        WebServiceUnitWeight.Cancel(custSN);
        sleep(waitTimeForClear);   
    }  
 } 
 
 //reset page
 function ResetPage()
 {
     ExitPage();
     clearActualWeight();
     clearInfo();
     callNextInput();
 }
 
 window.onbeforeunload= function() 
{
   ExitPage();
     
};

function ShowErrorMessage(msg)
{
    ShowMessage(msg);
    ShowInfo(msg);    
}

function clearActualWeight()
{
    actWeight ="";
    document.getElementById("<%=lblUnitWeightValue.ClientID%>").innerText = "";
}

function clearInfo()
{
    ShowInfo("");
    model="";
    productID = "";
    custSN = "";
     
    document.getElementById("<%=lblCustSN_Display.ClientID %>").innerText ="";
    document.getElementById("<%=lblProdId_Display.ClientID %>").innerText ="";
    document.getElementById("<%=lblModel_Display.ClientID %>").innerText = "";
    document.getElementById("<%=lblStdWeight_Display.ClientID %>").innerText ="";
    document.getElementById("<%=lblPAQC.ClientID %>").innerText = "";
}

function callNextInput() 
{
    getCommonInputObject().value = "";
    getCommonInputObject().focus();
    getAvailableData("processDataEntry");
}
   
    </script>

</asp:Content>
