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
<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="FAIInput.aspx.cs" Inherits="FAI_FAIInput" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
<style type="text/css">
</style>
 
<script type="text/javascript">

var msgDataEntryField = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryField").ToString() %>';
var msgDataEntryNull = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryNull").ToString() %>';
var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var msgScan9999 = '請刷 9999';
var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
var inputObj = "";
var line = "";
var station = "";
var custsn = "";
var prodid = "";
var SessionStartFlag = false;

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
			inputObj.focus();
			getAvailableData("processDataEntry");
			return;
		}

		if ('' == document.getElementById("<%=txtCustSN.ClientID%>").innerText) {
			beginWaitingCoverDiv();
			WebServiceFAIInput.checkProdId(inputdata, line, "<%=UserId%>", station, "<%=Customer%>", onCheckSucceed, onFail);
		}
		else if ('9999' == inputdata) {
			beginWaitingCoverDiv();
			WebServiceFAIInput.Save(document.getElementById("<%=txtCustSN.ClientID%>").innerText, line, "<%=UserId%>", station, "<%=Customer%>", onSucceed, onFail);
		}
		else {
			alert(msgScan9999);
			inputObj.value = "";
			inputObj.focus();
			getAvailableData("processDataEntry");
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
		
			ShowInfo(msgScan9999);
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
		else if ((result.length == 1) && (result[0] == SUCCESSRET)) 
		{
			var successInfo = "";                
			successInfo += "[ CUSTSN: " + custsn + " 已刷入" + "] " + msgSuccess;
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
		WebServiceFAIInput.Cancel(document.getElementById("<%=txtCustSN.ClientID%>").innerText, station);
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
    
</script>
        
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" >
            <Services>
                <asp:ServiceReference Path="Service/WebServiceFAIInput.asmx" />
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
</asp:Content>
