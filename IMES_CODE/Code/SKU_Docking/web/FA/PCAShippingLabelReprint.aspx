<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:PCAShippingLabelReprint page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-14  zhu lei               Create
 * 2012-02-27  Li.Ming-Jun           ITC-1360-0598
 * 2012-02-27  Li.Ming-Jun           ITC-1360-0600
 * 2012-02-27  Li.Ming-Jun           ITC-1360-0601
 * 2012-03-10  Li.Ming-Jun           ITC-1360-1365
 * 2012-04-10  Li.Ming-Jun           ITC-1360-1633
 * 2012-04-11  Li.Ming-Jun           ITC-1360-1638
 * 2012-04-12  Li.Ming-Jun           ITC-1360-1685
 * 2012-04-13  Li.Ming-Jun           ITC-1360-1704
 * 2012-05-09  Li.Ming-Jun           ITC-1360-1791
 * Known issues:
 * TODO:
 */ --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PCAShippingLabelReprint.aspx.cs" Inherits="FA_PCAShippingLabelReprint" Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServicePCAShippingLabelPrint.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 96%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td width="25%">
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td>
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="99%" IsClear="true" IsPaste="true" 
                            MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"
                            TabIndex="1"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <textarea id="txtReason" rows="5" style="width:99%;" runat="server" maxlength="80"
                            onkeypress="return imposeMaxLength(this)" onblur="ismaxlength(this)" onkeydown="Tab(this)"
                            tabindex="2"></textarea>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td align="right">
                        <input type="button" id="btnPrintSetting" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'"
                            onmouseout="this.className='iMes_button_onmouseout'" onclick="clkSetting()" tabindex="3" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td align="right">
                        <input type="button" id="btnReprint" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'"
                            onmouseout="this.className='iMes_button_onmouseout'" onclick="clkReprint()" tabindex="2" />
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="updatePanel" runat="server">
            </asp:UpdatePanel>
        </div>
    </div>

    <script language="javascript" type="text/javascript">
        var editor;
        var customer;
        var station;
        var inputObj;
        var pCode;
        var mbSno = "";
        var emptyPattern = /^\s*$/;

        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var msgReasonNull = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgReasonNull").ToString() %>';
        var msgInputMaxLength1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgInputMaxLength1").ToString() %>';
        var msgInputMaxLength2 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgInputMaxLength2").ToString() %>';
        var msgMBSnoNull = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoNull").ToString()%>';
        var msgMBSnoLength = '<%=this.GetLocalResourceObject(Pre + "_msgMBSnoLength").ToString()%>';

        window.onload = function() {
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            station = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            inputObj = getCommonInputObject();
            getAvailableData("input");
            inputObj.focus();
        }

        function clkSetting() {
            showPrintSetting(station, pCode);
        }

        function checkMBSno() {
            if (mbSno == "" || mbSno == null) {
                alert(msgMBSnoNull);
                return false;
            }

            if (!(mbSno.length == 9 || mbSno.length == 10 || mbSno.length == 11)) {
                alert(msgMBSnoLength);
                return false;
            }

            if (mbSno.length == 11 || (mbSno.length == 10 && (mbSno.substr(4, 1) == "M" || mbSno.substr(4, 1) == "B"))) {
                mbSno = SubStringSN(mbSno, "MB");
            }
            else {
                mbSno = SubStringSN(mbSno, "ProdId");
            }

            return true;
        }

        function input(data) {
            //document.getElementById("<%=txtReason.ClientID %>").focus();
            mbSno = data;
            clkReprint();
        }

        function clkReprint() {
            try {
                ShowInfo("");

                if (mbSno == "") {
                    mbSno = getCommonInputObject().value;
                }
                
                if (checkMBSno()) {
                    var strReason = document.getElementById("<%=txtReason.ClientID %>").value.trim();

//                    if (emptyPattern.test(strReason)) {
//                        alert(msgReasonNull);
//                        getAvailableData("input");
//                        document.getElementById("<%=txtReason.ClientID %>").focus();
//                        return;
//                    }

                    beginWaitingCoverDiv();
                    var printItemlist = getPrintItemCollection();

                    if (printItemlist == null) {
                        alert(msgPrintSettingPara);
                        getAvailableData("input");
                        endWaitingCoverDiv();
                        getCommonInputObject().focus();
                        return;
                    }

                    WebServicePCAShippingLabelPrint.Reprint(mbSno, strReason, "", editor, station, customer, printItemlist, printSucc, printFail);
                }
                else {
                    getAvailableData("input");
                    getCommonInputObject().focus();
                }
            }
            catch (e) {
                getAvailableData("input");
                endWaitingCoverDiv();
                getCommonInputObject().focus();
                alert(e);
            }
        }

        function printSucc(result) {
            if (result[0] == SUCCESSRET) {
                setPrintItemListParam(result[1]);
                printLabels(result[1][1], false);
                endWaitingCoverDiv();
                ShowSuccessfulInfo(true, "[" + mbSno + "] " + msgSuccess);
                initPage();
            }
            else {
                endWaitingCoverDiv();
                ShowMessage(result[0]);
                ShowInfo(result[0]);
                initPage();
            }
        }

        function setPrintItemListParam(backList) {
            var dCode = backList[0];
            var lstPrtItem = backList[1];
            var retMBSno = "";
            var keyCollection = new Array();
            var valueCollection = new Array();

            if (backList[2]) {
                retMBSno = backList[3];
            } else {
                retMBSno = mbSno;
            }

            keyCollection[0] = "@MBSNO";
            valueCollection[0] = generateArray(retMBSno);

            keyCollection[1] = "@DCode";
            valueCollection[1] = generateArray(dCode);
            setAllPrintParam(lstPrtItem, keyCollection, valueCollection);
           // setPrintParam(lstPrtItem, "MB CT Label", keyCollection, valueCollection);
        }

        function printFail(result) {
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            endWaitingCoverDiv();
            initPage();
        }

        function imposeMaxLength(obj) {
            var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
            return (obj.value.length < mlength);
        }

        function ismaxlength(obj) {
            var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
            if (obj.getAttribute && obj.value.length > mlength) {
                alert(msgInputMaxLength1 + mlength + msgInputMaxLength2);
                obj.value = obj.value.substring(0, mlength);
                reasonFocus();
            }
        }

        function Tab(reasonPara) {
            if (event.keyCode == 9) {
                getCommonInputObject().focus();
                event.returnValue = false;
            }
        }

        function reasonFocus() {
            document.getElementById("<%=txtReason.ClientID %>").focus();
        }

        function initPage() {
            clearData();
            mbSno = "";
            getCommonInputObject().value = "";
            getAvailableData("input");
            getCommonInputObject().focus();
        }       
    </script>

</asp:Content>
