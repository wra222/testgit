<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CombineOfflinePizzaForRCTO
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
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CombineOfflinePizzaForRCTO.aspx.cs" Inherits="PAK_CombineOfflinePizzaForRCTO" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
			<asp:ServiceReference Path="Service/WebServiceCombineOfflinePizzaForRCTO.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 80px;" />
                    <col />
                    <col style="width: 130px;" />
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
                        <asp:Label runat="server" ID="lblCartonSN" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCartonSNContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
					<td>
                        <asp:Label runat="server" ID="lblCartonCount" Text="" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCartonCountContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
            </table>
            <hr />
            
            <fieldset style="width: 100%">
                <legend align="left" style="height: 20px">
                    <asp:Label ID="lblCollectionData" runat="server" CssClass="iMes_label_13pt"></asp:Label>
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
                        <td style="width: 110px;">
                            <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                        </td>
                        <td>
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
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel>
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
var msgInputPizzaId = '<%=this.GetLocalResourceObject(Pre + "_msgInputPizzaId").ToString() %>';

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

var cartonSN = "";
var lstPrintItem;

window.onload = function() {
	try {
		tbl = "<%=gd.ClientID %>";
		inputObj = getCommonInputObject();
		getAvailableData("input");
		editor = "<%=UserId%>";
		customer = "<%=Customer%>";
		stationId = '<%=Request["Station"] %>';
		setPdLineCmbFocus();
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
    var url = "../PAK/RePrintCombineOfflinePizzaForRCTO.aspx?Station=" + stationId + "&PCode=" + document.getElementById("<%=pCode.ClientID%>").value + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountId + "&Login=" + login; 
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
	if (cartonSN == "") {
		if (!checkprint()) {
			ShowInfo(msgPrintSettingPara);
			getAvailableData("input");
			inputObj.focus();
			return;
        }
		cartonSN = data;
		beginWaitingCoverDiv();
		WebServiceCombineOfflinePizzaForRCTO.InputCartonId(cartonSN, lstPrintItem, line, editor, stationId, customer, OnSucc_InputCartonId, OnFail_InputCartonId);
		getAvailableData("input");
		inputObj.focus();
		return;
	}
	
	beginWaitingCoverDiv();
	WebServiceCombineOfflinePizzaForRCTO.InputPizzaId(cartonSN, data, line, editor, stationId, customer, OnSucc_InputPizzaId, OnFail_InputPizzaId);
	getAvailableData("input");
	inputObj.focus();
	return;
}

function OnSucc_InputCartonId(result) 
{
	if (result[0] == SUCCESSRET) 
	{
		endWaitingCoverDiv();
		setInfo(result);
		ShowInfo(msgInputPizzaId);
	}
	else 
	{
		endWaitingCoverDiv();
		var content = result[0];
		ShowMessage(content);
		ShowInfo(content);
	}
	
	getAvailableData("input");
	inputObj.focus();
}

function OnFail_InputCartonId(result) 
{
    endWaitingCoverDiv();
	ShowMessage(result.get_message());
	ShowInfo(result.get_message());
	getAvailableData("input");
	inputObj.focus();
	cartonSN = "";
}

function OnSucc_InputPizzaId(result) 
{
	if (result[0] == SUCCESSRET) 
	{
		endWaitingCoverDiv();
		if ("Finish" == result[1]) {
		    setPrintItemListParam1(lstPrintItem, cartonSN);
		    printLabels(lstPrintItem, false);

		    var successtmp = "[" + "CartonSN:" + cartonSN + "] " + msgSuccess;
		    initPage(PDLINE_KEEP);
		    ShowSuccessfulInfo(true, successtmp);
		}
		else {
		    var table = document.getElementById(tbl);
		    var idxPizza = result[2]+1;
		    table.rows[idxPizza].cells[4].innerText = document.getElementById("<%=lblCartonCountContent.ClientID %>").innerText;
		    ShowInfo(msgInputPizzaId);
		}
	}
	else 
	{
		endWaitingCoverDiv();
		var content = result[0];
		ShowMessage(content);
		ShowInfo(content);
	}
	
	getAvailableData("input");
	inputObj.focus();
}

function OnFail_InputPizzaId(result) 
{
    endWaitingCoverDiv();
	ShowMessage(result.get_message());
	ShowInfo(result.get_message());
	getAvailableData("input");
	inputObj.focus();
}

function setInfo(info) 
{
	//set value to the label
	setInputOrSpanValue(document.getElementById("<%=lblCartonSNContent.ClientID %>"), cartonSN);
	setInputOrSpanValue(document.getElementById("<%=lblCartonCountContent.ClientID %>"), info[1]);
	lstPrintItem = info[3];
	setTable(info[2], -1);
}

function setTable(info, updateIndex) {

	var bomList = info;

	for (var i = 0; i < bomList.length; i++) 
	{
		if (updateIndex == -1) 
		{
		}
		else 
		{
			if (updateIndex != i) continue;
		}

		var rw;
		var rowArray = new Array();
		rowArray.push(bomList[i][0]);
		rowArray.push(bomList[i][1]);
		rowArray.push(bomList[i][2]);
		rowArray.push(bomList[i][3]);
		rowArray.push(bomList[i][4]);
		rowArray.push(bomList[i][5]);
		

		//add data to table
		if (i < 12) 
		{
			eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, i+1);");
			if (updateIndex != -1) 
			{
				//setSrollByIndex(i, true, tbl);
			}
		}
		else {
			eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
			if (updateIndex != -1) 
			{
				setSrollByIndex(i, true, tbl);
			}
			rw.cells[1].style.whiteSpace = "nowrap";
		}
	}
}

function add_changeline_symbol(data) 
{
	var count = 0;
	var i = 0;
	var index = 0;
	var retStr = "";

	while (1) 
	{
		index = 0;
		count = 0;
		i = 0;
		for (i = 0; i < data.length; i++) 
		{
			if (data.substring(i, i + 1) == ',') 
			{
				count++;
				if (count == 3) index = i;
			}
		}

		if (count <= 2) { retStr += data; return retStr; }
		if (index > 1) 
		{
			retStr += data.substring(0, index) + " \r\n";
			data = data.substring(index + 1, data.length);
			continue;
		}
	}
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
	
	cartonSN = "";
	setInputOrSpanValue(document.getElementById("<%=lblCartonSNContent.ClientID %>"), "");
	setInputOrSpanValue(document.getElementById("<%=lblCartonCountContent.ClientID %>"), "");
	// the line following disable last set highlight item in the table.
	eval("setRowNonSelected_" + tbl + "()"); 
	ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
	ShowInfo("");
}

window.onbeforeunload = function() 
{
		OnCancel();
};

function OnCancel() 
{
	if (cartonSN != "") 
	{
		WebServiceCombineOfflinePizzaForRCTO.cancel(cartonSN);
		sleep(waitTimeForClear);
	}
}

function ExitPage() 
{
	OnCancel();
}

function ResetPage(pdline_select) 
{
	ExitPage();
	initPage(pdline_select);
	ShowInfo("");
}

</script>

</asp:Content>
