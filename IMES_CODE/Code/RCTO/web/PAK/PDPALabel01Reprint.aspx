<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx –2011/10/28 
* UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx –2011/10/28                     
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-08-09   Du.Xuan               Create   
* Known issues:
* TODO：
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="PDPALabel01Reprint.aspx.cs" Inherits="SA_PrintContentWarranty"
    Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServicePDPALabel01.asmx" />
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
                        <iMES:Input ID="txt" runat="server"  ProcessQuickInput="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" IsClear="false" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox id="txtReason" rows="5" style="width:100%;" 
                            runat="server" maxlength="80" onkeypress="return imposeMaxLength(this)" 
                            onblur="ismaxlength(this)" cols="20" name="S1"/>
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
        var msgPrintWLabel = '<%=this.GetLocalResourceObject(Pre + "_msgPrintWLabel").ToString() %>';
        var msgPrintDetail = '<%=this.GetLocalResourceObject(Pre + "_msgPrintDetail").ToString() %>';
        
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
            document.getElementById("<%=txtReason.ClientID %>").value = "";
            getCommonInputObject().value = "";
            ShowInfo("");
            customerSN = "";
        }

        function checkInput(inputData) {

            /*if ((inputData.length == 11) && (inputData.substring(0, 3) == "SCN")) {
                customerSN = inputData.substring(1, 11);
            }
            else if ((inputData.length == 10) && (inputData.substring(0, 2) == "CN")) {

                customerSN = inputData;
            }
            else {
                alert(msgInputCustSN);
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
            
           if (cust.length == 10 && cust.substring(0, 2) == "CN") {
                customerSN = cust;
            }
            else if (cust.length == 11 && cust.substring(0, 3) == "SCN" ) {
                customerSN = cust.substring(1, 11);
            }
            else {
                alert(msgInputCustSN);
                //ShowMessage(msgInputCustSN);
                callNextInput();
                return;
            }
            
            /*if (reason == "") {
                alert(msgInputReason);
                callNextInput();
                //ShowMessage(msgInputReason);
                return;
            }*/
            var lstPrintItem = getPrintItemCollection();   //UI transfer in Para: PrintItemlist

            if (lstPrintItem == "" || lstPrintItem == null) {
                alert(msgPrintSettingPara);
                callNextInput();
                return;
            }

            beginWaitingCoverDiv();
            WebServicePDPALabel01.Reprint(customerSN, reason, lstPrintItem,"", editor, station, customer, onPrintSucc, onPrintFail);       
            
        }

        function getTemp(result, label) {

            for (var i = 0; i < result.length; i++) {
                if (result[i].LabelType == label) {
                    return i;
                }
            }
            return -1;
        }

        function setPrintItemListParam(backPrintItemList, labelType) {
            //============================================generate PrintItem List==========================================
            //var lstPrtItem = backPrintItemList;
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
            setPrintParam(lstPrtItem, labelType, keyCollection, valueCollection);
        }


        function onPrintSucc(result) {
            ShowInfo("");
            endWaitingCoverDiv();
            var index = 0;
            var printlist = new Array();
            var tmpList = new Array("China label", "Taiwan Label", "ICASA Label L", "GOST Lable",
                    "KC Label", "WWAN ID Label", "Wimax Label", "LA NOM Label");
            var count = 0;
            var showList = "";

            var wlabel = result[2];
            var clabel = result[3];
            var cmessage = result[4];
            var llabel = result[5];
            
            try {
                if (result[0] == SUCCESSRET) {

                    for (var i = 0; i < tmpList.length; i++) {
                        //setPrintItemListParam(result[1], tmpList[i]);
                        if (i <= 4) {
                            if (tmpList[i] != clabel) {
                                continue;
                            }
                        }
                        else if (i <= 6) {
                            if (tmpList[i] != wlabel) {
                                continue;
                            }
                        }
                        else {
                            if (tmpList[i] != llabel) {
                                continue;
                            }
                        }

                        index = getTemp(result[1], tmpList[i]);
                        if (index >= 0) {
                            setPrintItemListParam(result[1][index], tmpList[i]);
                            printlist[count] = result[1][index];
                            count++;

                            if (showList != "") {
                                showList = showList + ",";
                            }
                            showList = showList + tmpList[i];
                        }
                    }

                    if (wlabel == "Wimax Label") {
                        alert(msgPrintWLabel);
                    }


                    //==========================================print process=======================================
                    /*
                    * Function Name: printLabels
                    * @param: printItems
                    * @param: isSerial
                    */
                    printLabels(printlist, false);
                    //==========================================end print process===================================
                    //ResetPage();
                    if (cmessage != "") {
                        alert(cmessage);
                    }

                    showList = msgPrintDetail + showList;
                    ShowSuccessfulInfoFormatDetail(true, "", "Customer SN", customerSN, showList);
                   
                }
                else if (result == null) {
                    ResetPage();
                    ShowMessage("msgSystemError");
                    ShowInfo("msgSystemError");
                }
                else {
                    ResetPage();
                    ShowMessage(result);
                    ShowInfo(result);
                }
            }
            catch (e) {
                alert(e.description);
                ResetPage();
                ShowInfo(e.description);
            }
            callNextInput();
        }


        function onPrintFail(error) {

            endWaitingCoverDiv();
            try {
                ResetPage();
                getCommonInputObject().value = "";
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
                callNextInput();
            } catch (e) {
                alert(e.description);
                
               
            }
        }
        
        function clkSetting() {

            showPrintSetting(station, pcode);
            callNextInput();

        }

        function ResetPage() {
            initPage();
            //callNextInput();
        }

        function callNextInput() {
            //getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("checkInput");
        }

        function imposeMaxLength(obj) {
            var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
            return (obj.value.length < mlength);
        }

        function ismaxlength(obj) {
            var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
            if (obj.getAttribute && obj.value.length > mlength) {	// alert('The Max Input Length is '+mlength+" characters, the overflow will be truncated!!");    
                alert(msgInputMaxLength1 + mlength + msgInputMaxLength2);
                obj.value = obj.value.substring(0, mlength);
                reasonFocus();
            }
        }
     
    </script>

</asp:Content>
