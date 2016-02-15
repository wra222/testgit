<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:ConbimeOfflinePizza page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * Known issues:
 * TODO:
 */ --%>
 <%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="ConbimeOfflinePizza.aspx.cs" Inherits="PAK_ConbimeOfflinePizza" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
	<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServiceConbimeOfflinePizza.asmx" />
        </Services>
    </asp:ScriptManager>
    
<script language="javascript" type="text/javascript">
var editor = '<%=UserId%>';
var customer = '<%=Customer%>';
var station = '<%=Station%>'
var inputObj;
var pCode = '<%=PCode%>';
var custsn = '';
var pizzaId = '';
var lstPrintItem;
var emptyPattern = /^\s*$/;
var accountId = '<%=Request["AccountId"] %>';
var login = '<%=Request["Login"] %>';
var tbl;
var DEFAULT_ROW_NUM = 13;
var defectCount = 0;
var defectInTable = [];

var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrintSettingPara") %>';
var msgInputMaxLength1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength1").ToString() %>';
var msgInputMaxLength2 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength2").ToString() %>';
var msgCustSNNull = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgKitIDNull").ToString()%>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';

var mesNoSelPdLine = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoSelectPdLine").ToString() %>';
var mesScanPizzaId = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgScanPizzaId").ToString() %>';
var errGetProduct = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_errGetProduct").ToString() %>';
var msgQualityCheck = '<%=this.GetLocalResourceObject(Pre + "_msgQualityCheck").ToString() %>';
document.body.onload = function() {
	try {
		tbl = "<%=gd.ClientID %>";
		inputObj = getCommonInputObject();
		inputObj.focus();
		getAvailableData("input");
	} catch (e) {
		alert(e.description);
		inputObj.focus();
	}
}

function clkSetting() {
	showPrintSetting(station, pCode);
}

function reprint() {
    var url = "../PAK/RePrintCombineOfflinePizza.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountId + "&Login=" + login; 
	var paramArray = new Array();
	paramArray[0] = getPdLineCmbValue();
	paramArray[1] = editor;
	paramArray[2] = customer;
	paramArray[3] = station;
	window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
}

function clkReprint() {
	ShowInfo("");
	lstPrintItem = getPrintItemCollection();
	if (lstPrintItem == "" || lstPrintItem == null) {
		alert(msgPrintSettingPara);
		getAvailableData("input");
		return;
	}
	if (getPdLineCmbValue() == "") 
	{
		alert(mesNoSelPdLine);
		setPdLineCmbFocus();
		getAvailableData("input");
		return;
	}
	if (custsn == '')
	{
		alert(msgCustSNNull);
		getAvailableData("input");
		getCommonInputObject().focus();
		return;
	}
	
	try {
		beginWaitingCoverDiv();
		WebServiceConbimeOfflinePizza.Reprint(custsn, "", getPdLineCmbValue(), editor, station, customer, lstPrintItem, onSucceedPrint, onFail);
	}
	catch (e) {
		getAvailableData("input");
		endWaitingCoverDiv();
		getCommonInputObject().focus();
		alert(e);
	}
}

function input(data) {
    ShowInfo("");
    if (getPdLineCmbValue() == "") {
        alert(mesNoSelPdLine);
        setPdLineCmbFocus();
        getAvailableData("input");
        return;
    }
	if (document.getElementById("<%=lblCustomerSnContent.ClientID%>").innerText == '') {
	    lstPrintItem = getPrintItemCollection();
	    if (lstPrintItem == "" || lstPrintItem == null) {
	        alert(msgPrintSettingPara);
	        getAvailableData("input");
	        return;
	    }
		
		if (CheckCustomerSN(data)){
			custsn = SubStringSN(data, "CustSN");
			beginWaitingCoverDiv();
			WebServiceConbimeOfflinePizza.getProduct(custsn, getPdLineCmbValue(), editor, station, customer, lstPrintItem, inputSucc, onFail);
		}
		else{
			alert("Wrong Code!");
			getAvailableData("input");
			getCommonInputObject().focus();
			return;
		}
	}
	else
	{
		if (check_if_can_jian_liao_finished()){
			pizzaId = data;
			beginWaitingCoverDiv();
			WebServiceConbimeOfflinePizza.Save(custsn, pizzaId, getPdLineCmbValue(), editor, station, customer, lstPrintItem, onSucceedPrint, onFail);
		}
		else{
			WebServiceConbimeOfflinePizza.jianLiao(custsn, data, JianLiaoSucc, JianLiaoFail);
		}
	}
	getAvailableData("input");
	getCommonInputObject().focus();
}

function inputSucc(result)
{
	endWaitingCoverDiv();
	if (result[0]) {
		document.getElementById("<%=lblCustomerSnContent.ClientID%>").innerText = custsn;
		document.getElementById("<%=lblModelContent.ClientID%>").innerText = result[1];
		lstPrintItem = result[3];
		if (null!=result[2] && result[2].length>0){
			defectInTable = result[2];
			setTable(result[2], -1);
		}
		else{
			ShowInfo(mesScanPizzaId);
		}
	}
	else {
		ShowInfo(errGetProduct);
	}
	
	getAvailableData("input");
	inputObj.focus();
}

function onSucceedPrint(result) {
	endWaitingCoverDiv();
	if (!((result.length == 2) && (result[0] == SUCCESSRET))) {
		initPage();
		var content = result; //msgSystemError;
		ShowMessage(content);
		ShowInfo(content);
		return;
	}
	var successTemp = "[" + custsn + "]" + msgSuccess;
	
	// paqc
	var tmp = "";
	if (result[1] == "8") 
	{
		tmp = msgQualityCheck;
	}
	successTemp = successTemp + "\n" + tmp;
	
	ShowSuccessfulInfo(true, successTemp);

	var prnList = lstPrintItem;
	setPrintItemListParam1(prnList, lstPrintItem[0].LabelType, custsn);
	printLabels(prnList, false);
	initPage();
}

function onFail(result) {
	ShowMessage(result.get_message());
	ShowInfo(result.get_message());
	endWaitingCoverDiv();
	initPage();
}

function setPrintItemListParam1(lstPrtItem, labelType, custsn) {
	var keyCollection = new Array();
	var valueCollection = new Array();
	keyCollection[0] = "@custsn";
	valueCollection[0] = generateArray(custsn);
	setAllPrintParam(lstPrtItem, keyCollection, valueCollection);
	//setPrintParam(lstPrtItem, labelType, keyCollection, valueCollection);
}

function imposeMaxLength(obj) {
	var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
	return (obj.value.length < mlength);
}

function initPage() {
	tbl = "<%=gd.ClientID %>";
	clearData();
	custsn = "";
	document.getElementById("<%=lblCustomerSnContent.ClientID%>").innerText = "";
	document.getElementById("<%=lblModelContent.ClientID%>").innerText = "";
	getCommonInputObject().value = "";
	getAvailableData("input");
	getCommonInputObject().focus();
	// the line following disable last set highlight item in the table.
	eval("setRowNonSelected_" + tbl + "()"); 
	ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
}

function OnCancel()
{
	WebServiceConbimeOfflinePizza.cancel(custsn);
}

function ExitPage(){
	OnCancel();
}

function ResetPage(){
	ExitPage();
	initPage();
	ShowInfo("");
}

window.onbeforeunload = function() {
    //if (inputFlag) 
    //{
    OnCancel();
    //}
};
function check_if_can_jian_liao_finished()
{
	var ret = true;
	//defectInTable[0]["scannedQty"] = 1;
	for (var i = 0; i < defectInTable.length; i++)
		//for (var i = 0; i < 2; i++)
	
	{
		if (defectInTable[i]["qty"] != defectInTable[i]["scannedQty"])
		{
			ret = false; break;
		}
	}
		
	return ret;		   
}

var __current_highlight = -1;
function JianLiaoSucc(result) 
{
	var findIndex = updateTable(result);
	if (findIndex == -1) { ShowInfo("error!"); return; }

	if (__current_highlight >= 0) setRowSelectedOrNotSelectedByIndex(__current_highlight, false, "<%=gd.ClientID %>");
	setTable(defectInTable, findIndex);
	setRowSelectedOrNotSelectedByIndex(findIndex, true, "<%=gd.ClientID %>");__current_highlight = findIndex;
	//ShowInfo("pick a matierial:" + result.PNOrItemName + " success.");
	ShowInfo("");
	var bFinished = check_if_can_jian_liao_finished();
	if (bFinished == true) {
		ShowInfo(mesScanPizzaId);
	}
}

function JianLiaoFail(result) 
{
	ShowMessage(result.get_message());
	ShowInfo(result.get_message());
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
		
		var rowArray = new Array();
		var rw;
		var collection = bomList[i]["collectionData"];
		var parts = bomList[i]["parts"];
		var tmpstr = "";

		//for (var j = 0; j < parts.length; j++) {
		//    tmpstr = tmpstr + " " + parts[j]["id"];
		//}
		if (bomList[i]["PartNoItem"] == null) 
		{
			tmpstr = " ";
		}
		else 
		{
			tmpstr = bomList[i]["PartNoItem"];
			// wu.de-hong, said that: 
			//     UC上有一句：@PartNo 中的Part No 如果是以'DIB' 为前缀的时候，需要删除该前缀
			// Liu, Qing-Biao (劉慶彪 ITC) [16:51]:
			//     应该在 service 里改，还是在 asp 端？
			// Wu, De-Hong (吳德宏 ITC) [16:51]:
			//     如果在改变PartNo值，就麻烦了。
			//     只是显示时，不显示罢了
			if ((tmpstr.length > 3) && (tmpstr.substring(0, 3) == "DIB")) 
			{
				tmpstr = tmpstr.substring(3);
			}
			tmpstr = tmpstr.replace(",DIB", ",");
			// fix the issue end.
		}
		tmpstr = add_changeline_symbol(tmpstr);
		rowArray.push(tmpstr); //part no/name

		if ((bomList[i]["tp"] == null) || (bomList[i]["tp"] == ""))
		{
			rowArray.push(" ");
		}
		else
		{
			rowArray.push(bomList[i]["tp"]); //"type"// must modified into "tp";
		}
		
		if (bomList[i]["description"] == null) 
		{
			rowArray.push(" ");
		}
		else 
		{
			rowArray.push(bomList[i]["description"]);
		}
		rowArray.push(bomList[i]["qty"]);
		rowArray.push(bomList[i]["scannedQty"]);
		coll = "";
		
		tmpstr = bomList[i]["collectionData"];
		rowArray.push(tmpstr); //["collectionData"]);

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

	//if ((bomList.length > 0) && (updateIndex == -1))
	//{
	//    setSrollByIndex(0, true, tbl);
	//}
}

function updateTable(result)
{
	var ret = -1;

	var found = 0;
	for (var i = 0; i < defectInTable.length; i++)
	{
		var ok = 0;
		for (var j = 0; j < defectInTable[i]["parts"].length; j++)
		{
			if (defectInTable[i]["parts"][j]["id"] == result["PNOrItemName"]) 
			{
				if (defectInTable[i]["type"] == result["ValueType"]) 
				{
					ret = i;
					ok = 1;
					if (defectInTable[i].scannedQty < defectInTable[i].qty)
					{
						defectInTable[i].scannedQty++;
						defectInTable[i].collectionData += result["CollectionData"] + " ";
					}
					else
					{
						defectInTable[i].collectionData = result["CollectionData"];
					}
					//setSrollByIndex(i, true, "<%=gd.ClientID%>");
					break;
				}
				else 
				{
					var xx = 0;
				}
			}
		}
		
		if (ok == 1) {found = 1; break;}
	}
	//if (found == 1) { ret = true; }
	
	return ret;
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

</script>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width:200px;"/>
                    <col />
                    <col style="width:150px;"/>
					<col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblPdline" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
				<tr>
                    <td>
                        <asp:Label ID="lblCustSN" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCustomerSnContent" runat="server" Width="100%" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
					<td>
						<asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
					</td>
					<td>
						<asp:Label ID="lblModelContent" runat="server" Width="100%" CssClass="iMes_label_13pt"></asp:Label>
					</td>
                </tr>
			</table>
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
			<table width="100%" border="0" style="table-layout: fixed;">
				<tr>
                    <td>
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan=2>
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="99%" IsClear="true" IsPaste="true" />
                    </td>
					<td>
						<input id="btnPrintSetting" type="button"  runat="server" 
                class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'"  onmouseout="this.className='iMes_button_onmouseout'"    onclick="return clkSetting()" />
						&nbsp;&nbsp;
						<input id="btnReprint" type="button"  runat="server"  class="iMes_button" onclick="reprint()" />
						<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        </ContentTemplate>
                        </asp:UpdatePanel>
					</td>
                </tr>
            </table>

        </div>
    </div>      
    
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel> 
      
</asp:Content>

