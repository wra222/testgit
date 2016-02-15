
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="OfflineLcdCtPrint.aspx.cs" Inherits="OfflineLcdCtPrint" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceOfflineLcdCtPrint.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
    <table border="0" width="95%">
    <tr>
	   <td></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblTitleModel" runat="server" Text="Model:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblModel" runat="server"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="chkModel" runat="server" onclick="CheckClick(this)" CssClass="iMes_CheckBox" />&nbsp;
			<asp:Label ID="lblTitleCheckModel" runat="server" Text="Need Input Model"></asp:Label>
        </td>
        <td colspan="4"></td>
    </tr>
	<tr>
        <td>
            <asp:Label ID="lblTitleCT" runat="server" Text="CT:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblCT" runat="server"></asp:Label>
        </td>
        <td colspan="5"></td>
    </tr>
    <tr>
	    <td style="width:12%" align="left"><asp:Label ID="lblEntry" runat="server" CssClass="iMes_DataEntryLabel" Text="Data Entry:"></asp:Label></td>
	    <td colspan="6" align="left">
	    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
        <ContentTemplate>
	    <iMES:Input ID="Input1" runat="server" CssClass="iMes_textbox_input_Yellow" MaxLength="30"
                                    Width="80%" CanUseKeyboard="true" ProcessQuickInput="true" IsPaste="true" />
		&nbsp;
		<input id="btnSetting" type="button" runat="server" style="width:100px" onclick="showPrintSettingDialog()"
	                    onmouseover="this.className='iMes_button_onmouseover'" 
	                    onmouseout="this.className='iMes_button_onmouseout'" class="iMes_button"/>
        </ContentTemplate>
        </asp:UpdatePanel>                                        
       </td>
    </tr>
    </table>
    </center>
       
</div>
<asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
    <ContentTemplate>          
    </ContentTemplate>   
</asp:UpdatePanel> 

<input type="hidden" runat="server" id= "hidPCBID" />
<asp:HiddenField ID="stationHF" runat="server" />
<input type="hidden" runat="server" id="pCode" /> 
<script type="text/javascript">

var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
var errLenModel = '<%=this.GetLocalResourceObject(Pre + "_errLenModel").ToString()%>';
var errLenCT = '<%=this.GetLocalResourceObject(Pre + "_errLenCT").ToString()%>';

var SUCCESSRET = "SUCCESSRET";
var editor = '<%=UserId%>';
var customer = '<%=Customer%>';
var station = '<%=Request["Station"] %>';
var pCode = '<%=Request["PCode"] %>';
var lstPrintItem;
var inputObj;
var inputValue;
var model = '';
var ct = '';

document.body.onload = function() {
	inputObj = getCommonInputObject();
	ShowInfo("");
	getAvailableData("ProcessInput");
	getCommonInputObject().focus();
}

function WaitInput() {
    getCommonInputObject().value = "";
    getCommonInputObject().focus();
    getAvailableData("ProcessInput");
}

function ProcessInput(inputData) {
	try {
		ShowInfo("");
		if (!checkprint()) {
		    ShowInfo(msgPrintSettingPara);
		    WaitInput();
			return;
		}
		inputValue = getCommonInputObject().value.trim();
		if (inputValue == "") {
			errorFlag = true;
			alert('Please input.');
			WaitInput();
			return;
		}
		model = '';
		if (document.getElementById("<%=chkModel.ClientID%>").checked) {
			if (document.getElementById("<%=lblModel.ClientID %>").innerHTML == '') {
				if ((inputValue.length != 14) || ('CR' != inputValue.substr(0,2))) {
				    ShowInfo(errLenModel);
					getAvailableData("ProcessInput");
					return;
				}
				model = inputValue;
				beginWaitingCoverDiv();
				WebServiceOfflineLcdCtPrint.CheckModel(model, customer, onCheckModelSucceed, onFail);
				return;
			}
			model = document.getElementById("<%=lblModel.ClientID %>").innerHTML;
		}
		/*if (inputData.length != 13) {
		    ShowInfo(errLenCT);
		    WaitInput();
			return;
		}*/
		ct = inputValue;
		beginWaitingCoverDiv();
		WebServiceOfflineLcdCtPrint.Print(model, ct, editor, station, customer, pCode, lstPrintItem, onSucceed, onFail);
		
	} catch (e) {
		alertAndCallNext(e.description);
	}
}

function alertAndCallNext(message) {
	endWaitingCoverDiv();
	alert(message);
	WaitInput();
}

function checkprint() {
	lstPrintItem = getPrintItemCollection();
	if (lstPrintItem == null)                 //判断 若PrintItem==null, 不继续打印，等待客户维护PrintSetting页面后，再刷入打印
	{
		return false;
	}
	return true;
}

function onCheckModelSucceed(result) {
	try {
		endWaitingCoverDiv();
		if (result == null) {
			var content = msgSystemError;
			ShowMessage(content);
			ShowInfo(content);
		}
		else if ((result.length == 1) && (result[0] == SUCCESSRET)) {
			document.getElementById("<%=lblModel.ClientID %>").innerHTML = model;
			ShowInfo("[請刷入CT]");
		}
		else {
			var content = result[0];
			ShowMessage(content);
			ShowInfo(content);
		}
	} catch (e) {
		alertAndCallNext(e.description);
    }
    WaitInput();
}

function onSucceed(result) {
	try {
		endWaitingCoverDiv();
		if (result == null) {
			var content = msgSystemError;
			ShowMessage(content);
			ShowInfo(content);
		}
		else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
			ShowSuccessfulInfo(true, "[CT:" + ct + "] " + "[Model:" + model + "] " + msgSuccess);
			if ('' == model)
			    setPrintItemListParam1(result[1][0], ct, 'NO');
			else
			    setPrintItemListParam1(result[1][0], ct, model);
			printLabels(result[1][0], false);
		}
		else {
			var content = result[0];
			ShowMessage(content);
			ShowInfo(content);
		}
	} catch (e) {
		alertAndCallNext(e.description);
    }
    WaitInput();
}

function onFail(error) {
	try {
		endWaitingCoverDiv();
		ShowMessage(error.get_message());
	} catch (e) {
		alertAndCallNext(e.description);
    }
    WaitInput();
}

function setPrintItemListParam1(backPrintItemList, ct, model)
{
	var lstPrtItem = backPrintItemList;
	var keyCollection = new Array();
	var valueCollection = new Array();

	keyCollection[0] = "@CT";
	valueCollection[0] = generateArray(ct);
	keyCollection[1] = "@Model";
	valueCollection[1] = generateArray(model);

	for (var jj = 0; jj < backPrintItemList.length; jj++) {
		backPrintItemList[jj].ParameterKeys = keyCollection;
		backPrintItemList[jj].ParameterValues = valueCollection;
	}
	//setPrintParam(lstPrtItem, "RCTO_Label", keyCollection, valueCollection);
}

function ExitPage() 
{ }

function ResetPage() {
	ExitPage();
	ShowInfo("");
	endWaitingCoverDiv();
}

function showPrintSettingDialog() {
	showPrintSetting(document.getElementById("<%=stationHF.ClientID%>").value, document.getElementById("<%=pCode.ClientID%>").value);
}

function CheckClick(singleChk) {
	if (!singleChk.checked) {
		document.getElementById("<%=lblModel.ClientID %>").innerHTML = '';
	}
}

</script>
</asp:Content>

