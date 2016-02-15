<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:VGALabelReprint page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-14  zhu lei               Create          
 * 2012-04-10  Li.Ming-Jun           ITC-1360-1616
 * Known issues:
 * TODO:
 * 编码完成，但数据库尚无相关可供测试数据。尚未调试，需整合阶段再行调试
 */ --%>
 <%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VGALabelReprint.aspx.cs" Inherits="SA_VGALabelReprint" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/SA/Service/WebServiceVGALabelPrint.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td width="10%">
                        <asp:Label ID="lblMBSn" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td>
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="99%" IsClear="true" IsPaste="true" 
                            MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$" ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]"
                            TabIndex="1" />
                    </td>
                </tr>     
                <tr>
                    <td>
                        <asp:Label ID="lblReason" runat="server" CssClass="iMes_label_13pt"></asp:Label>    
                    </td>
                    <td>
                        <textarea id="txtReason" rows="5" style="width:99%;" 
                        runat="server" maxlength="80" onkeypress="return imposeMaxLength(this)" 
                        onblur="ismaxlength(this)" onkeydown="Tab(this)" tabindex="2"></textarea>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <button id="btnPrintSetting" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="clkSetting()" tabindex="3"></button>
                    </td>  
                    
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <button id="btnReprint" runat="server" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" onclick="clkReprint()" tabindex="2"></button>
                    </td> 
                </tr>

                                           
            </table>
            <asp:UpdatePanel ID="updatePanel" runat="server"></asp:UpdatePanel> 
        </div>
    </div>      
    
    <script language="javascript" type="text/javascript">
        var editor;
        var customer;
        var station;
        var inputObj;
        var pCode;
        var mbSno;
        var emptyPattern = /^\s*$/;

        var msgSucc = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgSuccess") %>';
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrintSettingPara") %>';
        var msgReasonNull = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgReasonNull").ToString() %>';
        var msgInputMaxLength1 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength1").ToString() %>';
        var msgInputMaxLength2 = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputMaxLength2").ToString() %>';
        var msgMBSnoNull = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgMBSnoNull").ToString()%>';
        var msgMBSnoBit = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgMBSnoBit").ToString() %>';
        var msgMBSnoLength = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgMBSnoLength").ToString() %>';

        window.onload = function() {
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            station = '<%=Request["Station"] %>';
            pCode = '<%=Request["PCode"] %>';
            inputObj = getCommonInputObject();
            DisplayInfoText();     //显示MessageTextArea框
            getAvailableData("input");
            inputObj.focus();
        }

        function clkSetting() {
            showPrintSetting(station, pCode);
        }

        function input(data) {
            mbSno = data;
            clkReprint();
//            if (data != null && data != "") {
//                mbSno = data;
//            }
//            getAvailableData("input");
//            inputObj.focus();
        }
        
        function checkMBSno() {
            if (mbSno == "" || mbSno == null) {
                alert(msgMBSnoNull);
                //             ShowInfo(msgMBSnoNull);
                return false;
            }
            if (!(mbSno.length == 10 || mbSno.length == 11)) {
                alert(msgMBSnoLength);
                //             ShowInfo(msgMBSnoLength);
                return false;
            }
            //if (mbSno.substr(4, 1) != "M") {
            if ((mbSno.substr(4, 1) != "M") && (mbSno.substr(5, 1) != "M")) {
                if ((mbSno.substr(4, 1) != "B") && (mbSno.substr(5, 1) != "B")) {
                    alert(msgMBSnoBit);
                    //             ShowInfo(msgMBSnoBit);
                    return false;
                }
            }
            //mbSno = mbSno.substr(0, 10);
            mbSno = SubStringSN(mbSno, "MB");
            return true;
        }
        
        function clkReprint() {
            try {
                ShowInfo("");
                //if (mbSno != "") {
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

                    WebServiceVGALabelPrint.RePrint(mbSno, customer, strReason, editor, station, printItemlist, printSucc, printFail);
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
                setPrintItemListParam(result[2], result[1]);
                printLabels(result[2], false);
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

            keyCollection[0] = "@PCBNo";

            valueCollection[0] = generateArray(retMBSno);

            setPrintParam(lstPrtItem, "VGA Label", keyCollection, valueCollection);
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

