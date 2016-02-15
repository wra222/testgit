<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-PAK-UC Content & Warranty Print.docx –2011/10/13 Warrant
* UC:CI-MES12-SPEC-PAK-UC Content & Warranty Print.docx –2011/10/13            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-01-17   Du.Xuan               Create   
* Known issues:
* TODO：
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="ReprintConfigurationLabel.aspx.cs" Inherits="SA_ReprintContentLabel"
    Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServicePrintContentWarranty.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 150px;" />
                    <col />
                    <col style="width: 80px;" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" IsClear="false"  MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtReason" runat="server" CssClass="iMes_textbox_input_Normal" Width="99%" />
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr style="height: 30px">
                    <td colspan="4" align="right">
                        <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                        </button>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="right">
                        <button id="btnReprint" runat="server" onclick="clkReprint()" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'">
                        </button>
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="updatePanel" runat="server">
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <button id="hiddenbtn" runat="server" onserverclick="hiddenbtn_Click" style="display: none">
                    </button>
                    <input type="hidden" runat="server" id="station" />
                    <input type="hidden" runat="server" id="pCode" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <script type="text/javascript" language="javascript">

        var editor = "";
        var customer = "";
        var station = "";
        var pcode = "";

        var customerSN = "";

        var inputObj = "";
        var inputData = "";

        var snInput = true;

        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara")%>';
        var msgInputReason = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgReasonNull") %>';
        var msgNoPrintItem = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoPrintItem")%>';
        var msgInputCustSN = '<%=this.GetLocalResourceObject(Pre + "_msgInputCustSN").ToString() %>';

        document.body.onload = function() {

            try {
                editor = "<%=UserId%>";
                customer = "<%=Customer%>";
                station = '<%=Request["Station"]%>';
                pcode = '<%=Request["PCode"]%>';

                inputObj = getCommonInputObject();
                inputData = inputObj.value;
                DisplayInfoText();
                initPage();
                callNextInput();

            } catch (e) {
                alert(e.description);
            }
        }

        function initPage() {
            setInputOrSpanValue(document.getElementById("<%=txtReason.ClientID %>"), "");
            ShowInfo("");
            customerSN = "";
        }

        function checkInput(inputData) {

            /*if ((inputData.length == 11) && (inputData.substring(0, 3) == "PCN")) {
                customerSN = inputData.substring(1, 11);
            }
            else if ((inputData.length == 10) && (inputData.substring(0, 2) == "CN")) {

                customerSN = inputData;
            }
            else {
                alert(msgInputCustSN);
                //ShowMessage(msgInputCustSN);
                callNextInput();
                return;
            }
            */
            //document.getElementById("<%=txtReason.ClientID %>").focus();
            clkReprint();
            return;
        }

        function clkReprint() {

            var reason = document.getElementById("<%=txtReason.ClientID %>").value;
            var cust = getCommonInputObject().value;

            if ((cust.length == 11) && (cust.substring(0, 3) == "PCN")) {
                customerSN = cust.substring(1, 11);
            }
            else if ((cust.length == 10) && (cust.substring(0, 2) == "CN")) {

                customerSN = cust;
            }
            else{
                alert(msgInputCustSN);
                //ShowMessage(msgInputCustSN);
                callNextInput();
                return;
            }

            /*if (reason == "") {
                alert(msgInputReason);
                //ShowMessage(msgInputReason);
                callNextInput();
                return;
            }*/
            var lstPrintItem = getPrintItemCollection();   //UI transfer in Para: PrintItemlist

            if (lstPrintItem == "" || lstPrintItem == null) {
                alert(msgPrintSettingPara);
                callNextInput();
                return;
            }

            beginWaitingCoverDiv();
            WebServicePrintContentWarranty.ReprintConfigurationLabel(customerSN, reason, lstPrintItem, "", editor, station, customer, onPrintSucceed, onPrintFail);

        }

        function getTemp(result, label) {

            for (var i = 0; i < result.length; i++) {
                if (result[i].LabelType == label) {
                    return i;
                }
            }
            return -1;
        }

        function setPrintItemListParam(backPrintItemList, custSN, labelname) {

            // @prdid --Product ID
            // @mac --MAC Address   reprint的sp不用Mac
            // @model --机型12码
            //============================================generate PrintItem List==========================================
            var lstPrtItem = new Array();
            lstPrtItem[0] = backPrintItemList;
            
            var keyCollection = new Array();
            var valueCollection = new Array();

            keyCollection[0] = "@sn";
            valueCollection[0] = generateArray(customerSN);

            /*
            * Function Name: setPrintParam
            * @param: printItemCollection
            * @param: labelType
            * @param: keyCollection(Client: Array of string.    Server: List<string>)
            * @param: valueCollection(Client: Array of string array.    Server: List<List<string>>)
            */

            setPrintParam(lstPrtItem, labelname, keyCollection, valueCollection);
        }

        function onPrintSucceed(result) {

            ShowInfo("");
            endWaitingCoverDiv();
            var index = 0;
            var printlist = new Array();
            try {
                if (result == null) {
                    ShowMessage(msgSystemError);
                    //ShowInfo(msgSystemError);
                }
                else if (result[0] == SUCCESSRET) {
                    index = getTemp(result[1],"Content label");
                    setPrintItemListParam(result[1][index], customerSN,"Content label"); 
                    printlist[0] = result[1][index];

                    //==========================================print process=======================================

                    /*
                    * Function Name: printLabels
                    * @param: printItems
                    * @param: isSerial
                    */

                    printLabels(printlist, false);

                    //==========================================end print process===================================
                    //ResetPage();
                    ShowSuccessfulInfoFormat(true, "Customer SN", customerSN);
                }
                else {
                    ShowMessage(result[0]);
                }

            }
            catch (e) {
                alert(e.description);
                callNextInput();
            }
            callNextInput();
        }

        function onPrintFail(result) {

            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function clkSetting() {

            showPrintSetting(station, pcode);
            callNextInput();

        }

        function ResetPage() {
            initPage();
            callNextInput();
        }

        function callNextInput() {

            //getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("checkInput");
        }



     
    </script>

</asp:Content>
