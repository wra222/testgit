
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    ContentType="text/html;Charset=UTF-8" CodeFile="EEPLabelPrint.aspx.cs" Inherits="TRIS_EEPLabelPrint"
    Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="~/TRIS/Service/WebServiceEEPLabelPrint.asmx" />
        </Services>
    </asp:ScriptManager>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div id="bg" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <center>
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 80px;" />
                    <col />
                    <col style="width: 80px;" />
                    <col />
                </colgroup>
                <tr>
                    <td width="30%">
                        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                         <iMES:CmbPdLine ID="CmbPdLine1" runat="server" Width="100" IsPercentage="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblProdID" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="txtProdID" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="txtModel" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:Input ID="Input1" runat="server" ProcessQuickInput="true" Width="99%" CanUseKeyboard="true"
                            IsClear="true" IsPaste="true" MaxLength="30" InputRegularExpression="^[-0-9a-zA-Z\+\s\*\,]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    </td>
                </tr>
                
            </table>
            <table width="99%" style="vertical-align: middle" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="4" align="right">
                        <input id="btnPrintSetting" type="button"  runat="server" class="iMes_button" onclick="clkSetting()" 
                        onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                        
                        <input id="btnPrint" type="button"  runat="server"  class="iMes_button" onclick="reprint()" value="RePrint"
                        onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <div id="div4">
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline" >
                        <ContentTemplate>
                            <input type="hidden" runat="server" id="station" />
                            <input type="hidden" runat="server" id="pCode" />
                            <input type="hidden" runat="server" id="PrintLogName" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        var msgPdLineNull = '<%=this.GetLocalResourceObject(Pre + "_msgPdLineNull").ToString() %>';
        var msgProdIDNull = '<%=this.GetLocalResourceObject(Pre + "_msgProdIDNull").ToString() %>';
        var msgInvalidInput = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidInput").ToString() %>';
        var msgSFCFailed = '<%=this.GetLocalResourceObject(Pre + "_msgSFCFailed").ToString() %>';
        var msgSFCSucc = '<%=this.GetLocalResourceObject(Pre + "_msgSFCSucc").ToString() %>';
        var msgNotMatchProdIDRuler = '<%=this.GetLocalResourceObject(Pre + "_msgNotMatchProdIDRuler").ToString() %>';
        var msgWorkFlow = '<%=this.GetLocalResourceObject(Pre + "_msgWorkFlow").ToString() %>';
        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';

        var editor = "";
        var customer = "";
        var station = "";
        var pdLine = "";
        var productID = "";
        var model = "";
        var configCode = "";
        var customerSN = "";
        var labelType = "";
        var PrintLogName = "";

        var accountid = '<%=AccountId%>';
        var username = '<%=UserName%>';
        var login = '<%=Login%>';
		
		var WhetherNoCheckDDRCT = "";

        window.onload = function() {
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            station = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            
            initPage();
            callNextInput();
        }

        window.onbeforeunload = function() {
            ExitPage();
        }

        function initPage() {
            document.getElementById("<%=txtProdID.ClientID %>").innerText = "";
            document.getElementById("<%=txtModel.ClientID %>").innerText = "";
            productID = "";
            model = "";
            labelType = "";
			WhetherNoCheckDDRCT = "";
            ShowInfo("");
        }

        function checkInput(inputData) {
            ShowInfo("");
            inputData = inputData.trim().toUpperCase();
            inputData = Get2DCodeCustSN(inputData);
            if ((getPdLineCmbValue() == "") || (getPdLineCmbValue() == null)) {
                alert(msgPdLineNull);
                callNextInput();
            }
            else {
                if (inputData == "" || inputData == null) {
                    alert(msgProdIDNull);
                    callNextInput();
                }
                else if (inputData.length == 9) {
                    InputProdID(inputData);
                }
                else{
                    InputCustSN(inputData);
                }
            }

        }

        function InputProdID(prodID) {

            try {
                var printItemlist = getPrintItemCollection();

                if (printItemlist == null || printItemlist == "") {
                    alert(msgPrintSettingPara);
                    ResetPage();
                    callNextInput();
                    return;
                }
            }
            catch (e) {
                alert(e);
                ResetPage();
                callNextInput();
                return;
            }

            beginWaitingCoverDiv();

            var printItemlist = getPrintItemCollection();
            
            pdLine = getPdLineCmbValue();
            prodID = SubStringSN(prodID, "ProdId");
            WebServiceEEPLabelPrint.inputProdId(pdLine, prodID, station, PrintLogName, editor, customer, printItemlist, onInputSuccess, onInputFail);
        }

        function InputCustSN(custSN) {

            try {
                var printItemlist = getPrintItemCollection();

                if (printItemlist == null || printItemlist == "") {
                    alert(msgPrintSettingPara);
                    ResetPage();
                    callNextInput();
                    return;
                }
                if (!isCustSN(custSN)) {
                    alert(msgPrintSettingPara);
                    ResetPage();
                    callNextInput();
                    return;
                }
            }
            catch (e) {
                alert(e);
                ResetPage();
                callNextInput();
                return;
            }

            beginWaitingCoverDiv();
            var printItemlist = getPrintItemCollection();
            pdLine = getPdLineCmbValue();
            prodID = custSN;
            WebServiceEEPLabelPrint.inputProdId(pdLine, prodID, station, PrintLogName, editor, customer, printItemlist, onInputSuccess, onInputFail);
        }
		
        function onInputSuccess(result) {

            endWaitingCoverDiv();
            ShowInfo("");
            try {

                if (result[0] == SUCCESSRET) {
                    productID = result[1]["ProductID"];
                    customerSN = result[1]["CustSN"];
                    document.getElementById("<%=txtProdID.ClientID%>").innerText = productID;
                    document.getElementById("<%=txtModel.ClientID%>").innerText = result[1]["Model"]; ;

                    setPrintItemListParam1(result[2], productID);
                    printLabels(result[2], false);
                    //ShowMessage(result[0] + ":" + result[1]["ProductID"]);
                    ShowInfo(result[0] + ":" + result[1]["ProductID"], "green");
                }
                else {
                    ResetPage();
                    ShowMessage(result);
                    ShowInfo(result);
                    callNextInput();
                }
            }
            catch (e) {
                alert(e.description);
            }

            callNextInput();
        }
		
        function onInputFail(error) {
            try {
                endWaitingCoverDiv();
                ResetPage();
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
            }
            catch (e) {
                alert(e.description);
            }

            callNextInput();

        }

        function generateArray(val) {
            var ret = new Array();

            ret[0] = val;

            return ret;
        }

        function reprint() {
            var url = "EEPLabelRePrint.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&UserName=" + username + "&AccountId=" + accountid + "&Login=" + login;
            window.showModalDialog(url, "", 'dialogWidth:800px;dialogHeight:400px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
        }

        function setPrintItemListParam1(backPrintItemList, Proid) //Modify By Benson at 2011/03/30
        {
            var keyCollection = new Array();
            var valueCollection = new Array();

            keyCollection[0] = "@productID";
            
            valueCollection[0] = generateArray(Proid);
            

            for (jj = 0; jj < backPrintItemList.length; jj++) {
                backPrintItemList[jj].ParameterKeys = keyCollection;
                backPrintItemList[jj].ParameterValues = valueCollection;
            }
        }

        function clkSetting() {
            showPrintSetting(station, pCode);
        }

        function ExitPage() {
            if (productID != "") {
                WebServiceEEPLabelPrint.Cancel(productID, station);
                sleep(waitTimeForClear);
            }
        }

        function ResetPage() {
            ExitPage();
            initPage();
        }

        function callNextInput() {
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("checkInput");
        }



    </script>

</asp:Content>
