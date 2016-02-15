<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for PCA Repaire Input Page
 *             
 * UI:CI-MES12-SPEC-SA-UI PCA Repair Input.docx –2011/12/13 
 * UC:CI-MES12-SPEC-SA-UC PCA Repair Input.docx –2011/12/08            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-12  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="PCARepairInput.aspx.cs" Inherits="SA_PCARepairInput" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                    <td align="left">
                        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="4">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />                         
                    </td>
                    <td colspan="2" align="center">
                        <asp:CheckBox ID="chkMusic" Checked runat="server" CssClass="iMes_label_13pt" />
                    </td>
                </tr>
                
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lblMBSno" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 20%" align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtMBSno" runat="server" CssClass="iMes_label_13pt" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    
                    <td style="width: 10%" align="left">
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 20%" align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtFamily" runat="server" CssClass="iMes_label_13pt" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 10%" align="left">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 20%" align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtModel" runat="server" CssClass="iMes_label_13pt" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td></td>
                </tr>             
                
                <tr>
                    <td colspan="7" align="left">
                        <asp:Label ID="lblTableTitle" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="7">
                        <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                            <ContentTemplate>
                                <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                    GvExtWidth="100%" GvExtHeight="240px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                    Width="99.9%" Height="230px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                    HighLightRowPosition="3" HorizontalAlign="Left">
                                    <Columns>
                                        <asp:BoundField DataField="PdLine"  />
                                        <asp:BoundField DataField="TestStn"  />
                                        <asp:BoundField DataField="Defect" />
                                        <asp:BoundField DataField="Cause" />
                                        <asp:BoundField DataField="CDate" />
                                        <asp:BoundField DataField="UDate" />
                                    </Columns>
                                </iMES:GridViewExt>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                
                <tr>
                    <td align="left">
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td align="left" colspan="6">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <button id="btnProcess" runat="server" type="button" onclick="" style="display: none" />
                                <button id="btnExit" runat="server" type="button" onclick="" style="display: none" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <input type="hidden" runat="server" id="hidStation" />
                        <input type="hidden" runat="server" id="hidInput" />
                    </td>
                </tr>
            </table>
        </center>
    </div>

    <script type="text/javascript">        
        
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var flagSound = true;

        document.body.onload = function() {
            try {
                getPdLineCmbObj().setAttribute("AutoPostBack", "True");
                getAvailableData("processDataEntry");
            } catch (e) {
                alert(e.description);
            }

        }

        function processDataEntry(inputData) {
            ShowInfo("");
            flagSound = document.getElementById("<%=chkMusic.ClientID %>").checked;
            line = getPdLineCmbValue();
            if (line == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoSelectPdLine").ToString()%>');
                setPdLineCmbFocus();
                getAvailableData("processDataEntry");
                return;
            }
            if (!isPCARepairMBSno(inputData)) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgBadMBSno").ToString()%>');
                callNextInput();
                return;
            }
            /*
            * Answer to: ITC-1360-0205
            * Description: If 11-bit MBSno input, get the first 10 bits.
            */
            inputData = SubStringSN(inputData, "MB");
            document.getElementById("<%=hidInput.ClientID%>").value = inputData;
            beginWaitingCoverDiv();
            document.getElementById("<%=btnProcess.ClientID%>").click();
        }

        function playSound() {
            ShowSuccessfulInfo(flagSound, '[' + document.getElementById("<%=hidInput.ClientID%>").value + ']' + '<%=this.GetLocalResourceObject(Pre + "_msgReceived").ToString()%>');
        }

        function callNextInput() {
            getCommonInputObject().focus();
            getCommonInputObject().select();
            getAvailableData("processDataEntry");
        }

        window.onbeforeunload = function() {
            document.getElementById("<%=btnExit.ClientID%>").click();
        }
        
        window.onunload = function() {
            document.getElementById("<%=btnExit.ClientID%>").click();
        }
    </script>

</asp:Content>
