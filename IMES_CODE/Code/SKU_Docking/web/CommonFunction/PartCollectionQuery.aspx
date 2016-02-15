<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="PartCollectionQuery.aspx.cs" Inherits="CommonFunction_PartCollectionQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="width: 95%; margin: 0 auto;">
        <table width="100%" border="0" style="table-layout: fixed;">
            <tr>
                <td style="width: 20%">
                    <asp:Label ID="LabelModel" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td style="width: 80%">
                    <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" Width="80%" CanUseKeyboard="true"
                        IsClear="true" IsPaste="true" MaxLength="20" />
                </td>
            </tr>
        </table>
        <br />
        <asp:UpdatePanel ID="updatePanel2" runat="server" RenderMode="Inline">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnRefreshGV" EventName="ServerClick" />
            </Triggers>
            <ContentTemplate>
                <table width="96%">
                    <tr>
                        <td style="width: 100%; text-align:center;" colspan="2">
                            <asp:Label ID="LabelQuery" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset id="FS401" style="height: 180px;">
                                <legend id="Legend40" style="font-weight: bold; color: Blue">Board Input</legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 98%">
                                            <iMES:GridViewExt ID="GV40" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="98%" GvExtHeight="156px" Width="98%" SetTemplateValueEnable="true"
                                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No/Item Name" HeaderStyle-Width="25%" />
                                                    <asp:BoundField DataField="PartType" HeaderText="Part Type" HeaderStyle-Width="20%" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="40%" />
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="15%" />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset id="FS37" style="height: 180px;">
                                <legend id="Legend37" style="font-weight: bold; color: Blue">Combine CPU</legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 98%">
                                            <iMES:GridViewExt ID="GV37" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="98%" GvExtHeight="156px" Width="98%" SetTemplateValueEnable="true"
                                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No/Item Name" HeaderStyle-Width="25%" />
                                                    <asp:BoundField DataField="PartType" HeaderText="Part Type" HeaderStyle-Width="20%" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="40%" />
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="15%" />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset id="FS8C" style="height: 180px;">
                                <legend id="Legend8C" style="font-weight: bold; color: Blue">Combine Pizza</legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 98%">
                                            <iMES:GridViewExt ID="GV8C" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="98%" GvExtHeight="156px" Width="98%" SetTemplateValueEnable="true"
                                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No/Item Name" HeaderStyle-Width="25%" />
                                                    <asp:BoundField DataField="PartType" HeaderText="Part Type" HeaderStyle-Width="20%" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="40%" />
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="15%" />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset id="FS3C" style="height: 180px;">
                                <legend id="Legend3C" style="font-weight: bold; color: Blue">Combine TPM CT</legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 98%">
                                            <iMES:GridViewExt ID="GV3C" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="98%" GvExtHeight="156px" Width="98%" SetTemplateValueEnable="true"
                                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No/Item Name" HeaderStyle-Width="25%" />
                                                    <asp:BoundField DataField="PartType" HeaderText="Part Type" HeaderStyle-Width="20%" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="40%" />
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="15%" />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset id="FS3D" style="height: 180px;">
                                <legend id="Legend3D" style="font-weight: bold; color: Blue">Combine Thermal</legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 98%">
                                            <iMES:GridViewExt ID="GV3D" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="98%" GvExtHeight="156px" Width="98%" SetTemplateValueEnable="true"
                                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No/Item Name" HeaderStyle-Width="25%" />
                                                    <asp:BoundField DataField="PartType" HeaderText="Part Type" HeaderStyle-Width="20%" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="40%" />
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="15%" />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset id="FS39" style="height: 180px;">
                                <legend id="Legend39" style="font-weight: bold; color: Blue">CombineKP</legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 98%">
                                            <iMES:GridViewExt ID="GV39" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="98%" GvExtHeight="156px" Width="98%" SetTemplateValueEnable="true"
                                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No/Item Name" HeaderStyle-Width="25%" />
                                                    <asp:BoundField DataField="PartType" HeaderText="Part Type" HeaderStyle-Width="20%" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="40%" />
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="15%" />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset id="FS3A" style="height: 180px;">
                                <legend id="Legend3A" style="font-weight: bold; color: Blue">CombineKP2</legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 98%">
                                            <iMES:GridViewExt ID="GV3A" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="98%" GvExtHeight="156px" Width="98%" SetTemplateValueEnable="true"
                                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No/Item Name" HeaderStyle-Width="25%" />
                                                    <asp:BoundField DataField="PartType" HeaderText="Part Type" HeaderStyle-Width="20%" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="40%" />
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="15%" />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset id="FS3B" style="height: 180px;">
                                <legend id="Legend3B" style="font-weight: bold; color: Blue">CombineKP3</legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 98%">
                                            <iMES:GridViewExt ID="GV3B" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="98%" GvExtHeight="156px" Width="98%" SetTemplateValueEnable="true"
                                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No/Item Name" HeaderStyle-Width="25%" />
                                                    <asp:BoundField DataField="PartType" HeaderText="Part Type" HeaderStyle-Width="20%" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="40%" />
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="15%" />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset id="FSPKOK" style="height: 180px;">
                                <legend id="LegendPKOK" style="font-weight: bold; color: Blue">DDD Kitting </legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 98%">
                                            <iMES:GridViewExt ID="GVPKOK" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="98%" GvExtHeight="156px" Width="98%" SetTemplateValueEnable="true"
                                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No/Item Name" HeaderStyle-Width="25%" />
                                                    <asp:BoundField DataField="PartType" HeaderText="Part Type" HeaderStyle-Width="20%" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="40%" />
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="15%" />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset id="FSMP" style="height: 180px;">
                                <legend id="LegendMP" style="font-weight: bold; color: Blue">Packing Pizza</legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 98%">
                                            <iMES:GridViewExt ID="GVMP" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="98%" GvExtHeight="156px" Width="98%" SetTemplateValueEnable="true"
                                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No/Item Name" HeaderStyle-Width="25%" />
                                                    <asp:BoundField DataField="PartType" HeaderText="Part Type" HeaderStyle-Width="20%" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="40%" />
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="15%" />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset id="FSPKCK" style="height: 180px;">
                                <legend id="LegendPKCK" style="font-weight: bold; color: Blue">Pizza Check</legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 98%">
                                            <iMES:GridViewExt ID="GVPKCK" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="98%" GvExtHeight="156px" Width="98%" SetTemplateValueEnable="true"
                                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No/Item Name" HeaderStyle-Width="25%" />
                                                    <asp:BoundField DataField="PartType" HeaderText="Part Type" HeaderStyle-Width="20%" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="40%" />
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="15%" />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset id="FS3E" style="height: 180px;">
                                <legend id="Legend3E" style="font-weight: bold; color: Blue">ROMEO Battery</legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 98%">
                                            <iMES:GridViewExt ID="GV3E" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="98%" GvExtHeight="156px" Width="98%" SetTemplateValueEnable="true"
                                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No/Item Name" HeaderStyle-Width="25%" />
                                                    <asp:BoundField DataField="PartType" HeaderText="Part Type" HeaderStyle-Width="20%" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="40%" />
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="15%" />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset id="Fieldset1" style="height: 180px;">
                                <legend id="Legend1" style="font-weight: bold; color: Blue">3F</legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 98%">
                                            <iMES:GridViewExt ID="GV3F" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="98%" GvExtHeight="156px" Width="98%" SetTemplateValueEnable="true"
                                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No/Item Name" HeaderStyle-Width="25%" />
                                                    <asp:BoundField DataField="PartType" HeaderText="Part Type" HeaderStyle-Width="20%" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="40%" />
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="15%" />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset id="Fieldset2" style="height: 180px;">
                                <legend id="Legend2" style="font-weight: bold; color: Blue">3L</legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 98%">
                                            <iMES:GridViewExt ID="GV3L" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="98%" GvExtHeight="156px" Width="98%" SetTemplateValueEnable="true"
                                                GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="PartNo" HeaderText="Part No/Item Name" HeaderStyle-Width="25%" />
                                                    <asp:BoundField DataField="PartType" HeaderText="Part Type" HeaderStyle-Width="20%" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="40%" />
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="15%" />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updatePanel1" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <input type="hidden" id="HiddenModel" runat="server" />
                <button id="BtnRefreshGV" runat="server" onserverclick="BtnRefreshGV_Click" style="display: none">
                </button>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var station = "";
        var ClientHiddenModelID = "";
        var ClientBtnRefreshGVID = "";
        var ClientLabelQueryID = "";
        window.onload = function() {
            ClientHiddenModelID = '<%=HiddenModel.ClientID %>';
            ClientBtnRefreshGVID = '<%=BtnRefreshGV.ClientID %>';
            ClientLabelQueryID = '<%=LabelQuery.ClientID %>';
            getAvailableData("InputModel");
            inputObj = getCommonInputObject();
            inputObj.focus();
        };

        function InputModel(data) {
            document.getElementById(ClientHiddenModelID).value = data;
            document.getElementById(ClientLabelQueryID).innerText ="Model "+ data + " 查询中！";
            document.getElementById(ClientBtnRefreshGVID).click();
            getAvailableData("InputModel");

        }

        function ShowError(result) {
            ShowMessage(result);
            ShowInfo(result);
        }
    </script>

</asp:Content>
