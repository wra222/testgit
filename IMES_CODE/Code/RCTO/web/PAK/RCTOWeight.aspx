﻿<%--
/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for RCTOWeight Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI RCTO Weight
 * UC:CI-MES12-SPEC-PAK-UC RCTO Weight
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-09-08  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/
--%>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RCTOWeight.aspx.cs" Inherits="PAK_RCTOWeight" Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="~/CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager runat="server" ID="SM" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServiceRCTOWeight.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="divRCTOWeight" style="z-index: 0;">
        <table width="70%" cellpadding="0" cellspacing="0" border="0" align="center">
            <tr style="height: 100px">
                <td colspan="2" align="right">
                    <asp:Label ID="lblModel" runat="server" class="iMes_label_30pt_Black"> 
                        <%=this.GetLocalResourceObject(Pre + "_lblModel").ToString()%>
                    </asp:Label>
                </td>
                <td width="10%"></td>
                <td width="50%">
                    <asp:Label ID="txtModel" runat="server" class="iMes_label_30pt_Black"> 
                    </asp:Label>
                </td>
            </tr>
            <tr style="height: 100px">
                <td colspan="2" align="right">
                    <asp:Label ID="lblQty" runat="server" class="iMes_label_30pt_Black"> 
                        <%=this.GetLocalResourceObject(Pre + "_lblQty").ToString()%>
                    </asp:Label>
                </td>
                <td></td>
                <td>
                    <asp:Label ID="txtQty" runat="server" class="iMes_label_30pt_Black"> 
                    </asp:Label>
                </td>
            </tr>
            <tr style="height: 100px">
                <td colspan="2" align="right">
                    <asp:Label ID="lblWeight" runat="server" class="iMes_label_30pt_Black">
                        <%=this.GetLocalResourceObject(Pre + "_lblWeight").ToString()%>
                    </asp:Label>
                </td>
                <td></td>
                <td>
                    <asp:Label ID="txtWeight" runat="server" class="iMes_label_30pt_Red_Underline" ForeColor="Red">
                    </asp:Label>
                </td>
            </tr>
            <tr style="height: 100px">
                <td width="20%" align="right">
                    <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel">
                        <%=this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString()%>
                    </asp:Label>
                </td>
                <td colspan="3" align="left">
                    <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="80%"
                        CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                        ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" for="objMSComm" event="OnComm">  

// MSComm1????? OnComm ????? MSComm1_OnComm()??
objMSComm_OnComm();
  
    </script>

    <script language="javascript" type="text/javascript">
 


var msgSystemError =  '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var msgWeightNull ='<%=this.GetLocalResourceObject(Pre + "_msgWeightNull").ToString() %>';
var msgBadModel ='<%=this.GetLocalResourceObject(Pre + "_msgBadModel").ToString() %>';
var msgBadQty ='<%=this.GetLocalResourceObject(Pre + "_msgBadQty").ToString() %>';
var msgBadInput ='<%=this.GetLocalResourceObject(Pre + "_msgBadInput").ToString() %>';
var msgOverwrite ='<%=this.GetLocalResourceObject(Pre + "_msgOverwrite").ToString() %>';

var editor = "<%=UserId%>";
var WeightFilePath ="<%=RCTOWeightFilePath%>";
var ByPassCheckModel = "<%=ByPassCheckModel%>";
var pattQty = /^[0-9]{1,6}$/;

document.body.onload = function() {
    try {
        setCommPara("R",WeightFilePath);
        callNextInput();
    }
    catch (e) {
        alert(e.description);
    }
};

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
         document.getElementById("<%=txtWeight.ClientID %>").innerText = "";
       } else {
         document.getElementById("<%=txtWeight.ClientID %>").innerText = weight;
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
    actWeight = document.getElementById("<%=txtWeight.ClientID %>").innerText; 
    if (actWeight == null || actWeight == "")
    {
        if (Boolean( <%=isTestWeight%> ))
        {
            alert("TEST!");
            actWeight = "100.00";
            document.getElementById("<%=txtWeight.ClientID %>").innerText="100.00";
        }
    }
    if (actWeight == "")
    {
        alert(msgWeightNull);
        callNextInput();
        return ;
    }
    
    var inputData = inputStr.trim();
    if (inputData.length == 12)     //Input Model
    {
        //'PF', '6Z', '60', '172', '175', '173', '146', '131', '129', '151', '156', '111', '139', 'PO'
        if (inputData.indexOf("PF") == 0
            || inputData.indexOf("6Z") == 0
            || inputData.indexOf("60") == 0
            || inputData.indexOf("172") == 0
            || inputData.indexOf("175") == 0
            || inputData.indexOf("173") == 0
            || inputData.indexOf("146") == 0
            || inputData.indexOf("131") == 0
            || inputData.indexOf("129") == 0
            || inputData.indexOf("151") == 0
            || inputData.indexOf("156") == 0
            || inputData.indexOf("111") == 0
            || inputData.indexOf("139") == 0
            || inputData.indexOf("PO") == 0)
        {
            document.getElementById("<%=txtModel.ClientID %>").innerText = inputStr;
            if (document.getElementById("<%=txtQty.ClientID %>").innerText.trim() != "")
            {
                Save();
                return;
            }
            callNextInput();
            return ;
        }
        else
        {
            alert(msgBadModel);
            callNextInput();
            return;
        }
    }
    else if (pattQty.test(inputData))
    {
        qty = parseInt(inputData);
        if (qty > 0)
        {
            document.getElementById("<%=txtQty.ClientID %>").innerText = qty;
            if (document.getElementById("<%=txtModel.ClientID %>").innerText.trim() != "")
            {
                Save();
                return;
            }
            callNextInput();
            return ;
        }
        else
        {
            alert(msgBadQty);
            callNextInput();
            return;
        }
    }
    else
    {
        alert(msgBadInput);
        callNextInput();
        return;
    }
}
 
function Save()
{
    try
    {
        if(ByPassCheckModel != "Y")
        {
            WebServiceRCTOWeight.getModelWeight(document.getElementById("<%=txtModel.ClientID %>").innerText.trim(), onGetWeightSucceed, onFailed);
        }
        else
        {
            bSave = true;
            model = document.getElementById("<%=txtModel.ClientID %>").innerText.trim();
            factor = 1;
            if (model.indexOf("6") == 0) factor = 1000;
            setWeight = factor * parseFloat(document.getElementById("<%=txtWeight.ClientID %>").innerText.trim()) / parseInt(document.getElementById("<%=txtQty.ClientID %>").innerText.trim());
//            if (result != -1)
//            {
//                bSave = confirm(msgOverwrite.replace("OLD_WEIGHT", result.toFixed(2)).replace("NEW_WEIGHT", setWeight));
//            }
             
            if (bSave)
            {
                WebServiceRCTOWeight.setModelWeight(model, setWeight, editor, onSetWeightSucceed, onFailed);
                return;
            }

            ResetPage();
            return;
        }
    
        
    }
    catch(e) 
    {
        alert(e);
        ResetPage();
    }
}

function onGetWeightSucceed(result)
{     
    if (result == null) 
    {
        ShowInfo("");
        alert(msgSystemError);
        ShowInfo(msgSystemError);
        ResetPage();
        return;
    }
     
    bSave = true;
    model = document.getElementById("<%=txtModel.ClientID %>").innerText.trim();
    factor = 1;
    if (model.indexOf("6") == 0) factor = 1000;
    setWeight = (factor * parseFloat(document.getElementById("<%=txtWeight.ClientID %>").innerText.trim()) / parseInt(document.getElementById("<%=txtQty.ClientID %>").innerText.trim())).toFixed(2);
    if (result != -1)
    {
        bSave = confirm(msgOverwrite.replace("OLD_WEIGHT", result.toFixed(2)).replace("NEW_WEIGHT", setWeight));
    }
     
    if (bSave)
    {
        WebServiceRCTOWeight.setModelWeight(model, setWeight, editor, onSetWeightSucceed, onFailed);
        return;
    }

    ResetPage();
    return;
}

function onSetWeightSucceed(result)
{     
    ShowSuccessfulInfoFormat(true, "Model", document.getElementById("<%=txtModel.ClientID %>").innerText.trim());
    ResetPage();
    return;
}

function onFailed(result) {
    ResetPage();
    ShowMessage(result.get_message());
    ShowInfo(result.get_message()); 
    return;    
}
 
//reset page
function ResetPage()
{
    document.getElementById("<%=txtWeight.ClientID %>").innerText = "";
    document.getElementById("<%=txtModel.ClientID %>").innerText ="";
    document.getElementById("<%=txtQty.ClientID %>").innerText ="";
    callNextInput();
}
 
function ShowErrorMessage(msg)
{
    ShowMessage(msg);
    ShowInfo(msg);    
}

function callNextInput() 
{
    getCommonInputObject().value = "";
    getCommonInputObject().focus();
    getAvailableData("processDataEntry");
}
   
    </script>

</asp:Content>
