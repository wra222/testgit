﻿
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="EEPLabelRePrint.aspx.cs" Inherits="TRIS_EEPLabelRePrint"
    Title="Untitled Page" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
                <asp:ServiceReference Path="~/TRIS/Service/WebServiceEEPLabelPrint.asmx" />
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
                            CanUseKeyboard="true" IsPaste="true" IsClear="false" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*\,]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <textarea id="txtReason" style="overflow:auto; width: 99%; height: 100px"></textarea>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr style="height: 30px">
                    <td colspan="4" align="right">
                        <input id="btnPrintSetting" type="button"  runat="server" class="iMes_button" onclick="clkSetting()" 
                        onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="right">
                        <input id="btnReprint" type="button"  runat="server"  class="iMes_button" onclick="clkReprint()"
                        onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
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
                initPage();
                callNextInput();

            } catch (e) {
                alert(e.description);
            }
        }

        function initPage() {
            ShowInfo("");
            customerSN = "";
        }

        function checkInput(inputData) {
            clkReprint();
            return;
        }

        function clkReprint() {

            var reason = document.getElementById("txtReason").value;
            var sn = getCommonInputObject().value;
            sn = Get2DCodeCustSN(sn);
            var lstPrintItem = getPrintItemCollection();   //UI transfer in Para: PrintItemlist
            if (lstPrintItem == "" || lstPrintItem == null) {
                alert(msgPrintSettingPara);
                return;
            }
            beginWaitingCoverDiv();
            WebServiceEEPLabelPrint.Reprint(sn, reason, editor, station, customer, lstPrintItem, onPrintSucceed, onPrintFail);
        }
        
        function getTemp(result, label) {

            for (var i = 0; i < result.length; i++) {
                if (result[i].LabelType == label) {
                    return i;
                }
            }
            return -1;
        }

        function setPrintItemListParam(backPrintItemList, custSN) {
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            keyCollection[0] = "@productID";
            valueCollection[0] = generateArray(custSN);

            for (var jj = 0; jj < backPrintItemList.length; jj++) {
                backPrintItemList[jj].ParameterKeys = keyCollection;
                backPrintItemList[jj].ParameterValues = valueCollection;
            }
            //setPrintParam(lstPrtItem, "TRIS Carton Label", keyCollection, valueCollection);
        }

        function onPrintSucceed(result) {

            ShowInfo("");
            endWaitingCoverDiv();
            try {

                var index = 0;
                var printlist = new Array();
                
                if (result == null) {
                    ResetPage();
                    ShowMessage(msgSystemError);
                    ShowInfo(msgSystemError);
                    callNextInput();
                }
                else if (result[0] == SUCCESSRET) {

            
                    setPrintItemListParam(result[2],result[1]); // 使用Reprint的存储过程

                    printLabels(result[2], false);

                    //==========================================end print process===================================
                    //ResetPage();
                  //  ShowSuccessfulInfoFormat(true, "Customer SN", customerSN);
                    ShowInfo("Success!","green");
                }
                else {
                    ShowMessage(result[0]);
                    ShowInfo(result[0]);
                }

            }
            catch (e) {
                alert(e.description);
            }
            getCommonInputObject().value = "";
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