<%--
/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description:PalletCollection page
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2012-07-25 Kerwin                Create
 * Known issues:
 */
 --%>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PalletCollection.aspx.cs" Inherits="PAK_PalletCollection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/PalletCollectionWebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <div style="width: 96%; margin: 0 auto;">
        <table width="100%" border="0" style="table-layout: fixed;">
            <colgroup>
                <col style="width: 100px;" />
                <col />
                <col style="width: 100px;" />
                <col />
            </colgroup>
            <tr>
                <td>
                    <asp:Label ID="LabelLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td colspan="3">
                    <iMES:CmbPdLine ID="CmbPdLine" runat="server" Width="96%" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelFloor" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                </td>
                <td colspan="3">
                    <select id="SelectFloor" style="width: 96%">
                        <option value="2F">2F</option>
                        <option value="3F">3F</option>
                    </select>
                </td>
            </tr>
        </table>
        <br />
        <table width="100%">
            <tr>
                <td style="width: 60%;">
                    <fieldset id="InputArea" style="height: 198px;">
                        <legend id="InputLegend" style="font-weight: bold; color: Blue">Product Information</legend>
                        <table width="100%">
                            <colgroup>
                                <col style="width: 6%;" />
                                <col style="width: 24%;" />
                                <col style="width: 70%;" />
                            </colgroup>
                            <tr style="height: 32px;">
                                <td>
                                </td>
                                <td>
                                    <label id="LabelCartonNo">
                                        Carton No:</label>
                                </td>
                                <td>
                                    <input id="InputCartonNo" style="width: 96%;" maxlength="10" readonly="readonly" />
                                </td>
                            </tr>
                            <tr style="height: 32px;">
                                <td>
                                </td>
                                <td>
                                    <label id="LabelDeliveryNo">
                                        Delivery No:</label>
                                </td>
                                <td>
                                    <input id="InputDeliveryNo" style="width: 96%;" maxlength="10" readonly="readonly" />
                                </td>
                            </tr>
                            <tr style="height: 32px;">
                                <td>
                                </td>
                                <td>
                                    <label id="LabelPalletNo">
                                        Pallet No:</label>
                                </td>
                                <td>
                                    <input id="InputPalletNo" style="width: 96%;" maxlength="10" readonly="readonly" />
                                </td>
                            </tr>
                            <tr style="height: 32px;">
                                <td>
                                </td>
                                <td>
                                    <label id="LabelTotalQty">
                                        Total Qty:</label>
                                </td>
                                <td>
                                    <input id="InputTotalQty" style="width: 96%;" maxlength="10" readonly="readonly" />
                                </td>
                            </tr>
                            <tr style="height: 32px;">
                                <td>
                                </td>
                                <td>
                                    <label id="LabelPackedQty">
                                        Packed Qty:</label>
                                </td>
                                <td>
                                    <input id="InputPackedQty" style="width: 96%;" maxlength="10" readonly="readonly" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <td style="width: 40%;">
                    <fieldset id="CartonArea" style="height: 198px; width: 348px;">
                        <legend id="LegendCarton" style="font-weight: bold; color: Blue">Carton List</legend>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <Triggers>
                            </Triggers>
                            <ContentTemplate>
                                <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                    GvExtWidth="342px" GvExtHeight="176px" Width="94%" SetTemplateValueEnable="true"
                                    GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Center">
                                    <Columns>
                                        <asp:BoundField DataField="CartonNo" HeaderText="Carton No" HeaderStyle-Width="98%" />
                                    </Columns>
                                </iMES:GridViewExt>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </fieldset>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td style="width: 20%">
                    <asp:Label ID="LabelDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td style="width: 60%">
                    <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" Width="80%" CanUseKeyboard="true"
                        IsClear="true" IsPaste="true" MaxLength="12" />
                </td>
                <td style="width: 20%">
                    <input id="btnPrintSet" type="button" runat="server" class="iMes_button" onclick="showPrintSetting(station, PCode)" />&nbsp;
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        var editor;
        var customer;
        var hostname = "";
        var station = "";
        var PCode = "";
        var langPre = "eng_";
        
        var dataEntryObj;
        var AlertSuccess = "";
        var AlertSelectLine = "";
        var AlertSelectFloor = "";
        var AlertWrongCode = "";
        var AlertDeliveryFull="";
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "msgPrintSettingPara") %>';

        window.onload = function() {
            editor = '<%=Editor %>';
            customer = '<%=Customer %>';
            station = '<%=Request["Station"] %>';
            PCode = '<%=Request["PCode"] %>';
            langPre = '<%=Pre %>';
            AlertSelectLine = '<%=AlertSelectLine %>';
            AlertSelectFloor = '<%=AlertSelectFloor %>';
            AlertWrongCode = '<%=AlertWrongCode %>';
            AlertSuccess = '<%=AlertSuccess %>';
            AlertDeliveryFull = '<%=AlertDeliveryFull %>';

            dataEntryObj = getCommonInputObject();
            getAvailableData("InputDataEntry");
            try {
                dataEntryObj.focus();
            } catch (e) { }
        };

        function InputDataEntry(InputData) {
            var InputDataLength = InputData.length;
            switch (InputDataLength) {
                case 9:
                    var line = getPdLineCmbValue();
                    var floor = document.getElementById("SelectFloor").value;
                    if (!line) {
                        alert(AlertSelectLine);
                        dataEntryObj.focus();
                        getAvailableData("InputDataEntry");
                        return;
                    }
                    if (!floor) {
                        alert(AlertSelectFloor);
                        dataEntryObj.focus();
                        getAvailableData("InputDataEntry");
                        return;
                    }

                    var lstPrintItem = getPrintItemCollection();
                    if (lstPrintItem == null) {
                        alert(msgPrintSettingPara);
                        ResetPage();
                        return;
                    }
                    PalletCollectionWebService.InputCarton(InputData, floor, editor, line, station, customer, lstPrintItem, InputCartonSucceed);
                    break;
                case 4:
                    if (InputData == "7777") {
                        ClearAll();
                        getAvailableData("InputDataEntry");
                        dataEntryObj.focus();
                        break;
                    } else {
                        alert(AlertWrongCode);
                        dataEntryObj.focus();
                        getAvailableData("InputDataEntry");
                        break;
                    }
                default:
                    alert(AlertWrongCode);
                    dataEntryObj.focus();
                    getAvailableData("InputDataEntry");
                    break;
            }
        }

        function InputCartonSucceed(result) {
            if (result != null) {
                if (result.length == 2) {

                    if (result[1][0].TotalQty == result[1][0].PackedQty) {
                        ClearAll();
                        ShowSuccessfulInfo(true, result[1][0].CartonNo + " " + AlertSuccess+"\r\n"+AlertDeliveryFull);
                    } else {
                        document.getElementById("InputCartonNo").value = result[1][0].CartonNo;
                        document.getElementById("InputPalletNo").value = result[1][0].PalletNo;
                        document.getElementById("InputDeliveryNo").value = result[1][0].DeliveryNo;
                        document.getElementById("InputTotalQty").value = result[1][0].TotalQty;
                        document.getElementById("InputPackedQty").value = result[1][0].PackedQty;
                        ClearGvExtTable('<%=GridViewExt1.ClientID %>', 8);

                        for (var i = 1; i < result[1][1].length + 1; i++) {
                            rowArray = new Array();
                            rowArray.push(result[1][1][i - 1]);
                            if (i < 8) {
                                eval("var newRow=ChangeCvExtRowByIndex_<%=GridViewExt1.ClientID %>(rowArray,false, i)");
                            }
                            else {
                                eval("var newRow=AddCvExtRowToBottom_<%=GridViewExt1.ClientID %>(rowArray,false)");
                            }
                        }
                        ShowSuccessfulInfo(true, result[1][0].CartonNo + " " + AlertSuccess);
                    }
                    getAvailableData("InputDataEntry");
                    setPrintItemListParam(result[1][2], result[1][0].PalletNo, "RCTO_Pallet_Num_Label");

                } else if (result.length == 1) {
                    ClearAll();
                    ShowError(result[0]);
                    getAvailableData("InputDataEntry");
                    dataEntryObj.focus();
                }
            }
        }

        function ClearAll() {
            document.getElementById("InputCartonNo").value = "";
            document.getElementById("InputPalletNo").value = "";
            document.getElementById("InputDeliveryNo").value = "";
            document.getElementById("InputTotalQty").value = "";
            document.getElementById("InputPackedQty").value = "";
            ClearGvExtTable('<%=GridViewExt1.ClientID %>', 8);
        }

        function ShowError(result) {
            ShowMessage(result);
            ShowInfo(result);
        }

        function setPrintItemListParam(backPrintItemList, palletNo, labeltype) {

            var keyCollection = new Array();
            var valueCollection = new Array();

            keyCollection[0] = "@PalletNo";

            valueCollection[0] = generateArray(palletNo);

            setPrintParam(backPrintItemList, labeltype, keyCollection, valueCollection);

            printLabels(backPrintItemList, false);
        }

    </script>

</asp:Content>
