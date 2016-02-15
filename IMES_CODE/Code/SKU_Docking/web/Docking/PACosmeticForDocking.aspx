<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:PACosmetic page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-09  Du Xuan               Create          
 * Known issues:
 * ITC-1414-0233 wrong code 提示
 * TODO:
 */
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PACosmeticForDocking.aspx.cs" Inherits="PAK_PACosmeticForDocking" Title="无标题页" %>
    
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/Docking/Service/WebServicePACosmeticForDocking.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 100px;" />
                    <col />
                    <col style="width: 100px;" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblPdline" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="3">
                        <iMES:CmbPdLine ID="CmbPdLine" runat="server" Width="99%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPassQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPassQtyContent" runat="server" Text="0" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblFailQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblFailQtyContent" runat="server" Text="0" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel1" runat="server">
                <table width="100%" border="0" style="table-layout: fixed;">
                    <colgroup>
                        <col style="width: 100px;" />
                        <col />
                        <col style="width: 100px;" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:Label ID="lblCustomerSn" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCustomerSnContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductId" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductIdContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblModelContent" runat="server" Width="100%" CssClass="iMes_label_11pt"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server">
                <table width="100%" border="0" style="table-layout: fixed;">
                    <colgroup>
                        <col style="width: 100px;" />
                        <col />
                        <col style="width: 100px;" />
                        <col />
                    </colgroup>
                    <tr>
                        <td colspan="4">
                            <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" GvExtWidth="100%"
                                GvExtHeight="220px" Style="top: 0px; left: 0px" Width="99.9%" Height="210px"
                                SetTemplateValueEnable="False" HighLightRowPosition="3" AutoHighlightScrollByValue="True">
                            </iMES:GridViewExt>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <div id="div3">
            <table width="100%">
                <colgroup>
                    <col style="width: 120px;" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td>
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                            Width="99%" IsClear="true" IsPaste="true" />
                        <asp:CheckBox ID="chk9999" runat="server" Checked="false" Visible="False"></asp:CheckBox>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var inputFlag = false;
        var checkPicdFlag = false;
        var checkWwanFlag = false;
        var editor;
        var defectCache;
        var tbl;
        var DEFAULT_ROW_NUM = 7;
        var defectCount = 0;
        var defectInTable = [];
        var passQty = 0;
        var failQty = 0;
        var gprodId = "";
        var customer;
        var stationId;
        var inputObj;

        //error message
        var msgDuplicateData = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDuplicateData").ToString() %>';
        var msgInputValidDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputValidDefect").ToString() %>';
        var msgInputDefect = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgInputDefect").ToString() %>';
        var msgPrestationError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPrestationError").ToString() %>';
        var mesNoSelPdLine = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoSelectPdLine").ToString() %>';
        var msgPcidCheck = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPcidCheck").ToString() %>'
        var msgWwanCheck = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgWwanCheck").ToString() %>'
        var msgPcidError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgPcidError").ToString() %>'
        var msgWwanError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgWwanError").ToString() %>'
        var msgNeedCheck = 'Please check PCID / WWAN.';

        var msgCodeErr = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgCodeErr").ToString() %>'


        window.onload = function() {
            tbl = "<%=gd.ClientID %>";
            inputObj = getCommonInputObject();
            getAvailableData("input");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            inputObj.focus();
        };

        function initPage() {
            tbl = "<%=gd.ClientID %>";
            setInputOrSpanValue(document.getElementById("<%=lblCustomerSnContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblProductIdContent.ClientID %>"), "");
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), "");
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            setSrollByIndex(0, false);
            inputFlag = false;
            checkPicdFlag = false;
            checkWwanFlag = false;
            defectCount = 0;
            defectInTable = [];
            getPdLineCmbObj().disabled = false;
        }

        function input(data) {

            if (getPdLineCmbValue() == "") {
                alert(mesNoSelPdLine);
                callNextInput();
                return;
            }

            var line = getPdLineCmbValue();
            if (inputFlag) {
                if (data == "7777") {
                    ShowInfo("");
                    ResetPage();
                }
                else if (data == "9999") {
                    ShowInfo("");
                    if (defectInTable.length == 0) {
                        alert(msgInputDefect);
                        callNextInput();
                        return;
                    }
                    save();
                }
                else {
                    ShowInfo("");
                    inputDefect(data);
                }
            }
            else {
                if ((data.length == 9) || (data.length == 10)) {

                    gprodId = SubStringSN(data, "ProdId");
                    ShowInfo("");
                    beginWaitingCoverDiv();
                    WebServicePACosmeticDocking.input(line, gprodId, editor, stationId, customer, inputSucc, inputFail);
                    return;
                }
                else {
                    if (data != "7777") {
                        alert(msgCodeErr);
                        callNextInput();
                    }
                }
            }
        }

        function inputSucc(result) {

            endWaitingCoverDiv();
            setInfo(result);
            inputFlag = true;

            callNextInput();
        }

        function inputFail(result) {
            //show error message
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            callNextInput();
        }

        function isPass() {
            if (defectCount == 0) {
                return true;
            }

            return false;
        }

        function setInfo(info) {
            //set value to the label
            setInputOrSpanValue(document.getElementById("<%=lblCustomerSnContent.ClientID %>"), info[0]["customSN"]);
            setInputOrSpanValue(document.getElementById("<%=lblProductIdContent.ClientID %>"), info[0]["id"]);
            setInputOrSpanValue(document.getElementById("<%=lblModelContent.ClientID %>"), info[0]["modelId"]);
            //set defectCache value
            defectCache = info[2];
        }

        function save() {
            beginWaitingCoverDiv();
            WebServicePACosmeticDocking.save(gprodId, defectInTable, saveSucc, saveFail);
        }

        function saveSucc(result) {
            //show success message
            endWaitingCoverDiv();

            //initPage
            if (isPass()) {
                passQty++;
                setInputOrSpanValue(document.getElementById("<%=lblPassQtyContent.ClientID %>"), passQty);
            }
            else {
                failQty++;
                setInputOrSpanValue(document.getElementById("<%=lblFailQtyContent.ClientID %>"), failQty);
            }

            var tmpinfo = gprodId;
            ResetPage();
            ShowSuccessfulInfoFormat(true, "Product ID", tmpinfo);
            callNextInput();

        }

        function saveFail(result) {
            //show error message
            endWaitingCoverDiv();
            ResetPage();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());

        }

        function inputDefect(data) {
            if (isExistInTable(data)) {
                //error message
                alert(msgDuplicateData);
                callNextInput();
                return;
            }

            if (isExistInCache(data)) {
                var desc = getDesc(data);
                var rowArray = new Array();
                var rw;

                rowArray.push(data);
                rowArray.push(desc);

                //add data to table
                if (defectInTable.length < 6) {
                    eval("ChangeCvExtRowByIndex_" + tbl + "(rowArray, false, defectInTable.length + 1);");
                    //setSrollByIndex(defectInTable.length, true, tbl);
                }
                else {
                    eval("rw = AddCvExtRowToBottom_" + tbl + "(rowArray, false);");
                    //setSrollByIndex(defectInTable.length, true, tbl);
                    rw.cells[1].style.whiteSpace = "nowrap";
                }

                setSrollByIndex(defectInTable.length, false);

                defectInTable[defectCount++] = data;
            }
            else {
                alert(msgInputValidDefect);

            }
            callNextInput();
        }

        function isExistInTable(data) {
            if (defectInTable != undefined && defectInTable != null) {
                for (var i = 0; i < defectInTable.length; i++) {
                    if (defectInTable[i] == data) {
                        return true;
                    }
                }
            }

            return false;
        }

        function isExistInCache(data) {
            if (defectCache != undefined && defectCache != null) {
                for (var i = 0; i < defectCache.length; i++) {
                    if (defectCache[i]["id"] == data) {
                        return true;
                    }
                }
            }

            return false;
        }

        function getDesc(data) {
            if (defectCache != undefined && defectCache != null) {
                for (var i = 0; i < defectCache.length; i++) {
                    if (defectCache[i]["id"] == data) {
                        return defectCache[i]["description"];
                    }
                }
            }

            return "";
        }

        window.onbeforeunload = function() {
            if (inputFlag) {
                OnCancel();
            }
        };

        function OnCancel() {
            WebServicePACosmeticDocking.cancel(gprodId);
        }

        function ExitPage() {
            OnCancel();
        }

        function ResetPage() {
            ExitPage();
            initPage();
            ShowInfo("");
            callNextInput();
        }

        function callNextInput() {

            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
        }              
       
    </script>

</asp:Content>
