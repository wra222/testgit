<%--
/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for SMT Defect Input Page
 *             
 * UI:CI-MES12-SPEC-SA-UI SMT Defect Input.docx –2012/05/21
 * UC:CI-MES12-SPEC-SA-UC SMT Defect Input.docx –2012/05/21            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-21  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="SMTDefectInput.aspx.cs" Inherits="SA_SMTDefectInput" %>

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
                    <td align="left" colspan="3">
                        <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />                         
                    </td>
                </tr>
                
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lblPassQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 35%" align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtPassQty" runat="server" CssClass="iMes_label_13pt" Text="0" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lblFailQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 35%" align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtFailQty" runat="server" CssClass="iMes_label_13pt" Text="0" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr> 
                
                <tr>
                    <td align="left">
                        <asp:Label ID="lblMBSno" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left" colspan="3">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtMBSno" runat="server" CssClass="iMes_label_13pt" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>             
                
                <tr>
                    <td colspan="4" align="left">
                        <asp:Label ID="lblTableTitle" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="4">
                        <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                            <ContentTemplate>
                                <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                    GvExtWidth="100%" GvExtHeight="240px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                    Width="99.9%" Height="230px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                    HighLightRowPosition="3" HorizontalAlign="Left">
                                    <Columns>
                                        <asp:BoundField DataField="Defect" />
                                        <asp:BoundField DataField="Descr" />
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
                    <td align="left" colspan="3">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <button id="btnInputMBSno" runat="server" type="button" style="display: none" />
                                <button id="btnInputDefect" runat="server" type="button" style="display: none" />
                                <button id="btnClearDefect" runat="server" type="button" style="display: none" />
                                <button id="btnSave" runat="server" type="button" style="display: none" />
                                <button id="btnExit" runat="server" type="button" style="display: none" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="upHidden" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <input type="hidden" runat="server" id="hidStation" />
                                <input type="hidden" runat="server" id="hidInput" />
                                <input type="hidden" runat="server" id="hidCurrentDefectList" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </center>
    </div>

    <script type="text/javascript">        
        
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
            line = getPdLineCmbValue();
            if (line == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoSelectPdLine").ToString()%>');
                setPdLineCmbFocus();
                getAvailableData("processDataEntry");
                return;
            }

            inputData = inputData.trim();
            mbsno = document.getElementById("<%=txtMBSno.ClientID%>").innerText.trim();
            currenDefects = document.getElementById("<%=hidCurrentDefectList.ClientID%>").value.trim();
            
            //scan 7777 to clear defect list already input
            if (inputData == "7777") {
                if (mbsno == "") {
                    alert('<%=this.GetLocalResourceObject(Pre + "_msgNoMBSnoInput").ToString()%>');
                    callNextInput();
                    return;
                }
                if (currenDefects == "") {
                    callNextInput();
                    return;
                }            
                beginWaitingCoverDiv();
                document.getElementById("<%=btnClearDefect.ClientID%>").click();
                return;
            }
            
            //scan 9999 to save
            if (inputData == "9999") {
                if (mbsno == "") {
                    alert('<%=this.GetLocalResourceObject(Pre + "_msgNoMBSnoInput").ToString()%>');
                    callNextInput();
                    return;
                }
                if (currenDefects == "") {
                    alert('<%=this.GetLocalResourceObject(Pre + "_msgNoDefectInput").ToString()%>');
                    callNextInput();
                    return;
                }
                beginWaitingCoverDiv();
                document.getElementById("<%=btnSave.ClientID%>").click();
                return;
            }

            if (mbsno == "") {  //Need input MBSno                
                if (!isPCARepairMBSno(inputData)) {
                    alert('<%=this.GetLocalResourceObject(Pre + "_msgBadMBSno").ToString()%>');
                    callNextInput();
                    return;
                }
                
                inputData = SubStringSN(inputData, "MB");
                beginWaitingCoverDiv();
                document.getElementById("<%=hidInput.ClientID%>").value = inputData;
                document.getElementById("<%=btnInputMBSno.ClientID%>").click();
                return;
            }

            //else: Need input Defect (4 characters)
            if (inputData.length != 4) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgInvalidDefect").ToString()%>');
                callNextInput();
                return;
            }
            inputDefect = ":" + inputData + ":";
            //a.	如果输入的Defect 已经存在于Defect List 中，需要提示用户：“Duplicate Data!!”/“重复数据！！”
            if (currenDefects.indexOf(inputDefect) >= 0) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgDupDefect").ToString()%>');
                callNextInput();
                return;
            }
            beginWaitingCoverDiv();
            document.getElementById("<%=hidInput.ClientID%>").value = inputData;
            document.getElementById("<%=btnInputDefect.ClientID%>").click();
            return;
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
