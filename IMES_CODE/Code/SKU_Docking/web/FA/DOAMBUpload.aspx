<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for DOA MB Upload Page
 *             
 * UI:CI-MES12-SPEC-FA-UI DOA MB List Upload.docx
 * UC:CI-MES12-SPEC-FA-UC DOA MB List Upload.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-11-20  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="DOAMBUpload.aspx.cs" Inherits="FA_DOAMBUpload" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
            <Services>
            </Services>
        </asp:ScriptManager>
        <center>
            <table width="95%">
                <tr style="height:35px">
                    <td align="left" width="15%">
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td align="left" colspan="2">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                    </td>
                </tr>
                <tr style="height:35px">
                    <td><asp:Label ID="lblFile" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td width="70%"><asp:FileUpload ID="FileUp" runat="server" style="background-color:RGB(242,254,230);Width:100%;height:24px" /></td>
                    <td align="right">
                        <button type="button" style ="width:110px; height:24px;" id="btnUpload" onclick="clickUpload()">
                            <%=this.GetLocalResourceObject(Pre + "_btnUpload").ToString()%>
                        </button>                        
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table width="99%">
                            <tr>
                                <td width="30%" align="left">
                                    <asp:Label ID="lblPassList" runat="server" CssClass="iMes_label_13pt" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblFailList" runat="server" CssClass="iMes_label_13pt" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <iMES:GridViewExt ID="gve1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="100%" GvExtHeight="290px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                                Width="99.9%" Height="280px" OnRowDataBound="gd_DataBound1" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                                HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="MBSN" />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="center">
                                    <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <iMES:GridViewExt ID="gve2" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                                GvExtWidth="100%" GvExtHeight="290px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                                Width="99.9%" Height="280px" OnRowDataBound="gd_DataBound2" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                                HighLightRowPosition="3" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField DataField="MBSN" />
                                                    <asp:BoundField DataField="Cause"  />
                                                </Columns>
                                            </iMES:GridViewExt>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td />
                    <td />
                    <td align="right">
                        <button type="button" runat="server" onserverclick="btnToExcel_ServerClick" style ="width:110px; height:24px;" id="btnExport" />
                    </td>
                </tr>
            </table>     
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                <ContentTemplate>
                </ContentTemplate>
            </asp:UpdatePanel> 
            <asp:UpdatePanel ID="upHidden" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <input type="hidden" runat="server" id="hidMBSN" />
                </ContentTemplate>
            </asp:UpdatePanel> 
            <button id="btnUploadSingle" runat="server" type="button" onserverclick="btnUploadSingle_ServerClick" style="display: none" />            
            <button id="btnUploadList" runat="server" type="button" onserverclick="btnUploadList_ServerClick" style="display: none" />  
            <input type="hidden" id="hidSelectedData" />
        </center>
    </div>
    
    <script type="text/javascript">

        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";

        document.body.onload = function() {
            callNextInput();
        }

        function processDataEntry(inputData) {
            ShowInfo("");
            try {
                inputData = inputData.trim();
                if (inputData == "7777") {
                    ClearGvExtTable('<%=gve1.ClientID%>', '<%=initRowsCount%>' + 1);
                    ClearGvExtTable('<%=gve2.ClientID%>', '<%=initRowsCount%>' + 1);
                    callNextInput();
                    return;
                }

                if (inputData.length != 10 && inputData.length != 11) {
                    alert('<%=this.GetLocalResourceObject(Pre + "_msgBadMBSN").ToString()%>');
                    callNextInput();
                    return;
                }
                beginWaitingCoverDiv();
                document.getElementById('<%=hidMBSN.ClientID%>').value = inputData;
                document.getElementById('<%=btnUploadSingle.ClientID%>').click();
                callNextInput();
            } catch (e) {
                alert(e.description);
            }
        }

        function callNextInput() {
            getCommonInputObject().focus();
            getCommonInputObject().select();
            getAvailableData("processDataEntry");
        }
        
        function clickUpload() {
            ShowInfo("");
            fn = document.getElementById("<%=FileUp.ClientID%>").value;

            if (fn == "") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgNoFile").ToString()%>');
                return;
            }

            if (fn.substring(fn.length - 4).toUpperCase() != ".XLS" && fn.substring(fn.length - 5).toUpperCase() != ".XLSX") {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgBadFileName").ToString()%>');
                return;
            }

            try {
                sfso = new ActiveXObject("Scripting.FileSystemObject");
            }
            catch (err) {
                errmsg = "new ActiveXObject(\"Scripting.FileSystemObject\"):" + err.description;
                ShowMessage(errmsg);
                ShowInfo(errmsg);
                return;
            }

            if (!sfso.FileExists(fn)) {
                alert('<%=this.GetLocalResourceObject(Pre + "_msgFileNotExist").ToString()%>');
                return;
            }

            beginWaitingCoverDiv();
            document.getElementById('<%=btnUploadList.ClientID%>').click();
            return;
        }
    </script>

</asp:Content>
