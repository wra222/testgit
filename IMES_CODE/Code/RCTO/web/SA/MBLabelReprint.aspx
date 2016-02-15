<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:MBLabelReprint page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-14  zhu lei               Create
 * 2012-04-10  Li.Ming-Jun           ITC-1360-1615
 * Known issues:
 * TODO:
 * 编码完成，但数据库尚无相关可供测试数据。尚未调试，需整合阶段再行调试
 */ --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MBLabelReprint.aspx.cs" Inherits="SA_MBLabelReprint" Title="无标题页" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/SA/Service/WebServiceMBLabelPrint.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td width="10%">
                        <asp:Label ID="lblMBSn" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="2">
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                            Width="99%" IsPaste="true" MaxLength="50" isClear="true"
                            InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"
                            TabIndex="1"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="2">
                        <textarea id="txtReason" rows="5" style="width: 99%;" runat="server" maxlength="80"
                            onkeypress="return imposeMaxLength(this)" onblur="ismaxlength(this)" onkeydown="Tab(this)"
                            tabindex="2"></textarea>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <input type="radio" name="label" id="largeLabel" checked runat="server" /><asp:Label ID="lblLargeLabel" runat="server" CssClass="iMes_label_11pt"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="radio" name="label" id="smallLabel" runat="server" /><asp:Label ID="lblSmallLabel" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td align="right">
                        <button id="btnPrintSetting" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'"
                            onmouseout="this.className='iMes_button_onmouseout'" onclick="clkSetting()" tabindex="3">
                        </button>
                    </td>
                    
                </tr>
                <tr>
                    <td colspan="2">
                    </td>
                    <td align="right">
                        <button id="btnReprint" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'"
                            onmouseout="this.className='iMes_button_onmouseout'" onclick="clkReprint()" tabindex="2">
                        </button>
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
        var labelType = "";
        var emptyPattern = /^\s*$/;

        var msgSucc = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgSuccess") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrintSettingPara") %>';
        var msgReasonNull = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgReasonNull").ToString() %>';
        var msgInputMaxLength1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength1").ToString() %>';
        var msgInputMaxLength2 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength2").ToString() %>';
        var msgMBSnoNull = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgMBSnoNull").ToString()%>';
        var msgMBSnoBit = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgMBSnoBit").ToString() %>';
        var msgMBSnoLength = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgMBSnoLength").ToString() %>';
        var msgMBSnoLengthError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgMBSnoLengthError").ToString() %>';

        window.onload = function() {
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            station = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            inputObj = getCommonInputObject();
            DisplayInfoText();     //显示MessageTextArea框
            getAvailableData("processDataEntry");
            inputObj.focus();
        }

        function clkSetting() {
            showPrintSetting(station, pCode);
        }

        function processDataEntry(data) {
            //document.getElementById("<%=txtReason.ClientID %>").focus();
            if (data.trim() == "") {
                mbSno = "";
                alert(msgMBSnoNull);
                getCommonInputObject().focus();
                return;
            }
            mbSno = data.trim();
            clkReprint();
        }
/*
        function checkMBSno() {
            if (mbSno == "" || mbSno == null) {
                alert(msgMBSnoNull);
                return false;
            }
            if (!(mbSno.length == 10 || mbSno.length == 11)) {
                alert(msgMBSnoLength);
                return false;
            }
            if (mbSno.substr(4, 1) != "M") {
                alert(msgMBSnoBit);
                return false;
            }
            mbSno = mbSno.substr(0, 10);
            return true;
        }
*/
        function checkMBSno() {
            if (mbSno == "" || mbSno == null) {
                alert(msgMBSnoNull);
                return false;
            }
            if (!(mbSno.length == 10 || mbSno.length == 11)) {
                alert(msgMBSnoLength);
                return false;
            }
            if ((mbSno.substr(5, 1) == "M") && (mbSno.length == 10))
            {
                alert(msgMBSnoLengthError);
                return false;
            }
            //if ((mbSno.substr(4, 1) != "M") && (mbSno.substr(6, 1) != "M")) {
            if ((mbSno.substr(4, 1) != "M") && (mbSno.substr(5, 1) != "M")) {
                alert(msgMBSnoBit);
                return false;
            }
            //if (!(mbSno.length == 11 && mbSno.substr(4, 1) == "M")) {
            if ((mbSno.length == 11 && mbSno.substr(4, 1) == "M")) {
                mbSno = mbSno.substr(0, 10);
            }
            return true;
        }
        
        function clkReprint() {
            try {
                ShowInfo("");
                //DEBUG  Mantis 0001002 kaisheng 2012/06/20
                //------------------------------------------------
                if (mbSno == "") 
                {
                    processDataEntry(getCommonInputObject().value.trim());
                    return;
                }
               
                //------------------------------------------------
                if (checkMBSno()) {
                    var strReason = document.getElementById("<%=txtReason.ClientID %>").value.trim();

//                    if (emptyPattern.test(strReason)) {
//                        alert(msgReasonNull);
//                        getAvailableData("processDataEntry");
//                        document.getElementById("<%=txtReason.ClientID %>").focus();
//                        return;
//                    }
                    beginWaitingCoverDiv();
                    var printItemlist = getPrintItemCollection();

                    if (printItemlist == null) {
                        alert(msgPrintSettingPara);
                        getCommonInputObject().value = "";
                        getAvailableData("processDataEntry");
                        endWaitingCoverDiv();
                        getCommonInputObject().focus();
                        mbSno = "";
                        return;
                    }

                    if (document.getElementById("<%=largeLabel.ClientID%>").checked) {
                        labelType = "MB Label";
                    } else {
                        labelType = "MB Label-1";
                    }
                    WebServiceMBLabelPrint.RePrint(mbSno, customer, strReason, editor, station, printItemlist, labelType, printSucc, printFail);
                }
                else {
                    mbSno = "";
                    getCommonInputObject().value = "";
                    getAvailableData("processDataEntry");
                    getCommonInputObject().focus();
                    
                }
            }
            catch (e) {
                getCommonInputObject().value = "";
                getAvailableData("processDataEntry");
                endWaitingCoverDiv();
                getCommonInputObject().focus();
                mbSno = "";
                alert(e);
            }
        }

        function printSucc(result) {
            if (result[0] == SUCCESSRET) {
                if (result[2].length > 0) {
                    setPrintItemListParam(result[2], result[1]);
                    printLabels(result[2], false);
                }
                endWaitingCoverDiv();
                ShowSuccessfulInfo(true, "[" + mbSno + "] " + msgSucc);
                initPage();
            }
            else {
                ShowMessage(result[0]);
                ShowInfo(result[0]);
                endWaitingCoverDiv();
                initPage();
            }
        }

        function setPrintItemListParam(backPrintItemList, retMBSno) {
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();

            keyCollection[0] = "@BegNo";
            valueCollection[0] = generateArray(retMBSno);

            keyCollection[1] = "@EndNo";
            valueCollection[1] = generateArray(retMBSno);

            lstPrtItem[0].LabelType = labelType;

            setPrintParam(lstPrtItem, labelType, keyCollection, valueCollection);
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
            getAvailableData("processDataEntry");
            getCommonInputObject().focus();
        }       
    </script>
</asp:Content>
