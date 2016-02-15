<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CombineCartonDNfor146MB
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
    CodeFile="CombineCartonDNfor146MB.aspx.cs" Inherits="PAK_CombineCartonDNfor146MB" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
			<asp:ServiceReference Path="Service/WebServiceCombineCartonDNfor146MB.asmx" />
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
                        <asp:Label runat="server" ID="lblDeliveryNo" Text="Delivery No:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
				      
				        <select id="drpDeliveryNo" style="width: 100%" disabled onchange="GetDN(this.options[this.options.selectedIndex].value)"></select>
                    </td>
                </tr>
				
				<tr>
                    <td>
                        <asp:Label runat="server" ID="lblPCSinCarton" Text="PCS In Carton:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="txtPCSinCarton" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
					<td>
                        <asp:Label runat="server" ID="lblDeliveryQty" Text="Delivery Qty:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
					<td>
                        <asp:Label runat="server" ID="txtDeliveryQty" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
				<tr>
                    <td>
                        <asp:Label runat="server" ID="lblCartonRemainQty" Text="Carton Remain Qty:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td> 
                        <asp:Label runat="server" ID="txtCartonRemainQty" Text="" CssClass="iMes_label_13pt"></asp:Label>
                         
                    </td>
					<td>
                        <asp:Label runat="server" ID="lblDeliveryRemainQty" Text="Delivery Remain Qty:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
					<td>
                        <asp:Label runat="server" ID="txtDeliveryRemainQty" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
            </table>
            <hr />
            
            <fieldset style="width: 100%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblCollectionData" runat="server" CssClass="iMes_label_13pt" Text="Scanned PCBNo List"></asp:Label>
                </legend>   
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td>
                            <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                                GvExtHeight="228px" Style="top: 0px; left: 0px" Width="98%" Height="220px" SetTemplateValueEnable="False"
                                HighLightRowPosition="3"  AutoHighlightScrollByValue="True">
                            </iMES:GridViewExt>
                        </td>
                    </tr>
                </table>
            </fieldset>   
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
var msgErrQtyFormat = '<%=this.GetLocalResourceObject(Pre + "_msgErrQtyFormat").ToString() %>';
var msgErrModelNot173 = '<%=this.GetLocalResourceObject(Pre + "_msgErrModelNot173").ToString() %>';
var msgErrNotChooseDN = '<%=this.GetLocalResourceObject(Pre + "_msgErrNotChooseDN").ToString() %>';
var msgErrQtyScannedNeqDnRemain = '<%=this.GetLocalResourceObject(Pre + "_msgErrQtyScannedNeqDnRemain").ToString() %>';
var msgNoSelectPdLine = '<%=this.GetLocalResourceObject(Pre + "_msgNoSelectPdLine").ToString() %>';
var msgWin8SpsInput = '<%=this.GetLocalResourceObject(Pre + "_msgWin8SpsInput").ToString() %>';
var msgWin8SpsErr = '<%=this.GetLocalResourceObject(Pre + "_msgWin8SpsErr").ToString() %>';
var msgSelectDn = '<%=this.GetLocalResourceObject(Pre + "_msgSelectDn").ToString() %>';
var msgInputMBSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputMBSN").ToString() %>';
var msgErrNeqMbCode = '<%=this.GetLocalResourceObject(Pre + "_msgErrNeqMbCode").ToString() %>';
var msgErrNotInMbCodes = '<%=this.GetLocalResourceObject(Pre + "_msgErrNotInMbCodes").ToString() %>';

var tbl;
var DEFAULT_ROW_NUM = 13;

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

var totalRowCount = 0;

var cartonSN = "";
var mbsn = "";
var lstPrintItem;

var IDX_MBSN = 1;
var tblObj = document.getElementById("<%=gd.ClientID %>");
var selDeliveryNo = document.getElementById("drpDeliveryNo");
var freezeChooseDn = false;
var  remaincarton="";
window.onload = function() {
	try {
		tbl = "<%=gd.ClientID %>";
		inputObj = getCommonInputObject();
		getAvailableData("input");
		editor = "<%=UserId%>";
		customer = "<%=Customer%>";
		stationId = '<%=Request["Station"] %>';
		setPdLineCmbFocus();
		cartonSN = "";
		mbsn = "";
		
		selDeliveryNo.disabled = false;
		
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

function setPrintItemListParam1(backPrintItemList, cartonSN)
{
	//var lstPrtItem = backPrintItemList;
	var keyCollection = new Array();
	var valueCollection = new Array();

	keyCollection[0] = "@CartonSN";
	valueCollection[0] = generateArray(cartonSN);

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
    var url = "../PAK/RePrintCombineCartonDNfor146MB.aspx?Station=" + stationId + "&PCode=" + document.getElementById("<%=pCode.ClientID%>").value + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountId + "&Login=" + login; 
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
	
	if (step2_needChooseDn){
		if(selDeliveryNo.selectedIndex < 0 || '' == selDeliveryNo.options[selDeliveryNo.selectedIndex].value){
			ShowInfo(msgSelectDn);
			getAvailableData("input");
			inputObj.focus();
			return;
		}
		step2_needChooseDn = false;
	}
	
	if (needChkWin8sps){
		if (data != win8sps){
			ShowInfo(msgWin8SpsErr);
		}
		else{
			needChkWin8sps = false;
			UpdateUI_InputMBSN();
//			if (CheckToSave()){
//				DoSave();
//			}
		}
		getAvailableData("input");
		inputObj.focus();
		return;
	}
	else if (mbsn == ""){
		freezeChooseDn = false;
		beginWaitingCoverDiv();
		WebServiceCombineCartonDNfor146MB.InputMBSN(data, true, '<%=ShipMode%>', line, editor, stationId, customer, OnSucc_InputMBSN, OnFail_InputMBSN);
	}
	else if (data == "9999"){
		if ('' == document.getElementById("<%=txtDeliveryRemainQty.ClientID %>").innerText){
			ShowInfo('Err Qty');
			getAvailableData("input");
			inputObj.focus();
			return;
		}
		if (''==document.getElementById("<%=txtDeliveryRemainQty.ClientID %>").innerText || GetGridRowCount() != parseInt( document.getElementById("<%=txtDeliveryRemainQty.ClientID %>").innerText )){
			ShowInfo(msgErrQtyScannedNeqDnRemain);
			getAvailableData("input");
			inputObj.focus();
			return;
		}
		DoSave();;	}
	else {
		for (var i = 1; i < tblObj.rows.length; i++) {
			if (data == tblObj.rows[i].cells[IDX_MBSN].innerText.trim()) {
				ShowInfo('MBSN existed');
				getAvailableData("input");
				inputObj.focus();
				return;
			}
		}
		
		if ('<%=ShipMode%>' == 'RCTO'){
			if (mbsn.substr(0,2) != data.substr(0,2)){
				ShowInfo(msgErrNeqMbCode);
				getAvailableData("input");
				inputObj.focus();
				return;
			}
		}
		else { // FRU
			var existInMbsn = false;
			var tmpMbCode = data.substr(0,2);
			for (var i = 0; i < bk_MBSNs.length; i++) {
				if (bk_MBSNs[i] == tmpMbCode){
					existInMbsn = true;
					break;
				}
			}
			if (! existInMbsn){
				ShowInfo(msgErrNotInMbCodes);
				getAvailableData("input");
				inputObj.focus();
				return;
			}
		}
		
		if (!freezeChooseDn){
			///selDeliveryNo = document.getElementById("drpDeliveryNo");
			if (selDeliveryNo.selectedIndex <0 || selDeliveryNo.options[selDeliveryNo.selectedIndex].value ==""){
			    ShowInfo(msgErrNotChooseDN);
			    getAvailableData("input");
	            inputObj.focus();
	            return;
			}
			
			freezeChooseDn = true;
			
		}
		beginWaitingCoverDiv();
		WebServiceCombineCartonDNfor146MB.InputMBSN(data, false, '<%=ShipMode%>', line, editor, stationId, customer, OnSucc_InputMBSN, OnFail_InputMBSN);
	}
	
	getAvailableData("input");
	inputObj.focus();
}

function GetGridRowCount() {
	var n=0;
	for (var i = 1; i < tblObj.rows.length; i++) {
		var mb = tblObj.rows[i].cells[IDX_MBSN].innerText.trim();
		if (mb != "")
			n++;
	}
	return n;
}

var bk_MBSN;
var bk_DeliveryNoList;
var bk_ShipDatesOfDn;
var bk_ModelsOfDn;
var bk_QtysOfDn;
var bk_MBSNs;
var step2_needChooseDn = false; // 是第一片MB,needChooseDn

var needChkWin8sps = false;
var win8sps='';

function UpdateUI_InputMBSN(){
    CheckRMAOA3();
	mbsn = bk_MBSN;
	
//	var colArray = new Array();
//	   colArray.push(1+totalRowCount);
//	   colArray.push(mbsn);
//	   addRow(colArray);
//	
//	   var v = parseInt(document.getElementById("<%=txtCartonRemainQty.ClientID %>").innerText) - 1;
//	   remaincarton=v;
//	   document.getElementById("<%=txtCartonRemainQty.ClientID %>").innerText = v;
}
function CheckRMAOA3()
{
  mbsn = bk_MBSN;
	//check MB  OA3
	var arr = selDeliveryNo.options[selDeliveryNo.selectedIndex].value.split(",");
	var model146 = arr[1];
	WebServiceCombineCartonDNfor146MB.CheckRMAOA3(mbsn,model146, OnCheckoa3Succ_Save, Oncheckoa3Fail_Save);
}
function OnCheckoa3Succ_Save(result)
{
   if (result == null)
	{
		//ShowMessage(msgSystemError);
        ShowInfo(msgSystemError);
	}
	else if (result== SUCCESSRET) 
	{
		var colArray = new Array();
	   colArray.push(1+totalRowCount);
	   colArray.push(mbsn);
	   addRow(colArray);
	
	   var v = parseInt(document.getElementById("<%=txtCartonRemainQty.ClientID %>").innerText) - 1;
	   remaincarton=v;
	   document.getElementById("<%=txtCartonRemainQty.ClientID %>").innerText = v;
	   if (CheckToSave()){
					DoSave();
		}
	    else{
			ShowInfo(msgInputMBSN);
		}
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
function Oncheckoa3Fail_Save(result)
{
    ShowInfo(result.get_message());
	getAvailableData("input");
	inputObj.focus();

	}

function OnSucc_InputMBSN(result) 
{
	endWaitingCoverDiv();
	if (result == null)
	{
		//ShowMessage(msgSystemError);
        ShowInfo(msgSystemError);
	}
	else if (result.Success == SUCCESSRET) 
	{
		getPdLineCmbObj().disabled = true;
		
		bk_MBSN = result.MBSN;
		
		step2_needChooseDn = true;
		if (GetGridRowCount() > 0)
			step2_needChooseDn = false;
		
		needChkWin8sps = false;
		if (step2_needChooseDn){ //第一個 MB
			bk_DeliveryNoList = result.DeliveryNoList;
			bk_ShipDatesOfDn = result.ShipDatesOfDn;
			bk_ModelsOfDn = result.ModelsOfDn;
			bk_QtysOfDn = result.QtysOfDn;
			bk_MBSNs = result.MBSNs;
			bk_Deliveryshipway=result.DeliveryShipway;
			
			var sel = selDeliveryNo;
			var way = "";
			sel.options.length=0;
			sel.options.add(new Option("", ""));
			for(var i=0; i<bk_DeliveryNoList.length; i++){
			    if (bk_Deliveryshipway[i] == "T001")//T001:顯示空運, T002:顯示海運
                { way = "空運"; }
                else if (bk_Deliveryshipway[i] == "T002")
                { way = "海運"; }
                else
                {way=bk_Deliveryshipway[i];}
				var t = bk_ModelsOfDn[i] +' - '+ bk_DeliveryNoList[i] +' - '+ bk_ShipDatesOfDn[i]  +' - '+ bk_QtysOfDn[i]+' - '+way;
				sel.options.add(new Option(t, bk_DeliveryNoList[i] + "," + bk_ModelsOfDn[i]));
			}
			
			ShowInfo(msgSelectDn);
		}
		else{
			if ('<%=ShipMode%>'=='RCTO'){
				UpdateUI_InputMBSN();
//				if (CheckToSave()){
//					DoSave();
//				}
//				else{
//					ShowInfo(msgInputMBSN);
//				}
			}
			else if ('<%=ShipMode%>'=='FRU'){
				if ("" == win8sps){
					UpdateUI_InputMBSN();
//					if (CheckToSave()){
//						DoSave();
//					}
//					else{
//						ShowInfo(msgInputMBSN);
//					}
				}
				else{
					needChkWin8sps = true;
					ShowInfo(msgWin8SpsInput);
				}
			}
		}
		
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

function OnFail_InputMBSN(result) 
{
	endWaitingCoverDiv();
	//ShowMessage(result.get_message());
	ShowInfo(result.get_message());
	getAvailableData("input");
	inputObj.focus();
	
	if(GetGridRowCount() > 0){
		getPdLineCmbObj().disabled = true;
		selDeliveryNo.disabled = true;
	}
}

function CheckToSave(){
	if ('' != document.getElementById("<%=txtCartonRemainQty.ClientID %>").innerText){
		var nowRemain = parseInt(document.getElementById("<%=txtCartonRemainQty.ClientID %>").innerText);
		if (nowRemain <= 0){
			return true;
		}
	}
	return false;
}


function DoSave(){
	var lstProId = new Array();
	for (var i = 1; i < tblObj.rows.length; i++) {
		var mb = tblObj.rows[i].cells[IDX_MBSN].innerText.trim();
		if (mb != "")
			lstProId.push(mb);
	}
	lstPrintItem = getPrintItemCollection();
	var arr = selDeliveryNo.options[selDeliveryNo.selectedIndex].value.split(",");
	var dnsn = arr[0];
	var model146 = arr[1];
	var line = getPdLineCmbValue();
	beginWaitingCoverDiv();
	WebServiceCombineCartonDNfor146MB.Save(lstProId, dnsn, model146, lstPrintItem, '<%=ShipMode%>', line, editor, stationId, customer, OnSucc_Save, OnFail_Save);
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
		cartonSN = result.CartonSn;
		var palletNo = result.PalletNo;
		
		setPrintItemListParam1(lstPrintItem, cartonSN);
		printLabels(lstPrintItem, false);

		var successtmp = "[" + "CartonSN:" + cartonSN + "] " + msgSuccess;
		if ('' != palletNo)
			successtmp += " ; PalletNo=" + palletNo;
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

function addRow(info) {
	tbl = "<%=gd.ClientID %>";
	var rw;
	var rowArray = new Array();
	for (var i = 0; i < info.length; i++) 
	{
		rowArray.push(info[i]);
	}
	if (totalRowCount < <%=DEFAULT_ROWS%>) 
	{
		eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, totalRowCount+1);");
	}
	else {
		eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
		//rw.cells[1].style.whiteSpace = "nowrap";
	}
	totalRowCount++;
}

function initPage(pdline_select) 
{
	tbl = "<%=gd.ClientID %>";
	if (pdline_select == PDLINE_CLEAR) 
	{
		if (getPdLineCmbObj().disabled == false) 
		{
			getPdLineCmbObj().selectedIndex = 0;
		}
	}
	
	totalRowCount = 0;
	cartonSN = "";
	mbsn = "";
	freezeChooseDn = false;
	
	// the line following disable last set highlight item in the table.
	eval("setRowNonSelected_" + tbl + "()"); 
	ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
	//ShowInfo("");
	
	getPdLineCmbObj().disabled = false;
	selDeliveryNo.disabled = false;
	selDeliveryNo.options.length=0;
	document.getElementById("<%=txtPCSinCarton.ClientID %>").innerText = "";
	document.getElementById("<%=txtCartonRemainQty.ClientID %>").innerText = "";
	document.getElementById("<%=txtDeliveryQty.ClientID %>").innerText = "";
	document.getElementById("<%=txtDeliveryRemainQty.ClientID %>").innerText = "";
	
	win8sps = '';
	needChkWin8sps = false;
	step2_needChooseDn = false;
}

window.onbeforeunload = function() 
{
		
};

function ResetPage(pdline_select) 
{
	initPage(pdline_select);
	ShowInfo("");
}

function GetDN(dnVal){
	if(''==dnVal){
		document.getElementById("<%=txtPCSinCarton.ClientID %>").innerText = '';
		document.getElementById("<%=txtCartonRemainQty.ClientID %>").innerText = '';
		document.getElementById("<%=txtDeliveryQty.ClientID %>").innerText = '';
		document.getElementById("<%=txtDeliveryRemainQty.ClientID %>").innerText = '';
		win8sps = '';
	}
	else{
		var arr = dnVal.split(",");
		var dn = arr[0];
		var model = arr[1];
		var line = getPdLineCmbValue();
		beginWaitingCoverDiv();
		WebServiceCombineCartonDNfor146MB.GetDnQty(dn, model, '<%=ShipMode%>', line, editor, stationId, customer, OnSucc_GetDnQty, OnFail_GetDnQty);
	}
	
	getAvailableData("input");
	inputObj.focus();
}

function OnSucc_GetDnQty(result)
{
	endWaitingCoverDiv();
	if (result == null)
	{
		//ShowMessage(msgSystemError);
        ShowInfo(msgSystemError);
	}
	else if (result.Success == SUCCESSRET) 
	{
		document.getElementById("<%=txtPCSinCarton.ClientID %>").innerText = result.CnQty;
		document.getElementById("<%=txtCartonRemainQty.ClientID %>").innerText = result.CnQty;
		document.getElementById("<%=txtDeliveryQty.ClientID %>").innerText = result.DnQty;
		document.getElementById("<%=txtDeliveryRemainQty.ClientID %>").innerText = result.DnRemainQty;
		win8sps = result.Win8sps;
		
		step2_needChooseDn = false;
		getPdLineCmbObj().disabled = true;
		selDeliveryNo.disabled = true;
		
		var msg = msgInputMBSN;
		if('<%=ShipMode%>'=='RCTO'){
		  
			UpdateUI_InputMBSN();
			
			
			//if (CheckToSave()){
			//	DoSave();
				msg = '';
			//}
		}
		else if('<%=ShipMode%>'=='FRU'){
			if(''==win8sps){
				UpdateUI_InputMBSN();
				//if (CheckToSave()){
				//	DoSave();
					msg = '';
				//}
			}
			else{
				needChkWin8sps = true;
				msg = msgWin8SpsInput;
			}
		}
		
		if ('' != msg)
			ShowInfo(msg);
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

function OnFail_GetDnQty(result) 
{
	endWaitingCoverDiv();
	//ShowMessage(result.get_message());
	ShowInfo(result.get_message());
	getAvailableData("input");
	inputObj.focus();
	
	getPdLineCmbObj().disabled = true;
}

</script>

</asp:Content>
