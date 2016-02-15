<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for PoData(Delete for PL user) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI PO Data.docx –2011/11/08 
 * UC:CI-MES12-SPEC-PAK-UC PO Data.docx –2011/11/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-09  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="PoData_DeletePL.aspx.cs" Inherits="PAK_PoData_DeletePL" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../CommonControl/calendar/calendar.js"></script>
    <script type="text/VBscript" src="../CommonControl/calendar/calendar.vbs"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
            </Services>
        </asp:ScriptManager>
        <center>
            <fieldset style="width:98%">
                <table width="100%">
                    <tr>
                        <td width="15%"><asp:Label ID="lblShipment" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td width="20%"><asp:TextBox ID="txtShipment" runat="server" Width="99%" MaxLength="20" onfocus="CalDisappear();"></asp:TextBox></td>
                        <td width="10%"></td>
                        <td width="15%"><asp:Label ID="lblDN" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td width="20%"><asp:TextBox ID="txtDN" runat="server" Width="99%" onkeyup="value=value.replace(/[^a-zA-Z0-9]/g,'')" MaxLength="16" onfocus="CalDisappear();"></asp:TextBox></td>
                        <td width="5%"></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblFrom" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td>
                            <input type="text" id="txtDateFrom" readonly="readonly" onkeyup="toClearDate('txtDateFrom')" onfocus="CalDisappear();"/>
	                        <button type="button" id="btnFrom" onclick="showCalFrame(txtDateFrom)">...</button>
                        </td>
                        <td></td>
                        <td><asp:Label ID="lblTo" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td>
                            <input type="text" id="txtDateTo" readonly="readonly" onkeyup="toClearDate('txtDateTo')" onfocus="CalDisappear();"/>
	                        <button type="button" id="btnTo" onclick="showCalFrame(txtDateTo)">...</button>
                        </td>
                        <td></td>
                        <td align="right">
                            <button type="button" style ="width:110px; height:24px;" id="btnQuery" onclick="clickQuery()">
                                <%=this.GetLocalResourceObject(Pre + "_btnQuery").ToString()%>
                            </button>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblPO" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td><asp:TextBox ID="txtPO" runat="server" Width="99%" MaxLength="20" onfocus="CalDisappear();"></asp:TextBox></td>
                        <td></td>
                        <td><asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                        <td><asp:TextBox ID="txtModel" runat="server" Width="99%" MaxLength="20" onfocus="CalDisappear();"></asp:TextBox></td>
                        <td></td>
                        <td align="right">
                            <button type="button" style ="width:110px; height:24px;" id="btnClear" onclick="clickClear()">
                                <%=this.GetLocalResourceObject(Pre + "_btnClear").ToString()%>
                            </button>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width:98%">
                <table width="100%">
                    <tr>
                        <td width="48%">
                            <asp:Label ID="lblDNList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="4%"></td>
                        <td width="29%" align="right">
                            <button type="button" style ="width:110px; height:24px;" id="btnDelDN" onclick="clickDelDN()">
                                <%=this.GetLocalResourceObject(Pre + "_btnDelDN").ToString()%>
                            </button>
                        </td>
                        <td width="19%" align="right">
                            <button type="button" style ="width:110px; height:24px;" id="btnDelShip" onclick="clickDelShip()">
                                <%=this.GetLocalResourceObject(Pre + "_btnDelShip").ToString()%>
                            </button>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="gve1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="100%" GvExtHeight="210px" OnGvExtRowClick="showDetailInfo(this)" OnGvExtRowDblClick=""
                                        Width="99.9%" Height="200px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                        HighLightRowPosition="3" HorizontalAlign="Left">
                                        <Columns>
                                            <asp:BoundField DataField="DN" />
                                            <asp:BoundField DataField="ShipNo"  />
                                            <asp:BoundField DataField="PoNo"  />
                                            <asp:BoundField DataField="Model"  />
                                            <asp:BoundField DataField="ShipDate"  />
                                            <asp:BoundField DataField="Qty"  />
                                            <asp:BoundField DataField="Status"  />
                                            <asp:BoundField DataField="CDate"  />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDNInfoList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td></td>
                        <td colspan="2">
                            <asp:Label ID="lblPalletList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="gve2" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="100%" GvExtHeight="210px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                        Width="99.9%" Height="200px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                        HighLightRowPosition="3" HorizontalAlign="Left">
                                        <Columns>
                                            <asp:BoundField DataField="AttrName"  />
                                            <asp:BoundField DataField="AttrValue"  />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td></td>
                        <td colspan="2">
                            <asp:UpdatePanel runat="server" ID="up3" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="gve3" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="100%" GvExtHeight="210px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                        Width="99.9%" Height="200px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                        HighLightRowPosition="3" HorizontalAlign="Left">
                                        <Columns>
                                            <asp:BoundField DataField="PNo"  />
                                            <asp:BoundField DataField="TotalQty"  />
                                            <asp:BoundField DataField="OKQty"  />
                                            <asp:BoundField DataField="DiffQty"  />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>                
                </table>
            </fieldset>       
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <button id="btnQueryData" runat="server" type="button" onclick="" style="display: none" />
                    <button id="btnShowDetail" runat="server" type="button" onclick="" style="display: none" />
                    <button id="btnDeleteDN" runat="server" type="button" onclick="" style="display: none" />
                    <button id="btnDeleteShip" runat="server" type="button" onclick="" style="display: none" />
                </ContentTemplate>
            </asp:UpdatePanel> 
            <asp:UpdatePanel ID="upHidden" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <input type="hidden" runat="server" id="hidDateFrom" />
                    <input type="hidden" runat="server" id="hidDateTo" />
                    <input type="hidden" runat="server" id="hidSelectedDN" />
                    <input type="hidden" runat="server" id="hidSelectedShipment" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
    </div>

    <script type="text/javascript">

        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var selectedRowIndex = -1;

        document.body.onload = function() {
            document.getElementById("<%=txtShipment.ClientID%>").focus();
        }
        initCalFrame("../CommonControl/calendar/");

        function toClearDate(id) {
            if (event.keyCode == 8 || event.keyCode == 46) document.getElementById(id).value = "";
        }
        
        function showDetailInfo(row) {
            CalDisappear();
            if (selectedRowIndex == row.index) {
                return;
            }
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex, false, "<%=gve1.ClientID %>");
            selectedRowIndex = row.index;
            setRowSelectedOrNotSelectedByIndex(selectedRowIndex, true, "<%=gve1.ClientID %>");
            
            if (document.getElementById("<%=hidSelectedDN.ClientID%>").value == row.cells[0].innerText.trim()) {
                return;
            }
            document.getElementById("<%=hidSelectedDN.ClientID%>").value = row.cells[0].innerText.trim();
            document.getElementById("<%=hidSelectedShipment.ClientID%>").value = row.cells[1].innerText.trim();
            beginWaitingCoverDiv();
            document.getElementById('<%=btnShowDetail.ClientID%>').click();
        }

        function clickQuery() {
            CalDisappear();

            /*
            * Answer to: ITC-1360-1344
            * Description: Only empty string/10-bit/16-bit are allowed for DN.
            */
            valDN = document.getElementById("<%=txtDN.ClientID%>").value;
            pattDN1 = /^$/;
            pattDN2 = /^[0-9A-Za-z]{10}$/;
            pattDN3 = /^[0-9A-Za-z]{16}$/;
            if (!pattDN1.exec(valDN) && !pattDN2.exec(valDN) && !pattDN3.exec(valDN)) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgBadDN").ToString()%>');
                document.getElementById("<%=txtDN.ClientID%>").select();
                return;
            }
            
            document.getElementById("<%=hidDateFrom.ClientID%>").value = document.getElementById("txtDateFrom").value;
            document.getElementById("<%=hidDateTo.ClientID%>").value = document.getElementById("txtDateTo").value;
            selectedRowIndex = -1;
            document.getElementById("<%=hidSelectedDN.ClientID%>").value = "";
            beginWaitingCoverDiv();
            document.getElementById('<%=btnQueryData.ClientID%>').click();
            return;
        }

        function clickClear() {
            CalDisappear();
            document.getElementById("<%=txtShipment.ClientID%>").value = "";
            document.getElementById("<%=txtDN.ClientID%>").value = "";
            document.getElementById("<%=txtPO.ClientID%>").value = "";
            document.getElementById("<%=txtModel.ClientID%>").value = "";
            document.getElementById("txtDateFrom").value = "";
            document.getElementById("txtDateTo").value = "";
            return;
        }

        function clickDelDN() {
            CalDisappear();
            if (document.getElementById("<%=hidSelectedDN.ClientID%>").value == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoRecordSelected").ToString()%>');
                return;
            }
            if (!confirm('<%=this.GetLocalResourceObject(Pre + "_msgConfirmDelDN").ToString()%>' + document.getElementById("<%=hidSelectedDN.ClientID%>").value + "?")) {
                return;
            }
            selectedRowIndex = -1;
            beginWaitingCoverDiv();
            document.getElementById('<%=btnDeleteDN.ClientID%>').click();
            return;
        }

        function clickDelShip() {
            CalDisappear();
            if (document.getElementById("<%=hidSelectedShipment.ClientID%>").value == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoRecordSelected").ToString()%>');
                return;
            }
            if (!confirm('<%=this.GetLocalResourceObject(Pre + "_msgConfirmDelShip").ToString()%>' + document.getElementById("<%=hidSelectedShipment.ClientID%>").value + "?")) {
                return;
            }
            selectedRowIndex = -1;
            beginWaitingCoverDiv();
            document.getElementById('<%=btnDeleteShip.ClientID%>').click();
            return;
        }
    </script>

</asp:Content>
