<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:UI for 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* Known issues:
* TODO：
*/
--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="FAIOutput.aspx.cs" Inherits="FAI_FAIOutput" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
<style type="text/css">
</style>

<div>
	<asp:ScriptManager ID="ScriptManager1" runat="server" >
		<Services>
			<asp:ServiceReference Path="Service/WebServiceFAIOutput.asmx" />
		</Services>
	</asp:ScriptManager>

	<center>

	<table width="95%" style="height:200px; vertical-align:middle" cellpadding="0" cellspacing="0">
		<tr><td>&nbsp;</td><td></td><td></td><td></td></tr>
		<tr>
			<td align="left" width="15%">
				<asp:Label ID="lblCustSN" runat="server"  CssClass="iMes_label_13pt" Text="CustSN:"></asp:Label>
			</td>
			<td align="left" width="35%">
				<asp:Label ID="txtCustSN" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
			</td>
			<td align="left" width="15%">
				<asp:Label ID="lblProductID" runat="server"  CssClass="iMes_label_13pt" Text="Product ID:"></asp:Label>
			</td>
			<td align="left" width="35%">
				<asp:Label ID="txtProductID" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
			</td>
		</tr>
		<tr>
			<td align="left" >
				<asp:Label ID="lblModel" runat="server"  CssClass="iMes_label_13pt" Text="Model:"></asp:Label>
			</td>
			<td align="left">
				<asp:Label ID="txtModel" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
			</td>
			<td align="left" >
				<asp:Label ID="lblCurrentStation" runat="server"  CssClass="iMes_label_13pt" Text="Current Station:"></asp:Label>
			</td>
			<td align="left">
				<asp:Label ID="txtCurrentStation" runat="server"  CssClass="iMes_label_11pt"></asp:Label>
			</td>
		</tr>
		<tr><td>&nbsp;</td><td colspan=3></td></tr>
		
		<tr>
			<td colspan="4"><asp:Label ID="lblGridTitle" runat="server" class="iMes_label_13pt" Text="Defect List:"> </asp:Label></td>
		</tr>
		<tr>
			<td colspan="4">
				<asp:UpdatePanel ID="upPanl" runat="server">
					<ContentTemplate>
						<iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" 
							GvExtWidth="100%" GvExtHeight="228px" style="top: 0px; left: 0px" Width="100%" Height="220px" SetTemplateValueEnable="False" HighLightRowPosition="3" AutoHighlightScrollByValue="True">
						</iMES:GridViewExt> 
					</ContentTemplate>
				</asp:UpdatePanel>
			</td>
		</tr>
		<tr><td>&nbsp;</td><td colspan=3></td></tr>
		
		<tr >
			<td style="width:20%" align="left" >
				<asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
			</td>
			<td align="left" colspan=3>
				<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
					<ContentTemplate>
						<iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
						CanUseKeyboard="true" IsPaste="true" MaxLength="50" 
						InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"/>
					</ContentTemplate>   
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="hiddenbtn" EventName="ServerClick" />
					</Triggers>                                     
				</asp:UpdatePanel>
			</td>     
		</tr>

		<tr><td>&nbsp;</td><td colspan=3></td></tr>
		<tr>
			<td>&nbsp;</td>
			<td colspan=3>
				<asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
					<ContentTemplate>
						<button id="btnHidden" runat="server" onserverclick="btnHidden_Click" style="display: none" ></button>
						<input id="pCode" type="hidden" runat="server" /> 
						<input id="hiddenStation" type="hidden" runat="server" />
						<button id="hiddenbtn" runat="server" style="display: none"></button>                           
					</ContentTemplate>   
				</asp:UpdatePanel> 
			</td>
		</tr>
	</table>
	</center>
</div>

<script type="text/javascript">

var msgDataEntryField = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryField").ToString() %>';
var msgDataEntryNull = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryNull").ToString() %>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var msgScan9999 = '請刷 9999';
var msgScan9999orDefect = '請刷 9999 或 Defect';
var msgScan9999or7777 = '請刷 9999 或 7777(修改Defect)';
var msgErrDefect = "DefectCode 錯誤";
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var inputObj = "";
var line = "";
var station = "";
var custsn = "";
var prodid = "";
var SessionStartFlag = false;
var defectCode = "";

var tbl;
var index = 1;
var DEFAULT_ROW_NUM = <%=DEFAULT_ROWS%>;
var initRowsCount = DEFAULT_ROW_NUM;
var msgExistGd = '<%=this.GetLocalResourceObject(Pre + "_msgExistGd").ToString() %>';
var scannedList=new Array();

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onload
//| Author		:	Jessica Liu
//| Create Date	:	6/12/2012
//| Description	:	加载接受输入数据事件
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
document.body.onload = function() {
	try {
		ShowInfo("");

		inputObj = getCommonInputObject();
		inputObj.focus();

		station = document.getElementById("<%=hiddenStation.ClientID %>").value;
		tbl = "<%=gd.ClientID %>";
		getAvailableData("processDataEntry")

	} catch (e) {
		alert(e.description);

		inputObj.focus();
	}

}

function processDataEntry(inputdata) 
{    
	try {
		if (inputdata == "") 
		{
			alert(msgDataEntryNull);
			callNextInput();
			return;
		}

		if ('' == document.getElementById("<%=txtCustSN.ClientID%>").innerText) {
			beginWaitingCoverDiv();
			WebServiceFAIOutput.checkProdId(inputdata, line, "<%=UserId%>", station, "<%=Customer%>", onCheckSucceed, onFail);
			return;
		}
		
		if ('9999' == inputdata) {
			beginWaitingCoverDiv();
			WebServiceFAIOutput.Save(document.getElementById("<%=txtCustSN.ClientID%>").innerText, scannedList, line, "<%=UserId%>", station, "<%=Customer%>", onSucceed, onFail);
			return;
		}
		else if ('7777' == inputdata) {
			scannedList=new Array();
			resetGd();
			ShowInfo(msgScan9999orDefect);
			callNextInput();
			return;
		}
		else {
			/*
			if (inputdata.length != 4){
				alert(msgErrDefect);
				ShowInfo(msgErrDefect);
				inputObj.focus();
				getAvailableData("processDataEntry");
				return;
			}
			*/
			
			if (isInTable(inputdata)) {
				ShowInfo("Defect:"+ inputdata +" "+ msgExistGd);
				callNextInput();
				return;
			}
			
			beginWaitingCoverDiv();
			WebServiceFAIOutput.CheckDefect(document.getElementById("<%=txtCustSN.ClientID%>").innerText, inputdata, line, "<%=UserId%>", station, "<%=Customer%>", onCheckDefectSucceed, onCheckDefectFailed);
		}
	} catch (e) {
		alert(e.description);
	}
}


function onCheckSucceed(result) {
	ShowInfo("");
	endWaitingCoverDiv();
			
	try {
		if (result == null) {
			//endWaitingCoverDiv();                
			var content = msgSystemError;
			ShowMessage(content);
			ShowInfo(content);
		}
		else if ((result.length == 5) && (result[0] == SUCCESSRET)) {
			custsn = result[1];
			document.getElementById("<%=txtCustSN.ClientID%>").innerText = result[1];
			document.getElementById("<%=txtProductID.ClientID%>").innerText = result[2];
			document.getElementById("<%=txtModel.ClientID%>").innerText = result[3];
			document.getElementById("<%=txtCurrentStation.ClientID%>").innerText = result[4];
			
			scannedList=new Array();
		
			ShowInfo(msgScan9999orDefect);
			inputObj.value = "";
			callNextInput();
		}
		else {
			//ShowInfo("");
			//endWaitingCoverDiv();
			var content1 = result[0];
			ShowMessage(content1);
			ShowInfo(content1);
		}

	} catch (e) {
		endWaitingCoverDiv();
		alert(e.description);
	}
}

function onCheckDefectSucceed(result) {
	ShowInfo("");
	endWaitingCoverDiv();
			
	try {
		if (result == null) {
			//endWaitingCoverDiv();                
			var content = msgSystemError;
			ShowMessage(content);
			ShowInfo(content);
		}
		else if (result[0] == SUCCESSRET) {
			var rowArray = new Array();
            rowArray.push(result[1]);
            rowArray.push(result[2]);
            addOrChange(rowArray);
			
			defectCode = result[1];
			scannedList.push(result[1]);
			
			ShowInfo(msgScan9999orDefect); // msgScan9999or7777
			inputObj.value = "";
			callNextInput();
		}
		else {
			//ShowInfo("");
			//endWaitingCoverDiv();
			var content1 = result[0];
			ShowMessage(content1);
			ShowInfo(content1);
		}

	} catch (e) {
		endWaitingCoverDiv();
		alert(e.description);
	}
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onSucceed
//| Author		:	Jessica Liu
//| Create Date	:	6/12/2012
//| Description	:	调用web service checkAndSave成功
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onSucceed(result) 
{
	ShowInfo("");
	endWaitingCoverDiv();
	inputObj.value = "";
	inputObj.focus(); 
	SessionStartFlag = false;
		   
	try {

		if (result == null) 
		{
			var content = msgSystemError;
			ShowMessage(content);
			ShowInfo(content);            
		}
		else if (result[0] == SUCCESSRET) 
		{
			var stationDesc = "";
			if ("" != result[1])
				stationDesc = "，機器回到" + result[1] + " - " + result[2];
			var successInfo = "";
			if ("" == defectCode)
				successInfo += "[ CUSTSN: " + custsn + " 已刷出 " + stationDesc + " ] " + msgSuccess;
			else
				successInfo = msgSuccess;
			
			ShowSuccessfulInfo(true, successInfo);
		}
		else 
		{
			ShowInfo("");
			var content1 = result[0];
			ShowMessage(content1);
			ShowInfo(content1);
		}

		custsn = "";
		document.getElementById("<%=txtCustSN.ClientID%>").innerText = "";
		document.getElementById("<%=txtProductID.ClientID%>").innerText = "";
		document.getElementById("<%=txtModel.ClientID%>").innerText = "";
		document.getElementById("<%=txtCurrentStation.ClientID%>").innerText = "";
		defectCode = "";
		resetGd();
		
		callNextInput();
		
	} catch (e) {
		alert(e.description);
	}

}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onFail
//| Author		:	Jessica Liu
//| Create Date	:	6/12/2012
//| Description	:	调用web service checkAndSave失败
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function onFail(error) 
{
	SetInitStatus();
	SessionStartFlag = false;
		
	try {
		ShowMessage(error.get_message());
		ShowInfo(error.get_message());
		
		defectCode = "";
		resetGd();

		callNextInput();

	} catch (e) {
		alert(e.description);
	}
}

function onCheckDefectFailed(error) 
{
	SetInitStatus();
	SessionStartFlag = false;
		
	try {
		ShowMessage(error.get_message());
		ShowInfo(error.get_message());
		
		callNextInput();

	} catch (e) {
		alert(e.description);
	}
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	callNextInput
//| Author		:	Jessica Liu
//| Create Date	:	6/12/2012
//| Description	:	等待快速控件继续输入
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function callNextInput() 
{
	inputObj.value = "";
	inputObj.focus();    
	getAvailableData("processDataEntry");
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	SetInitStatus
//| Author		:	Jessica Liu
//| Create Date	:	6/12/2012
//| Description	:	处理底层调用返回时，做的控件清空、取消hold界面等初始处理
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function SetInitStatus() 
{
	ShowInfo("");
	
	endWaitingCoverDiv();
	
	resetAll();

}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	onbeforeunload
//| Author		:	Jessica Liu
//| Create Date	:	6/12/2012
//| Description	:	onbeforeunload时调用
//| Input para.	:	
//| Ret value	:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
window.onbeforeunload = function() {
	ExitPage();
} 

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	ExitPage
//| Author		:	Jessica Liu
//| Create Date	:	6/12/2012
//| Description	:	退出页面时调用
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function ExitPage() 
{
	//if (SessionStartFlag == true) {
		WebServiceFAIOutput.Cancel(document.getElementById("<%=txtCustSN.ClientID%>").innerText, station);
		sleep(waitTimeForClear);
		SessionStartFlag = false;
	//}   
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	resetAll
//| Author		:	Jessica Liu
//| Create Date	:	6/12/2012
//| Description	:	重置所有控件到初始状态
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function resetAll() 
{
	inputObj.value = "";
	custsn = "";
	
	inputObj.focus();   
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	ResetPage
//| Author		:	Jessica Liu
//| Create Date	:	6/12/2012
//| Description	:	刷新页面时调用
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function ResetPage() 
{
	ExitPage();
	resetAll();
}

var iSelectedRowIndex = null;
function setGdHighLight(con) {
	if ((iSelectedRowIndex != null) ) {
		setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");     //去掉过去高亮行           
	}
	setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");     //设置当前高亮行
	iSelectedRowIndex = parseInt(con.index, 10);    //记住当前高亮行
}

var GridViewExt1ClientID = "<%=gd.ClientID %>";

///---------------------------------------------------
///| Name		    :	addOrChange
///| Description	:	update table Info 
///| Input para.	:	
///| Ret value      :
///---------------------------------------------------
function addOrChange(rowArray)
{
	AddRowInfo(rowArray);

	var gdObj = document.getElementById("<%=gd.ClientID %>");
	var con = gdObj.rows[index-1]; //index从1开始
	 //高亮当前行
	setGdHighLight(con);

}
function AddRowInfo(RowArray) {
	try {
		if (index < initRowsCount) {
			eval("ChangeCvExtRowByIndex_" + GridViewExt1ClientID + "(RowArray,false, index)");
		}
		else {
			eval("AddCvExtRowToBottom_" + GridViewExt1ClientID + "(RowArray,false)");
		}
		setSrollByIndex(index, false);
		index++;

	}
	catch (e) {
		ShowInfo(e.description);
		PlaySound();
	}
}

function isInTable(str)
{
	//judge custom sn in Table?
	var tblObj = document.getElementById("<%=gd.ClientID %>");
	var length = tblObj.rows.length;
	
	for (var i = 1; i < length; i++)
	{
		if (tblObj.rows[i].cells[0].innerText.trim() == str)
		{
			return true;
		}
	}
	return false;
}

function getTableContent() {
	var content = new Array( );
	var custList = new Array();
	var pnList = new Array();
	var errList = new Array();
	
	var tblObj = document.getElementById("<%=gd.ClientID %>");
	var length = tblObj.rows.length;

	for (var i = 1; i < length; i++) {
		if (tblObj.rows[i].cells[0].innerText.trim() == "") {
			break;
		}
		custList.push(tblObj.rows[i].cells[0].innerText.trim());
		pnList.push(tblObj.rows[i].cells[1].innerText.trim());
		errList.push("Pass");
	}
	content.push(custList);
	content.push(pnList);
	content.push(errList);
	return content;
}

function resetGd() {
	index = 1;
	iSelectedRowIndex = null;
	ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
	scannedList=new Array();
}

</script>
        
    
</asp:Content>
