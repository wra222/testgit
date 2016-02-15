<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CombinePalletWithoutCartonForFRU
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
    CodeFile="CombinePalletWithoutCartonForFRU.aspx.cs" Inherits="PAK_CombinePalletWithoutCartonForFRU" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
			<asp:ServiceReference Path="Service/WebServiceCombinePalletWithoutCartonForFRU.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 150px;" />
                    <col style="width: 200px;" />
                    <col style="width: 200px;" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblDeliveryNo" Text="Delivery No:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
				        <select id="drpDeliveryNo" style="width: 100%" onchange="GetPallets(this.options[this.options.selectedIndex].value)"></select>
                    </td>
                </tr>
				
				<tr>
                    <td>
                        <asp:Label runat="server" ID="lblModel" Text="Model:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="txtModel" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
					<td>
                        <asp:Label runat="server" ID="lblShipDate" Text="ShipDate:" CssClass="iMes_label_13pt"></asp:Label>
						<asp:Label runat="server" ID="txtShipDate" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
					<td>
                        <asp:Label runat="server" ID="lblDeliveryQty" Text="Delivery Qty:" CssClass="iMes_label_13pt"></asp:Label>
						<asp:Label runat="server" ID="txtDeliveryQty" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
				<tr>
                    <td>
                    </td>
                    <td>
                    </td>
					<td>
                        <asp:Label runat="server" ID="lblDnStatus" Text="DN Status:" CssClass="iMes_label_13pt"></asp:Label>
						<asp:Label runat="server" ID="txtDnStatus" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
					<td>
                        <asp:Label runat="server" ID="lblDnEditor" Text="DN Editor:" CssClass="iMes_label_13pt"></asp:Label>
						<asp:Label runat="server" ID="txtDnEditor" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
				
				<tr>
                    <td>
                        <asp:Label runat="server" ID="lblPalletNo" Text="Pallet No:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
				        <select id="drpPalletNo" style="width: 100%" onchange="GetCntCartonOfPallet(this.options[this.options.selectedIndex].value)"></select>
                    </td>
                </tr>
				
				<tr>
                    <td>
                        <asp:Label runat="server" ID="lblTotalCartonQty" Text="Total Carton Qty:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
					<td colspan="3">
						<asp:Label runat="server" ID="txtTotalCartonQty" Text="" CssClass="iMes_label_13pt"></asp:Label>
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
                                 InputRegularExpression="^[-0-9a-zA-Z#\+\s\*\/]*$" Width="99%" IsClear="true" IsPaste="true" />
                        </td>
                    </tr>
					<tr>
						<td></td>
						<td>
							<input id="btnSetting" type="button" runat="server" style="width:100px" onclick="showPrintSettingDialog()"
								onmouseover="this.className='iMes_button_onmouseover'" 
								onmouseout="this.className='iMes_button_onmouseout'" class="iMes_button"/>
							<input id="btnPrint" type="button"  runat="server"  class="iMes_button" onclick="DoSave()" value="Print" />
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
var msgErrQtyFormat = '<%=this.GetLocalResourceObject(Pre + "_msgErrQtyFormat").ToString() %>';
var msgErrModelNot173 = '<%=this.GetLocalResourceObject(Pre + "_msgErrModelNot173").ToString() %>';
var msgErrNotChooseDN = '<%=this.GetLocalResourceObject(Pre + "_msgErrNotChooseDN").ToString() %>';
var msgNoSelectPdLine = '<%=this.GetLocalResourceObject(Pre + "_msgNoSelectPdLine").ToString() %>';
var msgErrInput = '<%=this.GetLocalResourceObject(Pre + "_msgErrInput").ToString() %>';
var msgErrInputShipDate = '<%=this.GetLocalResourceObject(Pre + "_msgErrInputShipDate").ToString() %>';
var msgErrInputDeliveryNo = '<%=this.GetLocalResourceObject(Pre + "_msgErrInputDeliveryNo").ToString() %>';
var msgErrInputPalletNo = '<%=this.GetLocalResourceObject(Pre + "_msgErrInputPalletNo").ToString() %>';

var PDLINE_CLEAR = 1;
var PDLINE_KEEP = -1;

var line = '';
var editor;
var customer;
var stationId;
var curStation = "";
var accountId = '<%=Request["AccountId"] %>';
var login = '<%=Request["Login"] %>';
var inputObj;
var emptyPattern = /^\s*$/;

var lstPrintItem;

var selDeliveryNo = document.getElementById("drpDeliveryNo");
var selPalletNo = document.getElementById("drpPalletNo");

var reShipDate = new RegExp(/^([0-9\/]*)$/);

var dnsn ='';
var palletNo ='';

window.onload = function() {
	try {
		inputObj = getCommonInputObject();
		getAvailableData("input");
		editor = "<%=UserId%>";
		customer = "<%=Customer%>";
		stationId = '<%=Request["Station"] %>';
		
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

function setPrintItemListParam1(backPrintItemList, deliveryNo, palletNo)
{
	//var lstPrtItem = backPrintItemList;
	var keyCollection = new Array();
	var valueCollection = new Array();

	keyCollection[0] = "@DeliveryNo";
	valueCollection[0] = generateArray(deliveryNo);
	keyCollection[1] = "@PalletNo";
	valueCollection[1] = generateArray(palletNo);

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
    var url = "../PAK/RePrintCombinePalletWithoutCartonForFRU.aspx?Station=" + stationId + "&PCode=" + document.getElementById("<%=pCode.ClientID%>").value + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountId + "&Login=" + login; 
	var paramArray = new Array();
	paramArray[0] = ''; //getPdLineCmbValue();
	paramArray[1] = editor;
	paramArray[2] = customer;
	paramArray[3] = stationId;
	window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
}

function input(data) {
    ShowInfo("");
    /*
	line = getPdLineCmbValue();
	if (line == "") {
		ShowInfo(msgNoSelectPdLine);
		setPdLineCmbFocus();
		getAvailableData("input");
		inputObj.focus();
		return;
	}
	*/
	if (!checkprint()) {
		ShowInfo(msgPrintSettingPara);
		getAvailableData("input");
		inputObj.focus();
		return;
	}
	
	selDeliveryNo.options.length=0;
	selPalletNo.options.length=0;
	document.getElementById("<%=txtModel.ClientID %>").innerText = '';
	document.getElementById("<%=txtShipDate.ClientID %>").innerText = '';
	document.getElementById("<%=txtDeliveryQty.ClientID %>").innerText = '';
	document.getElementById("<%=txtTotalCartonQty.ClientID %>").innerText = '';
	document.getElementById("<%=txtDnStatus.ClientID %>").innerText = '';
	document.getElementById("<%=txtDnEditor.ClientID %>").innerText = '';
	
	if (16 == data.length){ // dn
		beginWaitingCoverDiv();
		WebServiceCombinePalletWithoutCartonForFRU.GetDnList(data, "", "", line, editor, stationId, customer, OnSucc_GetDnList, OnFail_GetDnList);
	}
	else if (10 == data.length){ // ShipDate
		if (!reShipDate.test(data)){
			ShowInfo(msgErrInputShipDate);
			getAvailableData("input");
			inputObj.focus();
			return;
		}
		beginWaitingCoverDiv();
		WebServiceCombinePalletWithoutCartonForFRU.GetDnList("", data, "", line, editor, stationId, customer, OnSucc_GetDnList, OnFail_GetDnList);
	}
	else if (12 == data.length){ // Model
		beginWaitingCoverDiv();
		WebServiceCombinePalletWithoutCartonForFRU.GetDnList("", "", data, line, editor, stationId, customer, OnSucc_GetDnList, OnFail_GetDnList);
	}
	else{
		ShowInfo(msgErrInput);
		getAvailableData("input");
		inputObj.focus();
		return;
	}
	
	getAvailableData("input");
	inputObj.focus();
}

function OnSucc_GetDnList(result) 
{
	endWaitingCoverDiv();
	if (result == null)
	{
		//ShowMessage(msgSystemError);
        ShowInfo(msgSystemError);
	}
	else if (result.Success == SUCCESSRET) 
	{
		selDeliveryNo.options.add(new Option("", ""));
		for (var i=0; i<result.DeliveryNoList.length; i++){
			var title = result.DeliveryNoList[i] + " , Model=" + result.DeliveryModelList[i] + " , ShipDate=" + result.DeliveryShipDateList[i] + " , Qty=" + result.DeliveryQtyList[i];
			selDeliveryNo.options.add(new Option(title, result.DeliveryNoList[i]));
		}
		ShowInfo(msgErrInputDeliveryNo);
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

function OnFail_GetDnList(result) 
{
	endWaitingCoverDiv();
	//ShowMessage(result.get_message());
	ShowInfo(result.get_message());
	getAvailableData("input");
	inputObj.focus();
	
}

function DoSave(){
	if (selDeliveryNo.selectedIndex < 0 || ''==selDeliveryNo.options[selDeliveryNo.selectedIndex].value){
		ShowInfo(msgErrInputDeliveryNo);
		getAvailableData("input");
		inputObj.focus();
		return;
	}
	if (selPalletNo.selectedIndex < 0 || ''==selPalletNo.options[selPalletNo.selectedIndex].value){
		ShowInfo(msgErrInputPalletNo);
		getAvailableData("input");
		inputObj.focus();
		return;
	}
	lstPrintItem = getPrintItemCollection();
	dnsn = selDeliveryNo.options[selDeliveryNo.selectedIndex].value;
	palletNo = selPalletNo.options[selPalletNo.selectedIndex].value;
	WebServiceCombinePalletWithoutCartonForFRU.Save(dnsn, palletNo, lstPrintItem, line, editor, stationId, customer, OnSucc_Save, OnFail_Save);
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
		lstPrintItem = result.PrintItem;
		
		setPrintItemListParam1(lstPrintItem, dnsn, palletNo);
		printLabels(lstPrintItem, false);
		
		var successtmp = "[" + "DN:" + dnsn + ", PalletNo:" + palletNo + "] " + msgSuccess;
		initPage(PDLINE_KEEP);
		ShowSuccessfulInfo(true, successtmp);
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
	selDeliveryNo.options.length=0;
	selPalletNo.options.length=0;
	document.getElementById("<%=txtModel.ClientID %>").innerText = "";
	document.getElementById("<%=txtShipDate.ClientID %>").innerText = "";
	document.getElementById("<%=txtDeliveryQty.ClientID %>").innerText = "";
	document.getElementById("<%=txtTotalCartonQty.ClientID %>").innerText = "";
	document.getElementById("<%=txtDnStatus.ClientID %>").innerText = '';
	document.getElementById("<%=txtDnEditor.ClientID %>").innerText = '';
	dnsn = '';
	palletNo = '';
}

window.onbeforeunload = function() 
{
		
};

function ResetPage(pdline_select) 
{
	initPage(pdline_select);
	ShowInfo("");
}

function GetPallets(dn){
    selPalletNo.options.length=0;
	document.getElementById("<%=txtModel.ClientID %>").innerText = '';
	document.getElementById("<%=txtShipDate.ClientID %>").innerText = '';
	document.getElementById("<%=txtDeliveryQty.ClientID %>").innerText = '';
	document.getElementById("<%=txtTotalCartonQty.ClientID %>").innerText = '';
	document.getElementById("<%=txtDnStatus.ClientID %>").innerText = '';
	document.getElementById("<%=txtDnEditor.ClientID %>").innerText = '';
	if (''==dn){
		
	}
	else{
		beginWaitingCoverDiv();
		WebServiceCombinePalletWithoutCartonForFRU.GetPalletList(dn, line, editor, stationId, customer, OnSucc_GetPalletList, OnFail_GetPalletList);
	}
	
	getAvailableData("input");
	inputObj.focus();
}

function OnSucc_GetPalletList(result) 
{
	endWaitingCoverDiv();
	if (result == null)
	{
		//ShowMessage(msgSystemError);
        ShowInfo(msgSystemError);
	}
	else if (result.Success == SUCCESSRET) 
	{
		document.getElementById("<%=txtModel.ClientID %>").innerText = result.Model;
		document.getElementById("<%=txtShipDate.ClientID %>").innerText = result.ShipDate;
		document.getElementById("<%=txtDeliveryQty.ClientID %>").innerText = result.DnQty;
		document.getElementById("<%=txtDnStatus.ClientID %>").innerText = result.DnStatus;
		document.getElementById("<%=txtDnEditor.ClientID %>").innerText = result.DnEditor;
		
		selPalletNo.options.add(new Option("", ""));
		for (var i=0; i<result.PalletNoList.length; i++){
			selPalletNo.options.add(new Option(result.PalletNoList[i], result.PalletNoList[i]));
		}
		ShowInfo(msgErrInputPalletNo);
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

function OnFail_GetPalletList(result) 
{
	endWaitingCoverDiv();
	//ShowMessage(result.get_message());
	ShowInfo(result.get_message());
	getAvailableData("input");
	inputObj.focus();
}

function GetCntCartonOfPallet(palletSn){
	document.getElementById("<%=txtTotalCartonQty.ClientID %>").innerText = '';
	if (palletSn != ''){
		beginWaitingCoverDiv();
		WebServiceCombinePalletWithoutCartonForFRU.GetCntCartonOfPallet(palletSn, line, editor, stationId, customer, OnSucc_GetCntCartonOfPallet, OnFail_GetCntCartonOfPallet);
	}
	getAvailableData("input");
	inputObj.focus();
}

function OnSucc_GetCntCartonOfPallet(result) 
{
	endWaitingCoverDiv();
	if (result == null)
	{
		//ShowMessage(msgSystemError);
        ShowInfo(msgSystemError);
	}
	else if (result.Success == SUCCESSRET) 
	{
		document.getElementById("<%=txtTotalCartonQty.ClientID %>").innerText = result.TotalCartonQty;
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

function OnFail_GetCntCartonOfPallet(result) 
{
	endWaitingCoverDiv();
	//ShowMessage(result.get_message());
	ShowInfo(result.get_message());
	getAvailableData("input");
	inputObj.focus();
}

</script>

</asp:Content>
