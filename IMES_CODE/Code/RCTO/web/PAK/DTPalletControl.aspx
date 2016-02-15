<%--
/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description:DTPalletControl page
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2012-02-20 Kerwin                Create
 * 2012-03-03 Kerwin                ITC-1360-1034
 * 2012-03-03 Kerwin                ITC-1360-1059
 * 2012-03-03 Kerwin                ITC-1360-1068
 * 2012-03-03 Kerwin                ITC-1360-1072
 * 2012-03-03 Kerwin                ITC-1360-1073
 * 2012-03-03 Kerwin                ITC-1360-1184
 * 2012-03-03 Kerwin                ITC-1360-1852
 * Known issues:
 */
 --%>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DTPalletControl.aspx.cs" Inherits="PAK_DTPalletControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/DTPalletControlWebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <div style="width: 96%; margin: 0 auto;">
        <br />
        <fieldset id="InputArea">
            <legend id="InputLegend" style="font-weight: bold; color: Blue">Pallet Information</legend>
            <table width="100%">
                <tr>
                    <td style="width: 20%">
                        <label id="LabelFrom">
                            From:</label>
                    </td>
                    <td style="width: 30%">
                        <input id="InputFrom" style="width: 160px;" maxlength="10" readonly="readonly"/>
                        <input type="button" id="btnCal1" style="width: 23px;" onclick="showCalendar('InputFrom')" value="..." />
                    </td>
                    <td style="width: 20%">
                        <label id="LabelTo">
                            To:</label>
                    </td>
                    <td style="width: 30%">
                        <input id="InputTo" style="width: 160px;" maxlength="10" readonly="readonly"/>
                        <input type="button" id="Button1" style="width: 23px;" onclick="showCalendar('InputTo')" value="..." />
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <label id="LabelPalletNo">
                            Pallet No:</label>
                    </td>
                    <td style="width: 30%">
                        <input id="InputPalletNo" style="width: 160px;" maxlength="50" onkeydown="InputPalletKeyDown()" />
                    </td>
                    <td style="width: 15%">
                        <input id="BtnQuery" type="button" value="Query" onclick="Query()" style="width: 80px;
                            text-align: center; cursor: pointer;" />
                    </td>
                    <td style="width: 15%">
                        <input id="BtnExcel" type="button" value="Excel" onclick="if(CheckToExcel())" onserverclick="ToExcel"
                            style="width: 80px; text-align: center; cursor: pointer;" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <br />
        <fieldset id="DisplayArea">
            <legend id="DisplayLegend" style="font-weight: bold; color: Blue">Pallet List</legend>
            <table width="100%">
                <tr>
                    <td style="width: 100%;" align="left" colspan="4">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="BtnExcel" />
                            </Triggers>
                            <ContentTemplate>
                                <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                    GvExtWidth="98%" GvExtHeight="176px" Width="98%" SetTemplateValueEnable="true"
                                    GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                    <Columns>
                                        <asp:BoundField DataField="PalletNo" HeaderText="Pallet No" HeaderStyle-Width="25%" />
                                        <asp:BoundField DataField="Editor" HeaderText="Editor" HeaderStyle-Width="25%" />
                                        <asp:BoundField DataField="Cdt" HeaderText="Cdt" HeaderStyle-Width="25%" />
                                    </Columns>
                                </iMES:GridViewExt>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <input id="HiddenPalletNo" type="hidden" runat="server" />
            <input id="HiddenFrom" type="hidden" runat="server" />
            <input id="HiddenTo" type="hidden" runat="server" />
            <input id="HiddenDate" type="hidden" />
        </fieldset>
    </div>

    <script language="javascript" type="text/javascript">
        var editor;
        var customer;
        var hostname = "";
        var station = "";
        var line = "";

        var AlertInputFrom = "";
        var AlertInputTo = "";
        var AlertInputCorrectFrom = "";
        var AlertInputCorrectTo = "";
        var AlertFromLargeTo = "";
        var AlertInputPalletNo = "";
        var AlertSuccess = "";
        var AlertNoData = "";
        window.onload = function() {
            editor = '<%=Editor %>';
            customer = '<%=Customer %>';

            AlertInputFrom = '<%=AlertInputFrom %>';
            AlertInputTo = '<%=AlertInputTo %>';
            AlertInputCorrectFrom = '<%=AlertInputCorrectFrom %>';
            AlertInputCorrectTo = '<%=AlertInputCorrectTo %>';
            AlertFromLargeTo = '<%=AlertFromLargeTo %>';
            AlertInputPalletNo = '<%=AlertInputPalletNo %>';
            AlertSuccess = '<%=AlertSuccess %>';
            AlertNoData = '<%=AlertNoData %>';
            document.getElementById("InputPalletNo").focus();
        };
        var DTPalletNo = "";
        function InputPalletKeyDown() {
            if (event.keyCode == 9 || event.keyCode == 13) {
                DTPalletNo = document.getElementById("InputPalletNo").value.trim();
                if (DTPalletNo) {
                    document.getElementById("InputPalletNo").value = "";
                    ClearGvExtTable('<%=GridViewExt1.ClientID %>', 8);
                    DTPalletControlWebService.DT(DTPalletNo, editor, line, station, customer, SaveSucceed);
                } else {
                    alert(AlertInputPalletNo);
                }
            }
        }

        
        function Query() {
            var palletNo = document.getElementById("InputPalletNo").value.trim();
            var fromDate = document.getElementById("InputFrom").value.trim().replace("-", "/").replace("-", "/");
            var toDate = document.getElementById("InputTo").value.trim().replace("-", "/").replace("-", "/");

            if (palletNo == "") {
                if (fromDate == "") {
                    alert(AlertInputFrom);
                    document.getElementById("InputFrom").focus();
                    return;
                }

                if (toDate == "") {
                    alert(AlertInputTo);
                    document.getElementById("InputTo").focus();
                    return;
                }

                var pattern = /\d{4}\/\d{2}\/\d{2}/;
                if (!pattern.test(fromDate)) {
                    alert(AlertInputCorrectFrom);
                    document.getElementById("InputFrom").focus();
                    return;
                }

                if (!pattern.test(toDate)) {
                    alert(AlertInputCorrectTo);
                    document.getElementById("InputTo").focus();
                    return;
                }

                if (fromDate > toDate) {
                    alert(AlertFromLargeTo);
                    return;
                }

                var fromArray = fromDate.split("/");
                var Date1 = new Date(fromArray[0], fromArray[1] - 1, fromArray[2]);
                if (Date1.getMonth() + 1 != fromArray[1] || Date1.getDate() != fromArray[2] || Date1.getFullYear() != fromArray[0] || fromArray[0].length != 4) {
                    alert(AlertInputCorrectFrom);
                    document.getElementById("InputFrom").focus();
                    return false;
                }

                var toArray = toDate.split("/");
                var ParseToDate = new Date(toArray[0], toArray[1] - 1, toArray[2]);
                if (ParseToDate.getMonth() + 1 != toArray[1] || ParseToDate.getDate() != toArray[2] || ParseToDate.getFullYear() != toArray[0] || toArray[0].length != 4) {
                    alert(AlertInputCorrectTo);
                    document.getElementById("InputTo").focus();
                    return false;
                }

            }

            ClearGvExtTable('<%=GridViewExt1.ClientID %>', 8);
            DTPalletControlWebService.Query(palletNo, fromDate, toDate, QuerySucceed);
        }

        function CheckToExcel() {
            var palletNo = document.getElementById("InputPalletNo").value.trim();
            var fromDate = document.getElementById("InputFrom").value.trim().replace("-", "/").replace("-", "/");
            var toDate = document.getElementById("InputTo").value.trim().replace("-", "/").replace("-", "/");

            if (palletNo == "") {
                if (fromDate == "") {
                    alert(AlertInputFrom);
                    document.getElementById("InputFrom").focus();
                    return false;
                }

                if (toDate == "") {
                    alert(AlertInputTo);
                    document.getElementById("InputTo").focus();
                    return false;
                }

                var pattern = /\d{4}\/\d{2}\/\d{2}/;
                if (!pattern.test(fromDate)) {
                    alert(AlertInputCorrectFrom);
                    document.getElementById("InputFrom").focus();
                    return;
                }

                if (!pattern.test(toDate)) {
                    alert(AlertInputCorrectTo);
                    document.getElementById("InputTo").focus();
                    return false;
                }

                if (fromDate > toDate) {
                    alert(AlertFromLargeTo);
                    return;
                }
                var fromArray = fromDate.split("/");
                var Date1 = new Date(fromArray[0], fromArray[1] - 1, fromArray[2]);
                if (Date1.getMonth() + 1 != fromArray[1] || Date1.getDate() != fromArray[2] || Date1.getFullYear() != fromArray[0] || fromArray[0].length != 4) {
                    alert(AlertInputCorrectFrom);
                    document.getElementById("InputFrom").focus();
                    return false;
                }

                var toArray = toDate.split("/");
                var ParseToDate = new Date(toArray[0], toArray[1] - 1, toArray[2]);
                if (ParseToDate.getMonth() + 1 != toArray[1] || ParseToDate.getDate() != toArray[2] || ParseToDate.getFullYear() != toArray[0] || toArray[0].length != 4) {
                    alert(AlertInputCorrectTo);
                    document.getElementById("InputTo").focus();
                    return false;
                }
            }

            ClearGvExtTable('<%=GridViewExt1.ClientID %>', 8);
            document.getElementById('<%=HiddenPalletNo.ClientID%>').value = palletNo;
            document.getElementById('<%=HiddenFrom.ClientID%>').value = fromDate;
            document.getElementById('<%=HiddenTo.ClientID%>').value = toDate;
            return true;
        }

        function SaveSucceed(result) {
            if (result != null) {
                if (result.length == 2) {
                    if (result[0] != null && result[0].length > 0) {
                        for (var i = 1; i < result[0].length + 1; i++) {
                            rowArray = new Array();
                            rowArray.push(result[0][i - 1].plt);
                            rowArray.push(result[0][i - 1].editor);
                            rowArray.push(result[0][i - 1].cdt.format("yyyy/MM/dd HH:mm:ss"));
                            if (i < 8) {
                                eval("var newRow=ChangeCvExtRowByIndex_<%=GridViewExt1.ClientID %>(rowArray,false, i)");
                            }
                            else {
                                eval("var newRow=AddCvExtRowToBottom_<%=GridViewExt1.ClientID %>(rowArray,false)");
                            }
                        }
                    }
                    ShowSuccessfulInfo(true, "["+DTPalletNo+"] "+AlertSuccess);

                } else if (result.length == 1) {
                    ShowError(result[0]);
                }
            }

        }

        function QuerySucceed(result) {
            if (result != null) {
                if (result.length == 2) {
                    if (result[0] != null && result[0].length > 0) {
                        for (var i = 1; i < result[0].length + 1; i++) {
                            rowArray = new Array();
                            rowArray.push(result[0][i - 1].plt);
                            rowArray.push(result[0][i - 1].editor);
                            rowArray.push(result[0][i - 1].cdt.format("yyyy/MM/dd HH:mm:ss"));
                            if (i < 8) {
                                eval("var newRow=ChangeCvExtRowByIndex_<%=GridViewExt1.ClientID %>(rowArray,false, i)");
                            }
                            else {
                                eval("var newRow=AddCvExtRowToBottom_<%=GridViewExt1.ClientID %>(rowArray,false)");
                            }
                        }

                        ShowSuccessfulInfo(true, AlertSuccess);

                    } else {
                        ShowError(AlertNoData);
                    }

                } else if (result.length == 1) {
                    ShowError(result[0]);
                }
            }

        }

        function ShowError(result) {
            ShowMessage(result);
            ShowInfo(result);
        }

    </script>

</asp:Content>
