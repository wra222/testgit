
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="RULabelPrint.aspx.cs" Inherits="RULabelPrint" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="Service/WebServiceRULabelPrint.asmx" />
        </Services>
    </asp:ScriptManager>
    <center>
        <table border="0" width="95%">
            <tr>
	           <td></td>
	           <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPalletNoTop" runat="server" Text="PalletNo:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblPalletNoContent" runat="server"></asp:Label>    
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTotalCartonQty" runat="server" Text="Total Carton Qty:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblTotalCartonQtyContent" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
	            <td style="width:12%" align="left">
	                <asp:Label ID="lblPalletNo" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
	            </td>
	            <td colspan="6" align="left">
	                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                    <ContentTemplate>
	                <iMES:Input ID="Input1" runat="server" CssClass="iMes_textbox_input_Yellow" MaxLength="30"
                                                Width="99%" CanUseKeyboard="true" ProcessQuickInput="true" IsPaste="true" />
                    </ContentTemplate>
                    </asp:UpdatePanel>                                        
               </td>
            </tr>
            <tr>
	            <td style="width:12%" align="left">&nbsp;</td>
	            <td colspan="5" align="left">&nbsp;</td>	   
	            <td align="right">
	                <table border="0" width="100%">
	                    <tr>
	                        <td style="width:80%" align="right">
	                            <input id="btnSetting" type="button"  runat="server" style="width:100px" onclick="showPrintSettingDialog()"
	                                onmouseover="this.className='iMes_button_onmouseover'" 
	                                onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button"/>
					            &nbsp;<input id="btnPrint" type="button"  runat="server" style="width:100px" onclick="print()"
	                                onmouseover="this.className='iMes_button_onmouseover'" 
	                                onmouseout="this.className='iMes_button_onmouseout'" class=" iMes_button" 
                                    align="right"/>
					            &nbsp;<input id="btnReprint" type="button"  runat="server"  class="iMes_button" onclick="reprint()" />
				            </td>                
	                    </tr>
	                </table>
                </td>
            </tr>
        </table>
    </center>
       
</div>
<asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
    <ContentTemplate>          
    </ContentTemplate>   
</asp:UpdatePanel> 

<asp:HiddenField ID="stationHF" runat="server" />
<input type="hidden" runat="server" id="pCode" /> 
<script type="text/javascript">

    var mesNoSelReason = '<%=this.GetLocalResourceObject(Pre + "_mesNoSelReason").ToString()%>';
    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var mesReasonOutRange = '<%=this.GetLocalResourceObject(Pre + "_mesReasonOutRange").ToString()%>';
    var mesNoPalletNo = '<%=this.GetLocalResourceObject(Pre + "_mesNoPalletNo").ToString()%>';
    var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
	var msgError = 'Error Input!';


    var SUCCESSRET = "SUCCESSRET";
    var editor = '<%=UserId%>';
    var customer = '<%=Customer%>';
    var lstPrintItem;
    var inputObj;
    var inpuPalletNo;
    var mac = '';
	var stationId = '<%=Request["Station"] %>';
	var accountId = '<%=Request["AccountId"] %>';
	var login = '<%=Request["Login"] %>';
	var lstPrintItem;
    
    document.body.onload = function() {
        inputObj = getCommonInputObject();
        ShowInfo("");
        getAvailableData("ProcessInput");
        getCommonInputObject().focus();
    }

    function ProcessInput(inputData) {
        try {
            ShowInfo("");
            lstPrintItem = getPrintItemCollection();
            if (lstPrintItem == null) {
                alert(msgPrintSettingPara);
                getAvailableData("ProcessInput");
                inputObj.focus();
                return;
            }
            inpuPalletNo = inputData
            if (inpuPalletNo == "") {
                errorFlag = true;
                msg = mesNoPalletNo;
                alert(msg);
                getCommonInputObject().focus();
                return;
            }
            if (inpuPalletNo.length == 10) {
                beginWaitingCoverDiv();
                WebServiceRULabelPrint.CheckPalletNo(inpuPalletNo, CheckPalletNoSucceed, CheckPalletNoFail);
                getCommonInputObject().value = "";
                getAvailableData("ProcessInput");
                getCommonInputObject().focus();
            }
            else {
                alert(msgError);
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
                getAvailableData("ProcessInput");
                return;
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }
   
    function alertAndCallNext(message) {
        endWaitingCoverDiv();
        alert(message);
        getAvailableData("ProcessInput");
    }

    function CheckPalletNoSucceed(result) {
        try {
            endWaitingCoverDiv();
            if (result == null) {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
            }
            else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
                document.getElementById("<%=lblPalletNoContent.ClientID %>").innerText = result[1][0];
                document.getElementById("<%=lblTotalCartonQtyContent.ClientID %>").innerText = result[1][1];
                var pCode = document.getElementById("<%=pCode.ClientID%>").value;
                WebServiceRULabelPrint.Print(result[1][0], result[1][1], editor, stationId, customer, pCode, lstPrintItem, onSucceed, onFail); 
                
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
                getAvailableData("ProcessInput");
            }
            else {
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function CheckPalletNoFail(error) {
        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());
        } catch (e) {
            alertAndCallNext(e.description);
        }

    }

    function onSucceed(result) {
        try {
            endWaitingCoverDiv();
            if (result == null) {
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
            }
            else if ((result.length == 2) && (result[0] == SUCCESSRET)) {

                ShowSuccessfulInfo(true, "[PalletNo:" + result[1][1] + "] " + msgSuccess);
                result[1][0][0].BatPiece = 0;
                setPrintItemListParam1(result[1][0], result[1][1]);
                printLabels(result[1][0], false);
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
            else {
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function onFail(error) {
        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());
        } catch (e) {
            alertAndCallNext(e.description);
        }

    }
    
    function setPrintItemListParam1(backPrintItemList, palletNo)
    {
        var lstPrtItem = backPrintItemList;
        var keyCollection = new Array();
        var valueCollection = new Array();
        var templateName = lstPrtItem[0].TemplateName

        keyCollection[0] = "@PalletNo";
        valueCollection[0] = generateArray(palletNo);

        keyCollection[1] = "@TemplateName";
        valueCollection[1] = generateArray(templateName);

        for (var jj = 0; jj < backPrintItemList.length; jj++) {
            backPrintItemList[jj].ParameterKeys = keyCollection;
            backPrintItemList[jj].ParameterValues = valueCollection;
        }
        //setPrintParam(lstPrtItem, "RCTO_Label", keyCollection, valueCollection);
    }
	
	function reprint() {
		var url = "../PAK/RePrintRULabel.aspx?Station=" + stationId + "&PCode=" + document.getElementById("<%=pCode.ClientID%>").value + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + accountId + "&Login=" + login; 
		var paramArray = new Array();
		paramArray[0] = '';
		paramArray[1] = editor;
		paramArray[2] = customer;
		paramArray[3] = stationId;
		window.showModalDialog(url, paramArray, 'dialogWidth:850px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
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
</script>
</asp:Content>

