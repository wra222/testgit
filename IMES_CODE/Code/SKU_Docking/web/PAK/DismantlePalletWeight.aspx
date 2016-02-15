<%--
/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description:Dismantle PalletWeight page
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2012-02-20 Kerwin                Create
 * 2012-03-02 Kerwin                ITC-1360-1040
 * 2012-03-02 Kerwin                ITC-1360-1043
 * 2012-03-03 Kerwin                ITC-1360-1070
 * Known issues:
 */
 --%>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DismantlePalletWeight.aspx.cs" Inherits="PAK_DismantlePalletWeight" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/DismantlePalletWeightWebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <div style="width: 96%; margin: 0 auto;">
        <br />
        <fieldset id="InputArea">
            <legend id="InputLegend" style="font-weight: bold; color: Blue">Pallet Information</legend>
            <table width="100%">
                <tr>
                    <td style="width: 20%">
                        <label id="LabelPltOrDn">
                            Pallet No or Delivery No:</label>
                    </td>
                    <td style="width: 50%">
                        <input id="InputPltOrDn" style="width: 160px;" maxlength="16" />
                        <input id="DealWithOneInput" style="display: none" />
                    </td>
                    <td style="width: 15%">
                        <input id="BtnQuery" type="button" value="Query" onclick="Query()" style="width: 80px;
                            text-align: center; cursor: pointer;" />
                    </td>
                    <td style="width: 15%">
                        <input id="BtnDismantle" type="button" value="Dismantle" onclick="Dismantle()" style="width: 80px;
                            text-align: center; cursor: pointer;" />
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
                            <ContentTemplate>
                                <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                    GvExtWidth="98%" GvExtHeight="176px" Width="98%" SetTemplateValueEnable="true"
                                    GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                    <Columns>
                                        <asp:BoundField DataField="DeliveryNo" HeaderText="Delivery No" HeaderStyle-Width="25%" />
                                        <asp:BoundField DataField="PalletNo" HeaderText="Pallet No" HeaderStyle-Width="25%" />
                                        <asp:BoundField DataField="Weight" HeaderText="Weight" HeaderStyle-Width="25%" />
                                        <asp:BoundField DataField="WeightL" HeaderText="Weight L" HeaderStyle-Width="25%" />
                                    </Columns>
                                </iMES:GridViewExt>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>

    <script language="javascript" type="text/javascript">
        var editor;
        var customer;
        var hostname = "";
        var station = "";
        var line = "";
        var pltOrDn = "";
        var AlertInputPalletOrDn = "Please input Pallet No or Delivery No!";
        var AlertInputCorrectPalletOrDn = "Please input correct Pallet No or Delivery No!";
        var MsgSuccess = "Success!";

        window.onload = function() {
            editor = '<%=Editor %>';
            customer = '<%=Customer %>';

            MsgSuccess = '<%=MsgSuccess %>';
            AlertInputPalletOrDn = '<%=AlertInputPalletOrDn %>';
            AlertInputCorrectPalletOrDn = '<%=AlertInputCorrectPalletOrDn %>';
            document.getElementById("InputPltOrDn").focus();
        };

        function Dismantle() {
            pltOrDn = document.getElementById("InputPltOrDn").value.trim();
            document.getElementById("InputPltOrDn").value = "";
            if (pltOrDn == "") {
                alert(AlertInputPalletOrDn);
                document.getElementById("InputPltOrDn").focus();
                return;
            }
            if (pltOrDn.length != 10 && pltOrDn.length != 16) {
                alert(AlertInputCorrectPalletOrDn);
                document.getElementById("InputPltOrDn").focus();
                return;
            }

            var pattern = /^[\d]+$/;
            if (!pattern.test(pltOrDn)) {
                alert(AlertInputCorrectPalletOrDn);
                document.getElementById("InputPltOrDn").focus();
                return;
            }
            ClearGvExtTable('<%=GridViewExt1.ClientID %>', 8);
            DismantlePalletWeightWebService.Dismantle(pltOrDn, editor, line, station, customer, SaveSucceed);
        }


        function Query() {
            pltOrDn = document.getElementById("InputPltOrDn").value.trim();
            document.getElementById("InputPltOrDn").value = "";
            if (pltOrDn == "") {
                alert(AlertInputPalletOrDn);
                document.getElementById("InputPltOrDn").focus();
                return;
            }
            if (pltOrDn.length != 10 && pltOrDn.length != 16) {
                alert(AlertInputCorrectPalletOrDn);
                document.getElementById("InputPltOrDn").focus();
                return;
            }

            var pattern = /^[\d]+$/;
            if (!pattern.exec(pltOrDn)) {
                alert(AlertInputCorrectPalletOrDn);
                document.getElementById("InputPltOrDn").focus();
                return;
            }

            ClearGvExtTable('<%=GridViewExt1.ClientID %>', 8);
            DismantlePalletWeightWebService.Query(pltOrDn, SaveSucceed);
        }

        function SaveSucceed(result) {
            if (result != null) {
                if (result.length == 2) {
                    if (result[0] != null) {
                        for (var i = 1; i < result[0].length + 1; i++) {
                            rowArray = new Array();
                            rowArray.push(result[0][i - 1].DeliveryNo);
                            rowArray.push(result[0][i - 1].PalletNo);
                            rowArray.push(result[0][i - 1].Weight);
                            rowArray.push(result[0][i - 1].WeightL);
                            if (i < 8) {
                                eval("var newRow=ChangeCvExtRowByIndex_<%=GridViewExt1.ClientID %>(rowArray,false, i)");
                            }
                            else {
                                eval("var newRow=AddCvExtRowToBottom_<%=GridViewExt1.ClientID %>(rowArray,false)");
                            }
                        }
                    }
                    ShowSuccessfulInfo(true, pltOrDn +" "+ MsgSuccess);
                } else if (result.length == 1) {
                    ShowError(result[0]);
                }
            }
            document.getElementById("InputPltOrDn").focus();
        }

        function ShowError(result) {
            ShowMessage(result);
            ShowInfo(result);
        }

    </script>

</asp:Content>
