<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CombineCartonPalletFor146
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 
* Known issues:
* TODO：
* 
*/
 --%>
<%@ MasterType VirtualPath ="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"  AutoEventWireup="true" 
    CodeFile="CombineCartonPalletFor146.aspx.cs" Inherits="PAK_CombineCartonPalletFor146" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
			<asp:ServiceReference Path="Service/WebServiceCombineCartonPalletFor146.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 150px;" />
                    <col style="width: 250px;" />
                    <col style="width: 150px;" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblPDLine" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbPdLine ID="CmbPdLine1" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
				<tr>
                    <td>
                        <asp:Label runat="server" ID="lblDN" Text="Delivery No:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label runat="server" ID="txtDN" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
				<tr>
                    <td>
                        <asp:Label runat="server" ID="lblCartonSN" Text="CartonSN:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label runat="server" ID="txtCartonSN" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
				
            </table>
            <hr />
            
            <div id="div3">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                        </td>
                        <td width="70%">
                            <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                 InputRegularExpression="^[-0-9a-zA-Z#\+\s\*]*$" Width="99%" IsClear="true" IsPaste="true" />
                        </td>
                        <td>
							<input id="btnSetting" type="button" runat="server" style="width:100px" onclick="showPrintSettingDialog()"
								onmouseover="this.className='iMes_button_onmouseover'" 
								onmouseout="this.className='iMes_button_onmouseout'" class="iMes_button"/>
							<input id="btnReprint" type="button"  runat="server"  class="iMes_button" onclick="reprint()" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server"></asp:UpdatePanel>
        
    </div>

<asp:HiddenField ID="stationHF" runat="server" />
<input type="hidden" runat="server" id="pCode" />

<script language="javascript" type="text/javascript">
var msgOceanShipping = '<%=this.GetLocalResourceObject(Pre + "_msgOceanShipping").ToString()%>';
var msgInputCooLabelSuccess = '<%=this.GetLocalResourceObject(Pre + "_msgInputCooLabelSuccess").ToString()%>';
var msgCoolableCustSNDoesnotMatchPizzaLabel = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgCoolableCustSNDoesnotMatchPizzaLabel").ToString() %>';
var msgPizzaLabelCanNotBeFound = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPizzaLabelCanNotBeFound").ToString() %>';
var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var msgQualityCheck = '<%=this.GetLocalResourceObject(Pre + "_msgQualityCheck").ToString() %>';
var msgInputWarrantyCard = '<%=this.GetLocalResourceObject(Pre + "_msgInputWarrantyCard").ToString() %>';
var msgWarrantySNNotMatch = '<%=this.GetLocalResourceObject(Pre + "_msgWarrantySNNotMatch").ToString() %>';
var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
var msgInputMBCT2 = '<%=this.GetLocalResourceObject(Pre + "_msgInputMBCT2").ToString() %>';
var msgNoSelectPdLine = '<%=this.GetLocalResourceObject(Pre + "_msgNoSelectPdLine").ToString() %>';

var PDLINE_CLEAR = 1;
var PDLINE_KEEP = -1;

var editor;
var customer;
var stationId;
var curStation = "";
var accountId = '<%=Request["AccountId"] %>';
var login = '<%=Request["Login"] %>';
var inputObj;
var emptyPattern = /^\s*$/;

var cartonSN = "";
var lstPrintItem;

window.onload = function() {
	try {
		inputObj = getCommonInputObject();
		getAvailableData("input");
		editor = "<%=UserId%>";
		customer = "<%=Customer%>";
		stationId = '<%=Request["Station"] %>';
		setPdLineCmbFocus();
		cartonSN = "";
		
		getAvailableData("input");
		inputObj.focus();
	}
	catch (e) {
		alert(e.description);
	}
};

function checkprint() {
	lstPrintItem = getPrintItemCollection();
	if (lstPrintItem == null)                 //判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
	{
		return false;
	}
	return true;
}

function setPrintItemListParam1(backPrintItemList, palletNo, cartonSN, category)
{
	//var lstPrtItem = backPrintItemList;
	var keyCollection = new Array();
	var valueCollection = new Array();

	keyCollection[0] = "@CartonSN";
	valueCollection[0] = generateArray(cartonSN);
	keyCollection[1] = "@Category";
	valueCollection[1] = generateArray(category);

	for (var jj = 0; jj < backPrintItemList.length; jj++) {
		backPrintItemList[jj].ParameterKeys = keyCollection;
		backPrintItemList[jj].ParameterValues = valueCollection;
	}
	//setPrintParam(lstPrtItem, "RCTO_Label", keyCollection, valueCollection);
}

function showPrintSettingDialog() {
	showPrintSetting(document.getElementById("<%=stationHF.ClientID%>").value, document.getElementById("<%=pCode.ClientID%>").value);
}

function reprint() {
    var url = "../PAK/RePrintCombineCartonPalletFor146.aspx?Station=" + stationId + "&PCode=" + document.getElementById("<%=pCode.ClientID%>").value + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountId + "&Login=" + login; 
	var paramArray = new Array();
	paramArray[0] = getPdLineCmbValue();
	paramArray[1] = editor;
	paramArray[2] = customer;
	paramArray[3] = stationId;
	window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
}

function input(data) {
    ShowInfo("");
    var line = getPdLineCmbValue();
	if (line == "") {
		ShowInfo(msgNoSelectPdLine);
		setPdLineCmbFocus();
		getAvailableData("input");
		inputObj.focus();
		return;
	}
	if (!checkprint()) {
		ShowInfo(msgPrintSettingPara);
		getAvailableData("input");
		inputObj.focus();
		return;
	}
	
	lstPrintItem = getPrintItemCollection();
	cartonSN = data;
	beginWaitingCoverDiv();
	WebServiceCombineCartonPalletFor146.Save(data, lstPrintItem, line, editor, stationId, customer, OnSucc_Save, OnFail_Save);
	
	getAvailableData("input");
	inputObj.focus();
}

function OnSucc_Save(result) 
{
	endWaitingCoverDiv();
	if (result == null)
	{
		//ShowMessage(msgSystemError);
        ShowInfo(msgSystemError);
	}
	else if (result.Success == SUCCESSRET) 
	{
		var palletNo = result.PalletNo;
		var dn = result.DN;
		var category = result.Category;
		if (category == "MBCT")
			category = "MB";
		else if (category == "MaterialCT")
			category = "CT";
		else if (category == "NoMaterialCT")
			category = "NoCT";
		
		var successtmp = "";
		var fullCartonsInPallet = result.FullCartonsInPallet;
		if('Y'==fullCartonsInPallet){
			lstPrintItem = result.PrintItem;
			setPrintItemListParam1(lstPrintItem, palletNo, cartonSN, category);
			printLabels(lstPrintItem, false);
			successtmp = "[" + "Pallet No=" + palletNo + "] " + msgSuccess;
		}
		else
			successtmp = msgSuccess;
		
		//initPage(PDLINE_KEEP);
		ShowSuccessfulInfo(true, successtmp);
		
		document.getElementById("<%=txtCartonSN.ClientID %>").innerText = cartonSN;
		document.getElementById("<%=txtDN.ClientID %>").innerText = dn;
	}
	else 
	{
		var content = result;
		//ShowMessage(content);
		ShowInfo(content);
	}
	
	getAvailableData("input");
	inputObj.focus();
}

function OnFail_Save(result) 
{
    endWaitingCoverDiv();
	//ShowMessage(result.get_message());
	ShowInfo(result.get_message());
	
	initPage(PDLINE_KEEP);
	
	getAvailableData("input");
	inputObj.focus();
}

function initPage(pdline_select) 
{
	if (pdline_select == PDLINE_CLEAR) 
	{
		if (getPdLineCmbObj().disabled == false) 
		{
			getPdLineCmbObj().selectedIndex = 0;
		}
	}
	
	cartonSN = "";
	
	getPdLineCmbObj().disabled = false;
	document.getElementById("<%=txtDN.ClientID %>").innerText = "";
	document.getElementById("<%=txtCartonSN.ClientID %>").innerText = "";
}

function ResetPage(pdline_select) 
{
	initPage(pdline_select);
	ShowInfo("");
}

</script>

</asp:Content>
